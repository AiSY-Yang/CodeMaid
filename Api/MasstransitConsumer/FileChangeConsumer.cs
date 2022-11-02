using GreenPipes;

using MassTransit;
using MassTransit.ConsumeConfigurators;
using MassTransit.Definition;

using MasstransitModels;

namespace Api.MasstransitConsumer
{
	public class FileChangeEventConsumer : IConsumer<FileChangeEvent>
	{
		public Task Consume(ConsumeContext<FileChangeEvent> context)
		{
			Console.WriteLine("文件{path}改变,重新读取数据",context.Message.FilePath);
			return Task.CompletedTask;
		}
	}
	public class OrderEtoConsumerDefinition : ConsumerDefinition<FileChangeEventConsumer>
	{

		protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<FileChangeEventConsumer> consumerConfigurator)
		{
			//endpointConfigurator.UseMessageRetry(r => r.Intervals(500, 1000));
		}
	}
}
