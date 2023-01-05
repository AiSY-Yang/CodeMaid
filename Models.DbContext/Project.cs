using Mapster;

using Models.CodeMaid;

namespace Models.CodeMaid
{
	/// <summary>
	/// 项目定义
	/// </summary>
	public class Project : DatabaseEntity
	{
		/// <summary>
		/// 项目名
		/// </summary>
		public string Name { get; set; } = null!;
		/// <summary>
		/// 项目路径
		/// </summary>
		public string Path { get; set; } = null!;
		/// <summary>
		/// Git分支
		/// </summary>
		public string GitBranch { get; set; } = null!;
		/// <summary>
		/// 是否添加枚举的remark信息
		/// </summary>
		public bool AddEnumRemark { get; set; }
		/// <summary>
		/// maid集合
		/// </summary>
		[AdaptIgnore]
		public List<Maid> Maids { get; set; } = null!;
	}
}