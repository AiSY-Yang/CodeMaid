using System.Diagnostics;
using System.Linq.Expressions;

using MaidContexts;

using Mapster;

using Microsoft.EntityFrameworkCore;

using Models.CodeMaid;

using Renci.SshNet;

namespace DataBaseSync;

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
		using SshClient? client = null;
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

		client?.Disconnect();
	}
	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="T">原对象和复制到的对象</typeparam>
	/// <typeparam name="T2">多对多关联的对象</typeparam>
	/// <param name="targetContext">数据库上下文</param>
	/// <param name="data">要同步的数据</param>
	/// <param name="include">选择多对多关系的表达式</param>
	/// <param name="action">重新建立关系的表达式 参数为新对象 源对象 多对多关系的数据列表</param>
	static void SyncMany<T, T2>(DbContext targetContext, IEnumerable<T> data, Expression<Func<T, IEnumerable<T2>>> include, Action<T, T, List<T2>> action) where T : DatabaseEntity where T2 : DatabaseEntity
	{
		Console.Write($"同步{typeof(T).Name}表");
		var ids = data.Select(x => x.Id).ToList();
		Console.Write($",一共{ids.Count}条数据");
		var has = targetContext.Set<T>().Include(include).Where(x => ids.Contains(x.Id)).ToList();
		Console.Write($",其中更新{has.Count}条,新增{ids.Count - has.Count}条");
		Console.WriteLine();
		Stopwatch stopwatch = new();
		stopwatch.Start();
		var i = 0;
		var func = include.Compile();
		var relativeIds = data.SelectMany(x => func(x)).Select(x => x.Id).ToList();
		var relativeData = targetContext.Set<T2>().Where(x => relativeIds.Contains(x.Id)).ToList();
		foreach (var item in relativeData)
		{
			Console.SetCursorPosition(0, Console.CursorTop);
			Console.Write($"{i}/{ids.Count}");
			var testData = relativeData.FirstOrDefault(x => x.Id == item.Id);
			if (testData is null)
			{
				testData = item.Adapt<T2>();
				relativeData.Add(testData);
				targetContext.Set<T2>().Add(testData);
			}
			else
			{
				item.Adapt(testData);
			}
		}

		foreach (var item in data)
		{
			Console.SetCursorPosition(0, Console.CursorTop);
			Console.Write($"{i}/{ids.Count}");
			var testData = has.FirstOrDefault(x => x.Id == item.Id);
			if (testData is null)
			{
				targetContext.Set<T>().Add(item.Adapt<T>());
			}
			else
			{
				item.Adapt(testData);
			}
			var id = func(item).Select(x => x.Id).ToList();
			var x = relativeData.Where(x => func(item).Select(x => x.Id).Contains(x.Id)).ToList();
			action(testData!, item, relativeData);
		}
		targetContext.SaveChanges();
		targetContext.ChangeTracker.Clear();
		Console.WriteLine($"执行完成,耗时{stopwatch.ElapsedMilliseconds / 1000}秒");
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
		Console.Write($",一共{ids.Count}条数据");
		var has = targetContext.Set<T>().Where(x => ids.Contains(x.Id)).ToList();
		Console.Write($",其中更新{has.Count}条,新增{ids.Count - has.Count}条");
		Console.WriteLine();
		Stopwatch stopwatch = new();
		stopwatch.Start();
		var i = 0;
		foreach (var item in list)
		{
			i++;
			Console.SetCursorPosition(0, Console.CursorTop);
			Console.Write($"{i}/{ids.Count}");
			var testData = has.FirstOrDefault(x => x.Id == item.Id);
			if (testData is null)
			{
				targetContext.Set<T>().Add(item.Adapt<T>());
			}
			else
			{
				item.Adapt(testData);
			}
		}
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
