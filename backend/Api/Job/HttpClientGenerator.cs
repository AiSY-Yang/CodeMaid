using System.Collections.Concurrent;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;

using Api.Extensions;
using Api.Tools;

using ExtensionMethods;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Readers;

using Models.CodeMaid;

using ServicesModels.Settings;

using static Api.Job.NamingConvert;

namespace Api.Job
{
	/// <summary>
	/// Http客户端生成器
	/// </summary>
	public class HttpClientGenerator
	{
		static readonly ConcurrentDictionary<string, string> Md5s = new();
		private readonly HttpClient httpClient;
		private readonly ILogger<HttpClientGenerator> logger;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="httpClientFactory"></param>
		/// <param name="logger"></param>
		public HttpClientGenerator(IHttpClientFactory httpClientFactory, ILogger<HttpClientGenerator> logger)
		{
			this.httpClient = httpClientFactory.CreateClient("ignoreCertificate");
			this.httpClient.Timeout = TimeSpan.FromSeconds(10);
			this.logger = logger;
		}

		/// <summary>
		/// 生成Http客户端
		/// </summary>
		/// <param name="maid">maid信息</param>
		/// <returns></returns>
		public async Task ExecuteAsync(Maid maid)
		{
			string json;
			if (Uri.TryCreate(maid.SourcePath, UriKind.Absolute, out var uri))
			{
				try
				{
					var res = await httpClient.GetAsync(uri);
					if (!res.IsSuccessStatusCode)
					{

						logger.LogError("生成HTTP客户端错误,maid{maid},{url}返回状态码为{StatusCode}", maid.Name, uri, res.StatusCode);
						return;
					}
					json = await res.Content.ReadAsStringAsync();
				}
				catch (Exception ex)
				{
					logger.LogError(ex, "生成HTTP客户端错误,maid{maid},{url}", maid.Name, uri);
					return;
				}
			}
			else
			{
				json = maid.SourcePath;
			}
			var md5 = json.Hash(HashOption.MD5_32);
			if (Md5s.TryGetValue(maid.SourcePath, out string? lastMd5) && lastMd5 == md5)
			{
				return;
			}
			else
			{
				Md5s[maid.SourcePath] = md5;
			}
			var setting = maid.Setting.Deserialize<HttpClientSyncSetting>() ?? new HttpClientSyncSetting();
			OpenApiDocument openApiDocument = new OpenApiStringReader().Read(json, out var diagnostic);
			RestfulApiDocument restfulApiDocument = new(openApiDocument);
			#region create model file
			if (setting.CreateModel)
			{
				//模型文件路径
				var modelPath = Path.Combine(maid.Project.Path, setting.ModelPath ?? maid.DestinationPath, restfulApiDocument.PropertyStyleName + "Model.cs");
				CompilationUnitSyntax modelUnit;
				if (File.Exists(modelPath))
					modelUnit = CSharpSyntaxTree.ParseText(File.ReadAllText(modelPath)).GetCompilationUnitRoot();
				else
					modelUnit = CSharpSyntaxTree.ParseText($$"""
				#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。
				using System.Text.Json.Serialization;
				{{(setting.CreateModel ? $"namespace RestfulClient.{restfulApiDocument.PropertyStyleName}Model;{Environment.NewLine}" : "")}}				
				#pragma warning restore CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。
				""").GetCompilationUnitRoot();
				var modelUnitNew = modelUnit;
				foreach (var item in restfulApiDocument.Models)
				{
					var obj = modelUnitNew.GetDeclarationSyntaxes<BaseTypeDeclarationSyntax>().Where(x => x.Identifier.Text == PropertyStyle(item.Name)).FirstOrDefault();
					if (obj == null)
						modelUnitNew = modelUnitNew.AddMembers(SyntaxFactory.ParseMemberDeclaration(item.ToString())!);
					else
						modelUnitNew = modelUnitNew.ReplaceNode(obj, SyntaxFactory.ParseMemberDeclaration(item.ToString())!);
				}
				await FileTools.Write(modelPath, modelUnit, modelUnitNew);
			}
			#endregion

			//文件路径
			var PATH = Path.Combine(maid.Project.Path, maid.DestinationPath, restfulApiDocument.PropertyStyleName + ".cs");
			CompilationUnitSyntax unit;
			if (File.Exists(PATH))
			{
				unit = CSharpSyntaxTree.ParseText(File.ReadAllText(PATH)).GetCompilationUnitRoot();
			}
			else
			{
				var server = openApiDocument.Servers.FirstOrDefault()?.Url ?? "http://localhost:5000";
				var text = $$"""
using System.Net.Http.Json;
using System.Text.Encodings.Web;
using System.Text.Json;

using Microsoft.Extensions.Logging;

using RestfulClient.{{restfulApiDocument.PropertyStyleName}}Model;

namespace RestfulClient;

public partial class {{restfulApiDocument.PropertyStyleName}}
{
	private readonly HttpClient httpClient;
	private readonly ILogger<{{restfulApiDocument.PropertyStyleName}}> logger;

	public {{restfulApiDocument.PropertyStyleName}}(HttpClient httpClient, ILogger<{{restfulApiDocument.PropertyStyleName}}> logger, IOptions<{{restfulApiDocument.PropertyStyleName}}Options> options)
	{
		var url = options.Value.Url;
		httpClient.BaseAddress = new Uri(url);
		httpClient.Timeout = TimeSpan.FromMinutes(5);
		this.httpClient = httpClient;
		this.logger = logger;
	}
	public async Task<T> GetResult<T>(HttpRequestMessage request, HttpResponseMessage response)
	{
		try
		{
			response.EnsureSuccessStatusCode();
			return (await response.Content.ReadFromJsonAsync<T>())!;
		}
		catch (HttpRequestException ex)
		{
			logger.LogError(ex, "{name}请求失败:,请求地址{address},状态码{statusCode},参数{body}响应{s2}"
				, nameof({{restfulApiDocument.PropertyStyleName}})
				, request.RequestUri
				, response.StatusCode
				, request.Content == null ? null : await request.Content.ReadAsStringAsync()
				, await response.Content.ReadAsStringAsync());
			throw;
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "{name}请求失败:,请求地址{address},状态码{statusCode},参数{body}响应{s2}"
				, nameof({{restfulApiDocument.PropertyStyleName}})
				, request.RequestUri
				, response.StatusCode
				, request.Content == null ? null : await request.Content.ReadAsStringAsync()
				, await response.Content.ReadAsStringAsync());
			throw;
		}
	}
}
""";
				unit = CSharpSyntaxTree.ParseText(text).GetCompilationUnitRoot();
			}
			var c = unit.GetDeclarationSyntaxes<ClassDeclarationSyntax>().First();
			var newC = c;

			foreach (var api in restfulApiDocument.Api)
			{
				var methodName = api.MethodName;
				var m = newC.ChildNodes().OfType<MethodDeclarationSyntax>().ToList().FirstOrDefault(x => x.Identifier.Text == methodName);
				if (m != null)
				{
					newC = newC.ReplaceNode(m, (MethodDeclarationSyntax)SyntaxFactory.ParseMemberDeclaration(api.CreateMethod())!);
					//当已经存在方法的时候跳过
					continue;
				}
				else
				{
					newC = newC.AddMembers((MethodDeclarationSyntax)SyntaxFactory.ParseMemberDeclaration(api.CreateMethod())!);
				}

			}
			await FileTools.Write(PATH, CSharpSyntaxTree.ParseText("").GetCompilationUnitRoot(), unit.ReplaceNode(c, newC));
			logger.LogInformation("生成{name}Http客户端", restfulApiDocument.PropertyStyleName);
		}
	}
	static class ApiModelExtensions
	{
		public static OpenApiSchemaInfo GetTypeInfo(this OpenApiSchema OpenApiSchema)
		{
			if (OpenApiSchema is null) return new OpenApiSchemaInfo()
			{
				CanBeNull = false,
				Required = false,
				Type = "Stream",
				IsArray = false,
				Item = null,
				IsRef = false,
			};
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
	class RestfulApiDocument
	{
		public RestfulApiDocument(OpenApiDocument openApiDocument)
		{
			Title = openApiDocument.Info.Title;
			Description = openApiDocument.Info.Description;
			//Gets all schemas for class generation
			foreach (var item in openApiDocument.Components.Schemas)
			{
				RestfulModel restfulModel;
				//for c# tuple while has a schema like "System.ValueTuple`2[[System.String, System.Private.CoreLib, Version=7.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e],[System.String, System.Private.CoreLib, Version=7.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]]"
				// ignore it 
				if (item.Key.EndsWith(']')) continue;
				//is enum object
				if (item.Value.Enum.Count > 0)
				{
					var enums = new List<EnumField>();
					for (int i = 0; i < item.Value.Enum.Count; i++)
					{
						EnumField enumField = new()
						{
							Name = item.Value.Extensions.ContainsKey("x-enumNames")
							? (item.Value.Extensions["x-enumNames"] as OpenApiArray)![i] switch
							{
								OpenApiString s => s.Value,
								_ => throw new Exception()
							}
							: item.Value.Enum[i] switch
							{
								OpenApiInteger number => "Value" + number.Value,
								OpenApiLong number => "Value" + number.Value,
								OpenApiString s => s.Value,
								_ => throw new Exception()
							},
							Value = item.Value.Enum[i] switch
							{
								OpenApiInteger number => number.Value,
								OpenApiLong number => number.Value,
								OpenApiString s => null,
								_ => throw new Exception()
							},
						};
						enums.Add(enumField);
					}
					restfulModel = new RestfulEnumModel()
					{
						Name = item.Key,
						EnumFields = enums,
						SchemaType = item.Value.Type,
						Summary = item.Value.Description,
					};
				}
				else
				{
					restfulModel = new RestfulClassModel()
					{
						Name = item.Key,
						SchemaType = item.Value.Type,
						Summary = item.Value.Description,
						ClassFields = item.Value.Properties.Select(x => new ClassField()
						{
							Name = x.Key,
							Type = x.Value.GetTypeInfo(),
							Summary = x.Value.Description,
						}).ToList(),
					};
				}
				Models.Add(restfulModel);
			}
			foreach (var item in openApiDocument.Paths)
			{
				foreach (var operation in item.Value.Operations)
				{
					//如果响应不包含200则不进行记录
					if (!operation.Value.Responses.Any(x => x.IsSuccessResponse())) continue;
					var successResponse = operation.Value.Responses.Where(x => x.IsSuccessResponse()).First();
					var responseType = successResponse.Value != null && successResponse.Value.Content.Any() ?
								successResponse.Value.Content.First().Value.Schema.GetTypeInfo()
								: new OpenApiSchemaInfo() { CanBeNull = false, Type = OpenApiSchemaInfo.stream, IsArray = false, IsRef = false, Required = true };
					RestfulApiModel restfulApiModel = new()
					{
						Path = item.Key,
						Method = operation.Key,
						ResponseType = responseType,
						MaybeReturnNull = operation.Value.Responses.Any(x => x.Key == "204"),
						Id = operation.Value.OperationId,
						Summary = operation.Value.Summary,
						BodyParameter = operation.Value.RequestBody?.Content.Select(x => new BodyContent()
						{
							ContentType = x.Key,
							BodyType = x.Value.Schema.GetTypeInfo(),
							Form = x.Value.Schema.Properties.Select(x => new ClassField() { Name = x.Key, Type = x.Value.GetTypeInfo(), Summary = x.Value.Description }).ToList(),
						}).ToList(),
						QueryParameter = operation.Value.Parameters.Where(x => x != null)
						.Where(x => x.In == ParameterLocation.Query).Select(x => new ClassField() { Name = x.Name, Type = x.Schema.GetTypeInfo(), Summary = x.Description }).ToList(),
						PathParameter = operation.Value.Parameters
						.Where(x => x != null)
						.Where(x => x.In == ParameterLocation.Path)
						.Select(x => new ClassField() { Name = x.Name, Type = x.Schema.GetTypeInfo(), Summary = x.Description }).ToList(),
						HeaderParameter = operation.Value.Parameters.Where(x => x != null)
						.Where(x => x.In == ParameterLocation.Header).Select(x => new ClassField() { Name = x.Name, Type = x.Schema.GetTypeInfo(), Summary = x.Description }).ToList(),
					};
					Api.Add(restfulApiModel);
				}
			}
		}
		/// <summary>
		/// 标题
		/// </summary>
		public string Title { get; private set; }
		public string PropertyStyleName => Title.ToNamingConvention(NamingConvention.PascalCase);
		public string Description { get; private set; }

		/// <summary>
		/// Api
		/// </summary>
		public List<RestfulApiModel> Api { get; private set; } = new List<RestfulApiModel>();
		public List<RestfulModel> Models { get; private set; } = new List<RestfulModel>();
	}


	class RestfulClassModel : RestfulModel
	{
		public required List<ClassField> ClassFields { get; set; }
		public override string ToString()
		{
			var className = PropertyStyle(Name);
			StringBuilder stringBuilder = new();
			stringBuilder.AppendLine($"/// <summary>");
			stringBuilder.AppendLine($"/// {Summary}");
			stringBuilder.AppendLine($"/// </summary>");
			stringBuilder.AppendLine($"public class {className}");
			stringBuilder.AppendLine("{");
			foreach (var item in ClassFields)
			{
				var fieldName = PropertyStyle(item.Name);
				stringBuilder.AppendLine($"	/// <summary>");
				stringBuilder.AppendLine($"	/// {item.Summary}");
				stringBuilder.AppendLine($"	/// </summary>");
				stringBuilder.AppendLine($"	[JsonPropertyName(\"{item.Name}\")]");
				stringBuilder.AppendLine($"	public {item.Type.ParameterCode} {(fieldName == className ? $"{className}_Field" : fieldName)} {{ get; set; }}");
			}
			stringBuilder.AppendLine("}");
			return stringBuilder.ToString();
		}

	}
	class RestfulEnumModel : RestfulModel
	{
		public required List<EnumField> EnumFields { get; set; }
		public override string ToString()
		{
			var enumName = PropertyStyle(Name);
			StringBuilder stringBuilder = new();
			if (SchemaType == "string") stringBuilder.AppendLine($"[JsonConverter(typeof(JsonEnumMemberJsonConverter))]");
			stringBuilder.AppendLine($"public enum {enumName}");
			stringBuilder.AppendLine("{");
			foreach (var item in EnumFields)
			{
				var fieldName = PropertyStyle(item.Name);
				if (item.Value is null)
				{
					if (SchemaType == "string") stringBuilder.AppendLine($"\t[System.Runtime.Serialization.EnumMember(Value = \"{item.Name}\")]");
					stringBuilder.AppendLine($"	{fieldName},");
				}
				else
				{
					if (SchemaType == "string") stringBuilder.AppendLine($"\t[System.Runtime.Serialization.EnumMember(Value = \"{item.Name}\")]");
					stringBuilder.AppendLine($"	{fieldName} = {item.Value},");
				}
			}
			stringBuilder.AppendLine("}");
			return stringBuilder.ToString();
		}
	}

	abstract class RestfulModel : Field
	{
		/// <summary>
		/// 类型 如果是枚举的话可能自定义类型转换器
		/// </summary>
		public required string SchemaType { get; set; }
		/// <summary>
		/// 类或者枚举的注释
		/// </summary>
		public required string? Summary { get; set; }
	}
	/// <summary>
	/// 枚举字段信息
	/// </summary>
	class EnumField : Field
	{
		public required long? Value { get; set; }
	}
	/// <summary>
	/// 类的字段信息
	/// </summary>
	public class ClassField : Field
	{
		/// <summary>
		/// 属性类型
		/// </summary>
		public required OpenApiSchemaInfo Type { get; set; }
		private string? summary;
		/// <summary>
		/// 字段的注释
		/// </summary>
		public required string? Summary { get => summary?.Replace('\n', ' ').Replace('\r', ' '); set => summary = value; }

		#region Method
		/// <summary>
		/// AddContent内容
		/// </summary>
		public string GetGetAddContent()
		{
			var name = VariableStyle(Name);
			//只有当非必填 且参数不可以为null时才忽略
			return (Type.Required || Type.CanBeNull) switch
			{
				true => $"\t\t{Content("")};",
				false => $"\t\tif ({name} is not null) {Content(".Value")};",
			};
			string Content(string s) => Type.Type switch
			{
				"Stream" => Type.CanBeNull ? $"content.Add({name}{s}.content, nameof({name}), {name}{s}.fileName)" : $"content.Add({name}{s}.content, nameof({name}), {name}{s}.fileName)",
				"Guid" => $"content.Add(new StringContent({name}{s}.ToString()), nameof({name}))",
				"string" => $"content.Add(new StringContent({name}), nameof({name}))",
				"bool" or "int" or "long" => $"content.Add(JsonContent.Create({name}{s}), nameof({name}))",
				_ => $"content.Add(JsonContent.Create({name}), nameof({name}))",
			};
		}
		/// <summary>
		/// 函数参数
		/// </summary>
		public string GetParameter()
		{
			var name = VariableStyle(Name);
			return $"{Content()} {name}";
			string Content() => Type.Type switch
			{
				OpenApiSchemaInfo.stream => Type.Required ? $"(HttpContent content, string fileName)" : $"(HttpContent content, string fileName)?",
				_ => Type.ParameterCode,
			};
		}
		#endregion
	}
	/// <summary>
	/// 字段信息
	/// </summary>
	public abstract class Field
	{
		/// <summary>
		/// 变量名称
		/// </summary>
		public required string Name { get; set; }
	}

	/// <summary>
	/// schema信息
	/// </summary>
	public class OpenApiSchemaInfo
	{
		/// <summary>
		/// 作为参数的类型 判断required
		/// </summary>
		public string ParameterCode => Code + (CanBeNull || !Required ? "?" : "");
		/// <summary>
		/// 作为返回值的类型 不判断required
		/// </summary>
		/// <returns></returns>
		public string ReturnCode => Code + (CanBeNull ? "?" : "");
		string Code => (IsRef ? NamingConvert.PropertyStyle(Type)
			: Item is null ? Type : $"List<{Item.Code}>");
		/// <summary>
		/// 流类型
		/// </summary>
		public const string stream = "Stream";
		/// <summary>
		/// 类型
		/// </summary>
		public required string Type { get; set; }
		/// <summary>
		/// 是否必须
		/// </summary>
		public required bool Required { get; set; }
		/// <summary>
		/// 是否可以为null
		/// </summary>
		public required bool CanBeNull { get; set; }
		/// <summary>
		/// 是否是数组
		/// </summary>
		public bool IsArray { get; internal set; }
		/// <summary>
		/// 是否是模型引用
		/// </summary>
		public bool IsRef { get; internal set; }
		public OpenApiSchemaInfo? Item { get; internal set; }
	}
	/// <summary>
	/// 命名风格转换器
	/// </summary>
	public static class NamingConvert
	{
		static readonly char[] number = new char[] { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0' };
		/// <summary>
		/// 属性风格名称
		/// </summary>
		public static string PropertyStyle(string s) => StringExtension.ToNamingConvention(number.Contains(s[0]) ? $"Field_{s}" : s, NamingConvention.PascalCase);
		/// <summary>
		/// 变量风格名称
		/// </summary>
		public static string VariableStyle(string s) => StringExtension.ToNamingConvention(number.Contains(s[0]) ? $"Field_{s}" : s, NamingConvention.camelCase);
	}


	class RestfulApiModel
	{
		/// <summary>
		/// 接口路径
		/// </summary>
		public required string Path { get; set; }
		/// <summary>
		/// Http方法
		/// </summary>
		public required OperationType Method { get; set; }
		/// <summary>
		/// 响应类型
		/// </summary>
		public required OpenApiSchemaInfo ResponseType { get; set; }
		/// <summary>
		/// 是否会有null响应 响应码为204
		/// </summary>
		public required bool MaybeReturnNull { get; set; }
		/// <summary>
		/// 注释
		/// </summary>
		public required string Summary { get; set; }
		/// <summary>
		/// body参数
		/// </summary>
		public required List<BodyContent>? BodyParameter { get; set; }

		/// <summary>
		/// Operation Id
		/// </summary>
		public required string? Id { private get; set; }
		/// <summary>
		/// 无效的符号
		/// </summary>
		static readonly char[] InvalidChars = new[] { '<', '>', '{', '}' };
		public string MethodName
		{
			get => Id is null
				? Path.Replace('/', '_').ToNamingConvention(NamingConvention.PascalCase)
				: new string(Id.Where(x => !InvalidChars.Contains(x)).ToArray()).ToNamingConvention(NamingConvention.PascalCase);
		}
		/// <summary>
		/// 查询参数
		/// </summary>
		public required List<ClassField> QueryParameter { get; internal set; }
		/// <summary>
		/// 路由参数
		/// </summary>
		public required List<ClassField> PathParameter { get; internal set; }
		/// <summary>
		/// Header参数
		/// </summary>
		public required List<ClassField> HeaderParameter { get; internal set; }

		/// <summary>
		/// 生成请求方法
		/// </summary>
		/// <returns></returns>
		public string CreateMethod()
		{
			//组装请求url
			var url = Path.TrimStart('/');
			//更改路由参数命名规则
			foreach (var parameter in PathParameter)
				url = url.Replace($"{{{parameter.Name}}}", $"{{{parameter.Name.ToNamingConvention(NamingConvention.camelCase)}}}");
			//添加查询参数
			var queryParameters = new List<string>();
			foreach (var item in QueryParameter)
			{
				if (item.GetParameter().StartsWith("List<"))
					queryParameters.Add($"queryParameters.Add(string.Join('&', {VariableStyle(item.Name)}.Select(x => $\"{VariableStyle(item.Name)}={{x}}\")));");
				else
					queryParameters.Add($"queryParameters.Add($\"{UrlEncoder.Default.Encode(item.Name)}={{{VariableStyle(item.Name)}}}\");");
				if (item.Type.CanBeNull || !item.Type.Required)
					queryParameters[^1] = $"if ({VariableStyle(item.Name)} != null) " + queryParameters[^1];
			}
			//合并所有参数 作为方法的入参
			var para = PathParameter.Union(QueryParameter).Union(HeaderParameter).ToList();
			//此处只处理一种contentType
			if (BodyParameter != null)
			{
				var item = BodyParameter.First();
				if (item.HasJsonContentType)
				{
					para.Add(new ClassField() { Name = "body", Type = item.BodyType, Summary = null });
				}
				else
				{
					foreach (var form in item.Form)
					{
						para.Add(form);
					}
				}
			}
			//确定body内容
			var body = BodyParameter?.FirstOrDefault();
			var argument = string.Join(", ", para.Select(x => x.GetParameter()));
			var queryParametersCode = queryParameters.Count > 0 ? $"		var queryParameters = new List<string>();\r\n{string.Concat(queryParameters.Select(x => $"\t\t{x}\r\n"))}" : "";
			var urlQueryParametersCode = queryParameters.Count > 0 ? "{(queryParameters.Count > 0 ? \"?\" + string.Join('&', queryParameters) : \"\")}" : "";
			var paramNamesXml = string.Concat(para.Select(x => $"	/// <param name=\"{x.Name.ToNamingConvention(NamingConvention.camelCase)}\">{x.Summary}</param>{Environment.NewLine}"));
			return $$"""
					/// <summary>
					/// {{Summary}}
					/// </summary>
					/// <returns></returns>
				{{paramNamesXml}}	public async Task<{{ResponseType.ReturnCode}}{{(MaybeReturnNull ? "?" : "")}}> {{MethodName}}({{argument}})
					{
				{{queryParametersCode}}{{(body != null
				? body.HasJsonContentType
					? $"		var content = JsonContent.Create(body);\r\n"
					: $"""
							var content = new MultipartFormDataContent();
					{string.Join("\r\n", body.Form.Select(x => x.GetGetAddContent()))}

					"""
				: "")
				//body
				}}		var httpRequestMessage = new HttpRequestMessage()
						{
							Method = HttpMethod.{{Method}},
							RequestUri = new Uri($"{{url}}{{urlQueryParametersCode}}", UriKind.Relative),{{(body != null ? "\r\n\t\t\tContent = content," : "")}}
						};
						{{
						//如果返回的是流的话 默认提前响应
						ResponseType.Type switch
						{
							OpenApiSchemaInfo.stream => $"var response = await httpClient.SendAsync(httpRequestMessage, HttpCompletionOption.ResponseHeadersRead);",
							_ => $"var response = await httpClient.SendAsync(httpRequestMessage);"
						}}}
						{{(MaybeReturnNull ? "if (response.StatusCode == System.Net.HttpStatusCode.NoContent) return null;" : "")}}
						{{ResponseType.Type switch
						{
							OpenApiSchemaInfo.stream => $"return await response.Content.ReadAsStreamAsync();",
							"string" => $"return await response.Content.ReadAsStringAsync();",
							_ => $"return await GetResult<{ResponseType.ReturnCode}>(httpRequestMessage, response);"
						}}}
					}

				""";
		}
	}
	/// <summary>
	/// Body内容
	/// </summary>
	public class BodyContent
	{
		/// <summary>
		/// body的contentType
		/// </summary>
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
		public required OpenApiSchemaInfo BodyType { get; set; }
		/// <summary>
		/// body的form参数
		/// </summary>
		public required List<ClassField> Form { get; set; }
	}

}
