using Api.Services;

using ExtensionMethods;

using MaidContexts;

using MassTransit;

using MasstransitModels;

using Microsoft.EntityFrameworkCore;

using Models.CodeMaid;

namespace Api.MasstransitConsumer
{
	///<inheritdoc/>
	public class ProjectUpdateEventConsumer : IConsumer<ProjectUpdateEvent>
	{
		private readonly ILogger<ProjectUpdateEventConsumer> logger;
		private readonly MaidContext maidContext;

		///<inheritdoc/>
		public ProjectUpdateEventConsumer(ILogger<ProjectUpdateEventConsumer> logger, MaidContext maidContext)
		{
			this.logger = logger;
			this.maidContext = maidContext;
		}
		///<inheritdoc/>
		public async Task Consume(ConsumeContext<ProjectUpdateEvent> context)
		{
			var projectInformation = MaidService.Projects.GetOrAdd(context.Message.ProjectId, x => new MaidService.ProjectInformation());
			projectInformation.Project = await maidContext.Projects
				.AsSplitQuery()
				.Include(x => x.ProjectDirectories)
				.ThenInclude(x => x.ProjectDirectoryFiles)
				.FirstAsync(x => x.Id == context.Message.ProjectId);
			var project = projectInformation.Project;
			var watcher = projectInformation.FileSystemWatcher;

			if (!Directory.Exists(project.Path))
			{
				logger.LogError("项目{project}的路径{path}不存在", project.Name, project.Path);
				projectInformation.FileSystemWatcher = null;
				return;
			}
			project.ProjectDirectories.ForEach(x =>
			{
				x.IsDeleted = true;
			});
			await GetInfo(project.Path);

			async Task GetInfo(string path)
			{
				var name = Path.GetFileName(path);
				if (FileChangeEventConsumer.IsIgnoreDir(name)) return;
				var relativePath = Path.GetRelativePath(project.Path, path);
				var projectDirectory = project.ProjectDirectories.FirstOrDefault(x => x.Path == relativePath && x.Name == name);
				if (projectDirectory is null)
				{
					projectDirectory = new ProjectDirectory()
					{
						Project = project,
						ProjectDirectoryFiles = new List<ProjectDirectoryFile>(),
						Name = name,
						Path = Path.GetRelativePath(project.Path, path)
					};
					maidContext.Add(projectDirectory);
				}
				projectDirectory.IsDeleted = false;

				var directoryInfo = new DirectoryInfo(path);
				foreach (var item in directoryInfo.GetDirectories())
				{
					await GetInfo(item.FullName);
				}
				var existFiles = new HashSet<ProjectDirectoryFile>();
				foreach (var file in directoryInfo.GetFiles("*.cs"))
				{
					var f = File.ReadAllLines(file.FullName);
					var linesCount = f.Length;
					DateTimeOffset lastWriteTime = file.LastWriteTime;
					lastWriteTime = lastWriteTime.TruncateNanosecond();
					var projectDirectoryFile = projectDirectory.ProjectDirectoryFiles.Where(x => x.Name == file.Name).FirstOrDefault();
					//当文件改变过的时候 通知变更
					if (projectDirectoryFile == null || projectDirectoryFile.LastWriteTime != lastWriteTime)
						await context.Publish(new FileChangeEvent() { ProjectId = project.Id, FilePath = file.FullName, IsDelete = false });
					if (projectDirectoryFile != null)
					{
						if (projectDirectoryFile.IsDeleted == true)
						{
							projectDirectoryFile.IsDeleted = false;
							await context.Publish(new FileChangeEvent() { ProjectId = project.Id, FilePath = file.FullName, IsDelete = false });
						}
						existFiles.Add(projectDirectoryFile);
					}
				}
				var deletes = projectDirectory.ProjectDirectoryFiles.Where(x => !existFiles.Contains(x)).ToList();
				foreach (var item in deletes)
				{
					item.IsDeleted = true;
					await context.Publish(new FileChangeEvent() { ProjectId = project.Id, FilePath = item.Path, IsDelete = true });
				}
			}
			maidContext.SaveChanges();
			watcher = new(project.Path)
			{
				NotifyFilter = NotifyFilters.Attributes
				   | NotifyFilters.CreationTime
				   | NotifyFilters.DirectoryName
				   | NotifyFilters.FileName
				   //   | NotifyFilters.LastAccess
				   | NotifyFilters.LastWrite
				   //   | NotifyFilters.Security
				   | NotifyFilters.Size,
				Filter = "*.cs",
				IncludeSubdirectories = true,
				EnableRaisingEvents = true,
			};
			projectInformation.FileSystemWatcher = watcher;
			watcher.Changed += FileChanged;
			watcher.Renamed += FileRenamed;
			watcher.Created += FileChanged;
			watcher.Deleted += FileDeleted;

		}

		private static async void FileRenamed(object sender, RenamedEventArgs e)
		{
			await FileChange((FileSystemWatcher)sender, e.OldFullPath, true);
			await FileChange((FileSystemWatcher)sender, e.FullPath, false);
		}

		private static async void FileDeleted(object sender, FileSystemEventArgs e)
		{
			await FileChange((FileSystemWatcher)sender, e.FullPath, true);
		}
		private static async void FileChanged(object sender, FileSystemEventArgs e)
		{
			await FileChange((FileSystemWatcher)sender, e.FullPath, false);
		}
		/// <summary>
		/// 变更筛选器
		/// </summary>
		/// <param name="watcher"></param>
		/// <param name="filePath">文件路径</param>
		/// <param name="isDelete">是否是删除文件</param>
		/// <returns></returns>
		private static async Task FileChange(FileSystemWatcher watcher, string filePath, bool isDelete)
		{
			var pi = MaidService.Projects.FirstOrDefault(x => x.Value.FileSystemWatcher == watcher);
			if (pi.Key != 0)
			{
				var msg = new FileChangeEvent() { ProjectId = pi.Key, FilePath = filePath, IsDelete = isDelete };
				using var scope = Program.Services.CreateScope();
				await scope.ServiceProvider.GetRequiredService<IPublishEndpoint>().Publish(msg);
			}
		}
	}
	///<inheritdoc/>
	public class ProjectUpdateEventConsumerDefinition : ConsumerDefinition<ProjectUpdateEventConsumer>
	{
		///<inheritdoc/>
		protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<ProjectUpdateEventConsumer> consumerConfigurator, IRegistrationContext context)
		{
			endpointConfigurator
				.UseScheduledRedelivery(x => x.Interval(9999, TimeSpan.FromSeconds(1)));
		}
	}
}