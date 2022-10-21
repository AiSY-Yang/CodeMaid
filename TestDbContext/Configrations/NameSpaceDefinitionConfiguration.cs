using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Models.CodeMaid;
/// <summary>
/// 派生类的配置
/// </summary>
/// <typeparam name="Entity"></typeparam>
internal class NameSpaceDefinitionConfiguration : DatabaseEntityConfiguration<NameSpaceDefinition>
{
	public override void Configure(EntityTypeBuilder<NameSpaceDefinition> builder)
	{
		base.Configure(builder);
		builder.HasComment("命名空间定义");
		builder.Property(x => x.Name).HasComment("命名空间名称");
	}
}
