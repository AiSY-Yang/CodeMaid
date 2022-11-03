using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;

using MaidContexts;

using MassTransit;

using MasstransitModels;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using Models.CodeMaid;

using Serilog;

namespace Api
{
	class Maids
	{
		static IServiceProvider serviceProvider = null!;
		volatile static ConcurrentDictionary<FileSystemWatcher, Maid> Watchers = new();
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
					FileSystemWatcher watcher = new FileSystemWatcher(item.SourcePath);
					//watcher.NotifyFilter = NotifyFilters.Attributes
					//   | NotifyFilters.CreationTime
					//   | NotifyFilters.DirectoryName
					//   | NotifyFilters.FileName
					//   | NotifyFilters.LastAccess
					//   | NotifyFilters.LastWrite
					//   | NotifyFilters.Security
					//   | NotifyFilters.Size;

					watcher.Filter = "*.cs";
					watcher.IncludeSubdirectories = true;
					watcher.EnableRaisingEvents = true;
					watcher.Changed += Watcher_Changed;
					watcher.Renamed += Watcher_Renamed; ;
					Watchers.TryAdd(watcher, item);
					Log.Information("添加项目{project}的{func}监听器,路径为{path}", item.Project.Name, item.Name, item.SourcePath);
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
		private static async Task FileChange(Maid maid, string filePath)
		{
			if (Path.GetExtension(filePath) != ".TMP")
			{
				var msg = new FileChangeEvent() { FilePath = filePath, MaidId = maid.Id };
				using var scope = serviceProvider.CreateScope();
				Console.WriteLine(msg);
				await scope.ServiceProvider.GetRequiredService<IPublishEndpoint>().Publish(msg);
			}
		}
	}
}

