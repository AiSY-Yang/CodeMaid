using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Models.DbContext;

internal class ClassDefinitionConfiguration : EntityBaseConfiguration<ClassDefinition>
{
	public override void Configure(EntityTypeBuilder<ClassDefinition> builder)
	{
		base.Configure(builder);
	}
}
