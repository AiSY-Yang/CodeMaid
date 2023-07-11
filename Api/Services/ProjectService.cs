using MaidContexts;

using Microsoft.AspNetCore.Mvc;

using ServicesModels.Methods;

namespace Api.Services
{
	public class ProjectService
	{
		private MaidContext context;

		public ProjectService(MaidContext context)
		{
			this.context = context;
		}
	}
}