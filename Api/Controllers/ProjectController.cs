using System.Runtime.CompilerServices;

using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class ProjectController : ControllerBase, IAdd<object, bool>
	{
		private readonly ILogger<ProjectController> logger;
		private readonly ProjectServices projectServices;

		public ProjectController(ILogger<ProjectController> logger, ProjectServices projectServices)
		{
			this.logger = logger;
			this.projectServices = projectServices;
		}
		[HttpPost]
		public bool Add(object data)
		{
			return projectServices.Add(data);
		}
	}
	public class ProjectServices : IAdd<object, bool>
	{
		public ProjectServices(ProjectRepository projectRepository)
		{
		}

		public bool Add(object data)
		{
			throw new NotImplementedException();
		}
	}
	public class ProjectRepository
	{

	}

	interface IAdd<TData, TResult>
	{
		public TResult Add(TData data);
	}
}