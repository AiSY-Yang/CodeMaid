     using System.Runtime.CompilerServices;

using Mapster;

using Microsoft.AspNetCore.Mvc;

using ServicesModels.Methods;

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

		public TestController(ILogger<TestController> logger)
		{
			this.logger = logger;
		}



	}
}