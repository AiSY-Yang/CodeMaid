using System.Diagnostics;
using System.Net;
using System.Net.Mime;

using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;

using ServicesModels.Results;

namespace Api.Middleware
{
	/// <summary>
	/// 业务异常过滤器 按照rfc9457格式返回
	/// </summary>
	class HttpResponseExceptionFilter(IHttpContextAccessor httpContextAccessor) : Microsoft.AspNetCore.Mvc.Filters.IExceptionFilter
	{
		static MediaTypeCollection problemMediaType = new() {
			new MediaTypeHeaderValue(MediaTypeNames.Application.ProblemJson),
			new MediaTypeHeaderValue(MediaTypeNames.Application.ProblemXml),
		};


		public void OnException(Microsoft.AspNetCore.Mvc.Filters.ExceptionContext context)
		{
			switch (context.Exception)
			{
				case RFC9457Exception ex:
					context.Result = new Microsoft.AspNetCore.Mvc.ObjectResult(ex.Model)
					{ StatusCode = (int?)ex.Model.Status, ContentTypes = problemMediaType };
					context.ExceptionHandled = true;
					break;
				case ResultException ex:
					context.Result = new Microsoft.AspNetCore.Mvc.ObjectResult(new
					{
						type = ex.Code,
						status = ex.HttpStatusCode,
						title = ex.Message,
						detail = ex.Message,
						instance = ex.Message,
						traceId = httpContextAccessor.HttpContext?.Features.Get<IHttpActivityFeature>()?.Activity.TraceId,
					})
					{ StatusCode = (int?)ex.HttpStatusCode, ContentTypes = problemMediaType };
					context.ExceptionHandled = true;
					break;
				case I18nResultException ex:
					context.Result = new Microsoft.AspNetCore.Mvc.ObjectResult(new
					{
						type = ex.Code,
						StatusCode = (int?)ex.HttpStatusCode,
						title = ex.Message,
						detail = ex.Message,
						instance = ex.Message,
						traceId = httpContextAccessor.HttpContext?.Features.Get<IHttpActivityFeature>()?.Activity.TraceId,
					})
					{ StatusCode = (int?)ex.HttpStatusCode, ContentTypes = problemMediaType };
					context.ExceptionHandled = true;
					break;
				default:
					break;
			}
		}
	}
}
