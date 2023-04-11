using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Readers;
using ExtensionMethods;
using System.Text.Encodings.Web;
using System.Text;
using Api.Tools;
using Microsoft.CodeAnalysis;
using Models.CodeMaid;
using System.Text.Json;
using ServicesModels.Settings;
using System.Collections.Concurrent;
using Api.Extensions;

namespace Api.Job
{
	/// <summary>
	/// Http客户端生成器
	/// </summary>
	public class HttpClientGenerator
	{
		static readonly ConcurrentDictionary<string, string> Md5s = new();
		/// <summary>
		/// 生成Http客户端
		/// </summary>
		/// <param name="maid">maid信息</param>
		/// <returns></returns>
		public async Task ExecuteAsync(Maid maid)
		{
			HttpClient httpClient = new() { BaseAddress = new Uri(maid.SourcePath) };
			var json = await httpClient.GetStringAsync("");
			var md5 = json.Hash(HashOption.MD5_32);
			if (Md5s.TryGetValue(maid.SourcePath, out string? lastMd5) && lastMd5 == md5)
			{
				return;
			}
			else
			{
				Md5s[maid.SourcePath] = md5;
				await Console.Out.WriteLineAsync("生成");
			}
			OpenApiDocument openApiDocument = new OpenApiStringReader().Read(json, out _);
			var PATH = Path.Combine(maid.Project.Path, maid.DestinationPath, openApiDocument.Info.Title + ".cs");
			CompilationUnitSyntax unit;
			if (File.Exists(PATH))
			{
				unit = CSharpSyntaxTree.ParseText(File.ReadAllText(PATH)).GetCompilationUnitRoot();
			}
			else
			{
				var text = $$"""
using System.Net.Http.Json;

using SnakeCharmerModel;

public class {{openApiDocument.Info.Title}}
{
	private readonly HttpClient httpClient;

	public {{openApiDocument.Info.Title}}(HttpClient httpClient)
	{
		httpClient.BaseAddress = new Uri("http://localhost:5000");
#if DEBUG
		httpClient.BaseAddress = new Uri("http://localhost:5000");
#endif
		httpClient.Timeout = TimeSpan.FromMinutes(5);
		this.httpClient = httpClient;
	}
	public static async Task<T> GetResult<T>(HttpRequestMessage request, HttpResponseMessage response)
	{
		return (await response.Content.ReadFromJsonAsync<T>())!;
	}
}
""";
				unit = CSharpSyntaxTree.ParseText(text).GetCompilationUnitRoot();
			}
			var c = unit.GetDeclarationSyntaxes<ClassDeclarationSyntax>().First();
			var ms = c.ChildNodes().OfType<MethodDeclarationSyntax>().ToList();
			var newC = c;
			foreach (var item in openApiDocument.Paths)
			{
				foreach (var operation in item.Value.Operations)
				{
					var bodyId = string.Empty;
					var bodyParameterName = string.Empty;
					//响应类型的名称
					var responseTypeName = string.Empty;
					if (!operation.Value.Responses.Any(x => x.Key == "200"))
					{
						//log
						continue;
					}
					if (operation.Value.Responses["200"].Content.Any())
					{
						responseTypeName = operation.Value.Responses["200"].Content.FirstOrDefault().Value.Schema.GetTypeString();
					}
					else
					{
						responseTypeName = "Stream";
					}
					//返回值是否可能为null
					var hasNullResponse = operation.Value.Responses.Any(x => x.Key == "204");
					if (hasNullResponse)
					{
						responseTypeName += "?";
					}
					var methodName = operation.Value.OperationId?.ToNamingConvention(NamingConvention.PascalCase) ?? item.Key.Replace('/', '_').ToNamingConvention(NamingConvention.PascalCase);

					var m = ms.FirstOrDefault(x => x.Identifier.Text == methodName);
					if (m != null)
					{
						//当已经存在方法的时候跳过
						continue;
					}
					var stringBuilder = new StringBuilder();
					stringBuilder.AppendLine($"\t/// <summary>");
					stringBuilder.AppendLine($"\t/// {operation.Value.Summary}");
					stringBuilder.AppendLine($"\t/// </summary>");
					stringBuilder.AppendLine($"\t/// <returns></returns>");
					stringBuilder.Append($"\tpublic async ");
					//method return
					stringBuilder.Append($"Task<{responseTypeName}> ");
					//method name
					stringBuilder.Append($"{methodName}(");
					//method parameter				
					stringBuilder.Append(string.Join(", ", operation.Value.Parameters.Select(x => $"{x.Schema.GetTypeString()} {x.Name}")));
					if (operation.Value.RequestBody != null)
					{
						bodyId = operation.Value.RequestBody.Content.First().Value.Schema.GetTypeString();
						bodyParameterName = bodyId.ToNamingConvention(NamingConvention.camelCase);
						stringBuilder.Append($"{bodyId} {bodyParameterName}");
					}
					stringBuilder.AppendLine(")");
					stringBuilder.AppendLine("\t{");
					//add HttpRequestMessage
					stringBuilder.AppendLine("\t\tvar httpRequestMessage = new HttpRequestMessage()");
					stringBuilder.AppendLine("\t\t{");
					stringBuilder.AppendLine($"\t\t\tMethod = HttpMethod.{operation.Key},");
					//build url
					var url = $"{item.Key}?";
					foreach (var parameter in operation.Value.Parameters)
					{
						switch (operation.Value.Parameters.First().In)
						{
							case ParameterLocation.Query:
								url += $$"""{{UrlEncoder.Default.Encode(parameter.Name)}}={{{parameter.Name}}}&""";
								break;
							case ParameterLocation.Header:
								break;
							case ParameterLocation.Path:
								break;
							case ParameterLocation.Cookie:
								break;
							case null:
								break;
							default:
								break;
						}
					}
					stringBuilder.AppendLine($"\t\t\tRequestUri = new Uri($\"{url.TrimEnd('&', '?')}\", UriKind.Relative),");
					if (operation.Value.RequestBody != null)
					{
						switch (operation.Value.RequestBody.Content.Keys.First())
						{
							case "application/json":
							case "text/json":
							case "application/*+json":
								stringBuilder.AppendLine($"\t\t\tContent = JsonContent.Create({bodyParameterName}),");
								break;
						}
					}
					//HttpRequestMessage over
					stringBuilder.AppendLine("\t\t};");
					stringBuilder.AppendLine("		var response = await httpClient.SendAsync(httpRequestMessage);");
					if (hasNullResponse)
					{
						stringBuilder.AppendLine("""
							if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
							{
								return null;
							}
					""");
					}
					//achieve response data
					switch (responseTypeName)
					{
						case "Stream":
							stringBuilder.AppendLine($"\t\treturn await response.Content.ReadAsStreamAsync();");
							break;
						default:
							stringBuilder.AppendLine($"\t\treturn await GetResult<{responseTypeName}>(httpRequestMessage, response);");
							break;
					}
					stringBuilder.AppendLine("\t}");
					newC = newC.AddMembers(SyntaxFactory.ParseMemberDeclaration(stringBuilder.ToString())!);
				}
			}
			await FileTools.Write(PATH, unit, unit.ReplaceNode(c, newC));
		}
	}
	static class ApiModelExtensions
	{
		/// <summary>
		/// 获取OpenApi模型类型
		/// <a href="https://swagger.io/docs/specification/data-models/data-types/"/>
		/// </summary>
		/// <param name="OpenApiSchema"></param>
		/// <returns></returns>
		public static string GetTypeString(this OpenApiSchema OpenApiSchema)
		{
			var type = OpenApiSchema.Type switch
			{
				"string" => "string",
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
				"array" => "",
				_ => OpenApiSchema.Reference.Id,
			};
			return OpenApiSchema.Nullable ? $"{type}?" : type;
		}
	}
}
