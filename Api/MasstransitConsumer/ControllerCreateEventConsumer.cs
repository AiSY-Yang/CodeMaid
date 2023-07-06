using MassTransit;
using MassTransit.ConsumeConfigurators;
using MassTransit.Definition;

using MasstransitModels;

using Microsoft.EntityFrameworkCore;

namespace Api.MasstransitConsumer
{
	/// <summary>
	/// 创建控制器
	/// </summary>
	public class ControllerCreateEventConsumer : IConsumer<ControllerCreateEvent>
	{
		private readonly ILogger<ControllerCreateEventConsumer> logger;
		///<inheritdoc/>
		public ControllerCreateEventConsumer(ILogger<ControllerCreateEventConsumer> logger)
		{
			this.logger = logger;
		}

		///<inheritdoc/>
		public async Task Consume(ConsumeContext<ControllerCreateEvent> context)
		{
			await Task.CompletedTask;
		}
	}
	///<inheritdoc/>
	public class ControllerCreateEventConsumerDefinition : ConsumerDefinition<ControllerCreateEventConsumer>
	{
		///<inheritdoc/>
		protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<ControllerCreateEventConsumer> consumerConfigurator)
		{
		}
	}
}