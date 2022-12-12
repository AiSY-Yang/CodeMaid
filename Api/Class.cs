using System.Collections.Concurrent;

using MaidContexts;

using MassTransit;

using MasstransitModels;

using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

using Models.CodeMaid;

using Serilog;

namespace Api
{
	class Maids
	{
		static IServiceProvider serviceProvider = null!;
		static readonly ConcurrentDictionary<FileSystemWatcher, Maid> Watchers = new();
		/// <summary>
		/// 初始化文件监听器
		/// </summary>
		/// <param name="serviceProvider"></param>
		public static async void Init(IServiceProvider serviceProvider)
		{
			Maids.serviceProvider = serviceProvider;
			var scope = serviceProvider.CreateScope();
			MaidContext context = scope.ServiceProvider.GetRequiredService<MaidContext>();
			var maids = await context.Maids.Include(x => x.Project).ToListAsync();
			foreach (var item in maids)
			{
				if (!Watchers.Any(x => x.Value.Id == item.Id))
				{
					var path = Path.Combine(item.Project.Path, item.SourcePath);
					if (!Path.Exists(path))
					{
						Log.Error("项目{project}的{func}的源路径{path}不存在", item.Project.Name, item.Name, path);
						continue;
					}
					FileSystemWatcher watcher = new(path)
					{
						//NotifyFilter = NotifyFilters.Attributes
						//   | NotifyFilters.CreationTime
						//   | NotifyFilters.DirectoryName
						//   | NotifyFilters.FileName
						//   | NotifyFilters.LastAccess
						//   | NotifyFilters.LastWrite
						//   | NotifyFilters.Security
						//   | NotifyFilters.Size;

						Filter = "*.cs",
						IncludeSubdirectories = true,
						EnableRaisingEvents = true
					};
					watcher.Changed += Watcher_Changed;
					watcher.Renamed += Watcher_Renamed;
					Watchers.TryAdd(watcher, item);
					Log.Information("添加项目{project}的{func}监听器,路径为{path}", item.Project.Name, item.Name, path);
				}
			}
		}

		private static async void Watcher_Renamed(object sender, RenamedEventArgs e)
		{
			await FileChange(Watchers[(FileSystemWatcher)sender], e.FullPath);
		}

		private static async void Watcher_Changed(object sender, FileSystemEventArgs e)
		{
			await FileChange(Watchers[(FileSystemWatcher)sender], e.FullPath);
		}
		/// <summary>
		/// 变更筛选器 VS修改文件的时候可能使用的是创建 修改 重命名的操作 把中间文件排除掉
		/// </summary>
		/// <param name="maid"></param>
		/// <param name="filePath"></param>
		/// <returns></returns>
		private static async Task FileChange(Maid maid, string filePath)
		{
			if (Path.GetExtension(filePath) != ".TMP")
			{
				var msg = new FileChangeEvent() { FilePath = filePath, MaidId = maid.Id };
				using var scope = serviceProvider.CreateScope();
				await scope.ServiceProvider.GetRequiredService<IPublishEndpoint>().Publish(msg);
			}
		}
	}
}

