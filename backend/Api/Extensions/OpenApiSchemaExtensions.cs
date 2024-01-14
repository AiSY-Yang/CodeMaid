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
		public static OpenApiSchemaInfo GetTypeInfo(this OpenApiSchema OpenApiSchema)
		{
			var type = OpenApiSchema.Reference?.Id ?? OpenApiSchema.Type switch
			{
				"string" => OpenApiSchema.Format switch
				{
					"uuid" => "Guid",
					"binary" => "Stream",
					"date" => "DateOnly",
					"date-time" => "DateTimeOffset",
					_ => "string",
				},
				"number" => OpenApiSchema.Format switch
				{
					"float" => "float",
					"double" => "double",
					_ => "decimal",
				},
				"integer" => OpenApiSchema.Format switch
				{
					"int64" => "long",
					"int32" => "int",
					_ => "int",
				},
				"boolean" => "bool",
				"array" => $"array",
				_ => "JsonElement",
			};
			return new OpenApiSchemaInfo()
			{
				CanBeNull = OpenApiSchema.Nullable,
				Required = false,
				Type = type,
				IsArray = OpenApiSchema.Type == "array",
				Item = OpenApiSchema.Items?.GetTypeInfo(),
				IsRef = OpenApiSchema.Reference != null,
			};
		}
	}
}
