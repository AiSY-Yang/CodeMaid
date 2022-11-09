using Mapster;

using Models.CodeMaid;

namespace Models.CodeMaid
{
	/// <summary>
	/// 功能
	/// </summary>
	public class Maid : DatabaseEntity
	{
		/// <summary>
		/// 名称
		/// </summary>
		public string Name { get; set; } = null!;
		public Project Project { get; set; } = null!;
		/// <summary>
		/// 功能
		/// </summary>
		public MaidWork MaidWork { get; set; }
		/// <summary>
		/// 原路径
		/// </summary>
		public string SourcePath { get; set; } = null!;
		/// <summary>
		/// 目标路径
		/// </summary>
		public string DestinationPath { get; set; } = null!;
		/// <summary>
		/// 是否自动修复
		/// </summary>
		public bool Autonomous { get; set; }
		/// <summary>
		/// 序列化保存的设置
		/// </summary>
		public string? Setting { get; set; } = null!;
		[AdaptIgnore]
		public List<ClassDefinition> Classes { get; set; } = null!;
	}
}