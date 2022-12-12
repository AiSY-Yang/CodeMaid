using System.ComponentModel.DataAnnotations;

using Models.CodeMaid;

namespace Models.CodeMaid
{
	/// <summary>
	/// 类定义
	/// </summary>
	public class PropertyDefinition : DatabaseEntity
	{
		/// <summary>
		/// 所属类
		/// </summary>
		public ClassDefinition ClassDefinition { get; set; } = null!;
		/// <summary>
		/// 前导
		/// </summary>
		public string? LeadingTrivia { get; set; }
		/// <summary>
		/// 注释
		/// </summary>
		public string? Summary { get; set; }
		/// <summary>
		/// 备注
		/// </summary>
		public string? Remark { get; set; } = null!;
		/// <summary>
		/// 完整文本内容
		/// </summary>
		public string FullText { get; set; } = null!;
		/// <summary>
		/// 修饰符
		/// </summary>
		public string Modifiers { get; set; } = null!;
		/// <summary>
		/// 初始化器
		/// </summary>
		public string? Initializer { get; set; }
		/// <summary>
		/// 属性名称
		/// </summary>
		public string Name { get; set; } = null!;
		/// <summary>
		/// 数据类型
		/// </summary>
		public string Type { get; set; } = null!;
		/// <summary>
		/// 是否是枚举
		/// </summary>
		public bool IsEnum { get; set; }
		/// <summary>
		/// 是否包含Get
		/// </summary>
		public bool HasGet { get; set; }
		/// <summary>
		/// Get方法体
		/// </summary>
		public string? Get { get; set; } = null!;
		/// <summary>
		/// 是否包含Set
		/// </summary>
		public bool HasSet { get; set; }
		/// <summary>
		/// Set方法体
		/// </summary>
		public string? Set { get; set; } = null!;
		/// <summary>
		/// 属性列表
		/// </summary>
		public List<AttributeDefinition> Attributes { get; set; } = null!;
	}
}