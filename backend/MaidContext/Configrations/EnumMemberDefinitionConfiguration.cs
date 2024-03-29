using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Models.CodeMaid;
/// <summary>
/// 派生类的配置
/// </summary>
internal class EnumMemberDefinitionConfiguration : DatabaseEntityConfiguration<EnumMemberDefinition>
{
	public override void Configure(EntityTypeBuilder<EnumMemberDefinition> builder)
	{
		base.Configure(builder);
		ConfigureComment(builder);
	}
	/// <summary>
	/// Automatically generated comment configuration
	/// </summary>
	/// <param name="builder"></param>
	static void ConfigureComment(EntityTypeBuilder<EnumMemberDefinition> builder)
	{
		builder.Metadata.SetComment("枚举成员定义");
		builder.Property(x => x.Name).HasComment("枚举名称");
		builder.Property(x => x.Value).HasComment("枚举值");
		builder.Property(x => x.Summary).HasComment("注释");
		builder.Property(x => x.Description).HasComment("描述");
	}
}
