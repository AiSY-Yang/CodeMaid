using ExtensionMethods;

using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

using Models.Service;

using Swashbuckle.AspNetCore.SwaggerGen;

/// <summary>
/// 增加枚举的字符串表示和注释信息 字符串为x-enumNames 注释为x-enumSummarys
/// </summary>
public class EnumSchemaFilter : ISchemaFilter
{
	public void Apply(OpenApiSchema model, SchemaFilterContext context)
	{
		if (context.Type.IsEnum)
		{
			//model.Enum.Clear();
			//model.Description = "desp";
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
public class BusinessExceptionFilter : IOperationFilter
{
	///<inheritdoc cref="IOperationFilter.Apply(OpenApiOperation, OperationFilterContext)"/>
	public void Apply(OpenApiOperation operation, OperationFilterContext context)
	{
		var schema = context.SchemaGenerator.GenerateSchema(typeof(BusinessException.BusinessExceptionResult), context.SchemaRepository);
		operation.Responses.Add("400", new OpenApiResponse
		{
			Description = "业务异常",
			Content = new Dictionary<string, OpenApiMediaType> {{ "application/json", new OpenApiMediaType() { Schema = schema} },
		}
		});
	}
}