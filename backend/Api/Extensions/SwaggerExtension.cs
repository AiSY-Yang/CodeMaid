using Microsoft.OpenApi.Models;

namespace Api.Extensions
{
	/// <summary>
	/// Swagger扩展
	/// </summary>
	public static class SwaggerExtension
	{
		/// <summary>
		/// 是否是成功的响应 HttpCode>200 300>HttpCode
		/// </summary>
		/// <returns></returns>
		public static bool IsSuccessResponse(this KeyValuePair<string, OpenApiResponse> response)
		{
			return string.Compare(response.Key, "200", true) >= 0 && string.Compare(response.Key, "300", true) < 0;
		}
	}
}
