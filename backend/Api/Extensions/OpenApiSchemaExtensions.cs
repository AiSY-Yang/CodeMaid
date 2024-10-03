using Api.Job;

using ExtensionMethods;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.OpenApi.Models;

using Models.CodeMaid;

using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Api.Extensions
{
	static class OpenApiSchemaExtensions
	{
		public static OpenApiSchemaInfo GetTypeInfo(this OpenApiSchema openApiSchema)
		{
			return new OpenApiSchemaInfo()
			{
				CanBeNull = openApiSchema.Nullable,
				Required = false,
				ReferenceId = openApiSchema.Reference?.Id,
				Type = openApiSchema.Type,
				Format = openApiSchema.Format,
				IsArray = openApiSchema.Type == "array",
				Item = openApiSchema.Items?.GetTypeInfo(),
				IsRef = openApiSchema.Reference != null,
			};
		}
	}
}
