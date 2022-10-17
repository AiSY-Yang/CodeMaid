using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Models.DbContext;

internal class FieldDefinitionConfiguration : EntityBaseConfiguration<PropertyDefinition>
{
	public override void Configure(EntityTypeBuilder<PropertyDefinition> builder)
	{
		base.Configure(builder);
	}
}
