using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Models.Database;

/// <summary>
/// 基类的配置
/// </summary>
/// <typeparam name="Entity"></typeparam>
internal abstract class EntityBaseConfiguration<Entity> : IEntityTypeConfiguration<Entity> where Entity : DatabaseEntity
{
	public virtual void Configure(EntityTypeBuilder<Entity> builder)
	{
		builder.HasQueryFilter(x => !x.IsDeleted);
		builder.HasKey(x => x.Id);
	}
}
