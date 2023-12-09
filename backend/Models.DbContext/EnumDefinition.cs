using Mapster;

using Models.CodeMaid;

namespace Models.CodeMaid
{
	/// <summary>
	/// 枚举定义
	/// </summary>
	public class EnumDefinition : DatabaseEntity
	{
		/// <summary>
		/// 命名空间
		/// </summary>
		public string? NameSpace { get; set; } = null!;
		/// <summary>
		/// 枚举名
		/// </summary>
		public string Name { get; set; } = null!;
		/// <summary>
		/// 注释
		/// </summary>
		public string? Summary { get; set; } = null!;
		/// <summary>
		/// 备注
		/// </summary>
		public string Remark { get => string.Join(',', EnumMembers.Where(x => !x.IsDeleted).Select(x => $"{x.Value}-{x.Description ?? x.Summary ?? x.Name}")); }
		/// <summary>
		/// 前导
		/// </summary>
		public string? LeadingTrivia { get; set; }
		/// <summary>
		/// 成员列表
		/// </summary>
		[AdaptIgnore]
		public List<EnumMemberDefinition> EnumMembers { get; set; } = new();
	}
}