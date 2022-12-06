using Mapster;

using Models.CodeMaid;

namespace Models.CodeMaid
{
	/// <summary>
	/// 类定义
	/// </summary>
	public class AttributeDefinition : DatabaseEntity
	{
		/// <summary>
		/// Attribute名称
		/// </summary>
		public string Name { get; set; } = null!;
		/// <summary>
		/// Attribute文本
		/// </summary>
		public string Text { get; set; } = null!;
		/// <summary>
		/// 参数文本
		/// </summary>
		public string? ArgumentsText { get; set; } = null!;
		/// <summary>
		/// 参数
		/// </summary>
		public string? Arguments { get; set; } = null!;
		/// <summary>
		/// 包含的属性
		/// </summary>
		[AdaptIgnore]
		public PropertyDefinition PropertyDefinition { get; set; } = null!;
	}
}