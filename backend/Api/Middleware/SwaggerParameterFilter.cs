using System.Reflection;

using Microsoft.OpenApi.Models;

namespace Swashbuckle.AspNetCore.SwaggerGen;

/// <summary>
/// 将可为空的query参数添加nullable属性
/// </summary>
public class SwaggerNullableQueryParameterFilter : IParameterFilter
{
	static readonly NullabilityInfoContext _nullabilityContext = new();

	/// <inheritdoc/>
	public void Apply(OpenApiParameter parameter, ParameterFilterContext context)
	{
		var p = _nullabilityContext.Create(context.ParameterInfo);
		var maybeNull = p.WriteState is NullabilityState.Nullable;
		if (parameter.In == ParameterLocation.Query && maybeNull)
		{
			parameter.Schema.Nullable = true;
			parameter.Required = false;
		}
	}
}