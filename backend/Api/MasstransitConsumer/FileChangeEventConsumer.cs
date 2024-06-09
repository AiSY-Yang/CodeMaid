using System.Text.Json;

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

namespace Api.MasstransitConsumer
{
	///<inheritdoc/>
	public class FileChangeEventConsumer : IConsumer<Batch<FileChangeEvent>>
	{
		private readonly ILogger<FileChangeEventConsumer> logger;
		private readonly MaidContext maidContext;
		///<inheritdoc/>
		public FileChangeEventConsumer(ILogger<FileChangeEventConsumer> logger, MaidContext maidContext)
		{
			this.logger = logger;
			this.maidContext = maidContext;
		}
		///<inheritdoc/>
		public async Task Consume(ConsumeContext<Batch<FileChangeEvent>> context)
		{
			foreach (var eventMessage in context.Message)
			{
				var message = eventMessage.Message;
				if (File.Exists(Path.Combine(message.ProjectPath, ".git", "index.lock")) || File.Exists(Path.Combine(message.ProjectPath, ".git", "HEAD.lock")))
				{
					logger.LogInformation("分支切换中,稍后再试");
					throw new Exception("分支切换中,稍后再试");
				}
				if (!MaidService.Projects.TryGetValue(message.ProjectId, out var project)) throw new Exception("项目属性被锁定,稍后再试");
				if (!project.GitBranch.IsNullOrEmpty())
				{
					var head = File.ReadLines(Path.Combine(project.Path, ".git", "HEAD")).First();
					if (!head.EndsWith(project.GitBranch))
					{
						logger.LogInformation("指定分支为{breach},当前分支为{currentBreach},跳过本次执行", project.GitBranch, head);
						continue;
					}
				}
				var file = maidContext.ProjectDirectoryFiles
					.Include(x => x.EnumDefinitions)
					.Include(x => x.ProjectStructures)
					.ThenInclude(x => x.ClassDefinition)
					.ThenInclude(x => x.Properties)
					.ThenInclude(x => x.Attributes)
					.FirstOrDefault(x => x.ProjectDirectory.Project.Id == message.ProjectId && x.Path == message.FilePath);
				if (message.IsDelete)
				{
					if (file is null) return;
					file.ProjectStructures.Select(x => x.ClassDefinition).ToList()
						.ForEach(x => x.IsDeleted = true);
				}
				else
				{
					logger.LogInformation("{FilePath}更新", message.FilePath);
					if (file is null)
					{
						await context.Publish<ProjectUpdateEvent>(new ProjectUpdateEvent() { ProjectId = project.Id });
						return;
					}
					await MaidService.Update(file, maidContext, message.FilePath);
				}
				var addClasses = maidContext.ChangeTracker.Entries<ClassDefinition>().Where(x => x.State == EntityState.Added).ToList();
				var deleteClasses = maidContext.ChangeTracker.Entries<ClassDefinition>().Where(x => x.State == EntityState.Modified && x.Entity.IsDeleted == true).ToList();
				var modifiedClasses = maidContext.ChangeTracker.Entries<ClassDefinition>().Where(x => x.State == EntityState.Modified && x.Entity.IsDeleted == false).ToList();
				var addProperties = maidContext.ChangeTracker.Entries<PropertyDefinition>().Where(x => x.State == EntityState.Added).ToList();
				var deleteProperties = maidContext.ChangeTracker.Entries<PropertyDefinition>().Where(x => x.State == EntityState.Modified && x.Entity.IsDeleted == true).ToList();
				var modifiedProperties = maidContext.ChangeTracker.Entries<PropertyDefinition>().Where(x => x.State == EntityState.Modified && x.Entity.IsDeleted == false).ToList();
				try
				{
					await maidContext.SaveChangesAsync();
				}
				catch (Exception)
				{

					throw;
				}


				var maid = await maidContext.Maids
							.AsSplitQuery()
							.Include(x => x.Project)
							.Where(x => x.ProjectId == message.ProjectId)
							.ToListAsync();
				foreach (var item in maid)
				{
					switch (item.MaidWork)
					{
						case Models.CodeMaid.MaidWork.ConfigurationSync:
							{
								var setting = item.Setting.Deserialize<ConfigurationSyncSetting>();
								if (setting is null) continue;
								var sourcePath = Path.Combine(message.ProjectPath, setting.SourceDirectory);
								var targetPath = Path.Combine(message.ProjectPath, setting.TargetDirectory);
								var contextPath = setting.ContextPath is null ? null : Path.Combine(message.ProjectPath, setting.ContextPath);
								if (!file.Path.StartsWith(sourcePath)) continue;
								deleteClasses.ForEach(async x =>
								{
									var classDefinition = x.Entity;
									string fileName = Path.Combine(targetPath, $"{classDefinition.Name}Configuration.cs");
									if (File.Exists(fileName)) File.Delete(fileName);
									if (contextPath is not null) await MaidService.UpdateDataBaseContext(x.Entity, contextPath);
								});
								addClasses.Union(modifiedClasses).ToList().ForEach(async x =>
								{
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
								});
								addProperties.Union(deleteProperties.Union(modifiedProperties)).ToList().ForEach(async x =>
								{
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
								});
							}
							break;
						case Models.CodeMaid.MaidWork.DtoSync:
							{
								var setting = item.Setting.Deserialize<DtoSyncSetting>();
								if (setting is null) continue;
								var targetPath = Path.Combine(message.ProjectPath, setting.TargetDirectory);
								var classes = await maidContext.Projects
									.SelectMany(x => x.ProjectDirectories).SelectMany(x => x.ProjectDirectoryFiles).SelectMany(x => x.ProjectStructures).Select(x => x.ClassDefinition)
									.Where(x => x.ProjectId == project.Id)
									.Distinct()
									.ToListAsync();
								await MaidService.UpdateDto(classes, setting, targetPath);
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
								var sourcePath = Path.Combine(message.ProjectPath, setting.SourceDirectory);
								var targetPath = Path.Combine(message.ProjectPath, setting.TargetDirectory);
								if (!file.Path.StartsWith(sourcePath)) continue;
								deleteClasses.ForEach(x =>
								{
									var classDefinition = x.Entity;
									string fileName = Path.Combine(targetPath, $"{classDefinition.Name}Consumer.cs");
									if (File.Exists(fileName)) File.Delete(fileName);
								});
								addClasses.Union(modifiedClasses).ToList().ForEach(async x =>
								{
									var classDefinition = x.Entity;
									string fileName = Path.Combine(targetPath, $"{classDefinition.Name}Consumer.cs");
									await MaidService.MasstransitConsumerSync(x.Entity, fileName);
								});
							}

							break;
						default:
							break;
					}
				}
			}
			return;
		}
	}
	///<inheritdoc/>
	public class FileChangeEventConsumerDefinition : ConsumerDefinition<FileChangeEventConsumer>
	{
		///<inheritdoc/>
		protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<FileChangeEventConsumer> consumerConfigurator, IRegistrationContext context)
		{
			endpointConfigurator.ConcurrentMessageLimit = 1;
			endpointConfigurator
				.UseScheduledRedelivery(x => x.Interval(9999, TimeSpan.FromSeconds(1)));
			consumerConfigurator
				.Options<BatchOptions>(options => options.SetMessageLimit(9999)
				.SetConcurrencyLimit(1)
				.SetTimeLimit(TimeSpan.FromSeconds(2)));
		}
	}
}
