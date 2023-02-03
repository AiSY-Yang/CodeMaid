using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Nodes;

using ExtensionMethods;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

using Models.CodeMaid.Enum;

namespace Api.Controllers;

/// <summary>
/// 系统控制器
/// </summary>
[ApiController]
[Route("[controller]")]
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
	[AllowAnonymous]
	[Produces(typeof(Dictionary<string, List<(string description, string value)>>))]
	public JsonObject GetEnumDictionaries()
	{
		var result = new Dictionary<string, JsonNode?>();
		foreach (var type in typeof(Sex).Assembly.GetTypes().Where(x => x.IsEnum))
		{
			var array = new JsonArray();
			foreach (var item in Enum.GetValues(type))
				array.Add(new JsonObject(new Dictionary<string, JsonNode?>{
					{ "description",JsonValue.Create((item as Enum)!.GetDescription()) },
					{ "value",JsonValue.Create( item) },
				}));
			result.Add(string.Concat(char.ToLower(type.Name[0]), type.Name[1..]), array);
		}
		return new JsonObject(result.AsEnumerable());
	}

	/// <summary>
	/// 查询Controller下所有Action，方法名和参数名、参数类型
	/// </summary>
	public List<ControllerInfo> GetControllers()
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
				ControllerInfo info = new ControllerInfo() { Name = type.Name, ActionList = new List<ActionInfo>() };
				foreach (var method in type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
				{
					ActionInfo actionInfo = new ActionInfo() { Name = method.Name, ParamList = new List<ParameterInfo>() };
					actionInfo.ParamList.AddRange(method.GetParameters().ToList().Select(item => new ParameterInfo
					{
						Name = item.Name,
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
	public async Task<object> Echo()
	{
		var body = Encoding.UTF8.GetString((await HttpContext.Request.BodyReader.ReadAsync()).Buffer);
		var req = new
		{
			HttpContext.Connection.RemoteIpAddress,
			HttpContext.Connection.RemotePort,
			HttpContext.Session.Id,
			HttpContext.Session.Keys,
			HttpContext.Request.PathBase,
			HttpContext.Request.Path,
			HttpContext.Request.Method,
			HttpContext.Request.QueryString,
			HttpContext.Request.ContentLength,
			HttpContext.Request.IsHttps,
			HttpContext.Request.Protocol,
			HttpContext.Request.Scheme,
			Headers = HttpContext.Request.Headers.Select(x => new { x.Key, x.Value }),
			Body = body,
			Form = HttpContext.Request.Form.Select(x => new { x.Key, x.Value })
		};
		return req;
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