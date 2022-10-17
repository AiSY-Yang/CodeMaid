using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

using Models.Database.Interface;

namespace Reponsitory
{
	public interface IReponsitory<DatabaseContext, Entity> : IReponsitory<Entity>
		where DatabaseContext : DbContext where Entity : class
	{
		public DatabaseContext Context { get; }

		public EntityEntry<Entity> Add(Entity entity);
		public EntityEntry<Entity> Delete(Entity entity);
		public Task<EntityEntry<Entity>> Delete(long entityId);
	}
	public interface IReponsitory<Entity> : IReponsitory
	{
	}
	public interface IReponsitory { }
}