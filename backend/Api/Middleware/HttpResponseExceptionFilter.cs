using System.Diagnostics;
using System.Net;
using System.Net.Mime;

using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
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
		public void OnException(Microsoft.AspNetCore.Mvc.Filters.ExceptionContext context)
		{
			switch (context.Exception)
			{
				case CustomException ex:
					context.Result = ex.GetObjectResult();
					context.ExceptionHandled = false;
					break;
				default:
					break;
			}
		}
	}
}
