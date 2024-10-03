using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Models.CodeMaid;
/// <summary>
/// 派生类的配置
/// </summary>
internal class ClassDefinitionConfiguration : ClassDefinitionBaseConfiguration<ClassDefinition>
{
	public override void Configure(EntityTypeBuilder<ClassDefinition> builder)
	{
		base.Configure(builder);
		ConfigureComment(builder);
	}
	/// <summary>
	/// Automatically generated comment configuration
	/// </summary>
	/// <param name="builder"></param>
	static void ConfigureComment(EntityTypeBuilder<ClassDefinition> builder)
	{
		builder.Metadata.SetComment("类定义");
		builder.Property(x => x.ProjectId).HasComment("所属项目");
	}
}
