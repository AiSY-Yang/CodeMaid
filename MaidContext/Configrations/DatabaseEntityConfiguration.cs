using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Models.CodeMaid;
/// <summary>
/// 基类的配置
/// </summary>
/// <typeparam name="Entity"></typeparam>
internal abstract class DatabaseEntityConfiguration<Entity> : IEntityTypeConfiguration<Entity> where Entity : DatabaseEntity
{
	public virtual void Configure(EntityTypeBuilder<Entity> builder)
	{
		//builder.HasQueryFilter(x => !x.IsDeleted);
		builder.Metadata.SetComment("所有数据库对象的基类");
		builder.HasKey(x => x.Id);
		builder.Property(x => x.Id).HasComment("<inheritdoc cref=\"IDatabaseEntity.Id\"/>");
		builder.Property(x => x.CreateTime);
		builder.Property(x => x.UpdateTime);
		builder.Property(x => x.IsDeleted);
	}
}
