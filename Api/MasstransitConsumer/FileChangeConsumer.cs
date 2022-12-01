using Api.Services;

using ExtensionMethods;

using GreenPipes;

using MaidContexts;

using MassTransit;
using MassTransit.ConsumeConfigurators;
using MassTransit.Definition;

using MasstransitModels;

using Microsoft.EntityFrameworkCore;

using Serilog;

namespace Api.MasstransitConsumer
{
	public class FileChangeEventConsumer : IConsumer<FileChangeEvent>
	{
		private readonly ILogger<FileChangeEventConsumer> logger;
		private readonly MaidContext maidContext;

		public FileChangeEventConsumer(ILogger<FileChangeEventConsumer> logger, MaidContext maidContext)
		{
			this.logger = logger;
			this.maidContext = maidContext;
		}

		public async Task Consume(ConsumeContext<FileChangeEvent> context)
		{
			logger.LogInformation("文件{path}改变,重新读取数据", context.Message.FilePath);
			var maid = maidContext.Maids
						.Include(x => x.Project)
						.Include(x => x.Classes)
						.ThenInclude(x => x.Properties)
						.ThenInclude(x => x.Attributes)
						.First(x => x.Id == context.Message.MaidId);
			if (File.Exists(Path.Combine(maid.Project.Path, ".git", "index.lock")) || File.Exists(Path.Combine(maid.Project.Path, ".git", "HEAD.lock")))
			{
				logger.LogInformation("分支切换中,跳过本次执行");
				return;
			}
			if (!maid.Project.GitBranch.IsNullOrEmpty())
			{
				var head = File.ReadLines(Path.Combine(maid.Project.Path, ".git", "HEAD")).First();
				if (!head.EndsWith(maid.Project.GitBranch))
				{
					logger.LogInformation("指定分支为{breach},当前分支为{currentBreach},跳过本次执行", maid.Project.GitBranch, head);
					return;
				}
			}
			//检查更新
			//2022-12-01更改为更新所有文件 因为暂时还没想好删除文件怎么做
			//MaidService.Update(maid, context.Message.FilePath);
			MaidService.Update(maid);
			//如果有变化的话则发布变化事件
			if (maidContext.ChangeTracker.Entries().Any())
			{
				await maidContext.SaveChangesAsync();
				await context.Publish(new MaidChangeEvent() { MaidId = maid.Id });
			}
			return;
		}
	}
	public class FileChangeEventConsumerDefinition : ConsumerDefinition<FileChangeEventConsumer>
	{
		protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<FileChangeEventConsumer> consumerConfigurator)
		{
			endpointConfigurator.ConcurrentMessageLimit = 1;
		}
	}
}
