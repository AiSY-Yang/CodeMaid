using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace ContextBases
{
	/// <summary>
	/// a database context that can save deleted and modified record in backup database
	/// </summary>
	/// <typeparam name="T">backup database context</typeparam>
	public class BackupModifiedContextBase<T> : ContextBase where T : BackupContextBase
	{
		public BackupModifiedContextBase(DbContextOptions options) : base(options)
		{
		}
		public override int SaveChanges() => SaveChanges(true);
		public override int SaveChanges(bool acceptAllChangesOnSuccess)
		{
			OnSaveAsync().Wait();
			return base.SaveChanges(acceptAllChangesOnSuccess);
		}
		public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) => SaveChangesAsync(true, cancellationToken);
		public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
		{
			await OnSaveAsync();
			return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
		}
		async Task OnSaveAsync()
		{
			var list = ChangeTracker.Entries().Where(x => x.State == EntityState.Deleted || x.State == EntityState.Modified).ToList();
			if (list.Count > 0)
			{
				var c = Database.GetService<T>();
				c.AttachRange(list.Select(x => x.OriginalValues.ToObject()).ToList());
				await c.SaveChangesAsync();
			}
		}
	}
}