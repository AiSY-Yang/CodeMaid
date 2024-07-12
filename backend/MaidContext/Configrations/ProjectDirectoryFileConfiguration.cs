using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Models.CodeMaid;
/// <summary>
/// 派生类的配置
/// </summary>
internal class ProjectDirectoryFileConfiguration : DatabaseEntityConfiguration<ProjectDirectoryFile>
{
	public override void Configure(EntityTypeBuilder<ProjectDirectoryFile> builder)
	{
		base.Configure(builder);
		ConfigureComment(builder);
	}
	/// <summary>
	/// Automatically generated comment configuration
	/// </summary>
	static void ConfigureComment(EntityTypeBuilder<ProjectDirectoryFile> builder)
	{
		builder.Metadata.SetComment("项目文件");
		builder.Property(x => x.Name).HasComment("文件名");
		builder.Property(x => x.Path).HasComment("路径");
		builder.Property(x => x.IsAutoGen).HasComment("是否是自动生成的文件");
		builder.Property(x => x.LinesCount).HasComment("总行数");
		builder.Property(x => x.SpaceCount).HasComment("空行数");
		builder.Property(x => x.CommentCount).HasComment("注释行数");
		builder.Property(x => x.FileType).HasComment("文件类型(0-其他,1-C#文件)");
		builder.Property(x => x.LastWriteTime).HasComment("文件最后修改时间");
		builder.Property(x => x.ProjectId).HasComment("项目Id");
	}
}
