using MaidContexts;

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