using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Models.CodeMaid;
/// <summary>
/// 派生类的配置
/// </summary>
internal class ProjectDirectoryConfiguration : DatabaseEntityConfiguration<ProjectDirectory>
{
	public override void Configure(EntityTypeBuilder<ProjectDirectory> builder)
	{
		base.Configure(builder);
		ConfigureComment(builder);
	}
	/// <summary>
	/// Automatically generated comment configuration
	/// </summary>
	static void ConfigureComment(EntityTypeBuilder<ProjectDirectory> builder)
	{
		builder.Metadata.SetComment("项目目录");
		builder.Property(x => x.Name).HasComment("目录名");
		builder.Property(x => x.Path).HasComment("路径");
		builder.Property(x => x.ProjectId).HasComment("项目Id");
	}
}
