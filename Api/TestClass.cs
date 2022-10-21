using System.Security.Cryptography.Xml;
using System.Text;
using System.Text.RegularExpressions;

using MaidContexts;

using MaidReponsitory;

using Mapster;

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
						var ns = context.NameSpaceDefinitions
							.Include(x => x.Classes)
							.ThenInclude(x => x.Properties)
							.ThenInclude(x => x.Attributes)
							.FirstOrDefault(x => x.Name == namespaceDeclaration.Name.ToString());
						if (ns == null)
						{
							ns = new Models.CodeMaid.NameSpaceDefinition() { Name = namespaceDeclaration.Name.ToString() };
							nameSpaces.Add(ns);
						}
						else
						{
							new Models.CodeMaid.NameSpaceDefinition() { Name = namespaceDeclaration.Name.ToString(), UpdateTime = DateTimeOffset.Now }.Adapt(ns);
						}
						foreach (var node2 in namespaceDeclaration.Members)
						{
							if (node2 is ClassDeclarationSyntax classDeclaration)
							{
								var c = ns.Classes.FirstOrDefault(x => x.Name == classDeclaration.Identifier.ValueText);
								if (c == null)
								{
									c = CreateClassEntity(ns, classDeclaration);
									ns.Classes.Add(c);
									classes.Add(c);
								}
								else
								{
									CreateClassEntity(ns, classDeclaration).Adapt(c);
								}
								foreach (var item in classDeclaration.Members)
								{
									var newP = CreatePropertyEntity(ns, c, item);
									if (newP == null)
									{
										Console.WriteLine();
									}
									var p = c.Properties.FirstOrDefault(x => x.Name == newP.Name);
									if (p == null)
									{
										p = CreatePropertyEntity(ns, c, item);
										c.Properties.Add(p);
										context.Add(p);
									}
									else
									{
										CreatePropertyEntity(ns, c, item).Adapt(p);
									}
								}
							}
						}
					}
				}
			}
			await context.SaveChangesAsync();
			//生成配置
			var baseClassNameList = context.ClassDefinitions.Select(x => x.Base).Distinct();
			//先生成基类的配置
			var baseClassList = context.ClassDefinitions
				.Include(x => x.Properties)
				.Where(x => baseClassNameList.Contains(x.Name)).ToList();
			foreach (var item in baseClassList)
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
			var derivedClassList = context.ClassDefinitions
				.Include(x => x.Properties)
				.Where(x => !baseClassNameList.Contains(x.Name)).ToList();
			foreach (var item in derivedClassList)
			{
				string fileName = Path.Combine(configPath, $"{item.Name}Configuration.cs");
				if (File.Exists(fileName))
				{
					var compilationUnit = CSharpSyntaxTree.ParseText(File.ReadAllText(fileName)).GetCompilationUnitRoot();
					compilationUnit = UpdateDerivedConfigurationNode(compilationUnit, item);
					File.WriteAllText(fileName, compilationUnit.ToFullString());

				}
				else
				{
					var compilationUnit = CreateConfigurationNode(item);
					File.WriteAllText(fileName, compilationUnit.ToFullString());
				}
			}

			Console.WriteLine("--- over");
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
				stringBuilder.AppendLine(CreateBuilderStatement(item));
			}
			stringBuilder.AppendLine($"\t}}");
			stringBuilder.AppendLine($"}}");

			return ParseSyntaxTree(stringBuilder.ToString()).GetCompilationUnitRoot();
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
			stringBuilder.AppendLine($"\t\tbuilder.HasComment(\"{classDefinition.Summary}\");");
			foreach (var item in classDefinition.Properties)
			{
				stringBuilder.AppendLine(CreateBuilderStatement(item));
			}
			stringBuilder.AppendLine($"\t}}");
			stringBuilder.AppendLine($"}}");

			return ParseSyntaxTree(stringBuilder.ToString()).GetCompilationUnitRoot();
		}

		/// <summary>
		/// 使用当前类更新编译单元
		/// </summary>
		/// <param name="source"></param>
		/// <param name="classDefinition"></param>
		/// <returns></returns>
		static CompilationUnitSyntax UpdateBaseConfigurationNode(CompilationUnitSyntax source, ClassDefinition classDefinition)
		{
			try
			{
				source = UpdateConfigurationNode(source, classDefinition);
			}
			catch (Exception)
			{
				Log.Error("基类{name}配置格式错误,重新生成了配置文件", classDefinition.Name);
				source = CreateBaseConfigurationNode(classDefinition);
			}
			return source;
		}
		/// <summary>
		/// 使用当前派生类更新编译单元
		/// </summary>
		/// <param name="source"></param>
		/// <param name="classDefinition"></param>
		/// <returns></returns>
		static CompilationUnitSyntax UpdateDerivedConfigurationNode(CompilationUnitSyntax source, ClassDefinition classDefinition)
		{
			try
			{
				source = UpdateConfigurationNode(source, classDefinition);
			}
			catch (Exception ex)
			{
				Log.Error(ex, "派生类{name}配置格式错误,重新生成了配置文件", classDefinition.Name);
				source = CreateConfigurationNode(classDefinition);
			}
			return source;
		}

		/// <summary>
		/// 更新配置的编译单元
		/// </summary>
		/// <param name="source"></param>
		/// <param name="classDefinition"></param>
		/// <returns></returns>
		static CompilationUnitSyntax UpdateConfigurationNode(CompilationUnitSyntax source, ClassDefinition classDefinition)
		{
			var classCode = source.ChildNodes().First(x => x is ClassDeclarationSyntax);
			var methodCode = classCode.ChildNodes().First(x => x is MethodDeclarationSyntax);
			var blockCode = methodCode.ChildNodes().First(x => x is BlockSyntax);
			var newBlockCode = blockCode;
			//更新属性信息
			var propNameRegex = new Regex("Property\\(.*?\\.(.*?)\\)\\.");
			foreach (var item in newBlockCode.ChildNodes().Where(x => x is ExpressionStatementSyntax))
			{
				var match = propNameRegex.Match(item.ToFullString());
				if (match.Success && match.Groups.Count == 2)
				{
					string propName = match.Groups[1].Value;
					var prop = classDefinition.Properties.First(x => x.Name == propName);
					var expression = CreateBuilderStatement(prop) + Environment.NewLine;
					if (item.ToFullString() != expression)
					{
						newBlockCode = newBlockCode.ReplaceNode(item, ParseStatement(expression));
					}
				}
			}
			//更新表名注释
			var tableNameRegex = new Regex("builder\\.HasComment\\(\\\"(.*?)\\\"\\)");
			foreach (var item in newBlockCode.ChildNodes().Where(x => x is ExpressionStatementSyntax))
			{
				var match = tableNameRegex.Match(item.ToFullString());
				if (match.Success && match.Groups.Count == 2)
				{
					string tableName = match.Groups[1].Value;
					if (tableName == classDefinition.Summary)
					{
						continue;
					}
					else
					{
						var expression = $"\t\tbuilder.HasComment(\"{classDefinition.Summary}\");" + Environment.NewLine;
						newBlockCode = newBlockCode.ReplaceNode(item, ParseStatement(expression));
					}
				}
			}
			return source.ReplaceNode(blockCode, newBlockCode);
		}

		/// <summary>
		/// 生成属性的builder语句
		/// </summary>
		/// <param name="property"></param>
		/// <returns></returns>
		static string CreateBuilderStatement(PropertyDefinition property)
		{
			StringBuilder stringBuilder = new();
			stringBuilder.Append($"\t\tbuilder.Property(x => x.{property.Name})");
			if (property.Summary != null)
				stringBuilder.Append($".HasComment(\"{property.Summary?.Replace("\"", "\\\"")}\")");
			foreach (var attribute in property.Attributes)
			{
				switch (attribute.Name)
				{
					case "MaxLength":
						stringBuilder.Append($".HasMaxLength({attribute.Arguments})");
						break;
					default:
						break;
				}
			}
			stringBuilder.Append(';');
			return stringBuilder.ToString();
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
				Summary = GetSummay(classDeclaration.GetLeadingTrivia()),
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
				//获取注释
				var result = new PropertyDefinition()
				{
					ClassDefinition = owner,
					FullText = propertyDeclaration.ToString(),
					LeadingTrivia = leaderTrivia.ToFullString(),
					Summary = GetSummay(leaderTrivia),
					Name = propertyDeclaration.Identifier.Text,
					Modifiers = propertyDeclaration.Modifiers.ToString(),
					Initializer = propertyDeclaration.Initializer?.ToString(),
					Attributes = new(),
					//Base = classDeclaration.BaseList?.Types.ToString(),
				};

				foreach (var item in member.AttributeLists)
				{
					AttributeDefinition attributeDefinition = new AttributeDefinition()
					{
						Name = item.Attributes[0].Name.ToString(),
						Text = item.Attributes[0].ToString(),
						ArgumentsText = item.Attributes[0].ArgumentList?.ToString(),
						Arguments = item.Attributes[0].ArgumentList == null ? null : string.Join(", ", item.Attributes[0].ArgumentList?.Arguments.Select(x => x.ToString())!),
					};
					result.Attributes.Add(attributeDefinition);
				}

				return result;
			}
			return null;
		}

		static string GetSummay(SyntaxTriviaList trivias)
		{
			//获取注释
			StringBuilder summary = new StringBuilder();
			foreach (var item in trivias)
			{
				foreach (var line in item.ToFullString().Split('\n', StringSplitOptions.RemoveEmptyEntries))
				{
					var match = Regex.Match(line.Trim(), "///(.*)");
					if (match.Success)
					{
						var comment = match.Groups[1].Value.Trim();
						if (comment != "<summary>" && comment != "</summary>")
						{
							summary.Append(comment);
						}
					}
				}
			}
			return summary.ToString();
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
