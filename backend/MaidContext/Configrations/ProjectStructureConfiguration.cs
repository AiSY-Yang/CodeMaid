using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Models.CodeMaid;
/// <summary>
/// 派生类的配置
/// </summary>
internal class ProjectStructureConfiguration : DatabaseEntityConfiguration<ProjectStructure>
{
	public override void Configure(EntityTypeBuilder<ProjectStructure> builder)
	{
		base.Configure(builder);
		ConfigureComment(builder);
	}
	/// <summary>
	/// Automatically generated comment configuration
	/// </summary>
	static void ConfigureComment(EntityTypeBuilder<ProjectStructure> builder)
	{
		builder.Metadata.SetComment("项目结构");
	}
}
