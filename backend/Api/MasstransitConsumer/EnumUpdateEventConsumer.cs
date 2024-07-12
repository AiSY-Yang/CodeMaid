using MaidContexts;

using MassTransit;

using MasstransitModels;

using Microsoft.EntityFrameworkCore;

namespace Api.MasstransitConsumer
{
	/// <summary>
	/// enum update
	/// </summary>
	public class EnumUpdateEventConsumer : IConsumer<EnumUpdateEvent>
	{
		private readonly ILogger<EnumUpdateEventConsumer> logger;
		private readonly MaidContext maidContext;

		///<inheritdoc/>
		public EnumUpdateEventConsumer(ILogger<EnumUpdateEventConsumer> logger, MaidContext maidContext)
		{
			this.logger = logger;
			this.maidContext = maidContext;
		}
		///<inheritdoc/>
		public async Task Consume(ConsumeContext<EnumUpdateEvent> context)
		{
			var e = maidContext.EnumDefinitions.First(x => x.Id == context.Message.EnumId);
			var ps = await maidContext.Projects
				.Where(x => x.Id == context.Message.ProjectId)  
				.SelectMany(x => x.ClassDefinitions)
				.SelectMany(x => x.Properties.Where(x => x.Type == e.Name))
				.ToListAsync();
			if (e.IsDeleted) ps.ForEach(x => { x.IsEnum = false; x.EnumDefinition = null; });
			else ps.ForEach(x => { x.IsEnum = true; x.EnumDefinition = e; });
			await maidContext.SaveChangesAsync();
			//ps.ForEach()
		}
	}
	///<inheritdoc/>
	public class EnumUpdateEventConsumerDefinition : ConsumerDefinition<EnumUpdateEventConsumer>
	{
		///<inheritdoc/>
		protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<EnumUpdateEventConsumer> consumerConfigurator, IRegistrationContext context)
		{
		}
	}
}