using System.Runtime.CompilerServices;

using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
	interface IServices
	{
	}

	[ApiController]
	[Route("[controller]/[action]")]
	public class TestController : ControllerBase, IAdd<object, bool>
	{
		private readonly ILogger<TestController> logger;
		private readonly TestService projectServices;

		public TestController(ILogger<TestController> logger, TestService projectServices)
		{
			this.logger = logger;
			this.projectServices = projectServices;
		}
		[HttpPost]
		public bool Add(object data)
		{
			return projectServices.Add(data);
		}

		[HttpGet]
		public List<bool> notnull(object data)
		{
			return null;
			//return projectServices.Add(data);
		}
		[HttpGet]
		public bool notnull2(object data)
		{
			return true;
			//return projectServices.Add(data);
		}
		[HttpGet]
		public List<bool>? nullable(object data)
		{
			return null;
			//return projectServices.Add(data);
		}
		[HttpGet]
		public bool? nullable2(object data)
		{
			return null;
			//return projectServices.Add(data);
		}
	}
	public class TestService : IServices, IAdd<object, bool>
	{
		public TestService(TestRepository projectRepository)
		{
		}

		public ControllerBase Controller { get; init; }

		public bool Add(object data)
		{
			throw new NotImplementedException();
		}
	}
	public class TestRepository
	{

	}

	interface IAdd<TData, TResult>
	{
		public TResult Add(TData data);
	}
}