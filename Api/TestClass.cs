using System.Text;
using System.Text.RegularExpressions;

using MaidContexts;

using MaidReponsitory;

using MassTransit;

using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

using Models.CodeMaid;

using Serilog;

using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Api
{
	class TestClass
	{
		public TestClass(IServiceProvider service)
		{
			Service = service;
		}

		public IServiceProvider Service { get; }
		string modelPath = "C:\\Users\\kai\\source\\Github\\Template.WebApi\\Models.DbContext\\";
		string configPath = "C:\\Users\\kai\\source\\Github\\Template.WebApi\\TestDbContext\\Configrations\\";

		public async void Task()
		{
			var context = Service.GetRequiredService<MaidContext>();
			var nameSpaces = Service.GetRequiredService<NameSpaceDefinitionReponsitory>();
			var classes = Service.GetRequiredService<ClassDefinitionReponsitory>();
			foreach (var file in Directory.GetFiles(modelPath, "*.cs", SearchOption.AllDirectories))
			{
				var tree = CSharpSyntaxTree.ParseText(File.ReadAllText(file));
				var root = tree.GetCompilationUnitRoot();
				foreach (var node in root.Members)
				{
					if (node.IsKind(SyntaxKind.NamespaceDeclaration) || node.IsKind(SyntaxKind.FileScopedNamespaceDeclaration))
					{
						var namespaceDeclaration = (BaseNamespaceDeclarationSyntax)node;
						//是否已经存在解析好的命名空间
						var ns = context.NameSpaceDefinitions.FirstOrDefault(x => x.Name == namespaceDeclaration.Name.ToString());
						if (ns == null)
						{
							ns = await nameSpaces.AddAndSave(new Models.CodeMaid.NameSpaceDefinition() { Name = namespaceDeclaration.Name.ToString() });
						}
						foreach (var node2 in namespaceDeclaration.Members)
						{
							if (node2 is ClassDeclarationSyntax classDeclaration)
							{
								var c = context.ClassDefinitions.Include(x => x.NameSpaceDefinition).FirstOrDefault(x => x.Name == classDeclaration.Identifier.ValueText);
								if (c == null)
								{
									c = CreateClassEntity(ns, classDeclaration);
									await classes.AddAndSave(c);
								}
								foreach (var item in classDeclaration.Members)
								{
									var p = CreatePropertyEntity(ns, c, item);
									if (p != null)
									{
										c.Properties.Add(p);
									}
								}
							}
						}
					}
				}
			}
			//生成配置
			foreach (var baseClassName in context.ClassDefinitions.Select(x => x.Base).Distinct())
			{
				var parentList = context.ClassDefinitions.Where(x => x.Name == baseClassName).ToList();
				foreach (var item in parentList)
				{
					string fileName = Path.Combine(configPath, $"{item.Name}Configuration.cs");
					if (File.Exists(fileName))
					{
						var compilationUnit = CSharpSyntaxTree.ParseText(File.ReadAllText(fileName)).GetCompilationUnitRoot();
						compilationUnit = UpdateBaseConfigurationNode(compilationUnit, item);
						File.WriteAllText(fileName, compilationUnit.ToFullString());
					}
					else
					{
						var compilationUnit = CreateBaseConfigurationNode(item);
						File.WriteAllText(fileName, compilationUnit.ToFullString());
					}
				}
				var childList = context.ClassDefinitions.Where(x => x.Name != baseClassName).ToList();
				foreach (var item in childList)
				{
					string fileName = Path.Combine(configPath, $"{item.Name}Configuration.cs");
					if (File.Exists(fileName))
					{

					}
					else
					{
						var compilationUnit = CreateConfigurationNode(item);
						File.WriteAllText(fileName, compilationUnit.ToFullString());
					}
				}
			}

			Console.WriteLine("--- over");
			await context.SaveChangesAsync();
		}
		/// <summary>
		/// 创建基类的配置
		/// </summary>
		/// <param name="modelNameSpace">模型的命名空间</param>
		/// <param name="classDeclaration">类模型</param>
		/// <returns></returns>
		static CompilationUnitSyntax CreateBaseConfigurationNode(BaseNamespaceDeclarationSyntax modelNameSpace, SimpleBaseTypeSyntax classDeclaration)
		{
			var usingList = new List<UsingDirectiveSyntax>()
			{
			FormatUsing(UsingDirective(modelNameSpace.Name)),
			FormatUsing(UsingDirective(IdentifierName("Microsoft.EntityFrameworkCore.Metadata.Builders")))
			};

			var config = ClassDeclaration(new SyntaxList<AttributeListSyntax>(),
				TokenList(Token(SyntaxKind.WhitespaceTrivia), Token(SyntaxKind.PublicKeyword)),
				Identifier((classDeclaration.Type as IdentifierNameSyntax).Identifier.Text + "Configuration"),
				null,
				   null,
				new SyntaxList<TypeParameterConstraintClauseSyntax>(),
				new SyntaxList<MemberDeclarationSyntax>());
			return CompilationUnit(new SyntaxList<ExternAliasDirectiveSyntax>(),
				new SyntaxList<UsingDirectiveSyntax>(usingList),
				new SyntaxList<AttributeListSyntax>(),
				new SyntaxList<MemberDeclarationSyntax>(config));
		}
		//static CompilationUnitSyntax CreateBaseConfigurationNode(MaidContext context)
		//{
		//	var baseList = context.ClassDefinitions.Select(x => new { x.NameSpaceDefinition.Name, x.Base }).Distinct().ToList();
		//	var usingList = new List<UsingDirectiveSyntax>(baseList.Count + 1);
		//	foreach (var item in baseList)
		//	{
		//		usingList.Add(FormatUsing(UsingDirective(IdentifierName(item.Name + "." + item.Base))));
		//	}
		//	usingList.Add(FormatUsing(UsingDirective(IdentifierName("Microsoft.EntityFrameworkCore.Metadata.Builders"))));

		//	var config = ClassDeclaration(new SyntaxList<AttributeListSyntax>(), new SyntaxTokenList(),
		//Identifier((classDeclaration.Type as IdentifierNameSyntax).Identifier.Text + "Configuration"), null, null,
		//new SyntaxList<TypeParameterConstraintClauseSyntax>(),
		//new SyntaxList<MemberDeclarationSyntax>());
		//	return CompilationUnit(new SyntaxList<ExternAliasDirectiveSyntax>(),
		//		new SyntaxList<UsingDirectiveSyntax>(usingList),
		//		new SyntaxList<AttributeListSyntax>(),
		//		new SyntaxList<MemberDeclarationSyntax>(config));
		//}
		static CompilationUnitSyntax CreateBaseConfigurationNode(ClassDefinition classDefinition)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine($"using Microsoft.EntityFrameworkCore;");
			stringBuilder.AppendLine($"using Microsoft.EntityFrameworkCore.Metadata.Builders;");
			stringBuilder.AppendLine($"");
			stringBuilder.AppendLine($"using {classDefinition.NameSpaceDefinition.Name};");
			stringBuilder.AppendLine($"/// <summary>");
			stringBuilder.AppendLine($"/// 基类的配置");
			stringBuilder.AppendLine($"/// </summary>");
			stringBuilder.AppendLine($"/// <typeparam name=\"Entity\"></typeparam>");
			stringBuilder.AppendLine($"internal abstract class {classDefinition.Name}Configuration<Entity> : IEntityTypeConfiguration<Entity> where Entity : {classDefinition.Name}");
			stringBuilder.AppendLine($"{{");
			stringBuilder.AppendLine($"\tpublic virtual void Configure(EntityTypeBuilder<Entity> builder)");
			stringBuilder.AppendLine($"\t{{");
			foreach (var item in classDefinition.Properties)
			{
				stringBuilder.Append($"\t\tbuilder.Property(x => x.{item.Name})");
				if (item.Summary != null)
					stringBuilder.Append($".HasComment(\"{item.Summary?.Replace("\"", "\\\"")}\")");
				stringBuilder.AppendLine(";");
			}
			stringBuilder.AppendLine($"\t}}");
			stringBuilder.AppendLine($"}}");

			return ParseSyntaxTree(stringBuilder.ToString()).GetCompilationUnitRoot();
			//			var usingList = new List<UsingDirectiveSyntax>()
			//			{
			//			FormatUsing(UsingDirective(IdentifierName(classDefinition.NameSpaceDefinition.Name))),
			//			FormatUsing(UsingDirective(IdentifierName("Microsoft.EntityFrameworkCore.Metadata.Builders")))
			//			};
			//			var modifiers = TokenList();
			//			modifiers = modifiers.Add(Token(SyntaxKind.PublicKeyword).WithTrailingTrivia(ParseTrailingTrivia(" ")));
			//			var members = new SyntaxList<MemberDeclarationSyntax>()
			//			{
			//				MethodDeclaration(new SyntaxList<AttributeListSyntax>(),
			//				Identifier(classDefinition.Name + "Configuration").WithLeadingTrivia(ParseTrailingTrivia(" ")),
			//				PredefinedType(Token(SyntaxKind.VoidKeyword)),
			//null,
			//Identifier("Configure"),
			//TypeParameterList(Token())
			//				)
			//			};
			//			var config = ClassDeclaration(new SyntaxList<AttributeListSyntax>(),
			//				modifiers,
			//			Identifier(classDefinition.Name + "Configuration").WithLeadingTrivia(ParseTrailingTrivia(" ")),
			//			null,
			//			null,
			//			new SyntaxList<TypeParameterConstraintClauseSyntax>(),
			//			members);
			//			CompilationUnitSyntax.
			//			return CompilationUnit(new SyntaxList<ExternAliasDirectiveSyntax>(),
			//				new SyntaxList<UsingDirectiveSyntax>(usingList),
			//				new SyntaxList<AttributeListSyntax>(),
			//				new SyntaxList<MemberDeclarationSyntax>(config));
		}
		static CompilationUnitSyntax CreateConfigurationNode(ClassDefinition classDefinition)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine($"using Microsoft.EntityFrameworkCore;");
			stringBuilder.AppendLine($"using Microsoft.EntityFrameworkCore.Metadata.Builders;");
			stringBuilder.AppendLine($"");
			stringBuilder.AppendLine($"using {classDefinition.NameSpaceDefinition.Name};");
			stringBuilder.AppendLine($"/// <summary>");
			stringBuilder.AppendLine($"/// 派生类的配置");
			stringBuilder.AppendLine($"/// </summary>");
			stringBuilder.AppendLine($"/// <typeparam name=\"Entity\"></typeparam>");
			stringBuilder.AppendLine($"internal abstract class {classDefinition.Name}Configuration : {classDefinition.Base}Configuration<{classDefinition.Name}>");
			stringBuilder.AppendLine($"{{");
			stringBuilder.AppendLine($"\tpublic override void Configure(EntityTypeBuilder<{classDefinition.Name}> builder)");
			stringBuilder.AppendLine($"\t{{");
			stringBuilder.AppendLine($"\t\tbase.Configure(builder);");
			foreach (var item in classDefinition.Properties)
			{
				stringBuilder.Append($"\t\tbuilder.Property(x => x.{item.Name})");
				if (item.Summary != null)
					stringBuilder.Append($".HasComment(\"{item.Summary?.Replace("\"", "\\\"")}\")");
				stringBuilder.AppendLine(";");
			}
			stringBuilder.AppendLine($"\t}}");
			stringBuilder.AppendLine($"}}");

			return ParseSyntaxTree(stringBuilder.ToString()).GetCompilationUnitRoot();
		}

		static CompilationUnitSyntax UpdateBaseConfigurationNode(CompilationUnitSyntax source, ClassDefinition classDefinition)
		{
			try
			{
			}
			catch (Exception)
			{
				Log.Error("基类{name}配置格式错误,重新生成了配置文件", classDefinition.Name);
				source = CreateBaseConfigurationNode(classDefinition);
			}
			return source;
		}

		/// <summary>
		/// 创建类的实体
		/// </summary>
		/// <param name="nameSpace"></param>
		/// <param name="classDeclaration"></param>
		/// <returns></returns>
		static ClassDefinition CreateClassEntity(NameSpaceDefinition nameSpace, ClassDeclarationSyntax classDeclaration)
		{
			return new ClassDefinition()
			{
				NameSpaceDefinition = nameSpace,
				Name = classDeclaration.Identifier.Text,
				Base = classDeclaration.BaseList?.Types.ToString(),
			};
		}
		/// <summary>
		/// 创建属性的实体
		/// </summary>
		/// <param name="nameSpace"></param>
		/// <param name="classDeclaration"></param>
		/// <returns></returns>
		static PropertyDefinition? CreatePropertyEntity(NameSpaceDefinition nameSpace, ClassDefinition owner, MemberDeclarationSyntax member)
		{
			if (member is PropertyDeclarationSyntax propertyDeclaration)
			{
				var x = CSharpCompilation.Create(null, new[] { propertyDeclaration.SyntaxTree });
				var leaderTrivia = propertyDeclaration.GetLeadingTrivia();
				StringBuilder stringBuilder = new StringBuilder();
				foreach (var item in leaderTrivia)
				{
					foreach (var line in item.ToFullString().Split('\n', StringSplitOptions.RemoveEmptyEntries))
					{
						var match = Regex.Match(line.Trim(), "///(.*)");
						if (match.Success)
						{
							var comment = match.Groups[1].Value.Trim();
							if (comment != "<summary>" && comment != "</summary>")
							{
								stringBuilder.Append(comment);
							}
						}
					}
				}
				return new PropertyDefinition()
				{
					ClassDefinition = owner,
					FullText = propertyDeclaration.ToString(),
					LeadingTrivia = leaderTrivia.ToFullString(),
					Summary = stringBuilder.ToString(),
					Name = propertyDeclaration.Identifier.Text,
					Modifiers = propertyDeclaration.Modifiers.ToString(),
					Initializer = propertyDeclaration.Initializer?.ToString(),
					//Base = classDeclaration.BaseList?.Types.ToString(),
				};
			}
			return null;
		}
		/// <summary>
		/// 格式化Using
		/// </summary>
		/// <param name="usingDirective"></param>
		/// <returns></returns>
		static UsingDirectiveSyntax FormatUsing(UsingDirectiveSyntax usingDirective)
		{
			return UsingDirective(usingDirective.Name.WithLeadingTrivia(Whitespace(" ")).WithTrailingTrivia(Whitespace(""))).WithTrailingTrivia(EndOfLine("\r\n"));
		}
	}
}
