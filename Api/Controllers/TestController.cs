using System.Runtime.CompilerServices;

using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class TestController : ControllerBase, IDictQuery
	{
		private readonly ILogger<TestController> _logger;

		public TestController(ILogger<TestController> logger)
		{
			_logger = logger;
		}

		public void GetDict()
		{
			throw new NotImplementedException();
		}
	}
	interface IService
	{

	}
	interface IDictQuery
	{
		[HttpGet("Dict")]
		public void GetDict();
	}
	interface IDictService
	{
		public List<int> GetDict()
		{
			return new List<int>();
		}
	}
}