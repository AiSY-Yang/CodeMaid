using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Models.CodeMaid;
/// <summary>
/// 派生类的配置
/// </summary>
/// <typeparam name="Entity"></typeparam>
internal abstract class NameSpaceDefinitionConfiguration : DatabaseEntityConfiguration<NameSpaceDefinition>
{
	public override void Configure(EntityTypeBuilder<NameSpaceDefinition> builder)
	{
		base.Configure(builder);
		builder.Property(x => x.Name).HasComment("名称");
		builder.Property(x => x.Classes).HasComment("类");
	}
}
