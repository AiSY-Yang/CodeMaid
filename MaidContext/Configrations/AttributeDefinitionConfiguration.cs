using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Models.CodeMaid;
/// <summary>
/// 派生类的配置
/// </summary>
internal class AttributeDefinitionConfiguration : DatabaseEntityConfiguration<AttributeDefinition>
{
	public override void Configure(EntityTypeBuilder<AttributeDefinition> builder)
	{
		base.Configure(builder);
		ConfigureComment(builder);
	}
	/// <summary>
	/// Automatically generated comment configuration
	/// </summary>
	/// <param name="builder"></param>
	static void ConfigureComment(EntityTypeBuilder<AttributeDefinition> builder)
	{
		builder.Metadata.SetComment("属性定义");
		builder.Property(x => x.Name).HasComment("Attribute名称");
		builder.Property(x => x.Text).HasComment("Attribute文本");
		builder.Property(x => x.ArgumentsText).HasComment("参数文本");
		builder.Property(x => x.Arguments).HasComment("参数");
	}
}
