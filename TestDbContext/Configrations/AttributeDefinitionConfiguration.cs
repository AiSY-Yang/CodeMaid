using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Models.CodeMaid;
/// <summary>
/// 派生类的配置
/// </summary>
/// <typeparam name="Entity"></typeparam>
internal abstract class AttributeDefinitionConfiguration : DatabaseEntityConfiguration<AttributeDefinition>
{
	public override void Configure(EntityTypeBuilder<AttributeDefinition> builder)
	{
		base.Configure(builder);
		builder.HasComment("类定义");
		builder.Property(x => x.Name).HasComment("Attribute名称");
		builder.Property(x => x.Text).HasComment("Attribute文本");
		builder.Property(x => x.ArgumentsText).HasComment("参数文本");
		builder.Property(x => x.Arguments).HasComment("参数");
	}
}
