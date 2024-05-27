using MaidContexts;

using Microsoft.AspNetCore.Mvc;

using Models.CodeMaid;

using Npgsql.EntityFrameworkCore.PostgreSQL.Query.Expressions.Internal;

namespace Api.Controllers
{
	public class ProjectController : ApiControllerBase
	{
		private readonly MaidContext maidContext;

		public ProjectController(MaidContext maidContext)
		{
			this.maidContext = maidContext;
		}
		[HttpGet("")]
		public List<Project> GetList()
		{
			return maidContext.Projects.ToList();
		}

		[HttpPut("[action]")]
		public bool FlushAllFile(long id)
		{
			return true;
		}

	}
}
