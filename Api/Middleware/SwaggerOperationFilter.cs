using System.Reflection;

using ExtensionMethods;

using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

using ServicesModels.Results;

namespace Swashbuckle.AspNetCore.SwaggerGen;
/// <summary>
/// swagger路由转为全部小写
/// </summary>
public class LowerActionNameFilter : IOperationFilter
{
	///<inheritdoc cref="IOperationFilter.Apply(OpenApiOperation, OperationFilterContext)"/>
	public void Apply(OpenApiOperation operation, OperationFilterContext context)
	{
		context.ApiDescription.RelativePath = context.ApiDescription.RelativePath?.ToLower();
	}
}
/// <summary>
/// swagger增加统一的400错误类型
/// </summary>
public class AddBusinessExceptionResponse : IOperationFilter
{
	///<inheritdoc cref="IOperationFilter.Apply(OpenApiOperation, OperationFilterContext)"/>
	public void Apply(OpenApiOperation operation, OperationFilterContext context)
	{
		var schema = context.SchemaGenerator.GenerateSchema(typeof(IExceptionResult), context.SchemaRepository);
		operation.Responses.Add("400", new OpenApiResponse
		{
			Description = "业务异常",
			Content = new Dictionary<string, OpenApiMediaType> {{ "application/json", new OpenApiMediaType() { Schema = schema} },
		}
		});
	}
}

/// <summary>
/// 当方法返回可能为空的时候添加204响应
/// </summary>
public class Add204ResponseWhenReturnMaybeNull : IOperationFilter
{
	/// <summary>
	/// https://devblogs.microsoft.com/dotnet/announcing-net-6-preview-7/#libraries-reflection-apis-for-nullability-information
	/// </summary>
	static readonly NullabilityInfoContext _nullabilityContext = new();
	///<inheritdoc cref="IOperationFilter.Apply(OpenApiOperation, OperationFilterContext)"/>
	public void Apply(OpenApiOperation operation, OperationFilterContext context)
	{
		var p = new NullabilityInfoContext().Create(context.MethodInfo.ReturnParameter);
		var maybeNull = p.WriteState is NullabilityState.Nullable;
		if (maybeNull)
		{
			if (!operation.Responses.ContainsKey("204"))
			{
				operation.Responses.Add("204", new OpenApiResponse
				{
					Description = "No Content",
					Content = new Dictionary<string, OpenApiMediaType>(),
				});
			}
		}
	}
}