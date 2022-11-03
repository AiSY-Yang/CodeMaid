using Api.Services;

using GreenPipes;

using MaidContexts;

using MassTransit;
using MassTransit.ConsumeConfigurators;
using MassTransit.Definition;

using MasstransitModels;

using Microsoft.EntityFrameworkCore;

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
						.Include(x => x.Classes)
						.ThenInclude(x => x.Properties)
						.ThenInclude(x => x.Attributes)
						.First(x => x.Id == context.Message.MaidId);
			//检查更新
			MaidService.Update(maid, context.Message.FilePath);
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
		}
	}
}
