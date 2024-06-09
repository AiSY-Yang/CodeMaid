using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Models.CodeMaid;
/// <summary>
/// 派生类的配置
/// </summary>
internal class EnumDefinitionConfiguration : DatabaseEntityConfiguration<EnumDefinition>
{
	public override void Configure(EntityTypeBuilder<EnumDefinition> builder)
	{
		base.Configure(builder);
		ConfigureComment(builder);
	}
	/// <summary>
	/// Automatically generated comment configuration
	/// </summary>
	/// <param name="builder"></param>
	static void ConfigureComment(EntityTypeBuilder<EnumDefinition> builder)
	{
		builder.Metadata.SetComment("枚举定义");
		builder.Property(x => x.NameSpace).HasComment("命名空间");
		builder.Property(x => x.Name).HasComment("枚举名");
		builder.Property(x => x.Summary).HasComment("注释");
		builder.Property(x => x.LeadingTrivia).HasComment("前导");
		builder.Property(x => x.ProjectDirectoryFileId);
	}
}
