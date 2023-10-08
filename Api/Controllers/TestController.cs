#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
using System.Runtime.CompilerServices;
using System.Text;

using Mapster;

using Microsoft.AspNetCore.Mvc;

using ServicesModels.Methods;

namespace Api.Controllers
{
	interface IServices
	{
	}

	[ApiController]
	[Route("[controller]/[action]")]
	public class TestController : ControllerBase, IAdd<object, bool>
	{
		private readonly ILogger<TestController> logger;

		public TestController(ILogger<TestController> logger)
		{
			this.logger = logger;
		}

		[HttpPost("[action]")]
		public async Task<int> Test([FromQuery] int queryInt, [FromQuery] string queryString, [FromBody] BodyClass body)
		{
			return 1;
		}
		[HttpPost("[action]")]
		public async Task<int?> TestNullable([FromQuery] int? queryInt, [FromQuery] string? queryString, [FromBody] BodyClass? body)
		{
			return null;
		}
		public class BodyClass
		{
			public int Int { get; set; }
			public int? NullableInt { get; set; }
			public string String { get; set; }
			public string? NullableString { get; set; }
			public required int RequiredInt { get; set; }
			public required int? RequiredNullableInt { get; set; }
			public required string RequiredString { get; set; }
			public required string? RequiredNullableString { get; set; }
		}
	}
}
#pragma warning restore CS1591 // 缺少对公共可见类型或成员的 XML 注释