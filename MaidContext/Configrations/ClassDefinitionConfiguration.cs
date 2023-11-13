using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Models.CodeMaid;
/// <summary>
/// 派生类的配置
/// </summary>
internal class ClassDefinitionConfiguration : DatabaseEntityConfiguration<ClassDefinition>
{
	public override void Configure(EntityTypeBuilder<ClassDefinition> builder)
	{
		base.Configure(builder);
		builder.HasIndex(x => new { x.Name, x.MaidId }).IsUnique();
		ConfigureComment(builder);
	}
	/// <summary>
	/// Automatically generated comment configuration
	/// </summary>
	/// <param name="builder"></param>
	static void ConfigureComment(EntityTypeBuilder<ClassDefinition> builder)
	{
		builder.Metadata.SetComment("类定义");
		builder.Property(x => x.MaidId).HasComment("Maid对象Id");
		builder.Property(x => x.NameSpace).HasComment("命名空间");
		builder.Property(x => x.Modifiers).HasComment("修饰符");
		builder.Property(x => x.Name).HasComment("类名");
		builder.Property(x => x.Summary).HasComment("注释");
		builder.Property(x => x.Base).HasComment("基类或者接口名称");
		builder.Property(x => x.Using).HasComment("类引用的命名空间");
		builder.Property(x => x.LeadingTrivia).HasComment("前导");
		builder.Property(x => x.MemberType).HasComment("成员类型(0-类,1-接口,2-记录,3-结构体)");
	}
}
