using Microsoft.EntityFrameworkCore;

using Models.CodeMaid;

namespace MaidContexts
{
	public class MaidContext : DbContext
	{
		/// <summary>
		/// 项目定义
		/// </summary>
		public DbSet<Project> Projects { get; set; } = null!;
		/// <summary>
		/// 功能
		/// </summary>
		public DbSet<Maid> Maids { get; set; } = null!;
		/// <summary>
		/// 类定义
		/// </summary>
		public DbSet<ClassDefinition> ClassDefinitions { get; set; } = null!;
		/// <summary>
		/// 类定义
		/// </summary>
		public DbSet<PropertyDefinition> PropertyDefinitions { get; set; } = null!;
		/// <summary>
		/// 属性定义
		/// </summary>
		public DbSet<AttributeDefinition> AttributeDefinitions { get; set; } = null!;
		/// <summary>
		/// 枚举定义
		/// </summary>
		public virtual DbSet<EnumDefinition> EnumDefinitions { get; set; } = null!;
		/// <summary>
		/// 枚举成员定义
		/// </summary>
		public virtual DbSet<EnumMemberDefinition> EnumMemberDefinitions { get; set; } = null!;
		public MaidContext(DbContextOptions<MaidContext> options) : base(options)
		{
			if (!HasMigrate)
			{
				Database.Migrate();
				HasMigrate = true;
			}
		}
		private static bool HasMigrate = false;
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfigurationsFromAssembly(typeof(MaidContext).Assembly);
			base.OnModelCreating(modelBuilder);
		}
		public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
		{
			FakeDelete();
			SetUpdateTime();
			return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
		}
		public override int SaveChanges(bool acceptAllChangesOnSuccess)
		{
			FakeDelete();
			SetUpdateTime();
			return base.SaveChanges(acceptAllChangesOnSuccess);
		}
		/// <summary>
		/// 设置更新时间
		/// </summary>
		private void SetUpdateTime()
		{
			foreach (var item in ChangeTracker.Entries().Where(x => x.State == EntityState.Modified).Select(x => x.Entity))
			{
				if (item is DatabaseEntity a) a.UpdateTime = DateTimeOffset.UtcNow;
			}
		}
		/// <summary>
		/// 全局伪删除
		/// </summary>
		private void FakeDelete()
		{
			foreach (var item in ChangeTracker.Entries().Where(x => x.State == EntityState.Deleted).Where(x => x.Entity is DatabaseEntity))
			{
				if (item.Entity is DatabaseEntity entity)
				{
					item.State = EntityState.Modified;
					entity.IsDeleted = true;
				}
			}
		}
	}
}