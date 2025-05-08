using Api.Job;

using MaidContexts;

using MassTransit;

using MasstransitModels;

using Microsoft.EntityFrameworkCore;

using ServicesModels.Settings;

namespace Api.Worker
{
	/// <summary>
	/// 后台定时器
	/// </summary>
	public class InitService : IHostedService
	{
		readonly ILogger<TimedHostedService> _logger;
		readonly IServiceProvider services;
		/// <summary>
		/// 后台定时器
		/// </summary>
		public InitService(IServiceProvider services, ILogger<TimedHostedService> logger)
		{
			this.services = services;
			_logger = logger;
		}
		/// <inheritdoc/>
		public async Task StartAsync(CancellationToken cancellationToken)
		{
			var scope = services.CreateScope();
			var context = scope.ServiceProvider.GetRequiredService<MaidContext>();
			var bus = scope.ServiceProvider.GetRequiredService<IBusControl>();
			await bus.StartAsync();
			var publish = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();
			var project = context.Projects.Where(x => !x.IsDeleted).ToList();
			foreach (var item in project)
				await publish.Publish(new ProjectUpdateEvent() { ProjectId = item.Id });
		}
		/// <inheritdoc/>
		public Task StopAsync(CancellationToken cancellationToken)
		{
			return Task.CompletedTask;
		}
	}
}