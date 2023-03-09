using System.Runtime.CompilerServices;

using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class ProjectController : ControllerBase
	{
		private readonly ILogger<ProjectController> _logger;

		public ProjectController(ILogger<ProjectController> logger)
		{
			_logger = logger;
		}

	}
}