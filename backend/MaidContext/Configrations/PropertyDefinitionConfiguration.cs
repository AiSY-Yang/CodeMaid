using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Models.CodeMaid;
/// <summary>
/// 派生类的配置
/// </summary>
internal class PropertyDefinitionConfiguration : DatabaseEntityConfiguration<PropertyDefinition>
{
	public override void Configure(EntityTypeBuilder<PropertyDefinition> builder)
	{
		base.Configure(builder);
		builder.HasIndex(x => new { x.Name, x.ClassDefinitionId }).IsUnique();
		ConfigureComment(builder);
	}
	/// <summary>
	/// Automatically generated comment configuration
	/// </summary>
	/// <param name="builder"></param>
	static void ConfigureComment(EntityTypeBuilder<PropertyDefinition> builder)
	{
		builder.Metadata.SetComment("类定义");
		builder.Property(x => x.ClassDefinitionId).HasComment("所属类Id");
		builder.Property(x => x.LeadingTrivia).HasComment("前导");
		builder.Property(x => x.Summary).HasComment("注释");
		builder.Property(x => x.Remark).HasComment("备注");
		builder.Property(x => x.FullText).HasComment("完整文本内容");
		builder.Property(x => x.Modifiers).HasComment("修饰符");
		builder.Property(x => x.Initializer).HasComment("初始化器");
		builder.Property(x => x.Name).HasComment("属性名称");
		builder.Property(x => x.Type).HasComment("数据类型");
		builder.Property(x => x.IsEnum).HasComment("是否是枚举");
		builder.Property(x => x.HasGet).HasComment("是否包含Get");
		builder.Property(x => x.Get).HasComment("Get方法体");
		builder.Property(x => x.HasSet).HasComment("是否包含Set");
		builder.Property(x => x.Set).HasComment("Set方法体");
	}
}
