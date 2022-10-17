using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Models.DbContext;

internal class NameSpaceDefinitionConfiguration : EntityBaseConfiguration<NameSpaceDefinition>
{
	public override void Configure(EntityTypeBuilder<NameSpaceDefinition> builder)
	{
		base.Configure(builder);
	}
}
