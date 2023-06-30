using Microsoft.EntityFrameworkCore;

namespace ContextBases
{
	public abstract class ContextBase : DbContext
	{
		public ContextBase(DbContextOptions options) : base(options)
		{
		}
	}
}