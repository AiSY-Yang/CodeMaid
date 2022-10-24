using Models.CodeMaid;

namespace Models.CodeMaid
{
	/// <summary>
	/// 类定义
	/// </summary>
	public class ClassDefinition : DatabaseEntity
	{
		/// <summary>
		/// 命名空间
		/// </summary>
		public string? NameSpace { get; set; } = null!;
		/// <summary>
		/// 类名
		/// </summary>
		public string Name { get; set; } = null!;
		/// <summary>
		/// 注释
		/// </summary>
		public string? Summary { get; set; } = null!;
		/// <summary>
		/// 基类或者接口名称
		/// </summary>
		public string? Base { get; set; } = null!;
		/// <summary>
		/// 属性列表
		/// </summary>
		public List<PropertyDefinition> Properties { get; set; } = new();
	}
}