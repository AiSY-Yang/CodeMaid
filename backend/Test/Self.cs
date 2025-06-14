using System.CodeDom.Compiler;
using System.Net.Http.Json;
using System.Text.Encodings.Web;
using System.Text.Json;
using RestfulClient.SelfModel;


using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace RestfulClient;

public partial class Self
{
	private readonly HttpClient httpClient;
	private readonly ILogger<Self> logger;

	public Self(HttpClient httpClient, ILogger<Self> logger, IOptions<SelfOptions> options)
	{
		var url = options.Value.Url;
		var timeout = options.Value.TimeoutSecond;
		httpClient.BaseAddress = new Uri(url);
		httpClient.Timeout = TimeSpan.FromSeconds(timeout);
		this.httpClient = httpClient;
		this.logger = logger;
	}
	public async Task<T> GetResult<T>(HttpRequestMessage request, HttpResponseMessage response)
	{
		try
		{
			response.EnsureSuccessStatusCode();
			return (await response.Content.ReadFromJsonAsync<T>())!;
		}
		catch (HttpRequestException ex)
		{
			logger.LogError(ex, "{name}请求失败:请求地址{address},状态码{statusCode},参数{body}响应{s2}"
				, nameof(Self)
				, request.RequestUri
				, response.StatusCode
				, request.Content == null ? null : await request.Content.ReadAsStringAsync()
				, await response.Content.ReadAsStringAsync());
			throw;
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "{name}请求失败:,请求地址{address},状态码{statusCode},参数{body}响应{s2}"
				, nameof(Self)
				, request.RequestUri
				, response.StatusCode
				, request.Content == null ? null : await request.Content.ReadAsStringAsync()
				, await response.Content.ReadAsStringAsync());
			throw;
		}
	}
	/// <summary>
	/// 
	/// </summary>
	/// <returns></returns>
	[GeneratedCode("codeMaid", "1.0.0")]
	public async Task<string> Mainb624()
	{
		var httpRequestMessage = new HttpRequestMessage()
		{
			Method = HttpMethod.Get,
			RequestUri = new Uri($"version", UriKind.Relative),
		};
		var response = await httpClient.SendAsync(httpRequestMessage);
		
		return await response.Content.ReadAsStringAsync();
	}
	/// <summary>
	/// 命令行
	/// </summary>
	/// <returns></returns>
	/// <param name="body"></param>
	[GeneratedCode("codeMaid", "1.0.0")]
	public async Task<TaskServiceControllerCommandControllerCommandOutPut> CommandCommand(TaskServiceControllerCommandControllerCommandInfo? body)
	{
var content = JsonContent.Create(body);
		var httpRequestMessage = new HttpRequestMessage()
		{
			Method = HttpMethod.Post,
			RequestUri = new Uri($"api/Command/Command", UriKind.Relative),
			Content = content,
		};
		var response = await httpClient.SendAsync(httpRequestMessage);
		
		return await GetResult<TaskServiceControllerCommandControllerCommandOutPut>(httpRequestMessage, response);
	}
	/// <summary>
	/// 是否已存在
	/// </summary>
	/// <returns></returns>
	/// <param name="path"></param>
	[GeneratedCode("codeMaid", "1.0.0")]
	public async Task<bool> DirectoryExist(string? path)
	{
		var queryParameters = new List<string>();
		if (path != null) queryParameters.Add($"path={path}");
		var httpRequestMessage = new HttpRequestMessage()
		{
			Method = HttpMethod.Get,
			RequestUri = new Uri($"api/Directory/Exist{(queryParameters.Count > 0 ? "?" + string.Join('&', queryParameters) : "")}", UriKind.Relative),
		};
		var response = await httpClient.SendAsync(httpRequestMessage);
		
		return await GetResult<bool>(httpRequestMessage, response);
	}
	/// <summary>
	/// 创建目录
	/// </summary>
	/// <returns></returns>
	/// <param name="path"></param>
	[GeneratedCode("codeMaid", "1.0.0")]
	public async Task<bool> DirectoryCreate(string? path)
	{
		var queryParameters = new List<string>();
		if (path != null) queryParameters.Add($"path={path}");
		var httpRequestMessage = new HttpRequestMessage()
		{
			Method = HttpMethod.Post,
			RequestUri = new Uri($"api/Directory/Create{(queryParameters.Count > 0 ? "?" + string.Join('&', queryParameters) : "")}", UriKind.Relative),
		};
		var response = await httpClient.SendAsync(httpRequestMessage);
		
		return await GetResult<bool>(httpRequestMessage, response);
	}
	/// <summary>
	/// 删除目录
	/// </summary>
	/// <returns></returns>
	/// <param name="path"></param>
	[GeneratedCode("codeMaid", "1.0.0")]
	public async Task<bool> DirectoryDelete(string? path)
	{
		var queryParameters = new List<string>();
		if (path != null) queryParameters.Add($"path={path}");
		var httpRequestMessage = new HttpRequestMessage()
		{
			Method = HttpMethod.Post,
			RequestUri = new Uri($"api/Directory/Delete{(queryParameters.Count > 0 ? "?" + string.Join('&', queryParameters) : "")}", UriKind.Relative),
		};
		var response = await httpClient.SendAsync(httpRequestMessage);
		
		return await GetResult<bool>(httpRequestMessage, response);
	}
	/// <summary>
	/// 获取目录大小
	/// </summary>
	/// <returns></returns>
	/// <param name="path"></param>
	[GeneratedCode("codeMaid", "1.0.0")]
	public async Task<long> DirectorySize(string? path)
	{
		var queryParameters = new List<string>();
		if (path != null) queryParameters.Add($"path={path}");
		var httpRequestMessage = new HttpRequestMessage()
		{
			Method = HttpMethod.Get,
			RequestUri = new Uri($"api/Directory/Size{(queryParameters.Count > 0 ? "?" + string.Join('&', queryParameters) : "")}", UriKind.Relative),
		};
		var response = await httpClient.SendAsync(httpRequestMessage);
		
		return await GetResult<long>(httpRequestMessage, response);
	}
	/// <summary>
	/// 获取文件夹下的内容
	/// </summary>
	/// <returns></returns>
	/// <param name="path">完整路径</param>
	/// <param name="needFile">是否需要返回文件 true返回文件和文件夹 false只返回文件</param>
	/// <param name="recursive">是否需要递归查子文件夹</param>
	[GeneratedCode("codeMaid", "1.0.0")]
	public async Task<ApiControllersCommonsSystemFolder> DirectoryContent(string? path, bool? needFile, bool? recursive)
	{
		var queryParameters = new List<string>();
		if (path != null) queryParameters.Add($"path={path}");
		if (needFile != null) queryParameters.Add($"needFile={needFile}");
		if (recursive != null) queryParameters.Add($"recursive={recursive}");
		var httpRequestMessage = new HttpRequestMessage()
		{
			Method = HttpMethod.Get,
			RequestUri = new Uri($"api/Directory/Content{(queryParameters.Count > 0 ? "?" + string.Join('&', queryParameters) : "")}", UriKind.Relative),
		};
		var response = await httpClient.SendAsync(httpRequestMessage);
		
		return await GetResult<ApiControllersCommonsSystemFolder>(httpRequestMessage, response);
	}
	/// <summary>
	/// 获取指定路径下的文件
	/// </summary>
	/// <returns></returns>
	/// <param name="path"></param>
	[GeneratedCode("codeMaid", "1.0.0")]
	public async Task<List<string>> DirectoryFile(string? path)
	{
		var queryParameters = new List<string>();
		if (path != null) queryParameters.Add($"path={path}");
		var httpRequestMessage = new HttpRequestMessage()
		{
			Method = HttpMethod.Get,
			RequestUri = new Uri($"api/Directory/File{(queryParameters.Count > 0 ? "?" + string.Join('&', queryParameters) : "")}", UriKind.Relative),
		};
		var response = await httpClient.SendAsync(httpRequestMessage);
		
		return await GetResult<List<string>>(httpRequestMessage, response);
	}
	/// <summary>
	/// 移动文件夹
	/// </summary>
	/// <returns></returns>
	/// <param name="path"></param>
	/// <param name="to"></param>
	[GeneratedCode("codeMaid", "1.0.0")]
	public async Task<string> DirectoryMove(string? path, string? to)
	{
		var queryParameters = new List<string>();
		if (path != null) queryParameters.Add($"path={path}");
		if (to != null) queryParameters.Add($"to={to}");
		var httpRequestMessage = new HttpRequestMessage()
		{
			Method = HttpMethod.Put,
			RequestUri = new Uri($"api/Directory/Move{(queryParameters.Count > 0 ? "?" + string.Join('&', queryParameters) : "")}", UriKind.Relative),
		};
		var response = await httpClient.SendAsync(httpRequestMessage);
		
		return await response.Content.ReadAsStringAsync();
	}
	/// <summary>
	/// 复制文件夹
	/// </summary>
	/// <returns></returns>
	/// <param name="path"></param>
	/// <param name="to"></param>
	[GeneratedCode("codeMaid", "1.0.0")]
	public async Task<string> DirectoryCopy(string? path, string? to)
	{
		var queryParameters = new List<string>();
		if (path != null) queryParameters.Add($"path={path}");
		if (to != null) queryParameters.Add($"to={to}");
		var httpRequestMessage = new HttpRequestMessage()
		{
			Method = HttpMethod.Put,
			RequestUri = new Uri($"api/Directory/Copy{(queryParameters.Count > 0 ? "?" + string.Join('&', queryParameters) : "")}", UriKind.Relative),
		};
		var response = await httpClient.SendAsync(httpRequestMessage);
		
		return await response.Content.ReadAsStringAsync();
	}
	/// <summary>
	/// 获取指定目录的压缩包
	/// </summary>
	/// <returns></returns>
	/// <param name="path"></param>
	[GeneratedCode("codeMaid", "1.0.0")]
	public async Task<Stream> DirectoryZip(string? path)
	{
		var queryParameters = new List<string>();
		if (path != null) queryParameters.Add($"path={path}");
		var httpRequestMessage = new HttpRequestMessage()
		{
			Method = HttpMethod.Get,
			RequestUri = new Uri($"api/Directory/Zip{(queryParameters.Count > 0 ? "?" + string.Join('&', queryParameters) : "")}", UriKind.Relative),
		};
		var response = await httpClient.SendAsync(httpRequestMessage, HttpCompletionOption.ResponseHeadersRead);
		
		return await response.Content.ReadAsStreamAsync();
	}
	/// <summary>
	/// 文件是否存在
	/// </summary>
	/// <returns></returns>
	/// <param name="path"></param>
	[GeneratedCode("codeMaid", "1.0.0")]
	public async Task<bool> FileExist(string? path)
	{
		var queryParameters = new List<string>();
		if (path != null) queryParameters.Add($"path={path}");
		var httpRequestMessage = new HttpRequestMessage()
		{
			Method = HttpMethod.Get,
			RequestUri = new Uri($"File/Exist{(queryParameters.Count > 0 ? "?" + string.Join('&', queryParameters) : "")}", UriKind.Relative),
		};
		var response = await httpClient.SendAsync(httpRequestMessage);
		
		return await GetResult<bool>(httpRequestMessage, response);
	}
	/// <summary>
	/// 文件大小
	/// </summary>
	/// <returns></returns>
	/// <param name="path"></param>
	[GeneratedCode("codeMaid", "1.0.0")]
	public async Task<long?> FileSize(string? path)
	{
		var queryParameters = new List<string>();
		if (path != null) queryParameters.Add($"path={path}");
		var httpRequestMessage = new HttpRequestMessage()
		{
			Method = HttpMethod.Get,
			RequestUri = new Uri($"File/Size{(queryParameters.Count > 0 ? "?" + string.Join('&', queryParameters) : "")}", UriKind.Relative),
		};
		var response = await httpClient.SendAsync(httpRequestMessage);
		if (response.StatusCode == System.Net.HttpStatusCode.NoContent) return null;
		return await GetResult<long>(httpRequestMessage, response);
	}
	/// <summary>
	/// 下载文件
	/// </summary>
	/// <returns></returns>
	/// <param name="path"></param>
	[GeneratedCode("codeMaid", "1.0.0")]
	public async Task<Stream?> FileDownload(string? path)
	{
		var queryParameters = new List<string>();
		if (path != null) queryParameters.Add($"path={path}");
		var httpRequestMessage = new HttpRequestMessage()
		{
			Method = HttpMethod.Get,
			RequestUri = new Uri($"File/Download{(queryParameters.Count > 0 ? "?" + string.Join('&', queryParameters) : "")}", UriKind.Relative),
		};
		var response = await httpClient.SendAsync(httpRequestMessage, HttpCompletionOption.ResponseHeadersRead);
		if (response.StatusCode == System.Net.HttpStatusCode.NoContent) return null;
		return await response.Content.ReadAsStreamAsync();
	}
	/// <summary>
	/// 写入文本
	/// </summary>
	/// <returns></returns>
	/// <param name="filePath"></param>
	/// <param name="text"></param>
	[GeneratedCode("codeMaid", "1.0.0")]
	public async Task<string> FileWriteText(string? filePath, string? text)
	{
		var content = new MultipartFormDataContent();
		if (filePath is not null) content.Add(new StringContent(filePath), nameof(filePath));
		if (text is not null) content.Add(new StringContent(text), nameof(text));
		var httpRequestMessage = new HttpRequestMessage()
		{
			Method = HttpMethod.Post,
			RequestUri = new Uri($"File/WriteText", UriKind.Relative),
			Content = content,
		};
		var response = await httpClient.SendAsync(httpRequestMessage);
		
		return await response.Content.ReadAsStringAsync();
	}
	/// <summary>
	/// 写入文件
	/// </summary>
	/// <returns></returns>
	/// <param name="directory">目录名</param>
	/// <param name="file"></param>
	[GeneratedCode("codeMaid", "1.0.0")]
	public async Task<string> FileWrite(string? directory, (HttpContent content, string fileName)? file)
	{
		var queryParameters = new List<string>();
		if (directory != null) queryParameters.Add($"directory={directory}");
		var content = new MultipartFormDataContent();
		if (file is not null) content.Add(file.Value.content, nameof(file), file.Value.fileName);
		var httpRequestMessage = new HttpRequestMessage()
		{
			Method = HttpMethod.Post,
			RequestUri = new Uri($"File/Write{(queryParameters.Count > 0 ? "?" + string.Join('&', queryParameters) : "")}", UriKind.Relative),
			Content = content,
		};
		var response = await httpClient.SendAsync(httpRequestMessage);
		
		return await response.Content.ReadAsStringAsync();
	}
	/// <summary>
	/// 同步写入文件
	/// </summary>
	/// <returns></returns>
	/// <param name="filePath">文件完整路径</param>
	[GeneratedCode("codeMaid", "1.0.0")]
	public async Task<string> FileSyncWrite(string? filePath)
	{
		var queryParameters = new List<string>();
		if (filePath != null) queryParameters.Add($"filePath={filePath}");
		var httpRequestMessage = new HttpRequestMessage()
		{
			Method = HttpMethod.Post,
			RequestUri = new Uri($"File/SyncWrite{(queryParameters.Count > 0 ? "?" + string.Join('&', queryParameters) : "")}", UriKind.Relative),
		};
		var response = await httpClient.SendAsync(httpRequestMessage);
		
		return await response.Content.ReadAsStringAsync();
	}
	/// <summary>
	/// 文本内容
	/// </summary>
	/// <returns></returns>
	/// <param name="filePath"></param>
	[GeneratedCode("codeMaid", "1.0.0")]
	public async Task<string?> FileTextContent(string? filePath)
	{
		var queryParameters = new List<string>();
		if (filePath != null) queryParameters.Add($"filePath={filePath}");
		var httpRequestMessage = new HttpRequestMessage()
		{
			Method = HttpMethod.Get,
			RequestUri = new Uri($"File/TextContent{(queryParameters.Count > 0 ? "?" + string.Join('&', queryParameters) : "")}", UriKind.Relative),
		};
		var response = await httpClient.SendAsync(httpRequestMessage);
		if (response.StatusCode == System.Net.HttpStatusCode.NoContent) return null;
		return await response.Content.ReadAsStringAsync();
	}
	/// <summary>
	/// 多个文件文本内容
	/// </summary>
	/// <returns></returns>
	/// <param name="filePaths"></param>
	[GeneratedCode("codeMaid", "1.0.0")]
	public async Task<JsonElement> FileTextContents(List<string>? filePaths)
	{
		var queryParameters = new List<string>();
		if (filePaths != null) queryParameters.Add(string.Join('&', filePaths.Select(x => $"filePaths={x}")));
		var httpRequestMessage = new HttpRequestMessage()
		{
			Method = HttpMethod.Get,
			RequestUri = new Uri($"File/TextContents{(queryParameters.Count > 0 ? "?" + string.Join('&', queryParameters) : "")}", UriKind.Relative),
		};
		var response = await httpClient.SendAsync(httpRequestMessage);
		
		return await GetResult<JsonElement>(httpRequestMessage, response);
	}
	/// <summary>
	/// 删除文件
	/// </summary>
	/// <returns></returns>
	/// <param name="filePath"></param>
	[GeneratedCode("codeMaid", "1.0.0")]
	public async Task<bool> FileDelete(string? filePath)
	{
		var queryParameters = new List<string>();
		if (filePath != null) queryParameters.Add($"filePath={filePath}");
		var httpRequestMessage = new HttpRequestMessage()
		{
			Method = HttpMethod.Delete,
			RequestUri = new Uri($"File/Delete{(queryParameters.Count > 0 ? "?" + string.Join('&', queryParameters) : "")}", UriKind.Relative),
		};
		var response = await httpClient.SendAsync(httpRequestMessage);
		
		return await GetResult<bool>(httpRequestMessage, response);
	}
	/// <summary>
	/// 移动文件
	/// </summary>
	/// <returns></returns>
	/// <param name="sourceFileName"></param>
	/// <param name="destinationFileName"></param>
	[GeneratedCode("codeMaid", "1.0.0")]
	public async Task<bool> FileMove(string? sourceFileName, string? destinationFileName)
	{
		var queryParameters = new List<string>();
		if (sourceFileName != null) queryParameters.Add($"sourceFileName={sourceFileName}");
		if (destinationFileName != null) queryParameters.Add($"destinationFileName={destinationFileName}");
		var httpRequestMessage = new HttpRequestMessage()
		{
			Method = HttpMethod.Put,
			RequestUri = new Uri($"File/Move{(queryParameters.Count > 0 ? "?" + string.Join('&', queryParameters) : "")}", UriKind.Relative),
		};
		var response = await httpClient.SendAsync(httpRequestMessage);
		
		return await GetResult<bool>(httpRequestMessage, response);
	}
	/// <summary>
	/// 移动文件
	/// </summary>
	/// <returns></returns>
	/// <param name="sourceFileName"></param>
	/// <param name="destinationFileName"></param>
	[GeneratedCode("codeMaid", "1.0.0")]
	public async Task<bool> FileMoveAndOverWrite(string? sourceFileName, string? destinationFileName)
	{
		var queryParameters = new List<string>();
		if (sourceFileName != null) queryParameters.Add($"sourceFileName={sourceFileName}");
		if (destinationFileName != null) queryParameters.Add($"destinationFileName={destinationFileName}");
		var httpRequestMessage = new HttpRequestMessage()
		{
			Method = HttpMethod.Put,
			RequestUri = new Uri($"File/MoveAndOverWrite{(queryParameters.Count > 0 ? "?" + string.Join('&', queryParameters) : "")}", UriKind.Relative),
		};
		var response = await httpClient.SendAsync(httpRequestMessage);
		
		return await GetResult<bool>(httpRequestMessage, response);
	}
	/// <summary>
	/// 复制文件
	/// </summary>
	/// <returns></returns>
	/// <param name="sourceFileName"></param>
	/// <param name="destinationFileName"></param>
	[GeneratedCode("codeMaid", "1.0.0")]
	public async Task<bool> FileCopy(string? sourceFileName, string? destinationFileName)
	{
		var queryParameters = new List<string>();
		if (sourceFileName != null) queryParameters.Add($"sourceFileName={sourceFileName}");
		if (destinationFileName != null) queryParameters.Add($"destinationFileName={destinationFileName}");
		var httpRequestMessage = new HttpRequestMessage()
		{
			Method = HttpMethod.Put,
			RequestUri = new Uri($"File/Copy{(queryParameters.Count > 0 ? "?" + string.Join('&', queryParameters) : "")}", UriKind.Relative),
		};
		var response = await httpClient.SendAsync(httpRequestMessage);
		
		return await GetResult<bool>(httpRequestMessage, response);
	}
	/// <summary>
	/// 复制文件
	/// </summary>
	/// <returns></returns>
	/// <param name="sourceFileName"></param>
	/// <param name="destinationFileName"></param>
	[GeneratedCode("codeMaid", "1.0.0")]
	public async Task<bool> FileCopyAndOverWrite(string? sourceFileName, string? destinationFileName)
	{
		var queryParameters = new List<string>();
		if (sourceFileName != null) queryParameters.Add($"sourceFileName={sourceFileName}");
		if (destinationFileName != null) queryParameters.Add($"destinationFileName={destinationFileName}");
		var httpRequestMessage = new HttpRequestMessage()
		{
			Method = HttpMethod.Put,
			RequestUri = new Uri($"File/CopyAndOverWrite{(queryParameters.Count > 0 ? "?" + string.Join('&', queryParameters) : "")}", UriKind.Relative),
		};
		var response = await httpClient.SendAsync(httpRequestMessage);
		
		return await GetResult<bool>(httpRequestMessage, response);
	}
	/// <summary>
	/// Create file symbolic link
	/// </summary>
	/// <returns></returns>
	/// <param name="sourceFileName"></param>
	/// <param name="destinationFileName"></param>
	[GeneratedCode("codeMaid", "1.0.0")]
	public async Task<bool> FileLink(string? sourceFileName, string? destinationFileName)
	{
		var queryParameters = new List<string>();
		if (sourceFileName != null) queryParameters.Add($"sourceFileName={sourceFileName}");
		if (destinationFileName != null) queryParameters.Add($"destinationFileName={destinationFileName}");
		var httpRequestMessage = new HttpRequestMessage()
		{
			Method = HttpMethod.Put,
			RequestUri = new Uri($"File/Link{(queryParameters.Count > 0 ? "?" + string.Join('&', queryParameters) : "")}", UriKind.Relative),
		};
		var response = await httpClient.SendAsync(httpRequestMessage);
		
		return await GetResult<bool>(httpRequestMessage, response);
	}
	/// <summary>
	/// Decompression Zip file to a directory
	/// </summary>
	/// <returns></returns>
	/// <param name="zipFileName"></param>
	/// <param name="destinationDirectory"></param>
	[GeneratedCode("codeMaid", "1.0.0")]
	public async Task<List<string>> FileDecompressionZip(string? zipFileName, string? destinationDirectory)
	{
		var queryParameters = new List<string>();
		if (zipFileName != null) queryParameters.Add($"zipFileName={zipFileName}");
		if (destinationDirectory != null) queryParameters.Add($"destinationDirectory={destinationDirectory}");
		var httpRequestMessage = new HttpRequestMessage()
		{
			Method = HttpMethod.Post,
			RequestUri = new Uri($"File/DecompressionZip{(queryParameters.Count > 0 ? "?" + string.Join('&', queryParameters) : "")}", UriKind.Relative),
		};
		var response = await httpClient.SendAsync(httpRequestMessage);
		
		return await GetResult<List<string>>(httpRequestMessage, response);
	}
	/// <summary>
	/// set remote ssh cert auth
	/// </summary>
	/// <returns></returns>
	[GeneratedCode("codeMaid", "1.0.0")]
	public async Task<bool> FunctionSetRemoteSshCertAuth()
	{
		var httpRequestMessage = new HttpRequestMessage()
		{
			Method = HttpMethod.Post,
			RequestUri = new Uri($"api/Function/SetRemoteSSHCertAuth", UriKind.Relative),
		};
		var response = await httpClient.SendAsync(httpRequestMessage);
		
		return await GetResult<bool>(httpRequestMessage, response);
	}
	/// <summary>
	/// 
	/// </summary>
	/// <returns></returns>
	[GeneratedCode("codeMaid", "1.0.0")]
	public async Task<List<ModelsCodeMaidProject>> ProjectGetList()
	{
		var httpRequestMessage = new HttpRequestMessage()
		{
			Method = HttpMethod.Get,
			RequestUri = new Uri($"api/Project", UriKind.Relative),
		};
		var response = await httpClient.SendAsync(httpRequestMessage);
		
		return await GetResult<List<ModelsCodeMaidProject>>(httpRequestMessage, response);
	}
	/// <summary>
	/// 
	/// </summary>
	/// <returns></returns>
	/// <param name="id"></param>
	[GeneratedCode("codeMaid", "1.0.0")]
	public async Task<ModelsCodeMaidProject> ProjectGetDetail(long? id)
	{
		var httpRequestMessage = new HttpRequestMessage()
		{
			Method = HttpMethod.Get,
			RequestUri = new Uri($"api/Project/{id}", UriKind.Relative),
		};
		var response = await httpClient.SendAsync(httpRequestMessage);
		
		return await GetResult<ModelsCodeMaidProject>(httpRequestMessage, response);
	}
	/// <summary>
	/// 
	/// </summary>
	/// <returns></returns>
	/// <param name="id"></param>
	[GeneratedCode("codeMaid", "1.0.0")]
	public async Task<bool> ProjectFlushAllFile(long? id)
	{
		var queryParameters = new List<string>();
		if (id != null) queryParameters.Add($"id={id}");
		var httpRequestMessage = new HttpRequestMessage()
		{
			Method = HttpMethod.Put,
			RequestUri = new Uri($"api/Project/FlushAllFile{(queryParameters.Count > 0 ? "?" + string.Join('&', queryParameters) : "")}", UriKind.Relative),
		};
		var response = await httpClient.SendAsync(httpRequestMessage);
		
		return await GetResult<bool>(httpRequestMessage, response);
	}
	/// <summary>
	/// 
	/// </summary>
	/// <returns></returns>
	/// <param name="id"></param>
	[GeneratedCode("codeMaid", "1.0.0")]
	public async Task<bool> ProjectGitHooks(long? id)
	{
		var queryParameters = new List<string>();
		if (id != null) queryParameters.Add($"id={id}");
		var httpRequestMessage = new HttpRequestMessage()
		{
			Method = HttpMethod.Post,
			RequestUri = new Uri($"api/Project/GitHooks{(queryParameters.Count > 0 ? "?" + string.Join('&', queryParameters) : "")}", UriKind.Relative),
		};
		var response = await httpClient.SendAsync(httpRequestMessage);
		
		return await GetResult<bool>(httpRequestMessage, response);
	}
	/// <summary>
	/// 查询系统所有的枚举字典值和对应的描述
	/// </summary>
	/// <returns></returns>
	[GeneratedCode("codeMaid", "1.0.0")]
	public async Task<JsonElement> SystemGetEnumDictionaries()
	{
		var httpRequestMessage = new HttpRequestMessage()
		{
			Method = HttpMethod.Get,
			RequestUri = new Uri($"api/System/GetEnumDictionaries", UriKind.Relative),
		};
		var response = await httpClient.SendAsync(httpRequestMessage);
		
		return await GetResult<JsonElement>(httpRequestMessage, response);
	}
	/// <summary>
	/// 查询Controller下所有Action，方法名和参数名、参数类型
	/// </summary>
	/// <returns></returns>
	[GeneratedCode("codeMaid", "1.0.0")]
	public async Task<List<ApiControllersCommonsControllerInfo>?> SystemGetControllers()
	{
		var httpRequestMessage = new HttpRequestMessage()
		{
			Method = HttpMethod.Get,
			RequestUri = new Uri($"api/System/GetControllers", UriKind.Relative),
		};
		var response = await httpClient.SendAsync(httpRequestMessage);
		if (response.StatusCode == System.Net.HttpStatusCode.NoContent) return null;
		return await GetResult<List<ApiControllersCommonsControllerInfo>>(httpRequestMessage, response);
	}
	/// <summary>
	/// 回显请求信息
	/// </summary>
	/// <returns></returns>
	/// <param name="statusCode">返回的状态码</param>
	/// <param name="delay">接口返回延迟</param>
	[GeneratedCode("codeMaid", "1.0.0")]
	public async Task<Stream> SystemEcho(int statusCode, int delay)
	{
		var queryParameters = new List<string>();
		if (statusCode != null) queryParameters.Add($"statusCode={statusCode}");
		if (delay != null) queryParameters.Add($"delay={delay}");
		var httpRequestMessage = new HttpRequestMessage()
		{
			Method = HttpMethod.Patch,
			RequestUri = new Uri($"api/System/Echo{(queryParameters.Count > 0 ? "?" + string.Join('&', queryParameters) : "")}", UriKind.Relative),
		};
		var response = await httpClient.SendAsync(httpRequestMessage, HttpCompletionOption.ResponseHeadersRead);
		
		return await response.Content.ReadAsStreamAsync();
	}
	/// <summary>
	/// 转发
	/// </summary>
	/// <returns></returns>
	/// <param name="forward">转发地址</param>
	[GeneratedCode("codeMaid", "1.0.0")]
	public async Task<Stream> SystemForward(string? forward)
	{
		var queryParameters = new List<string>();
		if (forward != null) queryParameters.Add($"forward={forward}");
		var httpRequestMessage = new HttpRequestMessage()
		{
			Method = HttpMethod.Patch,
			RequestUri = new Uri($"api/System/Forward{(queryParameters.Count > 0 ? "?" + string.Join('&', queryParameters) : "")}", UriKind.Relative),
		};
		var response = await httpClient.SendAsync(httpRequestMessage, HttpCompletionOption.ResponseHeadersRead);
		
		return await response.Content.ReadAsStreamAsync();
	}
	/// <summary>
	/// 保持连接
	/// </summary>
	/// <returns></returns>
	[GeneratedCode("codeMaid", "1.0.0")]
	public async Task<Stream> SystemKeepAlive()
	{
		var httpRequestMessage = new HttpRequestMessage()
		{
			Method = HttpMethod.Get,
			RequestUri = new Uri($"api/System/KeepAlive", UriKind.Relative),
		};
		var response = await httpClient.SendAsync(httpRequestMessage, HttpCompletionOption.ResponseHeadersRead);
		
		return await response.Content.ReadAsStreamAsync();
	}
	/// <summary>
	/// 运行时版本
	/// </summary>
	/// <returns></returns>
	[GeneratedCode("codeMaid", "1.0.0")]
	public async Task<string> SystemRuntimeVersion()
	{
		var httpRequestMessage = new HttpRequestMessage()
		{
			Method = HttpMethod.Get,
			RequestUri = new Uri($"api/System/RuntimeVersion", UriKind.Relative),
		};
		var response = await httpClient.SendAsync(httpRequestMessage);
		
		return await response.Content.ReadAsStringAsync();
	}
}