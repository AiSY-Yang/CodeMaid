using Api.Services;

using MaidContexts;

using MassTransit;

using MasstransitModels;

using Microsoft.EntityFrameworkCore;

namespace Api.MasstransitConsumer
{
	/// <inheritdoc/>
	public class MaidChangeEventConsumer : IConsumer<MaidChangeEvent>
	{
		private readonly ILogger<MaidChangeEventConsumer> logger;
		private readonly MaidContext maidContext;

		/// <inheritdoc/>
		public MaidChangeEventConsumer(ILogger<MaidChangeEventConsumer> logger, MaidContext maidContext)
		{
			this.logger = logger;
			this.maidContext = maidContext;
		}

		/// <inheritdoc/>
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
			logger.LogInformation("项目{projectName}maid{maidName}开始工作", maid.Project.Name, maid.Name);
			await MaidService.Work(maid);
		}
	}
	/// <inheritdoc/>
	public class MaidChangeEventConsumerDefinition : ConsumerDefinition<MaidChangeEventConsumer>
	{
		/// <inheritdoc/>
		protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<MaidChangeEventConsumer> consumerConfigurator)
		{
		}
	}
}
