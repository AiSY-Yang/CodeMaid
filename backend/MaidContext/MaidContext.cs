using System.Linq.Expressions;
using System.Text.Json;

using ContextBases;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

using Models.CodeMaid;

namespace MaidContexts
{
	public class MaidContext : ContextBase
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
		/// <summary>
		/// 项目目录
		/// </summary>
		public virtual DbSet<ProjectDirectory> ProjectDirectories { get; set; } = null!;
		/// <summary>
		/// 项目文件
		/// </summary>
		public virtual DbSet<ProjectDirectoryFile> ProjectDirectoryFiles { get; set; } = null!;
	
		/// <summary>
		/// 项目结构
		/// </summary>
		public virtual DbSet<ProjectStructure> ProjectStructures { get; set; } = null!;
		public MaidContext(DbContextOptions<MaidContext> options) : base(options)
		{
		}
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
		protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
		{
			configurationBuilder.Properties<DateTimeOffset>(x => x.HaveConversion<DateTimeOffsetConverter>());
			configurationBuilder.Properties<JsonElement>(x => x.HaveConversion<JsonElementConverter>());
			base.ConfigureConventions(configurationBuilder);
		}
		/// <summary>
		/// 设置更新时间
		/// </summary>
		private void SetUpdateTime()
		{
			foreach (var item in ChangeTracker.Entries<DatabaseEntity>().Where(x => x.State == EntityState.Modified))
			{
				item.Entity.UpdateTime = DateTimeOffset.UtcNow;
			}
		}
		/// <summary>
		/// 全局伪删除
		/// </summary>
		private void FakeDelete()
		{
			foreach (var item in ChangeTracker.Entries<DatabaseEntity>().Where(x => x.State == EntityState.Deleted))
			{
				item.State = EntityState.Modified;
				item.Entity.IsDeleted = true;
			}
		}
	}
}