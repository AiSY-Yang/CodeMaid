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
		/// 所属项目
		/// </summary>
		public required Project Project { get; set; }
		/// <summary>
		/// 项目Id
		/// </summary>
		public long ProjectId { get; set; }
		/// <summary>
		/// 文件名
		/// </summary>
		public string Name { get; set; } = null!;
		/// <summary>
		/// 文件最后修改时间
		/// </summary>
		public DateTimeOffset LastWriteTime { get; set; }
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
		/// <summary>
		/// 项目结构
		/// </summary>
		public required List<ProjectStructure> ProjectStructures { get; set; }
		/// <summary>
		/// 关联枚举
		/// </summary>
		public required List<EnumDefinition> EnumDefinitions { get; set; }
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