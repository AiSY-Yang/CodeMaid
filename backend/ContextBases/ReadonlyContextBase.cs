using Microsoft.EntityFrameworkCore;

namespace ContextBases
{
	public abstract class ReadonlyContextBase : ContextBase
	{
		public ReadonlyContextBase(DbContextOptions options) : base(options)
		{
		}
		public override int SaveChanges() => throw new NotSupportedException("Readonly Context cannot be saved");
		public override int SaveChanges(bool acceptAllChangesOnSuccess) => throw new NotSupportedException("Readonly Context cannot be saved");

		public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default) => throw new NotSupportedException("Readonly Context cannot be saved");
		public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) => throw new NotSupportedException("Readonly Context cannot be saved");
	}
}