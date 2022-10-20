﻿using Microsoft.EntityFrameworkCore;

using Models.CodeMaid;

namespace MaidContexts
{
	public class MaidContext : DbContext
	{
		public DbSet<NameSpaceDefinition> NameSpaceDefinitions { get; set; } = null!;
		public DbSet<ClassDefinition> ClassDefinitions { get; set; } = null!;
		public DbSet<PropertyDefinition> PropertyDefinitions { get; set; } = null!;
		public DbSet<AttributeDefinition> AttributeDefinitions { get; set; } = null!;
		public MaidContext(DbContextOptions<MaidContext> options) : base(options)
		{
			Database.Migrate();
		}
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<NameSpaceDefinition>().HasKey(x=>x.Id);
			modelBuilder.Entity<ClassDefinition>().HasKey(x=>x.Id);
			modelBuilder.Entity<PropertyDefinition>().HasKey(x=>x.Id);
			modelBuilder.Entity<AttributeDefinition>().HasKey(x=>x.Id);
			modelBuilder.ApplyConfigurationsFromAssembly(typeof(MaidContext).Assembly);
			base.OnModelCreating(modelBuilder);
		}
		public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
		{
			SetUpdateTime();
			return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
		}
		public override int SaveChanges(bool acceptAllChangesOnSuccess)
		{
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
	}
}