using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Models.CodeMaid;
/// <summary>
/// 派生类的配置
/// </summary>
/// <typeparam name="Entity"></typeparam>
internal abstract class PropertyDefinitionConfiguration : DatabaseEntityConfiguration<PropertyDefinition>
{
	public override void Configure(EntityTypeBuilder<PropertyDefinition> builder)
	{
		base.Configure(builder);
		builder.Property(x => x.ClassDefinition).HasComment("");
		builder.Property(x => x.LeadingTrivia).HasComment("");
		builder.Property(x => x.Summary).HasComment("");
		builder.Property(x => x.FullText).HasComment("");
		builder.Property(x => x.Modifiers).HasComment("");
		builder.Property(x => x.Initializer).HasComment("");
		builder.Property(x => x.Name).HasComment("");
		builder.Property(x => x.Get).HasComment("");
		builder.Property(x => x.Set).HasComment("");
	}
}
