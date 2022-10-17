using Models.Database;

namespace Models.DbContext
{
	/// <summary>
	/// 命名空间定义
	/// </summary>
	public class NameSpaceDefinition : DatabaseEntity
	{
		/// <summary>
		/// 名称
		/// </summary>
		public string Name { get; set; } = null!;
		/// <summary>
		/// 类
		/// </summary>
		public List<ClassDefinition> Classes { get; set; } = new();
	}
}