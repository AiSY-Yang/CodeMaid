using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Models.CodeMaid;
/// <summary>
/// 派生类的配置
/// </summary>
/// <typeparam name="Entity"></typeparam>
internal abstract class PropertyDefinitionConfiguration : DatabaseEntityConfiguration<PropertyDefinition>
{
	public override void Configure(EntityTypeBuilder<PropertyDefinition> builder)
	{
		base.Configure(builder);
		builder.HasComment("类定义");
		builder.Property(x => x.ClassDefinition).HasComment("");
		builder.Property(x => x.LeadingTrivia).HasComment("前导");
		builder.Property(x => x.Summary).HasComment("注释");
		builder.Property(x => x.FullText).HasComment("完整文本内容");
		builder.Property(x => x.Modifiers).HasComment("修饰符");
		builder.Property(x => x.Initializer).HasComment("初始化器");
		builder.Property(x => x.Name).HasComment("属性名称").HasMaxLength(20);
		builder.Property(x => x.Get).HasComment("Get方法体");
		builder.Property(x => x.Set).HasComment("Set方法体");
		builder.Property(x => x.Attributes).HasComment("属性列表");
	}
}
