using Models.Database;

namespace Models.DbContext
{
	/// <summary>
	/// 类定义
	/// </summary>
	public class PropertyDefinition : DatabaseEntity
	{
		public ClassDefinition ClassDefinition { get; set; } = null!;
		public string? LeadingTrivia { get; set; }
		public string? Summary { get; set; }
		public string FullText { get; set; } = null!;
		public string Modifiers { get; set; } = null!;
		public string? Initializer { get; set; }
		public string Name { get; set; } = null!;
		public string? Get { get; set; } = null!;
		public string? Set { get; set; } = null!;
	}
}