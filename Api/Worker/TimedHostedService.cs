using Api.Job;

using Microsoft.EntityFrameworkCore;

namespace Api.Worker
{
	/// <summary>
	/// 后台定时器
	/// </summary>
	public class TimedHostedService : IHostedService
	{
		readonly ILogger<TimedHostedService> _logger;
		readonly IServiceProvider services;
		private Timer? _timer = null;
		/// <summary>
		/// 后台定时器
		/// </summary>
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
#if DEBUG
				_timer = new Timer(RunAsync, null, TimeSpan.Zero, TimeSpan.FromSeconds(600000));
#else
				_timer = new Timer(RunAsync, null, TimeSpan.Zero, TimeSpan.FromSeconds(60));
#endif
				return Task.CompletedTask;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "定时器开启异常");
				return Task.CompletedTask;
			}
		}
		/// <inheritdoc/>
		public Task StopAsync(CancellationToken cancellationToken)
		{
			_ = (_timer?.Change(Timeout.Infinite, 0));
			_timer?.Dispose();
			return Task.CompletedTask;
		}

		/// <summary>
		/// Http客户端生成任务
		/// </summary>
		/// <param name="state"></param>
		public async void RunAsync(object? state)
		{
			try
			{
				using var scope = services.CreateScope();
				var context = scope.ServiceProvider.GetRequiredService<MaidContexts.MaidContext>();
				var maids = context.Maids
					.Where(x => !x.IsDeleted)
					.Include(x => x.Project)
					.Where(x => x.MaidWork == Models.CodeMaid.MaidWork.HttpClientSync);
				//using Activity activityMain = new Activity("test");
				//activityMain.Start();
				//activityMain.DisplayName = "test";
				//var a = new ActivitySource("qq");
				//a.StartActivity(ActivityKind.Client, activity.Context, null, null, DateTimeOffset.Now, "");
				//using var l = new ActivityListener();
				//l.ActivityStarted = (x) =>
				//{
				//	Console.WriteLine(x.DisplayName);
				//};
				//l.ShouldListenTo = (x) => { Console.WriteLine(x.Name); return true; };

				foreach (var maid in maids)
				{
					//activity.SetParentId(activityMain.TraceId, activity.SpanId);
					await scope.ServiceProvider.GetRequiredService<HttpClientGenerator>().ExecuteAsync(maid);
				}
				//activity.Stop();
				//activity.ActivityTraceFlags = ActivityTraceFlags.None;
				//foreach (var item in activity.Events)
				//{
				//	await Console.Out.WriteLineAsync(item.Name);
				//}
				//activityMain.Stop();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "");
			}
		}

	}
}