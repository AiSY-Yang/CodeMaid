using Api.Job;

namespace Api.Worker
{
	/// <summary>
	/// 后台定时器
	/// </summary>
	public class TimedHostedService : IHostedService, IDisposable
	{
		readonly ILogger<TimedHostedService> _logger;
		readonly IServiceProvider services;
		private Timer? _timer = null;
		/// <inheritdoc/>
		public TimedHostedService(IServiceProvider services, ILogger<TimedHostedService> logger)
		{
			this.services = services;
			_logger = logger;
		}
		/// <inheritdoc/>
		public Task StartAsync(CancellationToken cancellationToken)
		{
			try
			{
				_timer = new Timer(RunAsync, null, TimeSpan.Zero, TimeSpan.FromSeconds(10));
				return Task.CompletedTask;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message + ex.Source + ex.StackTrace);
				return Task.CompletedTask;
			}
		}

		public async void RunAsync(object? state)
		{
			try
			{
				var context = services.GetRequiredService<MaidContexts.MaidContext>();
				//var maids = context.Maids.Where(x => x.MaidWork == Models.CodeMaid.MaidWork.HttpClientSync);
				//foreach (var item in maids)
				//{
					await new HttpClientGenerator().ExecuteAsync();
				//}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "");
			}
		}

		/// <inheritdoc/>
		public Task StopAsync(CancellationToken cancellationToken)
		{
			_ = (_timer?.Change(Timeout.Infinite, 0));
			return Task.CompletedTask;
		}
		/// <inheritdoc/>
		public void Dispose()
		{
			_timer?.Dispose();
		}
	}
}