using Models.Database;

namespace Models.DbContext
{
	/// <summary>
	/// 类定义
	/// </summary>
	public class ClassDefinition : DatabaseEntity
	{
		/// <summary>
		/// 命名空间定义
		/// </summary>
		public NameSpaceDefinition NameSpaceDefinition { get; set; } = null!;
		public string Name { get; set; } = null!;
		public string? Base { get; set; } = null!;
		public List<PropertyDefinition> Properties { get; set; } = new();
	}
}