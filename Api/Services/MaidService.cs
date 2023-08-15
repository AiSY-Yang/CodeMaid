using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;

using Api.Extensions;
using Api.Tools;

using ExtensionMethods;

using Humanizer;

using Mapster;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using Models.CodeMaid;

using Serilog;

using ServicesModels.Settings;

using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static ServicesModels.Settings.DtoSyncSetting;

namespace Api.Services
{
	/// <summary>
	/// maid服务
	/// </summary>
	public class MaidService
	{
		class UpdateResult
		{
			public required HashSet<string> ClassList;
			public required HashSet<string> EnumList;
		}

		static readonly HashSet<long> olds = new();

		/// <summary>
		/// 更新maid
		/// </summary>
		/// <param name="maid"></param>
		/// <param name="path">变动的文件路径</param>
		/// <param name="isDelete">是否是删除了文件</param>
		/// <returns></returns>
		public static async Task Update(Maid maid, string path, bool isDelete)
		{
			if (!olds.Contains(maid.Id))
			{
				Log.Information("maid{id}在程序启动后第一次更新,全量更新数据", maid.Id);
				await Update(maid);
				olds.Add(maid.Id);
			}
			else
			{
				if (isDelete)
				{
					Log.Information("文件{path}删除,全量更新数据", path);
					await Update(maid);
				}
				else
				{
					Log.Information("文件{path}改变,重新读取数据", path);
					await Update(maid, path);
				}
			}
		}
		/// <summary>
		/// 对maid全量更新
		/// </summary>
		/// <param name="maid"></param>
		/// <returns></returns>
		static async Task<Maid> Update(Maid maid)
		{
			var files = Directory.GetFiles(Path.Combine(maid.Project.Path, maid.SourcePath), "*.cs", SearchOption.AllDirectories);
			var result = new UpdateResult()
			{
				ClassList = new HashSet<string>(),
				EnumList = new HashSet<string>()
			};
			foreach (var file in files)
			{
				var list = await Update(maid, file);
				foreach (var item in list.ClassList)
				{
					result.ClassList.Add(item);
				}
				foreach (var item in list.EnumList)
				{
					result.EnumList.Add(item);
				}
			}
			//如果本次没有这个类则删除它
			maid.Classes.Where(x => !result.ClassList.Contains(x.Name)).ToList().ForEach(x => x.IsDeleted = true);
			maid.Enums.Where(x => !result.EnumList.Contains(x.Name)).ToList().ForEach(x => x.IsDeleted = true);
			return maid;
		}
		/// <summary>
		/// 指定文件更新maid信息
		/// </summary>
		/// <param name="maid"></param>
		/// <param name="path">文件路径</param>
		/// <returns></returns>
		static async Task<UpdateResult> Update(Maid maid, string path)
		{
			var result = new UpdateResult()
			{
				ClassList = new HashSet<string>(),
				EnumList = new HashSet<string>()
			};
			var tree = CSharpSyntaxTree.ParseText(await File.ReadAllTextAsync(path));
			var compilationUnit = tree.GetCompilationUnitRoot();
			//记录下文件的using
			var usingText = compilationUnit.Usings.ToFullString();
			#region 更新枚举
			var enums = compilationUnit.GetDeclarationSyntaxes<EnumDeclarationSyntax>();
			foreach (var enumNode in enums)
			{
				var e = maid.Enums.FirstOrDefault(x => x.Name == enumNode.Identifier.ValueText);
				if (e == null)
				{
					e = CreateEnumEntity(enumNode);
					maid.Enums.Add(e);
				}
				else
				{
					CreateEnumEntity(enumNode).Adapt(e);
				}
				//记录这个枚举现在有的成员
				HashSet<string> MemberList = new();
				//记录上次枚举的成员值到了多少 如果没有值的话上次值+1则为新的值
				int lastValue = -1;
				foreach (var enumMemberDeclaration in enumNode.Members.OfType<EnumMemberDeclarationSyntax>())
				{
					var p = e.EnumMembers.FirstOrDefault(x => x.Name == enumMemberDeclaration.Identifier.Text);
					if (p == null)
					{
						p = CreateEnumMemberEntity(enumMemberDeclaration, ref lastValue);
						e.EnumMembers.Add(p);
					}
					else
					{
						p = CreateEnumMemberEntity(enumMemberDeclaration, ref lastValue).Adapt(p);
					}
					MemberList.Add(p.Name);
				}
				//本次如果没有这个成员的话 则标记删除
				e.EnumMembers.Where(x => !MemberList.Contains(x.Name)).ToList().ForEach(x => x.IsDeleted = true);
				result.EnumList.Add(e.Name);
			}
			//如果需要给枚举增加remark信息的话 在更新的时候就加上
			if (maid.Project.AddEnumRemark)
			{
				var compilationUnitNew = compilationUnit.ReplaceNodes(enums, (x, y) =>
					  {
						  var remark = maid.Enums.FirstOrDefault(e => e.Name == x.Identifier.ValueText)?.Remark;
						  if (remark is not null) return x.AddOrReplaceRemark(remark);
						  else return x;
					  });
				await FileTools.Write(path, compilationUnit, compilationUnitNew);
			}
			#endregion
			#region 更新类
			List<TypeDeclarationSyntax> classes = compilationUnit.GetDeclarationSyntaxes<TypeDeclarationSyntax>();
			foreach (var classNode in classes)
			{
				var c = maid.Classes.FirstOrDefault(x => x.Name == classNode.Identifier.ValueText);
				if (c == null)
				{
					c = CreateClassEntity(classNode);
					maid.Classes.Add(c);
				}
				else
				{
					CreateClassEntity(classNode).Adapt(c);
				}
				result.ClassList.Add(c.Name);
				c.Using = usingText;
				//记录这个类现在有的属性
				HashSet<string> Properties = new();
				foreach (var propertyDeclaration in classNode.Members.OfType<PropertyDeclarationSyntax>())
				{
					var p = c.Properties.FirstOrDefault(x => x.Name == propertyDeclaration.Identifier.Text);
					if (p == null)
					{
						p = CreatePropertyEntity(c, propertyDeclaration);
						c.Properties.Add(p);
					}
					else
					{
						var pNew = CreatePropertyEntity(c, propertyDeclaration);
						pNew.Adapt(p);
						//删除所有已删除的特性
						p.Attributes.RemoveAll(x => !pNew.Attributes.Select(x => x.Name).Contains(x.Name));
						foreach (var attributeNew in pNew.Attributes)
						{
							var attr = p.Attributes.FirstOrDefault();
							if (attr is null)
							{
								p.Attributes.Add(attributeNew);
							}
							else
							{
								attributeNew.Adapt(attr);
							}
						}
					}
					p.ClassDefinitionId = c.Id;
					var e = maid.Enums.FirstOrDefault(x => !x.IsDeleted && x.Name == p.Type.TrimEnd('?'));
					if (e is not null)
					{
						p.IsEnum = true;
						p.EnumDefinition = e;
					}
					else
					{
						p.IsEnum = false;
						p.EnumDefinition = null;
					}
					Properties.Add(p.Name);
				}
				//本次如果没有这个属性的话 则标记删除
				c.Properties.Where(x => !Properties.Contains(x.Name)).ToList().ForEach(x => x.IsDeleted = true);
			}
			#endregion
			return result;
		}

		/// <summary>
		/// 执行Maid工作
		/// </summary>
		/// <param name="maid"></param>
		/// <returns></returns>
		public static async Task Work(Maid maid)
		{
			switch (maid.MaidWork)
			{
				case Models.CodeMaid.MaidWork.ConfigurationSync:
					await UpdateConfiguration(maid);
					break;
				case Models.CodeMaid.MaidWork.DtoSync:
					await UpdateDto(maid);
					break;
				case Models.CodeMaid.MaidWork.MasstransitConsumerSync:
					await MasstransitConsumerSync(maid);
					break;
				default:
					break;
			}
		}
		public static async Task MasstransitConsumerSync(Maid maid)
		{
			var setting = maid.Setting.Deserialize<ConfigurationSyncSetting>() ?? new ConfigurationSyncSetting();
			foreach (var classDefinition in maid.Classes)
			{
				var destinationPath = Path.Combine(maid.Project.Path, maid.DestinationPath, $"{classDefinition.Name}Consumer.cs");
				CompilationUnitSyntax compilationUnit;
				if (File.Exists(destinationPath))
					compilationUnit = CSharpSyntaxTree.ParseText(File.ReadAllText(destinationPath)).GetCompilationUnitRoot();
				else
				{
					compilationUnit = CSharpSyntaxTree.ParseText($$"""
						using MassTransit;

						using {{classDefinition.NameSpace}};

						using Microsoft.EntityFrameworkCore;

						namespace Api.MasstransitConsumer
						{
							public class {{classDefinition.Name}}Consumer : IConsumer<{{classDefinition.Name}}>
							{
								private readonly ILogger<{{classDefinition.Name}}Consumer> logger;

								public {{classDefinition.Name}}Consumer(ILogger<{{classDefinition.Name}}Consumer> logger)
								{
									this.logger = logger;
								}

								public async Task Consume(ConsumeContext<{{classDefinition.Name}}> context)
								{
								}
							}
							///<inheritdoc/>
							public class {{classDefinition.Name}}ConsumerDefinition : ConsumerDefinition<{{classDefinition.Name}}Consumer>
							{
								///<inheritdoc/>
								protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<{{classDefinition.Name}}Consumer> consumerConfigurator)
								{
								}
							}
						}
						""").GetCompilationUnitRoot();
				}
				CompilationUnitSyntax compilationUnitNew = compilationUnit;
				compilationUnitNew = compilationUnitNew.ReplaceNodes(compilationUnitNew.GetDeclarationSyntaxes<ClassDeclarationSyntax>(), (node, node2) =>
				{
					if (node.Identifier.Text.EndsWith("Consumer"))
					{
						return node2.AddOrReplaceSummary(classDefinition.Summary);
					}
					return node;
				});
				await FileTools.Write(destinationPath, compilationUnit, compilationUnitNew);
			}
		}


		#region Configuration
		/// <summary>
		/// 更新配置
		/// </summary>
		/// <param name="maid"></param>
		/// <returns></returns>
		public static async Task UpdateConfiguration(Maid maid)
		{
			//更新配置文件
			foreach (var item in maid.Classes)
			{
				string fileName = Path.Combine(maid.Project.Path, maid.DestinationPath, $"{item.Name}Configuration.cs");
				//当类被删除的时候 配置也一起删除掉
				if (item.IsDeleted)
				{
					if (File.Exists(fileName))
					{
						File.Delete(fileName);
					}
				}
				else
				{
					if (File.Exists(fileName))
					{
						var compilationUnit = CSharpSyntaxTree.ParseText(File.ReadAllText(fileName)).GetCompilationUnitRoot();
						var compilationUnitNew = UpdateConfigurationNode(compilationUnit, item);
						await FileTools.Write(fileName, compilationUnit, compilationUnitNew);
					}
					else
					{
						var compilationUnit = CreateConfigurationNode(item);
						await File.WriteAllTextAsync(fileName, compilationUnit.ToFullString());
					}
				}
			}

			//更新Context文件里的类
			var setting = maid.Setting.Deserialize<ConfigurationSyncSetting>() ?? new ConfigurationSyncSetting();
			if (setting.ContextPath != null)
			{
				string fileName = Path.Combine(maid.Project.Path, setting.ContextPath);
				var compilationUnit = CSharpSyntaxTree.ParseText(File.ReadAllText(fileName)).GetCompilationUnitRoot();
				//取出第一个类 要求第一个类必须就是dbContext
				var firstClass = compilationUnit.GetDeclarationSyntaxes<ClassDeclarationSyntax>().First();
				//做个用于修改的镜像
				var newClass = firstClass;
				foreach (var item in maid.Classes)
				{
					//如果类被删除或者改成了抽象类 则从context中移除 否则插入或者更新
					if (item.IsDeleted || item.IsAbstract)
					{
						newClass = newClass.RemovePropertyByType($"DbSet<{item.Name}>");
					}
					else
					{
						//取出最后一个属性 在之后做新属性的插入
						var lastProperty = newClass.Members.Where(x => x is PropertyDeclarationSyntax).LastOrDefault();
						//如果一个属性都没有 则插入到最后一个构造函数后面
						lastProperty ??= newClass.Members.Where(x => x is ConstructorDeclarationSyntax).LastOrDefault();
						//如果构造函数也没有 则插入到第一个方法后面
						lastProperty ??= newClass.Members.Where(x => x is MethodDeclarationSyntax).First();

						var prop = newClass.Members.OfType<PropertyDeclarationSyntax>().Where(x => x.Type.ToString() == $"DbSet<{item.Name}>").FirstOrDefault();
						if (prop is null)
						{
							newClass = newClass.InsertNodesAfter(lastProperty, new SyntaxNode[] {
								(PropertyDeclarationSyntax)ParseMemberDeclaration($"{(item.LeadingTrivia==null?"\t":string.Join('\n',item.LeadingTrivia.Split('\n').Select(x=>"\t"+x)))}public virtual DbSet<{item.Name}> {item.Name.Pluralize(inputIsKnownToBeSingular: false)} {{ get; set; }} = null!;")!
								.WithTrailingTrivia(ParseTrailingTrivia(Environment.NewLine))! });
						}
						else
						{
							newClass = newClass.ReplaceNode(prop, prop.AddOrReplaceSummary(item.Summary));
						}
					}
				}
				var compilationUnitNew = compilationUnit.ReplaceNode(firstClass, newClass);
				await FileTools.Write(fileName, compilationUnit, compilationUnitNew);
			}
		}


		/// <summary>
		/// 生成配置节点
		/// </summary>
		/// <param name="classDefinition"></param>
		/// <returns></returns>
		private static CompilationUnitSyntax CreateConfigurationNode(ClassDefinition classDefinition)
		{
			bool isAbstract = classDefinition.IsAbstract;
			bool hasBase = classDefinition.Base is not null;
			StringBuilder stringBuilder = new();
			stringBuilder.AppendLine($$""""
				using Microsoft.EntityFrameworkCore;
				using Microsoft.EntityFrameworkCore.Metadata.Builders;

				using {{classDefinition.NameSpace}};
				/// <summary>
				/// {{(isAbstract ? "基类的配置" : "派生类的配置")}}
				/// </summary>
				{{(
				//当没有基类的时候 直接实现接口
				!hasBase ? $"internal abstract class {classDefinition.Name}Configuration<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : {classDefinition.Name}"
				//当为抽象类的时候 说明是其他类的基类 配置也要是抽象类
				: isAbstract ? $"internal abstract class {classDefinition.Name}Configuration<TEntity> : {classDefinition.Base}Configuration<TEntity> where TEntity : {classDefinition.Name}"
				: $"internal class {classDefinition.Name}Configuration : {classDefinition.Base}Configuration<{classDefinition.Name}>"
				)}}
				{
					{{(!hasBase ? "public virtual void Configure(EntityTypeBuilder<TEntity> builder)"
					: isAbstract ? $"public override void Configure(EntityTypeBuilder<TEntity> builder)"
					: $"public override void Configure(EntityTypeBuilder<{classDefinition.Name}> builder)"
					)}}
					{
				"""");
			//当有基类的时候要调用基类的方法
			if (hasBase) stringBuilder.AppendLine($"\t\tbase.Configure(builder);");
			//当不是抽象类的时候要设置表名
			if (!isAbstract) stringBuilder.AppendLine($"\t\tbuilder.Metadata.SetComment(\"{classDefinition.Summary}\");");
			//设置属性
			foreach (var item in classDefinition.Properties)
			{
				if (!item.IsDeleted && CanBeMapToDataBase(item))
					stringBuilder.AppendLine(CreateBuilderStatement(item));
			}
			stringBuilder.AppendLine($"\t}}");
			stringBuilder.AppendLine($"}}");

			return ParseSyntaxTree(stringBuilder.ToString()).GetCompilationUnitRoot();
		}

		/// <summary>
		/// 更新配置节点
		/// </summary>
		/// <param name="source"></param>
		/// <param name="classDefinition"></param>
		/// <returns></returns>
		private static CompilationUnitSyntax UpdateConfigurationNode(CompilationUnitSyntax source, ClassDefinition classDefinition)
		{
			try
			{
				var classNode = source.GetDeclarationSyntaxes<ClassDeclarationSyntax>().First();
				var classNodeNew = classNode;
				if (!classDefinition.IsAbstract)
				{
					//当基类改变的时候 要自动更新继承关系
					var baseListString = $": {classDefinition.Base}Configuration<{classDefinition.Name}>";
					if (baseListString != classNode.BaseList!.ToString())
					{
						var baseList = BaseList(SingletonSeparatedList<BaseTypeSyntax>(SimpleBaseType(
						   GenericName(Identifier($"{classDefinition.Base}Configuration"))
						   .WithTypeArgumentList(TypeArgumentList(SingletonSeparatedList<TypeSyntax>(IdentifierName(classDefinition.Name))))
						   )));
						baseList = baseList.ReplaceToken(baseList.ColonToken, baseList.ColonToken.WithTrailingTrivia(classNode.BaseList.ColonToken.TrailingTrivia));
						;
						classNodeNew = classNodeNew.WithBaseList(baseList.WithTrailingTrivia(classNode.BaseList.GetTrailingTrivia()));
					}
				}
				else
				{
					//如果是新修改的抽象类 则同时改变配置文件
					if (!classNode.Modifiers.Any(x => x.Text == "abstract"))
						classNodeNew = classNodeNew.WithModifiers(classNodeNew.Modifiers.Add(ParseToken("abstract ")));
				}
				source = source.ReplaceNode(classNode, UpdateConfigurationNode(classNodeNew, classDefinition));
			}
			catch (Exception ex)
			{
				Log.Error(ex, "类{name}配置格式错误,重新生成了配置文件", classDefinition.Name);
				source = CreateConfigurationNode(classDefinition);
			}
			return source;
		}

		/// <summary>
		/// 更新配置的编译单元
		/// </summary>
		/// <param name="classNode"></param>
		/// <param name="classDefinition"></param>
		/// <returns></returns>
		private static ClassDeclarationSyntax UpdateConfigurationNode(ClassDeclarationSyntax classNode, ClassDefinition classDefinition)
		{
			var methodCode = classNode.ChildNodes().First(x => x is MethodDeclarationSyntax);
			//此处要保留原有块信息引用 以在后面可以替换节点
			var blockCode = methodCode.ChildNodes().First(x => x is BlockSyntax);
			var newBlockCode = (BlockSyntax)blockCode;
			//只有映射到数据库的字段才会更新配置
			foreach (var item in classDefinition.Properties.Where(CanBeMapToDataBase))
			{
				newBlockCode = UpdateOrInsertConfigurationStatement(newBlockCode, item);
			}
			//更新表名注释
			if (classDefinition.Summary is not null)
			{
				var tableNameRegex = new Regex("builder\\.Metadata\\.SetComment\\(\\\"(.*?)\\\"\\)");
				//记录是否匹配上了注释
				var isMatch = false;
				foreach (var item in newBlockCode.ChildNodes().OfType<ExpressionStatementSyntax>())
				{
					var match = tableNameRegex.Match(item.ToFullString());
					if (match.Success && match.Groups.Count == 2)
					{
						isMatch = true;
						string tableName = match.Groups[1].Value;
						if (tableName == classDefinition.Summary)
						{
							continue;
						}
						else
						{
							var expression = $"builder.Metadata.SetComment(\"{classDefinition.Summary}\");" + Environment.NewLine;
							newBlockCode = newBlockCode.ReplaceNode(item, ParseStatement(expression).WithLeadingTrivia(item.GetLeadingTrivia()));
						}
					}
				}
				//如果没匹配上的话就新增
				if (isMatch == false)
				{
					var expression = $"builder.Metadata.SetComment(\"{classDefinition.Summary}\");" + Environment.NewLine;
					newBlockCode = newBlockCode.InsertNodesAfter(newBlockCode.ChildNodes().First(), new SyntaxNode[] { ParseStatement(expression).WithLeadingTrivia(newBlockCode.ChildNodes().First().GetLeadingTrivia().Last()) });
				}
			}
			return classNode.ReplaceNode(blockCode, newBlockCode);
		}

		/// <summary>
		/// 更新或者插入配置文件的语句
		/// </summary>
		/// <param name="blockSyntax"></param>
		/// <param name="property"></param>
		/// <returns></returns>
		private static BlockSyntax UpdateOrInsertConfigurationStatement(BlockSyntax blockSyntax, PropertyDefinition property)
		{
			//更新属性信息
			var propNameRegex = new Regex("Property\\(.*?\\.(.*?)\\)[\\.|;]");
			foreach (var item in blockSyntax.ChildNodes().OfType<ExpressionStatementSyntax>())
			{
				var match = propNameRegex.Match(item.ToFullString());
				if (match.Success && match.Groups.Count == 2 && match.Groups[1].Value == property.Name)
				{
					switch (property.IsDeleted)
					{
						case true:
							return blockSyntax.RemoveNode(item, SyntaxRemoveOptions.KeepNoTrivia)!;
						case false:
							var expression = CreateBuilderStatement(property, "") + Environment.NewLine;
							return blockSyntax.ReplaceNode(item, ParseStatement(expression).WithLeadingTrivia(item.GetLeadingTrivia()));
					}
				}
				continue;
			}
			if (property.IsDeleted) return blockSyntax;
			else return blockSyntax.InsertNodesAfter(blockSyntax.ChildNodes().Last(), new[] { ParseStatement(CreateBuilderStatement(property) + Environment.NewLine) });
		}
		#endregion
		#region Dto
		/// <summary>
		/// 更新DTO的功能
		/// </summary>
		/// <param name="maid"></param>
		/// <returns></returns>
		static async Task UpdateDto(Maid maid)
		{
			var sourcePath = Path.Combine(maid.Project.Path, maid.SourcePath);
			var destinationPath = Path.Combine(maid.Project.Path, maid.DestinationPath);
			//确认目标路径的存在
			if (!Directory.Exists(destinationPath))
				Directory.CreateDirectory(destinationPath);
			//读取设置
			var setting = maid.Setting.Deserialize<DtoSyncSetting>() ?? Default;
			//所有的类都进行更新
			foreach (var c in maid.Classes.Where(x => x.UpdateTime > DateTimeOffset.Now.AddMinutes(-1)))
			{
				if (setting.CreateDirectory)
				{
					//生成Dto的目录
					string dirPath = Path.Combine(destinationPath, c.Name + setting.Suffix);
					if (c.IsDeleted)
					{
						if (Directory.Exists(dirPath))
						{
							Directory.Delete(dirPath, true);
						}
					}
					else
					{
						if (!Directory.Exists(dirPath)) Directory.CreateDirectory(dirPath);
						foreach (var dtoSetting in setting.DtoSyncItemSettings)
						{
							string className = c.Name + dtoSetting.Suffix;
							string fileName = Path.Combine(dirPath, className + ".cs");
							await UpdateDto(fileName, className, c, dtoSetting);
						}
					}
				}
				else
				{
					string fileName = Path.Combine(destinationPath, c.Name + setting.Suffix + ".cs");
					if (c.IsDeleted)
					{
						File.Delete(fileName);
					}
					else
					{
						foreach (var dtoSetting in setting.DtoSyncItemSettings)
						{
							string className = c.Name + dtoSetting.Suffix;
							await UpdateDto(fileName, className, c, dtoSetting);
						}
					}
				}
			}
			#region 同步属性的功能
			foreach (var file in Directory.GetFiles(destinationPath, "*.cs", SearchOption.AllDirectories))
			{
				var compilationUnit = CSharpSyntaxTree.ParseText(await File.ReadAllTextAsync(file)).GetCompilationUnitRoot();
				var cs = compilationUnit.GetDeclarationSyntaxes<ClassDeclarationSyntax>();
				var compilationUnitNew = compilationUnit.ReplaceNodes(cs,
					   (c, r) => c.ReplaceNodes(c.ChildNodes().OfType<PropertyDeclarationSyntax>(),
						(source, reWrite) => { return SyncProperties(c, source, maid); }));
				await FileTools.Write(file, compilationUnit, compilationUnitNew);
			}
			#endregion
		}
		/// <summary>
		/// 同步属性
		/// </summary>
		/// <param name="classDeclaration">需要同步的类</param>
		/// <param name="source">需要同步的属性</param>
		/// <param name="maid">保存好所有关联对象的maid</param>
		/// <returns></returns>
		static PropertyDeclarationSyntax SyncProperties(ClassDeclarationSyntax classDeclaration, PropertyDeclarationSyntax source, Maid maid)
		{
			//先找到这个类对应的数据库中源数据的类
			var c = maid.Classes.Where(x => classDeclaration.Identifier.Text.StartsWith(x.Name)).OrderByDescending(x => x.Name.Length).FirstOrDefault();
			//如果没找到类则不对属性进行更改
			if (c is null) return source;
			//适配Mapster的对象映射
			string propName = source.GetAttributeArguments("AdaptMember")?.Trim('\"') ?? source.Identifier.Text;
			if (propName.StartsWith("nameof") || propName.StartsWith("$"))
			{
				//常见形式
				//[AdaptMember(nameof(ProjectSampleSet.CreateUser) + nameof(ProjectSampleSet.CreateUser.DisplayName))]
				//[AdaptMember($"{nameof(ProjectSampleSet.CreateUser)}{nameof(ProjectSampleSet.CreateUser.DisplayName)}")]
				propName = string.Join("", Regex.Matches(propName, "\\.([_0-9\\w]*)\\)").Select(a => a.Groups[1]).ToList());
			}
			//同步属性的相关内容
			var x = GetClassesFromFlatteningClassName(maid, c, propName);
			if (x.Count == 0) return source;
			//当中间的任意一个属性被删除的时候 就把这个Dto里的属性也删除
			if (x.Any(x => x.IsDeleted == true))
			{
				return null!;
			}
			//同步Attribute
			foreach (var attribute in x.Last().Attributes.Where(x => !x.IsDeleted))
			{
				//个别Dto用不上的属性不添加
				if (new string[] { "Column", "DefaultValue", "NotMapped", "AdaptMember" }.Contains(attribute.Name))
				{
					continue;
				}
				//当原类型有Required Attribute  但是dto里面没有 则不添加此Attribute 常见于查询条件
				if (source.Type.ToString().EndsWith("?") && attribute.Name == "Required")
				{
					continue;
				}
				else
				{
					source = source.AddOrReplaceAttribute(attribute);
				}
			}
			//同步summary
			var summary = string.Join("::", x.Where(x => !x.Summary.IsNullOrWhiteSpace()).Select(x => x.Summary));
			if (summary.IsNullOrWhiteSpace())
			{
				return source;
			}
			//同步Type
			var p = x.Last();
			var type = p.Type;
			if ((IsBaseType(type) || p.IsEnum) && source.Type.ToString().Trim('?') != type.Trim('?'))
				source = source.WithType(ParseTypeName(type + (!type.EndsWith('?') && source.Type.ToString().EndsWith('?') ? "? " : " ")));
			return source.AddOrReplaceSummary(summary);
		}
		/// <summary>
		/// 根据属性名称找到可以组合成这个名称的一组属性
		/// </summary>
		/// <param name="maid">保存好所有关联对象的maid</param>
		/// <param name="c">原来的类</param>
		/// <param name="name">属性的名称</param>
		/// <returns></returns>
		static List<PropertyDefinition> GetClassesFromFlatteningClassName(Maid maid, ClassDefinition c, string name)
		{
			//直接在类本身里寻找这个属性
			var absoluteProp = c.Properties.FirstOrDefault(x => name == x.Name);
			//在基类中找可能的属性 因为如果属于基类的话 则也属于这个类 应该直接返回对应的属性
			var baseProp = maid.Classes.Where(x => x.Name == c.Base).SelectMany(x => x.Properties).FirstOrDefault(x => name == x.Name);
			//如果以上两个任意一个找到的话 取其中未被删除的属性返回 如果都删除的话任意返回一个
			if (absoluteProp != null || baseProp != null)
			{
				return absoluteProp == null ? new List<PropertyDefinition> { baseProp! }
					: baseProp == null ? new List<PropertyDefinition> { absoluteProp }
					: absoluteProp.IsDeleted ? new List<PropertyDefinition> { baseProp }
					: new List<PropertyDefinition> { absoluteProp };
			}

			var res = new List<PropertyDefinition>();
			//在这个类和基类里找可能的属性
			var props = c.Properties.Union(maid.Classes.Where(x => x.Name == c.Base).SelectMany(x => x.Properties))
				.Where(x => name.StartsWith(x.Name))
				.ToList();
			//根据类型查出来对应的类
			var classes = maid.Classes.Join(props, x => x.Name, x => x.Type.TrimEnd('?'), (c, p) => new { c, p })
				.OrderBy(x => x.p.IsDeleted)
				.ToList();
			foreach (var item in classes)
			{
				//去掉类名后当作下一个要查找的名称
				var nextName = name.TrimStart(item.p.Name);
				if (nextName == "")
				{
					res.Add(item.p);
					break;
				}
				else
				{
					var next = GetClassesFromFlatteningClassName(maid, item.c, nextName);
					if (next.Count > 0)
					{
						res.Add(item.p);
						res.AddRange(next);
						break;
					}
				}
			}
			return res;
		}

		/// <summary>
		/// 更新Dto
		/// </summary>
		/// <param name="fileName">文件的路径</param>
		/// <param name="className">类名</param>
		/// <param name="classDefinition">类信息</param>
		/// <param name="setting">设置</param>
		/// <returns></returns>
		static async Task UpdateDto(string fileName, string className, ClassDefinition classDefinition, DtoSyncItemSetting setting)
		{
			CompilationUnitSyntax compilationUnit;
			if (File.Exists(fileName))
				compilationUnit = CSharpSyntaxTree.ParseText(File.ReadAllText(fileName)).GetCompilationUnitRoot();
			else
				compilationUnit = CSharpSyntaxTree.ParseText(@$"using System.ComponentModel.DataAnnotations.Schema;
{classDefinition.Using}using {classDefinition.NameSpace};
namespace Models;
/// <summary>
/// {classDefinition.Summary}
/// </summary>
public class {className}
{{
}}").GetCompilationUnitRoot();

			var c = compilationUnit.GetDeclarationSyntaxes<ClassDeclarationSyntax>().First();
			var newC = c;
			foreach (var item in classDefinition.Properties
				.Where(setting.JustInclude.Count != 0, x => setting.JustInclude.Contains(x.Name))
				//精确的排除属性
				//.Where(x => !setting.ExcludeProperties.Any(s => s == x.Name))
				//模糊匹配的排除属性
				.Where(x => !x.Name.Contains(setting.ExcludeProperties))
				)
			{
				newC = AddOrUpdatePropertyDeclarationSyntax(newC, item);
			}
			var compilationUnitNew = compilationUnit.ReplaceNode(c, newC);
			await FileTools.Write(fileName, compilationUnit, compilationUnitNew);

			ClassDeclarationSyntax AddOrUpdatePropertyDeclarationSyntax(ClassDeclarationSyntax classDeclarationSyntax, PropertyDefinition property)
			{
				var node = classDeclarationSyntax.ChildNodes()
						.OfType<PropertyDeclarationSyntax>()
						.FirstOrDefault(x => x.Identifier.Text == property.Name);
				var newNode = (PropertyDeclarationSyntax)ParseMemberDeclaration(property.FullText)!;
				//如果这个属性被删除的话 则把映射出的对象也删除掉
				if (property.IsDeleted == true)
				{
					if (node != null)
					{
						return classDeclarationSyntax.RemoveNode(node, SyntaxRemoveOptions.KeepNoTrivia)!;
					}
				}
				//是否排除列表元素
				if (setting.ExcludeList)
				{
					if (newNode.Type.ToString().StartsWith("List"))
						return classDeclarationSyntax;
				}
				//是否排除复杂类型
				if (setting.ExcludeComplexTypes)
				{
					if (!IsBaseType(newNode.Type.ToString()))
						return classDeclarationSyntax;
				}
				//是否转换为可空类型
				if (setting.ConvertToNullable)
				{
					if (!newNode.Type.ToString().EndsWith("?"))
						newNode = newNode.WithType(ParseTypeName(property.Type + "?").WithTrailingTrivia(newNode.Type.GetTrailingTrivia()));
					if (newNode.Initializer != null)
					{
						//移除初始化器
						newNode = newNode.RemoveNode(newNode.Initializer, SyntaxRemoveOptions.KeepTrailingTrivia);
						//初始化器后面有个分号 也要去掉
						newNode = newNode!.WithSemicolonToken(Token(newNode.SemicolonToken.LeadingTrivia, SyntaxKind.None, newNode.SemicolonToken.TrailingTrivia));
						//去掉分号会把行尾一起去掉 这里要把行尾恢复
						var leadingTrivia = newNode.SemicolonToken.LeadingTrivia;
						var trailingTrivia = newNode.SemicolonToken.TrailingTrivia;
						SyntaxToken newToken = Token(leadingTrivia, SyntaxKind.None, trailingTrivia);
						bool addNewline = newNode.GetTrailingTrivia().ToString() == " ";
						if (addNewline)
							newNode = newNode.WithTrailingTrivia(Whitespace(Environment.NewLine));
					}
				}
				//新增的属性 原结点不存在这个属性 或者原结点有过这个属性但是被手动删除了
				//判断如果更新时间在一分钟内 则当成新增的属性进行更新
				if (node == null)
				{
#if DEBUG
					if (property.CreateTime < DateTimeOffset.Now.AddMinutes(-2))
#else
					if (property.CreateTime < DateTimeOffset.Now.AddMinutes(-2))
#endif
					{
						return classDeclarationSyntax.AddMembers(newNode);
					}
					else
					{
						return classDeclarationSyntax;
					}
				}
				return classDeclarationSyntax.ReplaceNode(node, newNode);
			}
		}
		#endregion Dto

		/// <summary>
		/// 生成属性的builder语句
		/// </summary>
		/// <param name="property">属性</param>
		/// <param name="leader">前导空白</param>
		/// <returns></returns>
		private static string CreateBuilderStatement(PropertyDefinition property, string leader = "\t\t")
		{
			StringBuilder stringBuilder = new();
			stringBuilder.Append($"{leader}builder.Property(x => x.{property.Name})");
			if (!property.Summary.IsNullOrWhiteSpace() || !property.Remark.IsNullOrWhiteSpace())
				stringBuilder.Append($".HasComment(\"{property.Summary?.Replace("\"", "\\\"")}{(property.EnumDefinition != null ? $"({property.EnumDefinition.Remark})" : "")}\")");
			foreach (var attribute in property.Attributes)
			{
				switch (attribute.Name)
				{
					//如果有人使用工具 有人没有使用工具 会造成修改实体的MaxLength后 配置文件里没有变化 而迁移会以fluentApi为准 会导致长度约束不生效 所以移除MaxLength的支持
					//case "MaxLength":
					//	stringBuilder.Append($".HasMaxLength({attribute.Arguments})");
					//	break;
					//常用于对decimal精度的指示
					case "Column":
						var match = Regex.Match(attribute.Arguments!, "\"(.*?)\\((\\d*),(\\d*)\\)\"");
						stringBuilder.Append($".HasPrecision({match.Groups[2]}, {match.Groups[3]})");
						break;
					case "Unicode":
						stringBuilder.Append($".IsUnicode({attribute.Arguments})");
						break;
					case "DefaultValue":
						stringBuilder.Append($".HasDefaultValue({attribute.Arguments})");
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
		private static bool IsBaseType(string type)
		{
			return new string[] { "int", "int?" , "short", "short?" , "long", "long?","decimal", "decimal?","float", "float?","double", "double?",
				"string", "string?",
				"bool", "bool?",
				"Guid", "Guid?",
				"DateTime", "DateTime?", "DateTimeOffset", "DateTimeOffset?",
				"DateOnly", "DateOnly?", "TimeOnly", "TimeOnly?",
				"JsonDocument","JsonDocument?",
				"JsonElement","JsonElement?",
			}.Contains(type);
		}
		/// <summary>
		/// 是否可以被映射到数据库 postgres支持数组
		/// </summary>
		/// <param name="property"></param>
		/// <returns></returns>
		private static bool CanBeMapToDataBase(PropertyDefinition property)
		{
			return (property.IsEnum || IsBaseType(RemoveGeneric(property.Type)))
				&& property.HasSet
				&& !property.Attributes.Any(x => x.Name == "NotMapped");
			string RemoveGeneric(string type)
			{
				if (type.EndsWith("[]")) return RemoveGeneric(type[0..^2]);
				if (type.StartsWith("List<") && type.EndsWith(">")) return RemoveGeneric(type[5..^1]);
				if (type.StartsWith("IList<") && type.EndsWith(">")) return RemoveGeneric(type[6..^1]);
				if (type.StartsWith("IEnumerable<") && type.EndsWith(">")) return RemoveGeneric(type[12..^1]);
				return type;
			}
		}


		/// <summary>
		/// 类型是否可以有MaxLength限制
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		private static bool IsCanHaveMaxLength(string type)
		{
			return new string[] { "string", "string?", }.Contains(type)
				|| type.StartsWith("List<");
		}

		/// <summary>
		/// 创建类的实体
		/// </summary>
		/// <param name="classDeclaration"></param>
		/// <returns></returns>
		private static ClassDefinition CreateClassEntity(TypeDeclarationSyntax classDeclaration)
		{
			return new ClassDefinition()
			{
				NameSpace = (classDeclaration.SyntaxTree.GetRoot().ChildNodes().FirstOrDefault(x => x is NamespaceDeclarationSyntax || x is FileScopedNamespaceDeclarationSyntax) as BaseNamespaceDeclarationSyntax)?.Name?.ToString(),
				Name = classDeclaration.Identifier.Text,
				Summary = classDeclaration.GetSummary(),
				LeadingTrivia = classDeclaration.GetLeadingTrivia().ToFullString(),
				Modifiers = string.Join(' ', classDeclaration.Modifiers.Select(x => x.Text)),
				Base = classDeclaration.BaseList?.Types.First().ToString(),
				IsDeleted = false,
				MemberType = classDeclaration switch
				{
					ClassDeclarationSyntax => MemberType.ClassDeclarationSyntax,
					InterfaceDeclarationSyntax => MemberType.InterfaceDeclarationSyntax,
					RecordDeclarationSyntax => MemberType.RecordDeclarationSyntax,
					StructDeclarationSyntax => MemberType.StructDeclarationSyntax,
					_ => throw new NotSupportedException("异常的成员类型")
				},
			};
		}
		/// <summary>
		/// 创建枚举的实体
		/// </summary>
		/// <param name="classDeclaration"></param>
		/// <returns></returns>
		private static EnumDefinition CreateEnumEntity(EnumDeclarationSyntax classDeclaration)
		{
			return new EnumDefinition()
			{
				NameSpace = (classDeclaration.SyntaxTree.GetRoot().ChildNodes().FirstOrDefault(x => x is NamespaceDeclarationSyntax || x is FileScopedNamespaceDeclarationSyntax) as BaseNamespaceDeclarationSyntax)?.Name?.ToString(),
				Name = classDeclaration.Identifier.Text,
				Summary = classDeclaration.GetSummary(),
				LeadingTrivia = classDeclaration.GetLeadingTrivia().ToFullString(),
			};
		}

		/// <summary>
		/// 创建属性的实体
		/// </summary>
		/// <param name="owner">拥有这个属性的类</param>
		/// <param name="propertyDeclaration"></param>
		/// <returns></returns>
		private static PropertyDefinition CreatePropertyEntity(ClassDefinition owner, PropertyDeclarationSyntax propertyDeclaration)
		{
			var leaderTrivia = propertyDeclaration.GetLeadingTrivia();
			var get = propertyDeclaration.AccessorList?.Accessors.FirstOrDefault(x => x.IsKind(SyntaxKind.GetAccessorDeclaration));
			var set = propertyDeclaration.AccessorList?.Accessors.FirstOrDefault(x => x.IsKind(SyntaxKind.SetAccessorDeclaration));
			var result = new PropertyDefinition()
			{
				ClassDefinition = owner,
				FullText = propertyDeclaration.ToFullString(),
				LeadingTrivia = leaderTrivia.ToFullString(),
				Summary = propertyDeclaration.GetSummary(),
				Remark = propertyDeclaration.GetRemark(),
				Name = propertyDeclaration.Identifier.Text,
				Type = propertyDeclaration.Type.ToString(),
				Modifiers = propertyDeclaration.Modifiers.ToString(),
				Initializer = propertyDeclaration.Initializer?.ToString(),
				Attributes = new(),
				HasGet = get != null,
				Get = get?.ExpressionBody?.ToFullString(),
				HasSet = set != null,
				Set = set?.ExpressionBody?.ToFullString(),
				IsDeleted = false,
			};

			foreach (var item in propertyDeclaration.AttributeLists)
			{
				var name = item.Attributes[0].Name.ToString();
				if (name == "MaxLength" && !IsCanHaveMaxLength(result.Type))
				{
					Log.Error("类{className}属性{propName}的类型为{type}不能有MaxLength限制", owner.Name, result.Name, result.Type);
					continue;
				}
				AttributeDefinition attributeDefinition = new()
				{
					Name = name,
					Text = item.Attributes[0].ToString(),
					ArgumentsText = item.Attributes[0].ArgumentList?.ToString(),
					Arguments = item.Attributes[0].ArgumentList == null ? null : string.Join(", ", item.Attributes[0].ArgumentList?.Arguments.Select(x => x.ToString())!),
					PropertyDefinition = result
				};
				result.Attributes.Add(attributeDefinition);
			}
			return result;
		}
		/// <summary>
		/// 创建枚举成员的实体
		/// </summary>
		/// <param name="enumMemberDeclarationSyntax"></param>
		/// <param name="lastValue">上一个枚举成员的值</param>
		/// <returns></returns>
		private static EnumMemberDefinition CreateEnumMemberEntity(EnumMemberDeclarationSyntax enumMemberDeclarationSyntax, ref int lastValue)
		{
			//枚举成员是否指定的有值
			if (enumMemberDeclarationSyntax.EqualsValue is null)
			{
				lastValue++;
			}
			else
			{
				lastValue = int.Parse(enumMemberDeclarationSyntax.EqualsValue.Value.ToFullString().Replace("_", ""));
			}
			var description = enumMemberDeclarationSyntax.AttributeLists.FirstOrDefault(x => x.Attributes[0].Name.ToString() == "Description")?.Attributes[0].ArgumentList?.Arguments.ToString().Trim('\"');
			return new EnumMemberDefinition()
			{
				Name = enumMemberDeclarationSyntax.Identifier.Text,
				Value = lastValue,
				Description = description,
				Summary = enumMemberDeclarationSyntax.GetSummary(),
			};
		}

	}
}
