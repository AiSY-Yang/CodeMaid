#define SSHON
using System.Diagnostics;

using MaidContexts;

using Mapster;

using Microsoft.EntityFrameworkCore;

using Models.CodeMaid;

using Renci.SshNet;

namespace DataBaseCopy;

internal class Program
{
	const string sourceConnectString = @"Server=localhost;Database=api;User Id=root;Password=123456;";
	const string targetConnectString = @"Server=localhost;Port=5432;Database=CodeMaid;User Id=postgres;Password=123456;";
	static void Main(string[] args)
	{
		var colorBackup = Console.ForegroundColor;
		Console.ForegroundColor = ConsoleColor.Red;
		Console.WriteLine("数据同步工具");
		Console.WriteLine("要求表的外键必须有显式的Id字段");
		Console.ForegroundColor = colorBackup;
		UseMapster();
#if SSHON
		using SshClient? client = null;
#endif
		#region 开启ssh隧道
		// var client = new SshClient("remote", 22, "root", "123456");
		//client.Connect();
		//var portForwarded = new ForwardedPortLocal("localhost", 15432, "remote", 15432);
		//client.AddForwardedPort(portForwarded);
		//portForwarded.Start();
		//Console.WriteLine("ssh隧道已连接");
		#endregion
		#region 开启数据库连接
		var sourceOptions = new DbContextOptionsBuilder<MaidContext>()
			.UseMySql(sourceConnectString, ServerVersion.AutoDetect(sourceConnectString))
			.Options;
		MaidContext sourceContext = new(sourceOptions);
		var targetOptions = new DbContextOptionsBuilder<MaidContext>()
			.UseNpgsql(targetConnectString)
			.EnableSensitiveDataLogging()
			.Options;
		MaidContext targetContext = new(targetOptions);
		sourceContext.Database.OpenConnection();
		targetContext.Database.OpenConnection();
		Console.WriteLine("数据库已连接");
		#endregion

		var projects = sourceContext.Projects.ToList();
		Sync(targetContext, projects);
		var maids = sourceContext.Maids.ToList();
		Sync(targetContext, maids);
#if SSHON
		client?.Disconnect();
#endif
	}
	/// <summary>
	/// 复制数据
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="targetContext">要写入数据的数据库</param>
	/// <param name="data">要被写入的数据</param>
	static void Sync<T>(DbContext targetContext, IEnumerable<T> data) where T : DatabaseEntity
	{
		var list = data.Where(x => x != null).Distinct().ToList();
		Console.Write($"同步{typeof(T).Name}表");
		var ids = list.Select(x => x.Id).ToList();
		Console.WriteLine($",一共{ids.Count}条数据");
		var has = targetContext.Set<T>().IgnoreQueryFilters().Where(x => ids.Contains(x.Id)).ToList();
		Stopwatch stopwatch = new();
		stopwatch.Start();
		var i = 0;
		foreach (var item in list)
		{
			i++;
			Console.SetCursorPosition(0, Console.CursorTop);
			Console.Write($"已完成{i}/{ids.Count}");
			var hasData = has.FirstOrDefault(x => x.Id == item.Id);
			if (hasData is null)
			{
				targetContext.Set<T>().Add(item.Adapt<T>());
			}
			else
			{
				item.Adapt(hasData);
			}
		}
		var updateCount = targetContext.ChangeTracker.Entries().Where(x => x.State == EntityState.Modified).Count();
		var addCount = targetContext.ChangeTracker.Entries().Where(x => x.State == EntityState.Added).Count();
		var unchangedCount = targetContext.ChangeTracker.Entries().Where(x => x.State == EntityState.Unchanged).Count();
		Console.WriteLine($",其中更新{updateCount}条,新增{addCount}条,未变化{unchangedCount}条");
		targetContext.SaveChanges();
		targetContext.ChangeTracker.Clear();
		Console.WriteLine($"执行完成,耗时{stopwatch.ElapsedMilliseconds / 1000}秒");
	}

	public static void UseMapster()
	{
		TypeAdapterConfig.GlobalSettings.Default
			.IgnoreMember((x, y) => x.Type.IsAssignableTo(typeof(DatabaseEntity)) || x.Type.IsAssignableTo(typeof(IEnumerable<DatabaseEntity>)))
			.MaxDepth(1);
		ConfigMapsterRule(TypeAdapterConfig.GlobalSettings.Default.Config);

	}
	/// <summary>
	/// 修改映射规则 如修改创建人 加上同步标志等
	/// </summary>
	/// <param name="adapterConfig"></param>
	public static void ConfigMapsterRule(TypeAdapterConfig adapterConfig)
	{
		adapterConfig.ForType<DatabaseEntity, DatabaseEntity>()
		.AfterMapping(x => x.UpdateTime = null)
		;
		//		adapterConfig.ForType<Workflow, Workflow>()
		//.AfterMapping(x => x.Title = "!!!copy from prod" + x.Title)
		//.AfterMapping(x => x.CreateUserId = CreateUser ?? x.CreateUserId)
		//.AfterMapping(x => x.MemberId = CreateMember ?? x.MemberId)
		//;
		//		adapterConfig.ForType<WorkflowParameter, WorkflowParameter>()
		//;
		//		adapterConfig.ForType<ToolLibrary, ToolLibrary>()
		//.AfterMapping(x => x.CreateUserId = CreateUser ?? x.CreateUserId)
		//;
		//		adapterConfig.ForType<BossOrder, BossOrder>()
		//.AfterMapping(x => x.TeamId = Team ?? x.TeamId)
		//;
		//		adapterConfig.ForType<BossDetection, BossDetection>()
		//.AfterMapping(x => x.TeamId = Team ?? x.TeamId)
		//;
	}
}
