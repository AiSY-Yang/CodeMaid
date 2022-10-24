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
using Microsoft.EntityFrameworkCore.Infrastructure;

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
		string configPath = "C:\\Users\\kai\\source\\Github\\Template.WebApi\\TestDbContext\\Configrations\\";
		//string modelPath = "D:\\Work\\marketDataPlatform\\src\\Core\\Models";
		//string configPath = "D:\\Work\\marketDataPlatform\\src\\Database\\EntityFramework\\EntityTypeConfigurations";

		public async void Task()
		{
			var context = Service.GetRequiredService<MaidContext>();
			var project = context.Projects
				.Include(x => x.Maids)
				.ThenInclude(x => x.Classes)
				.ThenInclude(x => x.Properties)
				.ThenInclude(x => x.Attributes)
				.First();
			var maid = project.Maids.First();
			foreach (var file in Directory.GetFiles(maid.SourcePath, "*.cs", SearchOption.AllDirectories))
			{
				var tree = CSharpSyntaxTree.ParseText(File.ReadAllText(file));
				var root = tree.GetCompilationUnitRoot();
				var classes = GetClassDeclarationSyntaxes(root);

				foreach (var classNode in classes)
				{
					var c = maid.Classes.FirstOrDefault(x => x.Name == classNode.Identifier.ValueText);
					if (c == null)
					{
						c = CreateClassEntity(classNode);
						maid.Classes.Add(c);
						context.Add(c);
					}
					else
					{
						CreateClassEntity(classNode).Adapt(c);
					}
					foreach (var item in classNode.Members)
					{
						var newP = CreatePropertyEntity(c, item);
						var p = c.Properties.FirstOrDefault(x => x.Name == newP.Name);
						if (p == null)
						{
							p = CreatePropertyEntity(c, item);
							c.Properties.Add(p);
							context.Add(p);
						}
						else
						{
							CreatePropertyEntity(c, item).Adapt(p);
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
				.ThenInclude(x => x.Attributes)
				.Where(x => baseClassNameList.Contains(x.Name) || x.Base == null).ToList();
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
			//在生成派生类的配置
			var derivedClassList = context.ClassDefinitions
				.Include(x => x.Properties)
				.ThenInclude(x => x.Attributes)
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

		static CompilationUnitSyntax CreateBaseConfigurationNode(ClassDefinition classDefinition)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine($"using Microsoft.EntityFrameworkCore;");
			stringBuilder.AppendLine($"using Microsoft.EntityFrameworkCore.Metadata.Builders;");
			stringBuilder.AppendLine($"");
			stringBuilder.AppendLine($"using {classDefinition.NameSpace};");
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
			stringBuilder.AppendLine($"using {classDefinition.NameSpace};");
			stringBuilder.AppendLine($"/// <summary>");
			stringBuilder.AppendLine($"/// 派生类的配置");
			stringBuilder.AppendLine($"/// </summary>");
			stringBuilder.AppendLine($"/// <typeparam name=\"Entity\"></typeparam>");
			stringBuilder.AppendLine($"internal class {classDefinition.Name}Configuration : {classDefinition.Base}Configuration<{classDefinition.Name}>");
			stringBuilder.AppendLine($"{{");
			stringBuilder.AppendLine($"\tpublic override void Configure(EntityTypeBuilder<{classDefinition.Name}> builder)");
			stringBuilder.AppendLine($"\t{{");
			stringBuilder.AppendLine($"\t\tbase.Configure(builder);");
			stringBuilder.AppendLine($"\t\tbuilder.HasComment(\"{classDefinition.Summary}\");");
			foreach (var item in classDefinition.Properties)
			{
				if (IsBaseType(item.Type))
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
			catch (Exception ex)
			{
				Log.Error(ex, "基类{name}配置格式错误,重新生成了配置文件", classDefinition.Name);
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
			var classCode = GetClassDeclarationSyntaxes(source).First();
			var methodCode = classCode.ChildNodes().First(x => x is MethodDeclarationSyntax);
			//此处要保留原有块信息引用 以在后面可以替换节点
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
					if (IsBaseType(prop.Type))
					{
						var expression = CreateBuilderStatement(prop, "") + Environment.NewLine;
						if (item.ToFullString() != expression)
						{
							newBlockCode = newBlockCode.ReplaceNode(item, ParseStatement(expression).WithLeadingTrivia(item.GetLeadingTrivia()));
						}
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
						var expression = $"builder.HasComment(\"{classDefinition.Summary}\");" + Environment.NewLine;
						newBlockCode = newBlockCode.ReplaceNode(item, ParseStatement(expression).WithLeadingTrivia(item.GetLeadingTrivia()));
					}
				}
			}
			return source.ReplaceNode(blockCode, newBlockCode);
		}

		/// <summary>
		/// 获取编译单元下的所有类
		/// </summary>
		/// <param name="compilationUnit"></param>
		/// <returns></returns>
		static List<ClassDeclarationSyntax> GetClassDeclarationSyntaxes(CompilationUnitSyntax compilationUnit)
		{
			//自动生成的文件是没有命名空间的
			var classCode = compilationUnit.ChildNodes().Where(x => x is ClassDeclarationSyntax).ToList();
			//如果是原有的文件可能有命名空间 取命名空间下的配置类
			var classCode2 = compilationUnit.ChildNodes()
				.Where(x => x is NamespaceDeclarationSyntax || x is FileScopedNamespaceDeclarationSyntax).SelectMany(x => x.ChildNodes())
					   .Where(x => x is ClassDeclarationSyntax).ToList();
			return classCode.Union(classCode2).Select(x => x as ClassDeclarationSyntax).ToList()!;
		}
		/// <summary>
		/// 生成属性的builder语句
		/// </summary>
		/// <param name="property"></param>
		/// <returns></returns>
		static string CreateBuilderStatement(PropertyDefinition property, string leader = "\t\t")
		{
			StringBuilder stringBuilder = new();
			stringBuilder.Append($"{leader}builder.Property(x => x.{property.Name})");
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
		/// 是否是基础类型
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		static bool IsBaseType(string type)
		{
			return new string[] { "int", "int?" , "short", "short?" , "long", "long?",
				"string", "string?",
				"bool", "bool?",
				"Guid", "Guid?",
				"DateTime", "DateTime?", "DateTimeOffset", "DateTimeOffset?" }.Contains(type);
		}
		/// <summary>
		/// 创建类的实体
		/// </summary>
		/// <param name="nameSpace"></param>
		/// <param name="classDeclaration"></param>
		/// <returns></returns>
		static ClassDefinition CreateClassEntity(ClassDeclarationSyntax classDeclaration)
		{
			return new ClassDefinition()
			{
				NameSpace = (classDeclaration.SyntaxTree.GetRoot().ChildNodes().FirstOrDefault(x => x is NamespaceDeclarationSyntax || x is FileScopedNamespaceDeclarationSyntax) as BaseNamespaceDeclarationSyntax).Name.ToString(),
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
		static PropertyDefinition? CreatePropertyEntity(ClassDefinition owner, MemberDeclarationSyntax member)
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
					Type = propertyDeclaration.Type.ToString(),
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
					attributeDefinition.PropertyDefinition = result;
					result.Attributes.Add(attributeDefinition);
				}

				return result;
			}
			return null;
		}

		/// <summary>
		/// 获取注释
		/// </summary>
		/// <param name="trivias"></param>
		/// <returns></returns>
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
