namespace Api.Controllers.Commons;

/// <summary>
/// 文件夹
/// </summary>
public class SystemFolder : SystemFile
{
	/// <summary>
	/// 子文件夹
	/// </summary>
	public List<SystemFolder> SystemFolders { get; set; } = new();
	/// <summary>
	/// 文件
	/// </summary>
	public List<SystemFile> SystemFiles { get; set; } = new();
}
/// <summary>
/// 文件
/// </summary>
public class SystemFile
{
	/// <summary>
	/// 名称
	/// </summary>
	public required string Name { get; set; }
	/// <summary>
	/// 完整路径
	/// </summary>
	public required string Path { get; set; }
}