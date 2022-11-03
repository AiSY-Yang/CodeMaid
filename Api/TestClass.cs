using MaidContexts;

using MassTransit;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.EntityFrameworkCore;

using static Api.Services.MaidService;

namespace Api
{
	class TestClass
	{
		public TestClass(IServiceProvider service)
		{
			Service = service;
		}

		public IServiceProvider Service { get; }

		public async void Task()
		{
			var context = Service.GetRequiredService<MaidContext>();
			var project = context.Projects
				.Where(x => x.Id == 2)
				.Include(x => x.Maids)
				.ThenInclude(x => x.Classes)
				.ThenInclude(x => x.Properties)
				.ThenInclude(x => x.Attributes)
				.First();
			var maid = project.Maids.First();
			//生成数据库的数据模型
			foreach (var file in Directory.GetFiles(maid.SourcePath, "*.cs", SearchOption.AllDirectories))
			{
				maid = Update(maid, file);
			}
			await context.SaveChangesAsync();

			await UpdateConfiguration(maid);

			Console.WriteLine("--- over");
		}

	}
}
