using System.Net.Mime;

using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
	/// <inheritdoc/>
	public class CommonController : ApiControllerBase
	{
	}


	/// <inheritdoc/>
	[ApiController]
	[Route("api/[controller]")]
	[ApiExplorerSettings(GroupName = "default")]
	[Produces(MediaTypeNames.Application.Json)]
	public class ApiControllerBase : Microsoft.AspNetCore.Mvc.ControllerBase
	{
	}
}
