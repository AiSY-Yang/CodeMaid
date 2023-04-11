using ServicesModels.Exceptions;

namespace Api.Middleware
{
	//业务异常过滤器
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
				Console.WriteLine(context.Exception.Message);
			}
		}
	}
}
