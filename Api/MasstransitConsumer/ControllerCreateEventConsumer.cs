using System.Text.Json;

using Api.Extensions;
using Api.Tools;

using MaidContexts;

using MassTransit;

using MasstransitModels;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

using ServicesModels.Methods;
using ServicesModels.Settings;

namespace Api.MasstransitConsumer
{
	/// <summary>
	/// 创建控制器
	/// </summary>
	public class ControllerCreateEventConsumer : IConsumer<ControllerCreateEvent>
	{
		private readonly ILogger<ControllerCreateEventConsumer> logger;
		private readonly MaidContext maidContext;

		///<inheritdoc/>
		public ControllerCreateEventConsumer(ILogger<ControllerCreateEventConsumer> logger, MaidContext maidContext)
		{
			this.logger = logger;
			this.maidContext = maidContext;
		}
		static readonly SyntaxTriviaList space = SyntaxTriviaList.Create(SyntaxFactory.SyntaxTrivia(SyntaxKind.WhitespaceTrivia, " "));
		static readonly SyntaxTriviaList indent1 = SyntaxTriviaList.Create(SyntaxFactory.SyntaxTrivia(SyntaxKind.WhitespaceTrivia, "\t"));
		static readonly SyntaxTriviaList indent2 = SyntaxTriviaList.Create(SyntaxFactory.SyntaxTrivia(SyntaxKind.WhitespaceTrivia, "\t\t"));
		static readonly SyntaxTriviaList eol = SyntaxTriviaList.Create(SyntaxFactory.SyntaxTrivia(SyntaxKind.EndOfLineTrivia, Environment.NewLine));
		static readonly BlockSyntax block = SyntaxFactory.Block(
							SyntaxFactory.Token(SyntaxKind.OpenBraceToken).WithLeadingTrivia(indent1).WithLeadingTrivia(indent1).WithTrailingTrivia(eol),
							new SyntaxList<StatementSyntax>(),
							SyntaxFactory.Token(SyntaxKind.CloseBraceToken).WithLeadingTrivia(indent1).WithTrailingTrivia(eol)
							);

		static readonly string[] ApiList = new string[] { nameof(IAdd) };
		///<inheritdoc/>
		public async Task Consume(ConsumeContext<ControllerCreateEvent> context)
		{
			var maid = maidContext.Maids
			.AsSplitQuery()
			.Include(x => x.Project)
			.Include(x => x.Enums)
			.ThenInclude(x => x.EnumMembers)
			.Include(x => x.Classes)
			.ThenInclude(x => x.Properties)
			.ThenInclude(x => x.Attributes)
			.First(x => x.Id == context.Message.MaidId);
			var entityName = context.Message.EntityName;
			var setting = maid.Setting.Deserialize<ControllerSetting>() ?? ControllerSetting.Default;
			var dic = new Dictionary<FileType, Dictionary<string, TypeSyntax>>() {
				{FileType.Controller,new Dictionary<string, TypeSyntax>()},
				{FileType.Service,new Dictionary<string, TypeSyntax>()},
			};
			//当更改实体的时候 创建所有相关的文件
			string serviceContent;
			if (File.Exists(context.Message.ServicePath))
				serviceContent = await File.ReadAllTextAsync(context.Message.ServicePath);
			else
				serviceContent = $$"""
					using Mapster;

					namespace Api.Services
					{
						public class {{entityName}}Service : IAdd<{{entityName}}AddDto, {{entityName}}>, IDelete<long, bool>
						{
							private MaidContext context;

							public {{entityName}}Service(MaidContext context) 
							{
								this.context = context;
							}
						}
					}
					""";
			var serviceInterface = new List<string>();
			string controllerContent;
			if (File.Exists(context.Message.ControllerPath))
				controllerContent = await File.ReadAllTextAsync(context.Message.ControllerPath);
			else
				controllerContent = $$"""
					using Api.Services;
					namespace Api.Controllers
					{
						public class {{entityName}}Controller
						{
							private {{entityName}}Service service;

							public {{entityName}}Controller()
							{
							}
						}
					}
					""";
			CompilationUnitSyntax serviceCompilationUnitSyntax = CSharpSyntaxTree.ParseText(serviceContent).GetCompilationUnitRoot();
			var serviceCompilationUnitSyntaxNew = serviceCompilationUnitSyntax.ReplaceNodes(serviceCompilationUnitSyntax.GetDeclarationSyntaxes<ClassDeclarationSyntax>(), (c, cNew) =>
			{
				cNew = UpdateMethod(FileType.Service, cNew);
				return cNew;
			});
			ClassDeclarationSyntax UpdateMethod(FileType type, ClassDeclarationSyntax c)
			{
				if (c.BaseList is null) return c;
				var baseList = c.BaseList.ChildNodes().OfType<SimpleBaseTypeSyntax>().Select(x => x.Type).OfType<GenericNameSyntax>().ToList();
				foreach (var item in baseList)
				{
					var identity = item.Identifier.Text;
					var interfaceSetting = setting.MethodInterfaces.FirstOrDefault(x => x.InterfaceName == identity);
					if (interfaceSetting is null) continue;
					var methodName = interfaceSetting.InterfaceName;
					var parameterType = item.TypeArgumentList.Arguments[0];
					var parameterTypeString = parameterType.ToString();
					var returnType = item.TypeArgumentList.Arguments[1];
					dic[type].Add(methodName, returnType);
					//获取原方法
					var method = c.Members.OfType<MethodDeclarationSyntax>().FirstOrDefault(x => x.Identifier.Text == methodName);
					var methodNew = method ?? SyntaxFactory.MethodDeclaration(
											 returnType.WithTrailingTrivia(space),
											 methodName
												).WithModifiers(
												new SyntaxTokenList(SyntaxFactory.Token(SyntaxKind.PublicKeyword).WithTrailingTrivia(space)));
					//返回值
					methodNew = methodNew.WithReturnType(returnType.WithTrailingTrivia(space));
					//接口类型
					switch (type)
					{
						case FileType.Controller:
							methodNew = methodNew.WithAttributeLists(new SyntaxList<AttributeListSyntax>() { });
							break;
						case FileType.Service:
							break;
						default:
							break;
					}
					//参数
					methodNew = methodNew.WithParameterList(SyntaxFactory.ParameterList(
											 SyntaxFactory.SeparatedList(new[] { SyntaxFactory.Parameter(
																													attributeLists: new SyntaxList<AttributeListSyntax>(),
																													modifiers: new SyntaxTokenList(),
																													type: parameterType.WithTrailingTrivia(space),
																													identifier: type switch {
																														FileType.Controller=>SyntaxFactory.Identifier("vo"),
																														FileType.Service=>SyntaxFactory.Identifier("dto"),
																														_=>throw new NotSupportedException($"FileType {type} Not Support"),
																													},
																													@default: null
																												) })).WithTrailingTrivia(eol)
						).WithLeadingTrivia(indent1);
					if (methodNew.Body is null) methodNew = methodNew.WithBody(block);
					//方法体
					var statements = methodNew.Body!.Statements;
					switch (interfaceSetting.MethodType)
					{
						case MethodType.Create:
							switch (type)
							{
								case FileType.Controller:
									break;
								case FileType.Service:
									statements = statements.Count != 0 ? statements : new SyntaxList<StatementSyntax>(new List<StatementSyntax>
										{
											SyntaxFactory.ParseStatement($"var entity = dto.Adapt<{entityName}>();"),
											SyntaxFactory.ParseStatement($"context.Add(entity);"),
											SyntaxFactory.ParseStatement($"context.SaveChanges();"),
											SyntaxFactory.ParseStatement($"return entity;"),
										}
									.Select(x => x.WithLeadingTrivia(indent2).WithTrailingTrivia(eol)));
									break;
								default:
									break;
							}
							break;
						case MethodType.Delete:
							switch (type)
							{
								case FileType.Controller:
									break;
								case FileType.Service:
									statements = statements.Count != 0 ? statements : new SyntaxList<StatementSyntax>(new List<StatementSyntax>
										{
											SyntaxFactory.ParseStatement($"var entity = context.Find<Project>(id);"),
											SyntaxFactory.ParseStatement($"context.Remove(entity);"),
											SyntaxFactory.ParseStatement($"var res = context.SaveChanges() > 0;"),
											SyntaxFactory.ParseStatement($"return res;"),
										}
									.Select(x => x.WithLeadingTrivia(indent2).WithTrailingTrivia(eol)));
									break;
								default:
									break;
							}
							break;
						case MethodType.Put:
							break;
						case MethodType.Patch:
							break;
						case MethodType.GetList:
							break;
						case MethodType.GetPageList:
							break;
						case MethodType.GetDictionary:
							break;
						default:
							break;
					}
					methodNew = methodNew.WithBody(methodNew.Body.WithStatements(statements));
					if (method is null)
						c = c.AddMembers(methodNew);
					else
						c = c.ReplaceNode(method, methodNew);
				}
				return c;
			}

			CompilationUnitSyntax controllerCompilationUnitSyntax = CSharpSyntaxTree.ParseText(controllerContent).GetCompilationUnitRoot();
			var cs = controllerCompilationUnitSyntax.GetDeclarationSyntaxes<ClassDeclarationSyntax>();
			var compilationUnitSyntaxNew = controllerCompilationUnitSyntax;
			compilationUnitSyntaxNew = compilationUnitSyntaxNew.ReplaceNodes(cs, (c, c2) =>
			{
				if (c.BaseList is null) return c2;
				var baselist = c.BaseList.ChildNodes().OfType<SimpleBaseTypeSyntax>().Select(x => x.Type).OfType<GenericNameSyntax>().ToList();
				foreach (var item in baselist)
				{
					var parameterType = item.TypeArgumentList.Arguments[0];
					var parameterTypeString = parameterType.ToString();
					var returnType = item.TypeArgumentList.Arguments[1];
					var identity = nameof(ServicesModels.Methods.IAdd);
					switch (item.Identifier.Text)
					{
						case nameof(ServicesModels.Methods.IAdd):
							{
								var action = c2.Members.OfType<MethodDeclarationSyntax>().FirstOrDefault(x => x.Identifier.Text == nameof(ServicesModels.Methods.IAdd));
								if (action is null)
								{
									var statements = new List<StatementSyntax>();
									string servicesParameter;
									statements.Add(SyntaxFactory.EmptyStatement().WithTrailingTrivia(eol));
									if (parameterTypeString.EndsWith("vo"))
									{
										statements.Add(SyntaxFactory.ParseStatement($"var data=vo.Adapt<{parameterTypeString.Replace("vo", "dto")}>()"));
										servicesParameter = "data";
									}
									statements.Add(SyntaxFactory.ParseStatement($"var res=service.{identity}(data)").WithTrailingTrivia(eol));
									statements.Add(SyntaxFactory.ParseStatement($"return res;").WithTrailingTrivia(eol));
									var method = SyntaxFactory.MethodDeclaration(
										returnType: returnType.WithTrailingTrivia(space),
										identifier: identity
										)
									.WithAttributeLists(new SyntaxList<AttributeListSyntax>() { })
									.WithModifiers(
										new SyntaxTokenList(SyntaxFactory.Token(SyntaxKind.PublicKeyword).WithTrailingTrivia(space)))
									.WithParameterList(
										SyntaxFactory.ParameterList(
											 SyntaxFactory.SeparatedList(new[] { SyntaxFactory.Parameter(
																													attributeLists: new SyntaxList<AttributeListSyntax>(),
																													modifiers: new SyntaxTokenList(),
																													type: parameterType.WithTrailingTrivia(space),
																													identifier: SyntaxFactory.Identifier("vo"),
																													@default: null
																												) })))
									.WithBody(
										   SyntaxFactory.Block(statements)
										   .WithLeadingTrivia(eol)
										   .WithTrailingTrivia(eol)
										   )
										;
									c2 = c.AddMembers(method);
								}
								break;
							}
					}
				}
				return c2;
			});
			await FileTools.Write(context.Message.ServicePath, serviceCompilationUnitSyntax, serviceCompilationUnitSyntaxNew);
			await FileTools.Write(context.Message.ControllerPath, controllerCompilationUnitSyntax, compilationUnitSyntaxNew);
			return;
		}
	}
	///<inheritdoc/>
	public class ControllerCreateEventConsumerDefinition : ConsumerDefinition<ControllerCreateEventConsumer>
	{
		///<inheritdoc/>
		protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<ControllerCreateEventConsumer> consumerConfigurator)
		{
		}
	}
}