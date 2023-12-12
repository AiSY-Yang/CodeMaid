using Api.Services;

using ExtensionMethods;

using MaidContexts;

using MassTransit;
using MassTransit.Batching;

using MasstransitModels;

using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace Api.MasstransitConsumer
{
	///<inheritdoc/>
	public class FileChangeEventConsumer : IConsumer<Batch<FileChangeEvent>>
	{
		private readonly ILogger<FileChangeEventConsumer> logger;
		private readonly MaidContext maidContext;
		///<inheritdoc/>
		public FileChangeEventConsumer(ILogger<FileChangeEventConsumer> logger, MaidContext maidContext)
		{
			this.logger = logger;
			this.maidContext = maidContext;
		}
		///<inheritdoc/>
		public async Task Consume(ConsumeContext<Batch<FileChangeEvent>> context)
		{
			var message = context.Message.First().Message;
			var maidId = message.MaidId;
			var maid = maidContext.Maids
						.AsSplitQuery()
						.Include(x => x.Project)
						.Include(x => x.Enums)
						.ThenInclude(x => x.EnumMembers)
						.Include(x => x.Classes)
						.ThenInclude(x => x.Properties)
						.ThenInclude(x => x.Attributes)
						.First(x => x.Id == maidId);
			if (File.Exists(Path.Combine(maid.Project.Path, ".git", "index.lock")) || File.Exists(Path.Combine(maid.Project.Path, ".git", "HEAD.lock")))
			{
				logger.LogInformation("分支切换中,跳过本次执行");
				return;
			}
			if (!maid.Project.GitBranch.IsNullOrEmpty())
			{
				var head = File.ReadLines(Path.Combine(maid.Project.Path, ".git", "HEAD")).First();
				if (!head.EndsWith(maid.Project.GitBranch))
				{
					logger.LogInformation("指定分支为{breach},当前分支为{currentBreach},跳过本次执行", maid.Project.GitBranch, head);
					return;
				}
			}
			switch (maid.MaidWork)
			{
				case Models.CodeMaid.MaidWork.ConfigurationSync:
					break;
				case Models.CodeMaid.MaidWork.DtoSync:
					break;
				case Models.CodeMaid.MaidWork.HttpClientSync:
					break;
				case Models.CodeMaid.MaidWork.ControllerSync:
					break;
				case Models.CodeMaid.MaidWork.MasstransitConsumerSync:
					break;
				default:
					break;
			}
			//检查更新
			await MaidService.Update(maid, message.FilePath, message.IsDelete);
			//如果有变化的话则发布变化事件
			if (maidContext.ChangeTracker.Entries().Where(x => x.State != EntityState.Unchanged).Any())
			{
				await maidContext.SaveChangesAsync();
				await context.Publish(new MaidChangeEvent() { MaidId = maid.Id });
			}
			return;
		}
	}
	///<inheritdoc/>
	public class FileChangeEventConsumerDefinition : ConsumerDefinition<FileChangeEventConsumer>
	{
		///<inheritdoc/>
		protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<FileChangeEventConsumer> consumerConfigurator, IRegistrationContext context)
		{
			endpointConfigurator.ConcurrentMessageLimit = 1;
			consumerConfigurator.Options<BatchOptions>(options => options.SetMessageLimit(999).SetConcurrencyLimit(1).SetTimeLimit(TimeSpan.FromSeconds(2)));
		}
	}
}
