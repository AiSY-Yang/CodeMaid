using Mapster;

using Models.CodeMaid;

namespace Models.CodeMaid
{
	/// <summary>
	/// 类定义
	/// </summary>
	public class ClassDefinition : ClassDefinitionBase
	{
		/// <summary>
		/// 所属项目
		/// </summary>
		public Project? Project { get; set; } = null!;
		/// <summary>
		/// 所属项目
		/// </summary>
		public long? ProjectId { get; set; }
	
		/// <summary>
		/// 属性列表
		/// </summary>
		[AdaptIgnore]
		public List<PropertyDefinition> Properties { get; set; } = new();
	}
}