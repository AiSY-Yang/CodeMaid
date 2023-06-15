using System.Runtime.CompilerServices;

using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
	interface IServices
	{
	}

	[ApiController]
	[Route("[controller]")]
	public class ProjectController : ControllerBase, IAdd<object, bool>
	{
		private readonly ILogger<ProjectController> logger;
		private readonly ProjectService projectServices;

		public ProjectController(ILogger<ProjectController> logger, ProjectService projectServices)
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
	public class ProjectService : IServices, IAdd<object, bool>
	{
		public ProjectService(ProjectRepository projectRepository)
		{
		}

		public ControllerBase Controller { get; init; }

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