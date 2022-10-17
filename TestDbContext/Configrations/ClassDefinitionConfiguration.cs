using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Models.CodeMaid;
/// <summary>
/// 派生类的配置
/// </summary>
/// <typeparam name="Entity"></typeparam>
internal abstract class ClassDefinitionConfiguration : DatabaseEntityConfiguration<ClassDefinition>
{
	public override void Configure(EntityTypeBuilder<ClassDefinition> builder)
	{
		base.Configure(builder);
		builder.Property(x => x.NameSpaceDefinition).HasComment("命名空间定义");
		builder.Property(x => x.Name).HasComment("");
		builder.Property(x => x.Base).HasComment("");
		builder.Property(x => x.Properties).HasComment("");
	}
}
