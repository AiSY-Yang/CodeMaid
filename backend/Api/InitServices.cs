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
	class InitServices
	{
		static IServiceProvider serviceProvider = null!;
		static readonly ConcurrentDictionary<FileSystemWatcher, Maid> Watchers = new();
		/// <summary>
		/// 初始化文件监听器
		/// </summary>
		/// <param name="serviceProvider"></param>
		public static async Task Init(IServiceProvider serviceProvider)
		{
			var scope = serviceProvider.CreateScope();
			var context = scope.ServiceProvider.GetRequiredService<MaidContext>();
			var bus = scope.ServiceProvider.GetRequiredService<IBusControl>();
			await bus.StartAsync();
			var publish = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();
			var project = context.Projects.Where(x => !x.IsDeleted).ToList();
			foreach (var item in project) await publish.Publish<ProjectUpdateEvent>(new ProjectUpdateEvent() { ProjectId = item.Id });
		}
	}
}

