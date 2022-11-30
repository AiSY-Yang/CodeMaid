using System.Runtime.CompilerServices;
using System.Text.Json.Nodes;

using ExtensionMethods;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Models.CodeMaid.Enum;

namespace Api.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class SystemController : ControllerBase
	{
		/// <summary>
		/// 查询系统所有的枚举字典值和对应的描述
		/// </summary>
		/// <returns></returns>
		[HttpGet("[action]")]
		[AllowAnonymous]
		public static JsonObject GetEnumDictionaries()
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
	}
}