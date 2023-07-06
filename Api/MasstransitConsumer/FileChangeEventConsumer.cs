using System.Text.Json;

using Api.Controllers;
using Api.Extensions;
using Api.Services;
using Api.Tools;

using ExtensionMethods;

using MaidContexts;

using MassTransit;
using MassTransit.ConsumeConfigurators;
using MassTransit.Definition;

using MasstransitModels;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

using ServicesModels.Settings;

using SharpYaml.Tokens;

namespace Api.MasstransitConsumer
{
	public class FileChangeEventConsumer : IConsumer<FileChangeEvent>
	{
		readonly static SyntaxTriviaList space = SyntaxTriviaList.Create(SyntaxFactory.SyntaxTrivia(SyntaxKind.WhitespaceTrivia, " "));
		readonly static SyntaxTriviaList endline = SyntaxTriviaList.Create(SyntaxFactory.SyntaxTrivia(SyntaxKind.EndOfLineTrivia, Environment.NewLine));
		private readonly ILogger<FileChangeEventConsumer> logger;
		private readonly MaidContext maidContext;

		public FileChangeEventConsumer(ILogger<FileChangeEventConsumer> logger, MaidContext maidContext)
		{
			this.logger = logger;
			this.maidContext = maidContext;
		}

		public async Task Consume(ConsumeContext<FileChangeEvent> context)
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
			if (File.Exists(Path.Combine(maid.Project.Path, ".git", "index.lock")) || File.Exists(Path.Combine(maid.Project.Path, ".git", "HEAD.lock")))
			{
				logger.LogInformation("分支切换中,跳过本次执行");
				return;
			}
			if (!maid.Project.GitBranch.IsNullOrEmpty())
			{
				var head = File.ReadLines(Path.Combine(maid.Project.Path, ".git", "HEAD")).First();
				if (!head.EndsWith(maid.Project.GitBranch))
				{
					logger.LogInformation("指定分支为{breach},当前分支为{currentBreach},跳过本次执行", maid.Project.GitBranch, head);
					return;
				}
			}
			if (maid.MaidWork == Models.CodeMaid.MaidWork.ControllerSync)
			{
				var content = await File.ReadAllTextAsync(context.Message.FilePath);
				var setting = maid.Setting.Deserialize<ControllerSetting>() ?? ControllerSetting.Default;
				CompilationUnitSyntax compilationUnitSyntax = CSharpSyntaxTree.ParseText(content).GetCompilationUnitRoot();
				var cs = compilationUnitSyntax.GetDeclarationSyntaxes<ClassDeclarationSyntax>();
				var compilationUnitSyntaxNew = compilationUnitSyntax;
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
										statements.Add(SyntaxFactory.EmptyStatement().WithTrailingTrivia(endline));
										if (parameterTypeString.EndsWith("vo"))
										{
											statements.Add(SyntaxFactory.ParseStatement($"var data=vo.Adapt<{parameterTypeString.Replace("vo", "dto")}>()"));
											servicesParameter = "data";
										}
										statements.Add(SyntaxFactory.ParseStatement($"var res=service.{identity}(data)").WithTrailingTrivia(endline));
										statements.Add(SyntaxFactory.ParseStatement($"return res;").WithTrailingTrivia(endline));
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
											   .WithLeadingTrivia(endline)
											   .WithTrailingTrivia(endline)
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
				await FileTools.Write(context.Message.FilePath, compilationUnitSyntax, compilationUnitSyntaxNew);
				return;
			}
			//检查更新
			await MaidService.Update(maid, context.Message.FilePath, context.Message.IsDelete);
			//如果有变化的话则发布变化事件
			if (maidContext.ChangeTracker.Entries().Where(x => x.State != EntityState.Unchanged).Any())
			{
				await maidContext.SaveChangesAsync();
				await context.Publish(new MaidChangeEvent() { MaidId = maid.Id });
			}
			return;
		}
	}
	public class FileChangeEventConsumerDefinition : ConsumerDefinition<FileChangeEventConsumer>
	{
		protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<FileChangeEventConsumer> consumerConfigurator)
		{
			endpointConfigurator.ConcurrentMessageLimit = 1;
		}
	}
}
