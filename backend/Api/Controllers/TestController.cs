#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
using System.ComponentModel.DataAnnotations;
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
	[Route("[controller]/[action]")]
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

			var path = "D:\\temp\\xx.tmp";
			if (System.IO.File.Exists(path)) System.IO.File.Delete(path);
			logger.LogCritical("文件删除完成");
			FileStream fileStream = new FileStream(path, FileMode.Create);
			logger.LogCritical("文件创建完成");
			await HttpContext.Request.Body.CopyToAsync(fileStream);
			//await HttpContext.Request.Form.Files.First().OpenReadStream().CopyToAsync(fileStream);
			logger.LogCritical("文件复制完成");
			await fileStream.FlushAsync();
			logger.LogCritical("文件刷新完成");
			await fileStream.DisposeAsync();
			var sour = System.IO.File.ReadAllBytes("C:\\Users\\kai\\Downloads\\genarsa_bio_argo_qualimap_v1.tar").Hash(HashOption.MD5_32).ToBase64String();
			await Console.Out.WriteLineAsync(sour);
			var md5 = System.IO.File.ReadAllBytes(path).Hash(HashOption.MD5_32).ToBase64String();
			await Console.Out.WriteLineAsync(md5);
			await Console.Out.WriteLineAsync((sour == md5).ToString());
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