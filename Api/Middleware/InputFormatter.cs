using System.Text;

using Microsoft.AspNetCore.Mvc.Formatters;

namespace Api.Middleware
{
	/// <summary>
	/// 支持从body接收字符串内容
	/// </summary>
	public class TextFormatter : TextInputFormatter
	{
		/// <inheritdoc/>
		public TextFormatter()
		{
			SupportedMediaTypes.Add("text/plain");
			SupportedEncodings.Add(Encoding.UTF8);
			SupportedEncodings.Add(Encoding.Unicode);
		}
		/// <inheritdoc/>
		protected override bool CanReadType(Type type)
		{
			return type == typeof(string);
		}
		/// <inheritdoc/>
		public override async Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context, Encoding encoding)
		{
			var httpContext = context.HttpContext;
			using var reader = new StreamReader(httpContext.Request.Body, encoding);

			try
			{
				var s = await reader.ReadToEndAsync();
				return await InputFormatterResult.SuccessAsync(s);
			}
			catch
			{
				return await InputFormatterResult.FailureAsync();
			}
		}
	}
}
