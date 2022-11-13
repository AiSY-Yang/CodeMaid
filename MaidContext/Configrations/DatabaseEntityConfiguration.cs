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
		builder.Property(x => x.Id).HasComment("<inheritdoc cref=\"IDatabaseEntity.Id\"/>");
		builder.Property(x => x.CreateTime).HasComment("<inheritdoc cref=\"IDatabaseEntity.CreateTime\"/>");
		builder.Property(x => x.UpdateTime).HasComment("<inheritdoc cref=\"IDatabaseEntity.UpdateTime\"/>");
		builder.Property(x => x.IsDeleted).HasComment("<inheritdoc cref=\"IDatabaseEntity.IsDeleted\"/>");
	}
}
