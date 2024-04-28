#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Net.Mime;

using ExtensionMethods;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

using ServicesModels.Methods;

namespace Api.Controllers
{
	interface IServices
	{
	}

	[ApiController]
	[Route("[controller]")]
	[ApiExplorerSettings(GroupName = "test")]
	public class TestController : Microsoft.AspNetCore.Mvc.ControllerBase, IAdd<object, bool>
	{
		private readonly ILogger<TestController> logger;

		public TestController(ILogger<TestController> logger)
		{
			this.logger = logger;
		}
		[HttpPost("[action]")]
		//[Consumes(MediaTypeNames.Application.Octet)]
		public async Task<int> Upload(string fileNme, [FromBody][BindNever] IFormFile file)
		{
			string filePath = "d://data/xxx.tmp";
			long size = HttpContext.Request.Headers.ContentLength!.Value;
			var queryParameters = new List<string>();
			if (filePath != null) queryParameters.Add($"filePath={filePath}");
			var content = new MultipartFormDataContent();
			if (file is not null) content.Add(new StreamContent(HttpContext.Request.Body), nameof(file), "ignore");
			var httpRequestMessage = new HttpRequestMessage()
			{
				Method = HttpMethod.Post,
				RequestUri = new Uri($"File/SyncWrite{(queryParameters.Count > 0 ? "?" + string.Join('&', queryParameters) : "")}", UriKind.Relative),
				Content = new StreamContent(HttpContext.Request.Body),
			};
			logger.LogCritical("准备请求");
			var client = new HttpClient();
			client.BaseAddress = new Uri("http://localhost:5241");
			logger.LogCritical("准备发送");
			var response = await client.SendAsync(httpRequestMessage);
			logger.LogCritical("发送完成 开始读取");
			var res = await response.Content.ReadAsStringAsync();
			logger.LogCritical("请求结束");
			logger.LogCritical(response.StatusCode.ToString());
			logger.LogCritical(res);

			return 1;
		}
		[HttpPost("[action]")]
		public int Test([FromQuery] int queryInt, [FromQuery][Required] string queryStringRequired, [FromQuery] string queryString, [FromBody] BodyClass body)
		{
			return 1;
		}
		[HttpPost("[action]")]
		public int? TestNullable([FromQuery] int? queryInt, [FromQuery][Required] string? queryStringRequired, [FromQuery] string? queryString, [FromBody] BodyClass? body)
		{
			return null;
		}
		[HttpPost("[action]")]
		public int? TestBody([FromBody] BodyClass body) => null;
		[HttpPost("[action]")]
		public int? TestNullableBody([FromBody] BodyClass? body) => null;
		[HttpPost("[action]")]
		public int? TestRequiredBody([FromBody] BodyClass body) => null;
		[HttpPost("[action]")]
		public int? TestRequiredNullableBody([FromBody] BodyClass? body) => null;
		[HttpGet]
		public BodyClass? TestResponse()
		{
			return null;
		}
		public class BodyClass
		{
			public int Int { get; set; }
			public int? NullableInt { get; set; }
			public string String { get; set; } = null!;
			public string? NullableString { get; set; }
			public required int RequiredInt { get; set; }
			public required int? RequiredNullableInt { get; set; }
			public required string RequiredString { get; set; }
			public required string? RequiredNullableString { get; set; }
		}
	}
}
#pragma warning restore CS1591 // 缺少对公共可见类型或成员的 XML 注释