using ServicesModels.Results;

namespace Api.Middleware
{
	/// <summary>
	/// 业务异常过滤器
	/// </summary>
	class HttpResponseExceptionFilter : Microsoft.AspNetCore.Mvc.Filters.IExceptionFilter
	{
		public void OnException(Microsoft.AspNetCore.Mvc.Filters.ExceptionContext context)
		{
			if (context.Exception is ResultException ex)
			{
				context.Result = new Microsoft.AspNetCore.Mvc.ObjectResult(new ExceptionResult(ex)) { StatusCode = (int?)ex.HttpStatusCode };
				context.ExceptionHandled = true;
			}
			else
			{
				var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<HttpResponse>>();
				logger.LogError(context.Exception, "exception response");
			}
		}
	}
}
