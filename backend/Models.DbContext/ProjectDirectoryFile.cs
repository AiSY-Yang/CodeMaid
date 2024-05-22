using Mapster;

using Models.CodeMaid;

namespace Models.CodeMaid
{
	/// <summary>
	/// 项目文件
	/// </summary>
	public class ProjectDirectoryFile : DatabaseEntity
	{
		/// <summary>
		/// 文件名
		/// </summary>
		public string Name { get; set; } = null!;
		/// <summary>
		/// 路径
		/// </summary>
		public string Path { get; set; } = null!;
		/// <summary>
		/// 项目目录
		/// </summary>
		public required ProjectDirectory ProjectDirectory { get; set; }
		/// <summary>
		/// 是否是自动生成的文件
		/// </summary>
		public bool IsAutoGen { get; set; }
		/// <summary>
		/// 总行数
		/// </summary>
		public int LinesCount { get; set; }
		/// <summary>
		/// 空行数
		/// </summary>
		public int SpaceCount { get; set; }
		/// <summary>
		/// 注释行数
		/// </summary>
		public int CommentCount { get; set; }
		/// <summary>
		/// 文件类型
		/// </summary>
		public required FileType FileType { get; set; }
	}

	/// <summary>
	/// 文件类型
	/// </summary>
	/// <remarks>0-其他,1-C#文件</remarks>
	public enum FileType
	{
		/// <summary>
		/// 其他
		/// </summary>
		Other,
		/// <summary>
		/// C#文件
		/// </summary>
		CSahrp,
	}
}