using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Models.CodeMaid;
/// <summary>
/// 派生类的配置
/// </summary>
internal class MaidConfiguration : DatabaseEntityConfiguration<Maid>
{
	public override void Configure(EntityTypeBuilder<Maid> builder)
	{
		base.Configure(builder);
		ConfigureComment(builder);
	}
	/// <summary>
	/// Automatically generated comment configuration
	/// </summary>
	/// <param name="builder"></param>
	static void ConfigureComment(EntityTypeBuilder<Maid> builder)
	{
		builder.Metadata.SetComment("功能");
		builder.Property(x => x.Name).HasComment("名称");
		builder.Property(x => x.MaidWork).HasComment("功能(0-配置同步功能,1-DTO同步,2-HTTP客户端生成,3-controller同步,4-生成 Masstransit Consumer)");
		builder.Property(x => x.SourcePath).HasComment("原路径");
		builder.Property(x => x.DestinationPath).HasComment("目标路径");
		builder.Property(x => x.Autonomous).HasComment("是否自动修复");
		builder.Property(x => x.Setting).HasComment("设置");
		builder.Property(x => x.ProjectId);
	}
}
