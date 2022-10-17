using MaidContexts;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

using Models.CodeMaid;

using Reponsitory;

namespace MaidReponsitory.Base
{
	public abstract class DatabaseEntityReponsitory<Entity> : IReponsitory<MaidContext, Entity> where Entity : DatabaseEntity
	{
		protected DatabaseEntityReponsitory(MaidContext context)
		{
			Context = context;
		}

		public MaidContext Context { get; }
		public DbSet<Entity> Set { get => Context.Set<Entity>(); }
		public IQueryable<Entity> ReadOnlyQuery { get => Context.Set<Entity>().AsNoTracking(); }
		public EntityEntry<Entity> Add(Entity entity)
		{
			return Context.Set<Entity>().Add(entity);
		}
		public async Task<Entity> AddAndSave(Entity entity)
		{
			Context.Set<Entity>().Add(entity);
			await Context.SaveChangesAsync();
			return entity;
		}

		public EntityEntry<Entity> Delete(Entity entity)
		{
			return Context.Set<Entity>().Remove(entity);
		}

		public async Task<EntityEntry<Entity>> Delete(long entityId)
		{
			var entity = await Set.FirstAsync(x => x.Id == entityId);
			return Context.Set<Entity>().Remove(entity);
		}
	}

}