using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Models.CodeMaid;
/// <summary>
/// 基类的配置
/// </summary>
internal abstract class DatabaseEntityConfiguration<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : DatabaseEntity
{
	public virtual void Configure(EntityTypeBuilder<TEntity> builder)
	{
		ConfigureComment(builder);
		builder.HasKey(x => x.Id);
	}
	/// <summary>
	/// Automatically generated comment configuration
	/// </summary>
	/// <param name="builder"></param>
	static void ConfigureComment(EntityTypeBuilder<TEntity> builder)
	{
		builder.Property(x => x.CreateTime).HasComment("创建时间");
		builder.Metadata.SetComment("所有数据库对象的基类");
		builder.Property(x => x.UpdateTime).HasComment("更新时间");
		builder.Property(x => x.IsDeleted).HasComment("是否有效");
	}
}
