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
	public class MaidChangeEventConsumer : IConsumer<MaidChangeEvent>
	{
		private readonly ILogger<MaidChangeEventConsumer> logger;
		private readonly MaidContext maidContext;

		public MaidChangeEventConsumer(ILogger<MaidChangeEventConsumer> logger, MaidContext maidContext)
		{
			this.logger = logger;
			this.maidContext = maidContext;
		}

		public async Task Consume(ConsumeContext<MaidChangeEvent> context)
		{
			logger.LogInformation("maid{id}开始工作", context.Message.MaidId);
			var maid = maidContext.Maids
						.Include(x => x.Classes)
						.ThenInclude(x => x.Properties)
						.ThenInclude(x => x.Attributes)
						.First(x => x.Id == context.Message.MaidId);
			await MaidService.Work(maid);
		}
	}
	public class MaidChangeEventConsumerDefinition : ConsumerDefinition<MaidChangeEventConsumer>
	{

		protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<MaidChangeEventConsumer> consumerConfigurator)
		{
		}
	}
}
