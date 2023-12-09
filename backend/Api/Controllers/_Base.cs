using System.Net.Mime;

using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
	/// <inheritdoc/>
	[ApiController]
	[Route("[controller]")]
	[ApiExplorerSettings(GroupName = "default")]
	[Produces(MediaTypeNames.Application.Json)]
	public class ControllerBase : Microsoft.AspNetCore.Mvc.ControllerBase
	{
	}
}
