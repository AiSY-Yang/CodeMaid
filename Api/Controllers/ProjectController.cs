using System.Runtime.CompilerServices;

using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class ProjectController : ControllerBase
	{
		private readonly ILogger<ProjectController> logger;
		private readonly ProjectRepository projectRepository;

		public ProjectController(ILogger<ProjectController> logger,ProjectRepository projectRepository)
		{
			this.logger = logger;
			this.projectRepository = projectRepository;
		}
		void Add(object a)
		{
			//return projectRepository.Add(a);
		}
	}
	public class ProjectServices
	{
		public ProjectServices(ProjectRepository projectRepository)
		{
		}
	}
	public class ProjectRepository
	{

	}
}