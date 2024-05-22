using Mapster;

using Models.CodeMaid;

namespace Models.CodeMaid
{
	/// <summary>
	/// 项目目录
	/// </summary>
	public class ProjectDirectory : DatabaseEntity
	{
		/// <summary>
		/// 目录名
		/// </summary>
		public string Name { get; set; } = null!;
		/// <summary>
		/// 路径
		/// </summary>
		public string Path { get; set; } = null!;
		/// <summary>
		/// 项目
		/// </summary>
		public Project Project { get; set; } = null!;
		/// <summary>
		/// 目录
		/// </summary>
		public List<ProjectDirectoryFile> ProjectDirectoryFiles { get; set; } = null!;
	}
}