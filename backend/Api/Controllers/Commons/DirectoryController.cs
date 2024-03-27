using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Commons;

/// <summary>
/// 文件夹信息
/// </summary>
[ApiController]
public class DirectoryController : ApiControllerBase
{
	/// <summary>
	/// 是否已存在
	/// </summary>
	/// <param name="path"></param>
	/// <returns></returns>
	[HttpGet("[action]")]
	public bool Exist(string path)
	{
		return Directory.Exists(path);
	}

	/// <summary>
	/// 创建目录
	/// </summary>
	/// <param name="path"></param>
	/// <returns></returns>
	[HttpPost("[action]")]
	public bool Create(string path)
	{
		Directory.CreateDirectory(path);
		return true;
	}

	/// <summary>
	/// 删除目录
	/// </summary>
	/// <param name="path"></param>
	/// <returns></returns>
	[HttpPost("[action]")]
	public bool Delete(string path)
	{
		if (Directory.Exists(path))
		{
			//由于nfs系统问题 尝试删除三次
			try
			{
				Directory.Delete(path, true);
			}
			catch (Exception)
			{
				try
				{
					Directory.Delete(path, true);
				}
				catch (Exception)
				{
					try
					{
						Directory.Delete(path, true);
					}
					catch (Exception)
					{
						throw;
					}
				}
			}
		}
		return true;
	}

	/// <summary>
	/// 获取目录大小
	/// </summary>
	/// <param name="path"></param>
	/// <returns></returns>
	[HttpGet("[action]")]
	public long Size(string path)
	{
		var directoryInfo = new DirectoryInfo(path);
		return GetDirectorySize(directoryInfo);
		long GetDirectorySize(DirectoryInfo directoryInfo)
		{
			var startDirectorySize = default(long);
			if (directoryInfo == null || !directoryInfo.Exists)
				return startDirectorySize; //Return 0 while Directory does not exist.

			//Add size of files in the Current Directory to main size.
			foreach (var fileInfo in directoryInfo.GetFiles())
			{
				var target = fileInfo.ResolveLinkTarget(true);
				Interlocked.Add(ref startDirectorySize, target switch
				{
					FileInfo file when target.Exists => file.Length,
					DirectoryInfo dir => GetDirectorySize(dir),
					null => fileInfo.Length,
					_ => 0,
				});
			}

			Parallel.ForEach(directoryInfo.GetDirectories(), (subDirectory) =>
		Interlocked.Add(ref startDirectorySize, GetDirectorySize(subDirectory)));

			return startDirectorySize;  //Return full Size of this Directory.
		}
	}
	/// <summary>
	/// 获取文件夹下的内容
	/// </summary>
	/// <param name="path">完整路径</param>
	/// <param name="needFile">是否需要返回文件 true返回文件和文件夹 false只返回文件</param>
	/// <param name="recursive">是否需要递归查子文件夹</param>
	/// <returns></returns>
	[HttpGet("[action]")]
	public SystemFolder Content(string path, bool needFile, bool recursive)
	{
		var dir = new DirectoryInfo(path);
		SystemFolder folder = new() { Name = dir.Name, Path = dir.FullName };
		if (needFile)
			foreach (var fileInfo in dir.GetFiles())
				folder.SystemFiles.Add(new SystemFile() { Name = fileInfo.Name, Path = fileInfo.FullName });
		if (recursive)
			foreach (var directoryInfo in dir.GetDirectories())
				folder.SystemFolders.Add(Content(directoryInfo.FullName, needFile, recursive));
		else
			foreach (var directoryInfo in dir.GetDirectories())
				folder.SystemFolders.Add(new SystemFolder() { Name = directoryInfo.Name, Path = directoryInfo.FullName });
		return folder;
	}

	/// <summary>
	/// 获取指定路径下的文件
	/// </summary>
	/// <param name="path"></param>
	/// <returns></returns>
	[HttpGet("[action]")]
	public string[] File(string path)
	{
		if (Directory.Exists(path))
		{
			return Directory.GetFiles(path);
		}
		else
		{
			return Array.Empty<string>();
		}
	}
	/// <summary>
	/// 移动文件夹
	/// </summary>
	/// <param name="path"></param>
	/// <param name="to"></param>
	/// <returns></returns>
	[HttpPut("[action]")]
	public string Move(string path, string to)
	{
		if (Directory.Exists(path))
			Directory.Move(path, to);
		return to;
	}
}