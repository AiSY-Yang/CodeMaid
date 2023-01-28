using System.Runtime.CompilerServices;

using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class ProjectController : ControllerBase, IDictQuery
	{
		private readonly ILogger<ProjectController> _logger;

		public ProjectController(ILogger<ProjectController> logger)
		{
			_logger = logger;
		}

		[HttpGet("Dict")]
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