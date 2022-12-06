using Mapster;

using Models.CodeMaid;

namespace Models.CodeMaid
{
	/// <summary>
	/// 枚举成员定义
	/// </summary>
	public class EnumMemberDefinition : DatabaseEntity
	{
		/// <summary>
		/// 枚举名称
		/// </summary>
		public string Name { get; set; } = null!;
		/// <summary>
		/// 枚举值
		/// </summary>
		public int Value { get; set; }
		/// <summary>
		/// 注释
		/// </summary>
		public string? Summary { get; set; }
		/// <summary>
		/// 描述
		/// </summary>
		public string? Description { get; set; }
	}
}