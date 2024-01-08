using System.Reflection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Api.Middleware.Swagger;

/// <summary>
/// 将可为空的body参数添加nullable属性
/// </summary>
public class SwaggerNullableBodyFilter : IRequestBodyFilter
{
    static readonly NullabilityInfoContext _nullabilityContext = new();

    /// <inheritdoc/>
    public void Apply(OpenApiRequestBody requestBody, RequestBodyFilterContext context)
    {
        if (context.BodyParameterDescription != null)
        {
            var p = _nullabilityContext.Create(context.BodyParameterDescription.ParameterInfo());
            var maybeNull = p.WriteState is NullabilityState.Nullable;
            requestBody.Required = !maybeNull;
        }
    }
}