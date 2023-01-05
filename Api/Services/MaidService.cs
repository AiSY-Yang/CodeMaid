using System.Text;
using System.Text.RegularExpressions;

using Api.Tools;

using ExtensionMethods;

using Humanizer;

using Mapster;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using Models.CodeMaid;

using Serilog;

using ServicesModels;

using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static ServicesModels.DtoSyncSetting;

namespace Api.Services
{
	public class MaidService
	{
		/// <summary>
		/// 对miad全量更新
		/// </summary>
		/// <param name="maid"></param>
		/// <returns></returns>
		public static async Task<Maid> Update(Maid maid)
		{
			var files = Directory.GetFiles(Path.Combine(maid.Project.Path, maid.SourcePath), "*.cs", SearchOption.AllDirectories);
			HashSet<string> classList = new();
			foreach (var file in files)
			{
				var list = await Update(maid, file);
				foreach (var item in list)
				{
					classList.Add(item);
				}
			}
			//如果本次没有这个类则删除它
			maid.Classes.Where(x => !classList.Contains(x.Name)).ToList().ForEach(x => x.IsDeleted = true);
			return maid;
		}
		/// <summary>
		/// 指定文件更新maid信息
		/// </summary>
		/// <param name="maid"></param>
		/// <param name="path">文件路径</param>
		/// <returns></returns>
		public static async Task<HashSet<string>> Update(Maid maid, string path)
		{
			var tree = CSharpSyntaxTree.ParseText(File.ReadAllText(path));
			var compilationUnit = tree.GetCompilationUnitRoot();
			//记录下文件的using
			var usingText = compilationUnit.Usings.ToFullString();
			#region 更新枚举
			var enums = GetDeclarationSyntaxes<EnumDeclarationSyntax>(compilationUnit);
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
						CreateEnumMemberEntity(enumMemberDeclaration, ref lastValue).Adapt(p);
					}
					MemberList.Add(p.Name);
				}
				//本次如果没有这个成员的话 则标记删除
				e.EnumMembers.Where(x => !MemberList.Contains(x.Name)).ToList().ForEach(x => x.IsDeleted = true);
			}
			//如果需要给枚举增加remark信息的话 在更新的时候就加上
			if (maid.Project.AddEnumRemark)
			{
				var compilationUnitNew = compilationUnit.ReplaceNodes(enums,
					  (x, y) =>
					  {
						  var remark = maid.Enums.FirstOrDefault(e => e.Name == x.Identifier.ValueText)?.Remark;
						  if (remark is not null) return AddOrReplaceRemark(x, remark);
						  else return x;
					  });
				await FileTools.Write(path, compilationUnit, compilationUnitNew);
			}
			#endregion
			#region 更新类
			var classes = GetDeclarationSyntaxes<ClassDeclarationSyntax>(compilationUnit);
			//记录本次更新的类的名称
			HashSet<string> classList = new();
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
				classList.Add(c.Name);
				c.Using = usingText;
				//记录这个类现在有的属性
				HashSet<string> PropertityList = new();
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
						var newp = CreatePropertyEntity(c, propertyDeclaration);
						newp.Adapt(p);
						//删除所有已删除的属性
						p.Attributes.RemoveAll(x => !newp.Attributes.Select(x => x.Name).Contains(x.Name));
						foreach (var attrNew in newp.Attributes)
						{
							var attr = p.Attributes.FirstOrDefault();
							if (attr is null)
							{
								p.Attributes.Add(attrNew);
							}
							else
							{
								attrNew.Adapt(attr);
							}
						}
					}
					p.IsEnum = maid.Enums.Any(x => x.Name == p.Type);
					PropertityList.Add(p.Name);
				}
				//本次如果没有这个属性的话 则标记删除
				c.Properties.Where(x => !PropertityList.Contains(x.Name)).ToList().ForEach(x => x.IsDeleted = true);
			}
			#endregion
			return classList;
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
				default:
					break;
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
			//获取所有的基类
			var baseClassNameList = maid.Classes.Select(x => x.Base).Distinct();
			//先生成基类的配置
			var baseClassList = maid.Classes
				.Where(x => baseClassNameList.Contains(x.Name) || x.Base == null).ToList();
			foreach (var item in baseClassList)
			{
				string fileName = Path.Combine(maid.Project.Path, maid.DestinationPath, $"{item.Name}Configuration.cs");
				if (File.Exists(fileName))
				{
					var compilationUnit = CSharpSyntaxTree.ParseText(File.ReadAllText(fileName)).GetCompilationUnitRoot();
					var compilationUnitNew = UpdateBaseConfigurationNode(compilationUnit, item);
					await FileTools.Write(fileName, compilationUnit, compilationUnitNew);
				}
				else
				{
					var compilationUnit = CreateBaseConfigurationNode(item);
					await File.WriteAllTextAsync(fileName, compilationUnit.ToFullString());
				}
			}
			//再生成派生类的配置
			var derivedClassList = maid.Classes
				.Where(x => !baseClassNameList.Contains(x.Name)).ToList();
			foreach (var item in derivedClassList)
			{
				string fileName = Path.Combine(maid.Project.Path, maid.DestinationPath, $"{item.Name}Configuration.cs");
				if (File.Exists(fileName))
				{
					var compilationUnit = CSharpSyntaxTree.ParseText(File.ReadAllText(fileName)).GetCompilationUnitRoot();
					var compilationUnitNew = UpdateDerivedConfigurationNode(compilationUnit, item);
					await FileTools.Write(fileName, compilationUnit, compilationUnitNew);

				}
				else
				{
					var compilationUnit = CreateDerivedConfigurationNode(item);
					await File.WriteAllTextAsync(fileName, compilationUnit.ToFullString());
				}
			}
			//更新Context文件里的类
			if (maid.Setting != null)
			{
				var setting = maid.Setting.AsJsonToObject<ConfigurationSyncSetting>() ?? new ConfigurationSyncSetting();
				if (setting.ContextPath != null)
				{
					string fileName = Path.Combine(maid.Project.Path, setting.ContextPath);
					var compilationUnit = CSharpSyntaxTree.ParseText(File.ReadAllText(fileName)).GetCompilationUnitRoot();
					//取出第一个类 要求第一个类必须就是dbcontext
					var firstClass = GetDeclarationSyntaxes<ClassDeclarationSyntax>(compilationUnit).First();
					//做个用于修改的镜像
					var newClass = firstClass;
					foreach (var item in derivedClassList)
					{
						//取出最后一个属性 在之后做新属性的插入
						var lastPropertity = newClass.Members.Where(x => x is PropertyDeclarationSyntax).LastOrDefault();
						//如果一个属性都没有 则插入到最后一个构造函数后面
						lastPropertity ??= newClass.Members.Where(x => x is ConstructorDeclarationSyntax).LastOrDefault();
						//如果构造函数也没有 则插入到第一个方法后面
						lastPropertity ??= newClass.Members.Where(x => x is MethodDeclarationSyntax).First();

						var prop = newClass.Members.OfType<PropertyDeclarationSyntax>().Where(x => x.Type.ToString() == $"DbSet<{item.Name}>").FirstOrDefault();
						if (prop is null)
						{
							newClass = newClass.InsertNodesAfter(lastPropertity, new SyntaxNode[] {
								(PropertyDeclarationSyntax)ParseMemberDeclaration($"{(item.LeadingTrivia==null?"\t":string.Join('\n',item.LeadingTrivia.Split('\n').Select(x=>"\t"+x)))}public virtual DbSet<{item.Name}> {item.Name.Pluralize(inputIsKnownToBeSingular: false)} {{ get; set; }} = null!;")!
								.WithTrailingTrivia(ParseTrailingTrivia(Environment.NewLine))! });
						}
						else
						{
							newClass = newClass.ReplaceNode(prop, AddOrReplaceSummary(prop, item.Summary));
						}
					}
					var compilationUnitNew = compilationUnit.ReplaceNode(firstClass, newClass);
					await FileTools.Write(fileName, compilationUnit, compilationUnitNew);
				}
			}
		}


		/// <summary>
		/// 生成基类的配置
		/// </summary>
		/// <param name="classDefinition"></param>
		/// <returns></returns>
		private static CompilationUnitSyntax CreateBaseConfigurationNode(ClassDefinition classDefinition)
		{
			StringBuilder stringBuilder = new();
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
		/// <summary>
		/// 生成派生类的配置
		/// </summary>
		/// <param name="classDefinition"></param>
		/// <returns></returns>
		private static CompilationUnitSyntax CreateDerivedConfigurationNode(ClassDefinition classDefinition)
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
			stringBuilder.AppendLine($"\t\tbuilder.Metadata.SetComment(\"{classDefinition.Summary}\");");
			foreach (var item in classDefinition.Properties)
			{
				if ((IsBaseType(item.Type) || item.IsEnum) && item.HasSet)
					stringBuilder.AppendLine(CreateBuilderStatement(item));
			}
			stringBuilder.AppendLine($"\t}}");
			stringBuilder.AppendLine($"}}");

			return ParseSyntaxTree(stringBuilder.ToString()).GetCompilationUnitRoot();
		}

		/// <summary>
		/// 使用当前类更新基类的配置结点
		/// </summary>
		/// <param name="source">源数据</param>
		/// <param name="classDefinition">类</param>
		/// <returns></returns>
		private static CompilationUnitSyntax UpdateBaseConfigurationNode(CompilationUnitSyntax source, ClassDefinition classDefinition)
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
		/// 使用当前派生类更新派生类的配置节点
		/// </summary>
		/// <param name="source"></param>
		/// <param name="classDefinition"></param>
		/// <returns></returns>
		private static CompilationUnitSyntax UpdateDerivedConfigurationNode(CompilationUnitSyntax source, ClassDefinition classDefinition)
		{
			try
			{
				source = UpdateConfigurationNode(source, classDefinition);
			}
			catch (Exception ex)
			{
				Log.Error(ex, "派生类{name}配置格式错误,重新生成了配置文件", classDefinition.Name);
				source = CreateDerivedConfigurationNode(classDefinition);
			}
			return source;
		}

		/// <summary>
		/// 更新配置的编译单元
		/// </summary>
		/// <param name="source"></param>
		/// <param name="classDefinition"></param>
		/// <returns></returns>
		private static CompilationUnitSyntax UpdateConfigurationNode(CompilationUnitSyntax source, ClassDefinition classDefinition)
		{
			var classCode = GetDeclarationSyntaxes<ClassDeclarationSyntax>(source).First();
			var methodCode = classCode.ChildNodes().First(x => x is MethodDeclarationSyntax);
			//此处要保留原有块信息引用 以在后面可以替换节点
			var blockCode = methodCode.ChildNodes().First(x => x is BlockSyntax);
			var newBlockCode = (BlockSyntax)blockCode;
			foreach (var item in classDefinition.Properties.Where(x => (IsBaseType(x.Type) || x.IsEnum) && x.HasSet))
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
			return source.ReplaceNode(blockCode, newBlockCode);
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
			foreach (var item in blockSyntax.ChildNodes().Where(x => x is ExpressionStatementSyntax))
			{
				var match = propNameRegex.Match(item.ToFullString());
				if (match.Success && match.Groups.Count == 2 && match.Groups[1].Value == property.Name)
				{
					var expression = CreateBuilderStatement(property, "") + Environment.NewLine;
					return blockSyntax.ReplaceNode(item, ParseStatement(expression).WithLeadingTrivia(item.GetLeadingTrivia()));
				}
				continue;
			}
			return blockSyntax.InsertNodesAfter(blockSyntax.ChildNodes().Last(), new[] { ParseStatement(CreateBuilderStatement(property) + Environment.NewLine) });
		}
		#endregion
		#region Dto
		static async Task UpdateDto(Maid maid)
		{
			var sourcePath = Path.Combine(maid.Project.Path, maid.SourcePath);
			var destPath = Path.Combine(maid.Project.Path, maid.DestinationPath);
			//确认目标路径的存在
			if (!Directory.Exists(destPath))
				Directory.CreateDirectory(destPath);
			if (maid.Setting != null)
			{
				//读取设置
				DtoSyncSetting settings = maid.Setting!.AsJsonToObject<DtoSyncSetting>() ?? new DtoSyncSetting();
				//所有的类都进行更新
				foreach (var item in maid.Classes)
				{
					//生成Dto的目录
					string dirPath = Path.Combine(destPath, item.Name + settings.DirectorySuffix);
					if (!Directory.Exists(dirPath))
						Directory.CreateDirectory(dirPath);
					foreach (var setting in settings.DtoSyncSettings)
					{
						await UpdateDto(dirPath, item, setting);
					}
				}
			}
			#region 同步属性注释的功能
			foreach (var file in Directory.GetFiles(destPath, "*.cs", SearchOption.AllDirectories))
			{
				var compilationUnit = CSharpSyntaxTree.ParseText(await File.ReadAllTextAsync(file)).GetCompilationUnitRoot();
				var cs = GetDeclarationSyntaxes<ClassDeclarationSyntax>(compilationUnit);
				var compilationUnitNew = compilationUnit.ReplaceNodes(cs,
					   (c, r) => c.ReplaceNodes(c.ChildNodes().OfType<PropertyDeclarationSyntax>(),
						(source, reWrite) => { return SyncPropertity(c, source, maid); }));
				await FileTools.Write(file, compilationUnit, compilationUnitNew);
			}
			#endregion
		}
		/// <summary>
		/// 同步属性
		/// </summary>
		/// <param name="classdeclaration">需要同步的类</param>
		/// <param name="source">需要同步的属性</param>
		/// <param name="maid">保存好所有关联对象的maid</param>
		/// <returns></returns>
		static PropertyDeclarationSyntax SyncPropertity(ClassDeclarationSyntax classdeclaration, PropertyDeclarationSyntax source, Maid maid)
		{
			//先找到这个类对应的数据库中源数据的类
			var c = maid.Classes.Where(x => classdeclaration.Identifier.Text.StartsWith(x.Name)).OrderByDescending(x => x.Name.Length).FirstOrDefault();
			//如果没找到类则不对属性进行更改
			if (c is null) return source;
			//如果这个属性是这个类的属性 则不进行修改
			var prop = c.Properties.FirstOrDefault(x => x.Name == source.Identifier.Text);
			if (prop != null)
			{
				foreach (var attr in prop.Attributes)
				{
					source = AddOrReplaceAttribute(source, attr);
				}
				return source;
			}
			var x = GetClassesFromFlatteningClassName(maid, c, source.Identifier.Text);
			if (x.Count == 0) return source;
			//同步Attribute
			foreach (var attribute in x.Last().Attributes)
			{
				source = AddOrReplaceAttribute(source, attribute);
			}
			//同步summary
			var summary = string.Join("::", x.Where(x => x.Summary.IsNullOrWhiteSpace()).Select(x => x.Summary));
			if (summary.IsNullOrWhiteSpace())
			{
				return source;
			}
			return AddOrReplaceSummary(source, summary);
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
			//直接在类本身里寻找这个属性 如果有的话就返回这个属性
			var absoluteProps = c.Properties.Where(x => name == x.Name).ToList();
			if (absoluteProps.Count > 0)
			{
				return absoluteProps;
			}
			//在基类中找可能的属性 因为如果属于基类的话 则也属于这个类 应该直接返回对应的属性
			var baseProps = maid.Classes.Where(x => x.Name == c.Base).SelectMany(x => x.Properties).Where(x => name.StartsWith(x.Name)).ToList();
			if (baseProps.Count > 0)
			{
				return baseProps;
			}
			var res = new List<PropertyDefinition>();
			//在这个类里找可能的属性
			var props = c.Properties.Where(x => name.StartsWith(x.Name)).ToList();
			//props Room
			//根据类型查出来对应的类
			var classes = maid.Classes.Join(props, x => x.Name, x => x.Type.TrimEnd('?'), (c, p) => new { c, p });
			foreach (var item in classes)
			{
				//去掉类名后当作下一个要查找的名称
				var nextName = name.TrimStart(item.p.Name);
				if (nextName == "")
				{
					res.Add(item.p);
				}
				else
				{
					var next = GetClassesFromFlatteningClassName(maid, item.c, nextName);
					if (next.Count > 0)
					{
						res.Add(item.p);
						res.AddRange(next);
					}
				}
			}
			return res;
		}

		/// <summary>
		/// 更新Dto
		/// </summary>
		/// <param name="path">文件的路径</param>
		/// <param name="classDefinition">类信息</param>
		/// <param name="setting">设置</param>
		/// <returns></returns>
		static async Task UpdateDto(string path, ClassDefinition classDefinition, DtoSyncSettingItem setting)
		{
			string DtoName = classDefinition.Name + setting.Suffix;
			string fileName = Path.Combine(path, DtoName + ".cs");
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
public class {DtoName}
{{
}}").GetCompilationUnitRoot();

			var c = GetDeclarationSyntaxes<ClassDeclarationSyntax>(compilationUnit).First();
			var newC = c;
			foreach (var item in classDefinition.Properties
				.Where(setting.JustInclude.Count != 0, x => setting.JustInclude.Contains(x.Name))
				//精确的排除属性
				//.Where(x => !setting.ExcludePropertity.Any(s => s == x.Name))
				//模糊匹配的排除属性
				.Where(x => !x.Name.Contains(setting.ExcludePropertity))
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
		/// 获取编译单元下的所有指定的类型对象
		/// </summary>
		/// <param name="compilationUnit"></param>
		/// <returns></returns>
		private static List<T> GetDeclarationSyntaxes<T>(CompilationUnitSyntax compilationUnit) where T : BaseTypeDeclarationSyntax
		{
			//没有命名空间的类型
			var declarationSyntax = compilationUnit.ChildNodes().Where(x => x is T).ToList();
			//如果原有的文件有命名空间 取命名空间下的成员
			var declarationSyntax2 = compilationUnit.ChildNodes()
				.Where(x => x is NamespaceDeclarationSyntax || x is FileScopedNamespaceDeclarationSyntax).SelectMany(x => x.ChildNodes())
					   .Where(x => x is T).ToList();
			return declarationSyntax.Union(declarationSyntax2).Select(x => x as T).ToList()!;
		}

		/// <summary>
		/// 生成属性的builder语句
		/// </summary>
		/// <param name="leader">前导空白</param>
		/// <returns></returns>
		private static string CreateBuilderStatement(PropertyDefinition property, string leader = "\t\t")
		{
			StringBuilder stringBuilder = new();
			stringBuilder.Append($"{leader}builder.Property(x => x.{property.Name})");
			if (!property.Summary.IsNullOrWhiteSpace() || !property.Remark.IsNullOrWhiteSpace())
				stringBuilder.Append($".HasComment(\"{property.Summary?.Replace("\"", "\\\"")}{(property.Remark is null ? null : $"({property.Remark})")}\")");
			foreach (var attribute in property.Attributes)
			{
				switch (attribute.Name)
				{
					//如果有人使用工具 有人没有使用工具 会造成修改实体的maxlength后 配置文件里没有变化 而迁移会以fluentApi为准 会导致长度约束不生效 所以移除maxlength的支持
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
			}.Contains(type);
		}

		/// <summary>
		/// 创建类的实体
		/// </summary>
		/// <param name="classDeclaration"></param>
		/// <returns></returns>
		private static ClassDefinition CreateClassEntity(ClassDeclarationSyntax classDeclaration)
		{
			return new ClassDefinition()
			{
				NameSpace = (classDeclaration.SyntaxTree.GetRoot().ChildNodes().FirstOrDefault(x => x is NamespaceDeclarationSyntax || x is FileScopedNamespaceDeclarationSyntax) as BaseNamespaceDeclarationSyntax)?.Name?.ToString(),
				Name = classDeclaration.Identifier.Text,
				Summary = classDeclaration.GetSummay(),
				LeadingTrivia = classDeclaration.GetLeadingTrivia().ToFullString(),
				Base = classDeclaration.BaseList?.Types.ToString(),
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
				Summary = classDeclaration.GetSummay(),
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
				Summary = propertyDeclaration.GetSummay(),
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
			};

			foreach (var item in propertyDeclaration.AttributeLists)
			{
				AttributeDefinition attributeDefinition = new()
				{
					Name = item.Attributes[0].Name.ToString(),
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
				lastValue = int.Parse(enumMemberDeclarationSyntax.EqualsValue.Value.ToFullString());
			}
			var desp = enumMemberDeclarationSyntax.AttributeLists.FirstOrDefault(x => x.Attributes[0].Name.ToString() == "Description")?.Attributes[0].ArgumentList?.Arguments.ToString().Trim('\"');
			return new EnumMemberDefinition()
			{
				Name = enumMemberDeclarationSyntax.Identifier.Text,
				Value = lastValue,
				Description = desp,
				Summary = enumMemberDeclarationSyntax.GetSummay(),
			};
		}

		/// <summary>
		/// 添加或者替换remarks标签
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="typeDeclarationSyntax"></param>
		/// <param name="remark"></param>
		/// <returns></returns>
		private static T AddOrReplaceRemark<T>(T typeDeclarationSyntax, string remark) where T : MemberDeclarationSyntax
		{
			//当原来没有注释的时候不添加新的内容 防止破坏警告信息
			var summary = typeDeclarationSyntax.GetSummay();
			if (summary is null) return typeDeclarationSyntax;
			return AddOrReplaceXmlContent(typeDeclarationSyntax, "remarks", remark);
		}

		/// <summary>
		/// 添加或者替换summary标签
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="typeDeclarationSyntax"></param>
		/// <param name="summary">注释</param>
		/// <returns></returns>
		private static T AddOrReplaceSummary<T>(T typeDeclarationSyntax, string? summary) where T : MemberDeclarationSyntax
		{
			var oldsummary = typeDeclarationSyntax.GetSummay();
			if (oldsummary == summary || summary is null) return typeDeclarationSyntax;
			return AddOrReplaceXmlContent(typeDeclarationSyntax, "summary", summary);
		}
		/// <summary>
		/// 添加或者替换XML节点内容
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="typeDeclarationSyntax"></param>
		/// <param name="tagName"></param>
		/// <param name="text"></param>
		/// <returns></returns>
		private static T AddOrReplaceXmlContent<T>(T typeDeclarationSyntax, string tagName, string text) where T : MemberDeclarationSyntax
		{
			//是否是顶级的元素
			var isTop = true;
			//获取原来的缩进
			var indentation = typeDeclarationSyntax.GetLeadingTrivia().Last().ToString();
			if (indentation.IsNullOrWhiteSpace())
			{
				isTop = false;
			}
			//如果是summary 则内容要修改成多行的格式
			var content = tagName == "summary" ? $"{Environment.NewLine}{indentation}/// {text}{Environment.NewLine}{indentation}/// " : text;
			//拿到这个tag
			var xml = typeDeclarationSyntax.GetLeadingTrivia()
				.Select(x => x.GetStructure())
				.OfType<DocumentationCommentTriviaSyntax>()
				.FirstOrDefault()?.ChildNodes()
				.OfType<XmlElementSyntax>()
				.FirstOrDefault(x => x.StartTag.Name.ToString() == tagName);
			if (xml is null)
			{
				//新增标签 前面要有缩进 后面要有换行 且保留原来的trivia
				var tag = string.Empty;
				if (isTop)
				{
					tag = $"/// <{tagName}>{content}</{tagName}>{Environment.NewLine}";
				}
				else
				{
					tag = $"/// <{tagName}>{content}</{tagName}>{Environment.NewLine}{indentation}";
				}
				return typeDeclarationSyntax.WithLeadingTrivia(
					typeDeclarationSyntax.GetLeadingTrivia()
					.AddRange(SyntaxTriviaList.Create(SyntaxTrivia(SyntaxKind.SingleLineCommentTrivia, tag)))
					);
			}
			else
			{
				//替换标签
				var contentNode = xml.ChildNodes().OfType<XmlTextSyntax>().FirstOrDefault();
				var newContentNode = XmlText(content);
				return typeDeclarationSyntax.ReplaceNode(xml, xml.ReplaceNode(contentNode, newContentNode));
			}
		}
		/// <summary>
		/// 添加或者替换属性
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="typeDeclarationSyntax"></param>
		/// <param name="attribute"></param>
		/// <returns></returns>
		private static T AddOrReplaceAttribute<T>(T typeDeclarationSyntax, AttributeDefinition attribute) where T : MemberDeclarationSyntax
		{
			var attr = typeDeclarationSyntax.AttributeLists.SelectMany(x => x.Attributes).FirstOrDefault(x => x.Name.ToString() == attribute.Name);
			if (attr != null)
			{
				if (attribute.ArgumentsText is null)
				{
					return typeDeclarationSyntax;
				}
				else
				{
					return typeDeclarationSyntax.ReplaceNode(attr, attr.WithArgumentList(SyntaxFactory.ParseAttributeArgumentList(attribute.ArgumentsText)));
				}
			}
			else
			{
				var name = SyntaxFactory.ParseName(attribute.Name);
				var arguments = attribute.ArgumentsText is null ? null : SyntaxFactory.ParseAttributeArgumentList(attribute.ArgumentsText);
				//是否是顶级的元素
				var isTop = true;
				//获取原来的缩进
				var indentation = typeDeclarationSyntax.GetLeadingTrivia().Last().ToString();
				if (indentation.IsNullOrWhiteSpace())
				{
					isTop = false;
				}
				if (isTop)
				{
					return (T)typeDeclarationSyntax.WithoutLeadingTrivia()
						.AddAttributeLists(AttributeList().AddAttributes(SyntaxFactory.Attribute(name, arguments)))
						.WithLeadingTrivia(typeDeclarationSyntax.GetLeadingTrivia())
						;
				}
				else
				{
					//如果不是顶级元素的话 需要在member前面加上缩进和换行
					return (T)typeDeclarationSyntax.WithoutLeadingTrivia()
						.WithLeadingTrivia(SyntaxTrivia(SyntaxKind.EndOfLineTrivia, Environment.NewLine), typeDeclarationSyntax.GetLeadingTrivia().Last())
				.AddAttributeLists(AttributeList().AddAttributes(SyntaxFactory.Attribute(name, arguments)))
				.WithLeadingTrivia(typeDeclarationSyntax.GetLeadingTrivia())
				;
				}
			}

		}
	}
}
