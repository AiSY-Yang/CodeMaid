using System.Reflection;

using ExtensionMethods;

using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

using ServicesModels.Results;

using Swashbuckle.AspNetCore.SwaggerGen;
namespace Microsoft.Extensions.DependencyInjection;
/// <summary>
/// 增加枚举的字符串表示和注释信息 字符串为x-enumNames 注释为x-enumSummarys
/// </summary>
public class EnumSchemaFilter : ISchemaFilter
{
	/// <inheritdoc/>
	public void Apply(OpenApiSchema model, SchemaFilterContext context)
	{
		if (context.Type.IsEnum)
		{
			var name = new Microsoft.OpenApi.Any.OpenApiArray();
			Enum.GetNames(context.Type)
				.ToList()
				.ForEach(n =>
				{
					name.Add(new OpenApiString(n));
				});
			model.Extensions.Add("x-enumNames", name);
			//XML注释
			try
			{
				var desp = new Microsoft.OpenApi.Any.OpenApiArray();
				foreach (var enumName in context.Type.GetEnumNames())
				{
					var fullName = "F:" + context.Type.FullName + "." + enumName;
					var obj = new OpenApiObject
					{
						{ "name", new OpenApiString(enumName) },
						{ "summary", new OpenApiString(context.Type.Assembly.GetXMLMember().FirstOrDefault(x => x.ID == fullName)?.Summary) }
					};
					desp.Add(obj);
				}
				model.Extensions.Add("x-enumSummarys", desp);
			}
			catch (Exception)
			{
			}
		}
	}
}

/// <summary>
/// 如果属性为接口的实现 且属性没有自己的XML注释 接口有XML注释 则继承接口的XML注释 不匹配大小写
/// </summary>
public class InheritInterfaceXmlCommentSchemaFilter : ISchemaFilter
{
	/// <inheritdoc/>
	public void Apply(OpenApiSchema model, SchemaFilterContext context)
	{
		switch (model.Type)
		{
			case "object":
				foreach (var item in model.Properties.Where(x => x.Value.Description == null))
				{
					foreach (var @interface in context.Type.GetInterfaces())
					{
						var fullName = "P:" + @interface.FullName + "." + item.Key;
						item.Value.Description = context.Type.Assembly.GetXMLMember().FirstOrDefault(x => x.ID.Equals(fullName, StringComparison.InvariantCultureIgnoreCase))?.Summary;
					}
				}
				break;
			default:
				break;
		}
	}
}
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
		var p = _nullabilityContext.Create(context.MethodInfo.ReturnParameter);
		var maybeNull = p.WriteState is NullabilityState.Nullable;
		Console.WriteLine($"{context.ApiDescription.RelativePath} {maybeNull}");
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