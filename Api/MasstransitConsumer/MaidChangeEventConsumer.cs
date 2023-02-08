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
			var maid = maidContext.Maids
				.AsNoTracking()
						.Include(x => x.Project)
						.Include(x => x.Enums)
						.ThenInclude(x => x.EnumMembers)
						.Include(x => x.Classes)
						.ThenInclude(x => x.Properties)
						.ThenInclude(x => x.Attributes)
						.Include(x => x.Classes)
						.ThenInclude(x => x.Properties)
						.ThenInclude(x => x.EnumDefinition)
						.ThenInclude(x => x!.EnumMembers)
						.First(x => x.Id == context.Message.MaidId);
			logger.LogInformation("项目{projectname}maid{maidname}开始工作", maid.Project.Name, maid.Name);
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
