using System.Net.Mime;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Api.Controllers.Commons;
/// <summary>
/// 文件相关接口
/// </summary>
[ApiController]
[Route("[controller]")]
public class FileController : ApiControllerBase
{
	/// <summary>
	/// 
	/// </summary>
	private ILogger<FileController> logger;
	public FileController(ILogger<FileController> logger)
	{
		this.logger = logger;
	}
	/// <summary>
	/// 文件是否存在
	/// </summary>
	/// <returns></returns>
	[HttpGet("[action]")]
	public ActionResult<bool> Exist(string path)
	{
		return System.IO.File.Exists(path);
	}
	/// <summary>
	/// 文件大小
	/// </summary>
	/// <returns></returns>
	[HttpGet("[action]")]
	[ProducesResponseType(200)]
	[ProducesResponseType(204)]
	public long? Size(string path)
	{
		var file = new FileInfo(path);
		if (file.Exists)
			if (file.LinkTarget != null)
			{
				return (file.ResolveLinkTarget(true) as FileInfo)?.Length;
			}
			else
			{
				return file.Length;
			}
		return null;
	}
	/// <summary>
	/// 下载文件
	/// </summary>
	/// <param name="path"></param>
	/// <returns></returns>
	[HttpGet("[action]")]
	[ProducesResponseType(200)]
	[ProducesResponseType(404)]
	public ActionResult? Download(string path)
	{
		var file = new FileInfo(path);
		if (file.Exists)
		{
			if (file.LinkTarget != null)
			{
				file = file.ResolveLinkTarget(true) as FileInfo;
				if (file == null) return NotFound();
			}
		}
		else return NotFound();
		return File(System.IO.File.OpenRead(file.FullName), MediaTypeNames.Application.Octet);
	}

	/// <summary>
	/// 写入文本
	/// </summary>
	/// <param name="filePath">目录名</param>
	/// <param name="text">文件</param>
	/// <returns></returns>
	[HttpPost("[action]")]
	[RequestSizeLimit(long.MaxValue)]
	public async Task<string> WriteText([FromForm] string filePath, [FromForm] string text)
	{
		await System.IO.File.WriteAllTextAsync(filePath, text);
		return filePath;
	}
	/// <summary>
	/// 写入文件
	/// </summary>
	/// <param name="directory">目录名</param>
	/// <param name="file">文件</param>
	/// <returns></returns>
	[HttpPost("[action]")]
	[RequestSizeLimit(long.MaxValue)]
	public async Task<string> Write(string directory, IFormFile file)
	{
		Directory.CreateDirectory(directory);
		var filePath = Path.Combine(directory, file.FileName);
		using var stream = new FileStream(filePath, FileMode.Create);
		await file.CopyToAsync(stream);
		await stream.FlushAsync();
		return filePath;
	}

	/// <summary>
	/// 同步写入文件
	/// </summary>
	/// <param name="filePath">文件完整路径</param>
	/// <param name="_">文件</param>
	/// <returns></returns>
	[HttpPost("[action]")]
	[RequestSizeLimit(long.MaxValue)]
	public async Task<string> SyncWrite(string filePath, [BindNever] IFormFile file)
	{
		logger.LogInformation($"SyncWrite {filePath}");
		var stream = new FileStream(filePath, FileMode.Create);
		logger.LogInformation($"SyncWrite {filePath} stream created");
		await HttpContext.Request.Body.CopyToAsync(stream);
		logger.LogInformation($"SyncWrite {filePath} stream copied");
		await stream.FlushAsync();
		logger.LogInformation($"SyncWrite {filePath} stream flushed");
		stream.Dispose();
		logger.LogInformation($"SyncWrite {filePath} stream disposed");
		return filePath;
	}
	/// <summary>
	/// 文本内容
	/// </summary>
	/// <param name="filePath"></param>
	/// <returns></returns>
	[HttpGet("[action]")]
	[ProducesResponseType(200)]
	[ProducesResponseType(204)]
	public async Task<string?> TextContent(string filePath)
	{
		return System.IO.File.Exists(filePath) ? await System.IO.File.ReadAllTextAsync(filePath) : null;
	}
	/// <summary>
	/// 多个文件文本内容
	/// </summary>
	/// <param name="filePaths"></param>
	/// <returns></returns>
	[HttpGet("[action]")]
	public async Task<Dictionary<string, string?>> TextContents(string[] filePaths)
	{
		var result = new Dictionary<string, string?>();
		foreach (var filePath in filePaths.Distinct())
		{
			result.Add(filePath, System.IO.File.Exists(filePath) ? await System.IO.File.ReadAllTextAsync(filePath) : null);
		};
		return result;
	}
	/// <summary>
	/// 删除文件
	/// </summary>
	/// <param name="filePath"></param>
	/// <returns></returns>
	[HttpDelete("[action]")]
	public bool Delete(string filePath)
	{
		if (System.IO.File.Exists(filePath)) System.IO.File.Delete(filePath);
		return true;
	}
	/// <summary>
	/// 移动文件
	/// </summary>
	/// <param name="sourceFileName"></param>
	/// <param name="destinationFileName"></param>
	/// <returns></returns>
	[HttpPut("[action]")]
	public bool Move(string sourceFileName, string destinationFileName)
	{
		System.IO.File.Move(sourceFileName, destinationFileName);
		return true;
	}
	/// <summary>
	/// 移动文件
	/// </summary>
	/// <param name="sourceFileName"></param>
	/// <param name="destinationFileName"></param>
	/// <returns></returns>
	[HttpPut("[action]")]
	public bool MoveAndOverWrite(string sourceFileName, string destinationFileName)
	{
		System.IO.File.Move(sourceFileName, destinationFileName, true);
		return true;
	}

}
