using System.Reflection;
using System.Text;
using System.Text.Json.Nodes;

using ExtensionMethods;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

using Models.CodeMaid;
using Models.CodeMaid.Enum;

namespace Api.Controllers;

/// <summary>
/// 系统控制器
/// </summary>
[AllowAnonymous]
public class SystemController : ControllerBase
{
	private readonly ILogger<SystemController> logger;

	/// <summary>
	/// 
	/// </summary>
	/// <param name="logger"></param>
	public SystemController(ILogger<SystemController> logger)
	{
		this.logger = logger;
	}

	/// <summary>
	/// 查询系统所有的枚举字典值和对应的描述
	/// </summary>
	/// <returns></returns>
	[HttpGet("[action]")]
	[Produces(typeof(Dictionary<string, List<(string description, string value)>>))]
	public JsonObject GetEnumDictionaries()
	{
		var result = new Dictionary<string, JsonNode?>();
		IEnumerable<Type> types = Array.Empty<Type>();
		types = types.Union(typeof(Sex).Assembly.GetTypes().Where(x => x.IsEnum));
		types = types.Union(typeof(MaidWork).Assembly.GetTypes().Where(x => x.IsEnum));
		foreach (var type in types)
		{
			Dictionary<int, JsonObject> objects = new();
			foreach (var item in type.GetFields(BindingFlags.Public | BindingFlags.Static)
				.Where(x => x.GetCustomAttribute<System.Text.Json.Serialization.JsonIgnoreAttribute>() == null))
			{
				var order = item.GetCustomAttribute<System.Text.Json.Serialization.JsonPropertyOrderAttribute>()?.Order ?? objects.Count + 1;
				objects[order] = new JsonObject(new Dictionary<string, JsonNode?>{
					{ "description",JsonValue.Create((item.GetValue(type) as Enum)!.GetDescription()) },
					{ "value",JsonValue.Create( item.GetValue(type)) },
				});
			}
			JsonArray array = new(objects.OrderBy(x => x.Key).Select(x => x.Value).ToArray());
			//小驼峰命名
			result.Add(type.Name.ToNamingConvention(NamingConvention.camelCase), array);
		}
		return new JsonObject(result.AsEnumerable());
	}

	/// <summary>
	/// 查询Controller下所有Action，方法名和参数名、参数类型
	/// </summary>
	[HttpGet("[action]")]
	public List<ControllerInfo>? GetControllers()
	{
		var assembly = Assembly.GetExecutingAssembly();
		Type[] types = assembly.GetTypes();
		List<ControllerInfo> controlInfoList = new();

		bool IsController(Type type) => type.IsClass && type.IsPublic && type.IsSubclassOf(typeof(Controller));

		static bool IsInherit(Type type, Type[] typeArray)
		{
			foreach (var currenType in typeArray)
			{
				if (type.Equals(currenType) || type.IsSubclassOf(currenType))
					return true;
			}
			return false;
		}

		static bool IsActionResult(MethodInfo method)
		{
			return method.GetCustomAttributes<HttpMethodAttribute>().Any() ||
				   IsInherit(method.ReturnParameter.ParameterType, new[]
					   {
						   typeof(ActionResult),
						   typeof(IActionResult)
					   }
				   );
		}

		foreach (var type in types)
		{
			if (IsController(type))
			{
				ControllerInfo info = new() { Name = type.Name, ActionList = new List<ActionInfo>() };
				foreach (var method in type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
				{
					ActionInfo actionInfo = new() { Name = method.Name, ParamList = new List<ParameterInfo>() };
					actionInfo.ParamList.AddRange(method.GetParameters().ToList().Select(item => new ParameterInfo
					{
						Name = item.Name!,
						Type = item.ParameterType
					})
					);
					if (IsActionResult(method))
					{
						info.ActionList.Add(actionInfo);
					}
				}

				controlInfoList.Add(info);
			}
		}

		return controlInfoList;
	}

	/// <summary>
	/// 回显请求信息
	/// </summary>
	/// <returns></returns>
	[HttpGet("[action]")]
	[HttpPost("[action]")]
	[HttpPut("[action]")]
	[HttpDelete("[action]")]
	[HttpHead("[action]")]
	[HttpOptions("[action]")]
	[HttpPatch("[action]")]
	public async Task<object> Echo(int? statusCode)
	{
		var req = new
		{
			AddressFamily = HttpContext.Connection.RemoteIpAddress?.AddressFamily.ToString(),
			RemoteIpAddress = HttpContext.Connection.RemoteIpAddress?.ToString(),
			HttpContext.Connection.RemotePort,
			PathBase = HttpContext.Request.PathBase.Value,
			Path = HttpContext.Request.Path.Value,
			HttpContext.Request.Method,
			QueryString = HttpContext.Request.QueryString.Value,
			HttpContext.Request.ContentLength,
			HttpContext.Request.IsHttps,
			HttpContext.Request.Protocol,
			HttpContext.Request.Scheme,
			HttpContext.Request.Headers,
			Form = HttpContext.Request.HasFormContentType ? HttpContext.Request.Form.Select(x => new { x.Key, x.Value }) : null,
			Files = HttpContext.Request.HasFormContentType ? HttpContext.Request.Form.Files : null,
			Body = Encoding.UTF8.GetString((await HttpContext.Request.BodyReader.ReadAsync()).Buffer),
		};
		var res = new ObjectResult(req);
		if (statusCode != null) res.StatusCode = statusCode;
		return res;
	}
	/// <summary>
	/// 转发
	/// </summary>
	/// <param name="httpClientFactory"></param>
	/// <param name="forward">转发地址</param>
	/// <returns></returns>
	[HttpGet("[action]")]
	[HttpPost("[action]")]
	[HttpPut("[action]")]
	[HttpDelete("[action]")]
	[HttpHead("[action]")]
	[HttpOptions("[action]")]
	[HttpPatch("[action]")]
	public async Task Forward([FromServices] IHttpClientFactory httpClientFactory, string forward)
	{
		var client = httpClientFactory.CreateClient();
		client.BaseAddress = new Uri(forward);
		var msg = new HttpRequestMessage()
		{
			Method = new HttpMethod(HttpContext.Request.Method),
			Content = new StreamContent(HttpContext.Request.Body),
		};
		foreach (var item in HttpContext.Request.Headers)
		{
			msg.Headers.Add(item.Key, item.Value.Select(x => x));
		}
		if (HttpContext.Request.ContentType != null)
		{
			msg.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(HttpContext.Request.ContentType);
		}
		var res = client.Send(msg);
		HttpContext.Response.Body = await res.Content.ReadAsStreamAsync();
		await HttpContext.Response.CompleteAsync();
	}
}

/// <summary>
/// 控制器信息
/// </summary>
public class ControllerInfo
{
	/// <summary>
	/// 控制器名称
	/// </summary>
	public required string Name { get; set; }
	/// <summary>
	/// action列表
	/// </summary>
	public required List<ActionInfo> ActionList { get; set; }
}
/// <summary>
/// action信息
/// </summary>
public class ActionInfo
{
	/// <summary>
	/// action名称
	/// </summary>
	public required string Name { get; set; }
	/// <summary>
	/// 参数列表
	/// </summary>
	public required List<ParameterInfo> ParamList { get; set; }
}
/// <summary>
/// 参数信息
/// </summary>
public class ParameterInfo
{
	/// <summary>
	/// 参数名称
	/// </summary>
	public required string Name { get; set; }
	/// <summary>
	/// 参数类型
	/// </summary>
	public required Type Type { get; set; }
}