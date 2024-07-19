using System.Text.Json;
using System.Text.RegularExpressions;

using Api.Extensions;
using Api.Services;
using Api.Tools;

using ExtensionMethods;

using MaidContexts;

using MassTransit;
using MassTransit.Batching;

using MasstransitModels;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.EntityFrameworkCore;

using Models.CodeMaid;

using ServicesModels.Settings;

using FileType = Models.CodeMaid.FileType;

namespace Api.MasstransitConsumer
{
	///<inheritdoc/>
	public class FileChangeEventConsumer : IConsumer<Batch<FileChangeEvent>>
	{
		private readonly ILogger<FileChangeEventConsumer> logger;
		private readonly MaidContext maidContext;
		private static readonly string[] sourceArray = [".git", "bin", "obj", "node_modules", "Migrations"];
		
		///<inheritdoc/>
		public FileChangeEventConsumer(ILogger<FileChangeEventConsumer> logger, MaidContext maidContext)
		{
			this.logger = logger;
			this.maidContext = maidContext;
		}
		///<inheritdoc/>
		public async Task Consume(ConsumeContext<Batch<FileChangeEvent>> context)
		{
			//只保留最新的文件状态 因为VS保存文件的时候会采用删除 重命名 修改的方式
			var messageList = context.Message.Select(x => x.Message).Where(x => x.FilePath.EndsWith(".cs")).Reverse().DistinctBy(x => x.FilePath).ToList();
			var messageGroup = messageList.GroupBy(x => x.ProjectId).Select(x => new { ProjectId = x.Key, Message = x }).ToList();
			foreach (var group in messageGroup)
			{
				if (!MaidService.Projects.TryGetValue(group.ProjectId, out var projectInformation)) throw new Exception("项目属性被锁定,稍后再试");
				var project = projectInformation.Project;
				var projectPath = project.Path;
				//检查是否是正常的更新
				if (File.Exists(Path.Combine(projectPath, ".git", "index.lock")) || File.Exists(Path.Combine(projectPath, ".git", "HEAD.lock")))
				{
					logger.LogInformation("分支切换中,稍后再试");
					await context.Redeliver(TimeSpan.FromSeconds(3));
					return;
				}
				if (!project.GitBranch.IsNullOrEmpty())
				{
					var head = File.ReadLines(Path.Combine(projectPath, ".git", "HEAD")).First();
					if (!Regex.IsMatch(head, project.GitBranch))
					{
						logger.LogInformation("指定分支为{breach},当前分支为{currentBreach},跳过本次执行", project.GitBranch, head);
						continue;
					}
				}
				var maid = await maidContext.Maids
							.AsSplitQuery()
							.Include(x => x.Project)
							.Where(x => x.ProjectId == project.Id)
							.ToListAsync();
				//记录下已有的所有枚举
				var enums = await maidContext.EnumDefinitions.Where(x => x.ProjectDirectoryFile.ProjectId == project.Id).ToListAsync();
				Dictionary<string, DtoSyncSetting> needSyncProperties = new();

				foreach (var message in group.Message)
				{
					var filePath = message.FilePath;
					var dirPath1 = Path.GetDirectoryName(filePath);
					if (dirPath1 is null) continue;
					var dirName = Path.GetFileName(dirPath1);
					if (dirName is null) continue;
					var relativePath = Path.GetRelativePath(project.Path, dirPath1);
					if (IsIgnoreDir(dirName)) continue;
					var projectDirectory = maidContext.ProjectDirectories.FirstOrDefault(x => x.ProjectId == project.Id && x.Path == relativePath && x.Name == dirName);
					if (projectDirectory is null)
					{
						projectDirectory = new ProjectDirectory()
						{
							Project = null!,
							ProjectId = project.Id,
							ProjectDirectoryFiles = new List<ProjectDirectoryFile>(),
							Name = dirName,
							Path = Path.GetRelativePath(project.Path, dirPath1)
						};
						maidContext.Add(projectDirectory);
					}
					var projectFile = maidContext.ProjectDirectoryFiles
						.Include(x => x.EnumDefinitions)
						.Include(x => x.ProjectStructures)
						.ThenInclude(x => x.ClassDefinition)
						.Include(x => x.ProjectStructures)
						.ThenInclude(x => x.PropertyDefinitions)
						.ThenInclude(x => x.Attributes)
						.FirstOrDefault(x => x.ProjectDirectory.Project.Id == message.ProjectId && x.Path == filePath);
					if (message.IsDelete)
					{
						if (projectFile is null) return;
						logger.LogInformation("{FilePath}删除", message.FilePath);
						projectFile.ProjectStructures.Select(x => x.ClassDefinition).ToList()
							.ForEach(x => x.IsDeleted = true);
						projectFile.ProjectStructures.SelectMany(x => x.PropertyDefinitions).ToList()
							.ForEach(x => x.IsDeleted = true);
					}
					else
					{
						logger.LogInformation("{FilePath}更新", message.FilePath);
						var file = new FileInfo(filePath);
						var fileType = file.Extension switch
						{
							".cs" => FileType.CSahrp,
							_ => FileType.Other,
						};
						var f = File.ReadAllLines(file.FullName);
						var linesCount = f.Length;
						DateTimeOffset lastWriteTime = file.LastWriteTime;
						lastWriteTime = lastWriteTime.TruncateNanosecond();
						var commentCount = 0;
						var spaceCount = 0;
						var isAutoGen = false;
						if (projectDirectory.Name == "Migrations" || f.FirstOrDefault() == "// <auto-generated />")
						{
							isAutoGen = true;
						}
						switch (fileType)
						{
							case FileType.Other:
								break;
							case FileType.CSahrp:
								foreach (var line in f)
								{
									var trim = line.Trim();
									if (trim == "" || trim == "{" || trim == "}")
										spaceCount++;
									else if (trim.StartsWith("//"))
										commentCount++;
								}
								break;
							default:
								break;
						}

						if (projectFile is null)
						{
							projectFile = new ProjectDirectoryFile()
							{
								Project = null!,
								ProjectId = project.Id,
								LinesCount = linesCount,
								CommentCount = commentCount,
								SpaceCount = spaceCount,
								ProjectDirectory = projectDirectory,
								FileType = fileType,
								IsAutoGen = isAutoGen,
								Name = file.Name,
								Path = file.FullName,
								LastWriteTime = lastWriteTime,
								IsDeleted = false,
								ProjectStructures = [],
								EnumDefinitions = [],
							};
							maidContext.Add(projectFile);
						}
						else
						{
							projectFile.LinesCount = linesCount;
							projectFile.CommentCount = commentCount;
							projectFile.SpaceCount = spaceCount;
							projectFile.ProjectDirectory = projectDirectory;
							projectFile.FileType = fileType;
							projectFile.IsAutoGen = isAutoGen;
							projectFile.Name = file.Name;
							projectFile.Path = file.FullName;
							projectFile.LastWriteTime = lastWriteTime;
							projectFile.IsDeleted = false;
						};
						await MaidService.Update(projectFile, maidContext, message.FilePath);
					}
					var addClasses = maidContext.ChangeTracker.Entries<ClassDefinition>().Where(x => x.State == EntityState.Added).ToList();
					var deleteClasses = maidContext.ChangeTracker.Entries<ClassDefinition>().Where(x => x.State == EntityState.Modified && x.Entity.IsDeleted == true).ToList();
					var modifiedClasses = maidContext.ChangeTracker.Entries<ClassDefinition>().Where(x => x.State == EntityState.Modified && x.Entity.IsDeleted == false).ToList();

					var properties = maidContext.ChangeTracker.Entries<PropertyDefinition>().ToList();
					var addProperties = properties.Where(x => x.State == EntityState.Added).ToList();
					var deleteProperties = properties.Where(x => x.State == EntityState.Modified && x.Entity.IsDeleted == true).ToList();
					var modifiedProperties = properties.Where(x => x.State == EntityState.Modified && x.Entity.IsDeleted == false).ToList();

					var addEnums = maidContext.ChangeTracker.Entries<EnumDefinition>().Where(x => x.State == EntityState.Added).ToList();
					var modifiedEnums = maidContext.ChangeTracker.Entries<EnumDefinition>().Where(x => x.State == EntityState.Modified && x.Entity.IsDeleted == false).ToList();

					properties.ForEach(x =>
						{
							var e = enums.Where(e => !e.IsDeleted && e.Name == x.Entity.Type).FirstOrDefault()
									?? addEnums.Select(x => x.Entity).Where(e => e.Name == x.Entity.Type).FirstOrDefault();
							if (e is not null)
							{
								x.Entity.IsEnum = true;
								x.Entity.EnumDefinition = e;
							}
						});
					try
					{
						await maidContext.SaveChangesAsync();
					}
					catch (Exception ex)
					{
						logger.LogError(ex, "数据保存失败");
					}
					foreach (var e in addEnums.Union(modifiedEnums))
					{
						await context.Publish(new EnumUpdateEvent() { EnumId = e.Entity.Id, ProjectId = project.Id });
					}
					foreach (var item in maid)
					{
						switch (item.MaidWork)
						{
							case Models.CodeMaid.MaidWork.ConfigurationSync:
								{
									var setting = item.Setting.Deserialize<ConfigurationSyncSetting>();
									if (setting is null) continue;
									var sourcePath = Path.Combine(project.Path, setting.SourceDirectory);
									if (!filePath.StartsWith(sourcePath)) continue;
									var targetPath = Path.Combine(project.Path, setting.TargetDirectory);
									if (!Directory.Exists(targetPath)) Directory.CreateDirectory(targetPath);
									var contextPath = setting.ContextPath is null ? null : Path.Combine(project.Path, setting.ContextPath);
									foreach (var x in deleteClasses)
									{
										if (!x.Entity.Properties.Any(x => x.ProjectDirectoryFile.Path.StartsWith(sourcePath))) continue;
										if (x.Entity.MemberType != MemberType.ClassDeclarationSyntax) continue;
										var classDefinition = x.Entity;
										string fileName = Path.Combine(targetPath, $"{classDefinition.Name}Configuration.cs");
										if (File.Exists(fileName)) File.Delete(fileName);
										if (contextPath is not null) await MaidService.UpdateDataBaseContext(x.Entity, contextPath);
									}
									foreach (var x in addClasses.Union(modifiedClasses).ToList())
									{
										if (!x.Entity.Properties.Any(x => x.ProjectDirectoryFile.Path.StartsWith(sourcePath))) continue;
										if (x.Entity.MemberType != MemberType.ClassDeclarationSyntax) continue;
										if (contextPath is not null) await MaidService.UpdateDataBaseContext(x.Entity, contextPath);
										var classDefinition = x.Entity;
										string fileName = Path.Combine(targetPath, $"{classDefinition.Name}Configuration.cs");
										if (File.Exists(fileName))
										{
											var compilationUnit = CSharpSyntaxTree.ParseText(File.ReadAllText(fileName)).GetCompilationUnitRoot();
											var compilationUnitNew = MaidService.UpdateConfigurationNode(compilationUnit, classDefinition);
											await FileTools.Write(fileName, compilationUnit, compilationUnitNew);
										}
										else
										{
											var compilationUnit = MaidService.CreateConfigurationNode(classDefinition);
											await File.WriteAllTextAsync(fileName, compilationUnit.ToFullString());
										}
									}
									foreach (var x in addProperties.Union(deleteProperties.Union(modifiedProperties)).ToList())
									{
										if (!x.Entity.ProjectDirectoryFile.Path.StartsWith(sourcePath)) continue;
										if (x.Entity.ClassDefinition.MemberType != MemberType.ClassDeclarationSyntax) continue;
										var propertyDefinition = x.Entity;
										string fileName = Path.Combine(targetPath, $"{propertyDefinition.ClassDefinition.Name}Configuration.cs");
										if (File.Exists(fileName))
										{
											var compilationUnit = CSharpSyntaxTree.ParseText(File.ReadAllText(fileName)).GetCompilationUnitRoot();
											var compilationUnitNew = MaidService.UpdateConfigurationNode(compilationUnit, propertyDefinition.ClassDefinition);
											await FileTools.Write(fileName, compilationUnit, compilationUnitNew);
										}
										else
										{
											var compilationUnit = MaidService.CreateConfigurationNode(propertyDefinition.ClassDefinition);
											await File.WriteAllTextAsync(fileName, compilationUnit.ToFullString());
										}
									}
								}
								break;
							case Models.CodeMaid.MaidWork.DtoSync:
								{
									var setting = item.Setting.Deserialize<DtoSyncSetting>();
									if (setting is null) continue;
									var sourcePath = Path.Combine(project.Path, setting.SourceDirectory);
									if (!filePath.StartsWith(sourcePath)) continue;
									var targetPath = Path.Combine(project.Path, setting.TargetDirectory);
									//确认目标路径的存在
									if (!Directory.Exists(targetPath)) Directory.CreateDirectory(targetPath);
									foreach (var x in deleteClasses)
									{
										if (setting.CreateDirectory)
										{
											string dirPath = Path.Combine(targetPath, x.Entity.Name + setting.Suffix);
											if (Directory.Exists(dirPath)) Directory.Delete(dirPath, true);
										}
										else
										{
											string fileName = Path.Combine(targetPath, x.Entity.Name + setting.Suffix + ".cs");
											File.Delete(fileName);
										}
									}
									foreach (var x in addClasses.Union(modifiedClasses).ToList())
									{
										if (setting.CreateDirectory)
										{
											string dirPath = Path.Combine(targetPath, x.Entity.Name + setting.Suffix);
											if (!Directory.Exists(dirPath)) Directory.CreateDirectory(dirPath);
											foreach (var dtoSetting in setting.DtoSyncItemSettings)
											{
												string className = x.Entity.Name + dtoSetting.Suffix;
												string fileName = Path.Combine(dirPath, className + ".cs");
												await MaidService.UpdateDto(fileName, className, x.Entity, dtoSetting);
											}
										}
										else
										{
											string fileName = Path.Combine(targetPath, x.Entity.Name + setting.Suffix + ".cs");
											foreach (var dtoSetting in setting.DtoSyncItemSettings)
											{
												string className = x.Entity.Name + dtoSetting.Suffix;
												await MaidService.UpdateDto(fileName, className, x.Entity, dtoSetting);
											}
										}
									}
									needSyncProperties[targetPath] = setting;
								}
								break;
							case Models.CodeMaid.MaidWork.HttpClientSync:
								break;
							case Models.CodeMaid.MaidWork.ControllerSync:
								break;
							case Models.CodeMaid.MaidWork.MasstransitConsumerSync:
								{
									var setting = item.Setting.Deserialize<MasstransitConsumerSyncSetting>();
									if (setting is null) continue;
									var sourcePath = Path.Combine(projectPath, setting.SourceDirectory);
									if (!filePath.StartsWith(sourcePath)) continue;
									var targetPath = Path.Combine(projectPath, setting.TargetDirectory);
									foreach (var x in deleteClasses)
									{
										if (!x.Entity.Properties.Any(x => x.ProjectDirectoryFile.Path.StartsWith(sourcePath))) continue;
										var classDefinition = x.Entity;
										string fileName = Path.Combine(targetPath, $"{classDefinition.Name}Consumer.cs");
										if (File.Exists(fileName)) File.Delete(fileName);
									}
									foreach (var x in addClasses.Union(modifiedClasses).ToList())
									{
										if (!x.Entity.Properties.Any(x => x.ProjectDirectoryFile.Path.StartsWith(sourcePath))) continue;
										var classDefinition = x.Entity;
										string fileName = Path.Combine(targetPath, $"{classDefinition.Name}Consumer.cs");
										await MaidService.MasstransitConsumerSync(x.Entity, fileName);
									}
								}
								break;
							default:
								break;
						}
					}
				}
				foreach (var item in needSyncProperties)
				{
					var ps = await maidContext.Projects
	.Where(x => x.Id == project.Id)
	.SelectMany(x => x.ProjectDirectories)
	.Where(x => x.Path.StartsWith(item.Value.SourceDirectory))
	.SelectMany(x => x.ProjectDirectoryFiles)
	.SelectMany(x => x.ProjectStructures)
	.SelectMany(x => x.PropertyDefinitions)
	.Include(x => x.ClassDefinition)
	.Include(x => x.Attributes)
	.Distinct()
	.ToListAsync();
					var cs = ps.Select(x => x.ClassDefinition).Distinct().ToList();
					await MaidService.UpdateDto(cs, item.Value, item.Key);
				}
			}
			return;
		}
		public static bool IsIgnoreDir(string dirName) => sourceArray.Contains(dirName);
	}

	///<inheritdoc/>
	public class FileChangeEventConsumerDefinition : ConsumerDefinition<FileChangeEventConsumer>
	{
		///<inheritdoc/>
		protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<FileChangeEventConsumer> consumerConfigurator, IRegistrationContext context)
		{
			endpointConfigurator.ConcurrentMessageLimit = 1;
			//endpointConfigurator
			//	.UseScheduledRedelivery(x => x.Interval(9999, TimeSpan.FromSeconds(1)));
			consumerConfigurator
				.Options<BatchOptions>(options => options.SetMessageLimit(9999)
				.SetConcurrencyLimit(1));
		}
	}
}
