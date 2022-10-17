using Models.Service;

namespace Api.Middleware
{
	//业务异常过滤器
	class HttpResponseExceptionFilter : Microsoft.AspNetCore.Mvc.Filters.IExceptionFilter
	{
		public void OnException(Microsoft.AspNetCore.Mvc.Filters.ExceptionContext context)
		{
			if (context.Exception is BusinessException ex)
			{
				context.Result = new Microsoft.AspNetCore.Mvc.ObjectResult(ex.Result);
				context.ExceptionHandled = true;
			}
			else
			{
				Console.WriteLine(context.Exception.Message);
			}
		}
	}
}
