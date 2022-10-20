using Models.CodeMaid;

namespace Models.CodeMaid
{
	/// <summary>
	/// 命名空间定义
	/// </summary>
	public class NameSpaceDefinition : DatabaseEntity
	{
		/// <summary>
		/// 命名空间名称
		/// </summary>
		public string Name { get; set; } = null!;
		/// <summary>
		/// 类列表
		/// </summary>
		public List<ClassDefinition> Classes { get; set; } = new();
	}
}