using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Models.CodeMaid;
/// <summary>
/// 派生类的配置
/// </summary>
/// <typeparam name="Entity"></typeparam>
internal class ProjectConfiguration : DatabaseEntityConfiguration<Project>
{
	public override void Configure(EntityTypeBuilder<Project> builder)
	{
		base.Configure(builder);
		builder.Metadata.SetComment("项目定义");
		builder.HasComment("项目定义");
		builder.Property(x => x.Name).HasComment("项目名");
		builder.Property(x => x.Path).HasComment("项目路径");
		builder.Property(x => x.GitBranch).HasComment("Git分支");
	}
}
