using System.Collections.Concurrent;
using System.Text.Encodings.Web;

using Api.Extensions;
using Api.Tools;

using ExtensionMethods;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Readers;

using Models.CodeMaid;

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
			OpenApiDocument openApiDocument = new OpenApiStringReader().Read(json, out _);
			RestfulApiDocument restfulApiDocument = new RestfulApiDocument(openApiDocument);
			var PATH = Path.Combine(maid.Project.Path, maid.DestinationPath, restfulApiDocument.Title + ".cs");
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

public class {{restfulApiDocument.Title}}
{
	private readonly HttpClient httpClient;

	public {{restfulApiDocument.Title}}(HttpClient httpClient)
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
			var newC = c;

			foreach (var api in restfulApiDocument.Api)
			{

				var bodyId = string.Empty;
				var bodyParameterName = string.Empty;
				//响应类型的名称
				var responseTypeName = api.ResponseType;

				var hasNullResponse = api.MaybeReturnNull;
				if (hasNullResponse)
				{
					responseTypeName += "?";
				}
				var methodName = api.MethodName;

				var m = newC.ChildNodes().OfType<MethodDeclarationSyntax>().ToList().FirstOrDefault(x => x.Identifier.Text == methodName);
				if (m != null)
				{
					//newC = newC.ReplaceNode(m, (MethodDeclarationSyntax)SyntaxFactory.ParseMemberDeclaration(api.CreateMethod()));
					//当已经存在方法的时候跳过
					continue;
				}
				else
				{
					newC = newC.AddMembers((MethodDeclarationSyntax)SyntaxFactory.ParseMemberDeclaration(api.CreateMethod())!);
				}

			}
			await FileTools.Write(PATH, CSharpSyntaxTree.ParseText("").GetCompilationUnitRoot(), unit.ReplaceNode(c, newC));
			Md5s[maid.SourcePath] = md5;
			await Console.Out.WriteLineAsync("生成");

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
				"array" => $"List<{OpenApiSchema.Items.GetTypeString()}>",
				_ => OpenApiSchema.Reference?.Id,
			};
			return OpenApiSchema.Nullable ? $"{type}?" : type;
		}
	}
	class RestfulApiDocument
	{
		public RestfulApiDocument(OpenApiDocument openApiDocument)
		{
			Title = openApiDocument.Info.Title.Replace(" ", "_");
			foreach (var item in openApiDocument.Paths)
			{
				foreach (var operation in item.Value.Operations)
				{
					//如果响应不包含200则不进行记录
					if (!operation.Value.Responses.Any(x => x.Key == "200"))
					{
						//log
						continue;
					}
					RestfulApiModel restfulApiModel = new RestfulApiModel()
					{
						Path = item.Key,
						Method = operation.Key,
						ResponseType = operation.Value.Responses["200"].Content.Any() ?
						operation.Value.Responses["200"].Content.FirstOrDefault().Value.Schema.GetTypeString()
						: "Stream",
						MaybeReturnNull = operation.Value.Responses.Any(x => x.Key == "204"),
						Id = operation.Value.OperationId,
						Summary = operation.Value.Summary,
						BodyParameter = operation.Value.RequestBody?.Content.Select(x => new BodyContent()
						{
							ContentType = x.Key,
							Body = x.Value.Schema.GetTypeString(),
							Form = x.Value.Schema.Properties.ToDictionary(x => x.Key, x => x.Value.GetTypeString()),
						}).ToList(),
						QueryParameter = operation.Value.Parameters.Where(x => x.In == ParameterLocation.Query).ToDictionary(x => x.Name, x => x.Schema.GetTypeString()),
						PathParameter = operation.Value.Parameters.Where(x => x.In == ParameterLocation.Path).ToDictionary(x => x.Name, x => x.Schema.GetTypeString()),
						HeaderParameter = operation.Value.Parameters.Where(x => x.In == ParameterLocation.Header).ToDictionary(x => x.Name, x => x.Schema.GetTypeString()),
					};
					if (restfulApiModel.Path.Contains("ImportFromExcel"))
					{
						Console.WriteLine(1);
					}
					Api.Add(restfulApiModel);
				}
			}
		}
		/// <summary>
		/// 标题
		/// </summary>
		public string Title { get; private set; }
		/// <summary>
		/// Api
		/// </summary>
		public List<RestfulApiModel> Api { get; private set; } = new List<RestfulApiModel>();
	}
	class RestfulApiModel
	{
		/// <summary>
		/// 接口路径
		/// </summary>
		public required string Path;
		/// <summary>
		/// Http方法
		/// </summary>
		public required OperationType Method;
		/// <summary>
		/// 响应类型
		/// </summary>
		public required string ResponseType;
		/// <summary>
		/// 是否会有null响应 响应码为204
		/// </summary>
		public required bool MaybeReturnNull;
		/// <summary>
		/// 注释
		/// </summary>
		public required string Summary;
		/// <summary>
		/// body参数
		/// </summary>
		public required List<BodyContent>? BodyParameter;

		/// <summary>
		/// Operation Id
		/// </summary>
		public required string Id;
		public string MethodName { get => Id.ToNamingConvention(NamingConvention.PascalCase) ?? Path.Replace('/', '_').ToNamingConvention(NamingConvention.PascalCase); }
		/// <summary>
		/// 查询参数
		/// </summary>
		public required Dictionary<string, string> QueryParameter { get; internal set; }
		/// <summary>
		/// 路由参数
		/// </summary>
		public required Dictionary<string, string> PathParameter { get; internal set; }
		/// <summary>
		/// Header参数
		/// </summary>
		public required Dictionary<string, string> HeaderParameter { get; internal set; }

		public string CreateMethod()
		{
#if DEBUG
			ResponseType = "object";
			BodyParameter?.ForEach(x => x.Body = x.Body is null ? null : "object");
#endif
			//组装请求url
			var url = Path;
			if (QueryParameter.Count > 0) url += "?";
			{
				var paraList = new List<string>();
				foreach (var item in QueryParameter.ToDictionary(x => x.Key.ToNamingConvention(NamingConvention.camelCase), x => x.Value))
					paraList.Add($"{UrlEncoder.Default.Encode(item.Key)}={{{item.Key}}}");
				url += string.Join('&', paraList);
			}
			//合并所有参数 作为方法的入参
			var para = PathParameter.Union(QueryParameter).Union(HeaderParameter).ToList();
			//此处只处理一种contentType
			if (BodyParameter != null)
			{
				var item = BodyParameter.First();
				if (item.Body != null)
				{
					para.Add(new KeyValuePair<string, string>("body", item.Body));
				}
			}
			para = para.Select(x => new KeyValuePair<string, string>(x.Key.ToNamingConvention(NamingConvention.camelCase), x.Value)).ToList();
			//确定body内容
			var body = BodyParameter?.FirstOrDefault();
			var bodyContnet = string.Empty;
			if (body != null)
				bodyContnet = body.HasJsonContentType ? $"var content = JsonContent.Create(body);" : "var content = new MultipartFormDataContent();";
			return $$"""
					/// <summary>
					/// {{Summary}}
					/// </summary>
					/// <returns></returns>
					public async Task<{{ResponseType}}> {{MethodName}}({{string.Join(", ", para.Select(x => $"{x.Value} {x.Key}"))}})
					{
						{{(body != null ? bodyContnet : "")}}
						var httpRequestMessage = new HttpRequestMessage()
						{
							Method = HttpMethod.{{Method}},
							RequestUri = new Uri($"{{url}}", UriKind.Relative),
							{{(body != null ? "Content = content," : "")}}
						};
						var response = await httpClient.SendAsync(httpRequestMessage);
						{{(MaybeReturnNull ? "if (response.StatusCode == System.Net.HttpStatusCode.NoContent) return null;" : "")}}
						{{ResponseType switch
			{
				"Stream" => $"return await response.Content.ReadAsStreamAsync();",
				_ => $"return await GetResult<{ResponseType}>(httpRequestMessage, response);"
			}}}
					}

				""";
		}
	}
	public class BodyContent
	{
		public required string ContentType { get; set; }
		///<inheritdoc cref="HttpRequestJsonExtensions.HasJsonContentType(HttpRequest)"/>
		public bool HasJsonContentType
		{
			get
			{
				if (!Microsoft.Net.Http.Headers.MediaTypeHeaderValue.TryParse(ContentType, out var mt)) return false;
				// Matches application/json
				if (mt.MediaType.Equals("application/json", StringComparison.OrdinalIgnoreCase)) return true;
				// Matches +json, e.g. application/ld+json
				if (mt.Suffix.Equals("json", StringComparison.OrdinalIgnoreCase)) return true;
				return false;
			}
		}
		/// <summary>
		/// Body的类型
		/// </summary>
		public required string? Body { get; set; }
		public required Dictionary<string, string> Form { get; set; }
	}

}
