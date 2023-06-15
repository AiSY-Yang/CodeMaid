using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;

using Api.Extensions;
using Api.Tools;

using ExtensionMethods;

using MassTransit.Internals.GraphValidation;

using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Extensions;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Readers;

using Models.CodeMaid;

using ServicesModels.Settings;

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
			string json;
			if (Uri.TryCreate(maid.SourcePath, UriKind.Absolute, out var uri))
			{
				HttpClient httpClient = new() { BaseAddress = uri };
				json = await httpClient.GetStringAsync("");
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
			var setting = maid.Setting == null ? new HttpClientSyncSetting() : JsonSerializer.Deserialize<HttpClientSyncSetting>(maid.Setting) ?? new HttpClientSyncSetting();
			OpenApiDocument openApiDocument = new OpenApiStringReader().Read(json, out _);
			RestfulApiDocument restfulApiDocument = new RestfulApiDocument(openApiDocument);
			#region create model file
			if (setting.CreateModel)
			{
				//模型文件路径
				var modelPath = Path.Combine(maid.Project.Path, maid.DestinationPath, restfulApiDocument.Title + "Model.cs");
				CompilationUnitSyntax modelUnit;
				if (File.Exists(modelPath))
					modelUnit = CSharpSyntaxTree.ParseText(File.ReadAllText(modelPath)).GetCompilationUnitRoot();
				else
					modelUnit = CSharpSyntaxTree.ParseText("""
				#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。
				using System.Text.Json.Serialization;
				namespace RestfulClient;
				
				#pragma warning restore CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。
				""").GetCompilationUnitRoot();
				var modelUnitNew = modelUnit;
				foreach (var item in restfulApiDocument.Models)
				{
					var obj = modelUnitNew.GetDeclarationSyntaxes<BaseTypeDeclarationSyntax>().Where(x => x.Identifier.Text == item.PropertyStyleName).FirstOrDefault();
					if (obj == null)
						modelUnitNew = modelUnitNew.AddMembers(SyntaxFactory.ParseMemberDeclaration(item.ToString())!);
					else
						modelUnitNew = modelUnitNew.ReplaceNode(obj, SyntaxFactory.ParseMemberDeclaration(item.ToString())!);
				}
				await FileTools.Write(modelPath, modelUnit, modelUnitNew);
			}
			#endregion

			//文件路径
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
using System.Text.Json;

namespace RestfulClient;

public partial class {{restfulApiDocument.Title}}
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
				_ => OpenApiSchema.Reference?.Id ?? "object",
			};
			return OpenApiSchema.Nullable ? $"{type}?" : type;
		}
	}
	class RestfulApiDocument
	{
		public RestfulApiDocument(OpenApiDocument openApiDocument)
		{
			Title = openApiDocument.Info.Title.Replace(" ", "_");
			foreach (var item in openApiDocument.Components.Schemas)
			{
				RestfulModel restfulModel;
				if (item.Value.Enum.Count > 0)
				{
					var enums = new List<EnumField>();
					for (int i = 0; i < item.Value.Enum.Count; i++)
					{
						EnumField enumField = new EnumField()
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
						Type = item.Value.Type,
						Summary = item.Value.Description,
					};
				}
				else
				{
					restfulModel = new RestfulClassModel()
					{
						Name = item.Key,
						Type = item.Value.Type,
						Summary = item.Value.Description,
						ClassFields = item.Value.Properties.Select(x => new ClassField()
						{
							Name = x.Key,
							Type = x.Value.GetTypeString(),
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
					if (!operation.Value.Responses.Any(x => x.IsSuccessResponse()))
					{
						//log
						continue;
					}
					RestfulApiModel restfulApiModel = new RestfulApiModel()
					{
						Path = item.Key,
						Method = operation.Key,
						ResponseType = operation.Value.Responses.Where(x => x.IsSuccessResponse()).First().Value.Content.Any() ?
						operation.Value.Responses.Where(x => x.IsSuccessResponse()).First().Value.Content.FirstOrDefault().Value.Schema.GetTypeString()
						: "Stream",
						MaybeReturnNull = operation.Value.Responses.Any(x => x.Key == "204"),
						Id = operation.Value.OperationId,
						Summary = operation.Value.Summary,
						BodyParameter = operation.Value.RequestBody?.Content.Select(x => new BodyContent()
						{
							ContentType = x.Key,
							BodyType = x.Value.Schema.GetTypeString(),
							Form = x.Value.Schema.Properties.Select(x => new ClassField() { Name = x.Key, Type = x.Value.GetTypeString(), Summary = x.Value.Description }).ToList(),
						}).ToList(),
						QueryParameter = operation.Value.Parameters.Where(x => x != null)
						.Where(x => x.In == ParameterLocation.Query).Select(x => new ClassField() { Name = x.Name, Type = x.Schema.GetTypeString(), Summary = x.Description }).ToList(),
						PathParameter = operation.Value.Parameters
						.Where(x => x != null)
						.Where(x => x.In == ParameterLocation.Path)
						.Select(x => new ClassField() { Name = x.Name, Type = x.Schema.GetTypeString(), Summary = x.Description, Required = true }).ToList(),
						HeaderParameter = operation.Value.Parameters.Where(x => x != null)
						.Where(x => x.In == ParameterLocation.Header).Select(x => new ClassField() { Name = x.Name, Type = x.Schema.GetTypeString(), Summary = x.Description }).ToList(),
					};
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
		public List<RestfulModel> Models { get; private set; } = new List<RestfulModel>();
	}


	class RestfulClassModel : RestfulModel
	{
		public required List<ClassField> ClassFields { get; set; }
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine($"/// <summary>");
			stringBuilder.AppendLine($"/// {Summary}");
			stringBuilder.AppendLine($"/// </summary>");
			stringBuilder.AppendLine($"public class {PropertyStyleName}");
			stringBuilder.AppendLine("{");
			foreach (var item in ClassFields)
			{
				stringBuilder.AppendLine($"	/// <summary>");
				stringBuilder.AppendLine($"	/// {item.Summary}");
				stringBuilder.AppendLine($"	/// </summary>");
				stringBuilder.AppendLine($"	[JsonPropertyName(\"{item.Name}\")]");
				stringBuilder.AppendLine($"	public {item.Type} {(item.PropertyStyleName == PropertyStyleName ? $"{item.PropertyStyleName}_Field" : item.PropertyStyleName)} {{ get; set; }}");
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
			StringBuilder stringBuilder = new StringBuilder();
			if (Type == "string") stringBuilder.AppendLine($"[JsonConverter(typeof(JsonStringEnumConverter))]");
			stringBuilder.AppendLine($"public enum {PropertyStyleName}");
			stringBuilder.AppendLine("{");
			foreach (var item in EnumFields)
			{
				if (item.Value is null)
				{
					if (Type == "string") stringBuilder.AppendLine($"\t[System.Runtime.Serialization.EnumMember(Value = \"{item.Name}\")]");
					stringBuilder.AppendLine($"	{item.PropertyStyleName},");
				}
				else
				{
					if (Type == "string") stringBuilder.AppendLine($"\t[System.Runtime.Serialization.EnumMember(Value = \"{item.Name}\")]");
					stringBuilder.AppendLine($"	{item.PropertyStyleName} = {item.Value},");
				}
			}
			stringBuilder.AppendLine("}");
			return stringBuilder.ToString();
		}
	}
	class RestfulModel : Field
	{
		/// <summary>
		/// 类型 如果是枚举的话可能自定义类型转换器
		/// </summary>
		public required string Type { get; set; }
		/// <summary>
		/// 类或者枚举的注释
		/// </summary>
		public required string? Summary { get; set; }

		/// <summary>
		/// 转换为C#类定义字符串
		/// </summary>
		/// <returns></returns>
		public override string? ToString() => base.ToString();
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
		private string? summary;
		/// <summary>
		/// 属性类型
		/// </summary>
		public required string Type { get; set; }
		/// <summary>
		/// 是否必须
		/// </summary>
		public bool Required { get; set; }
		/// <summary>
		/// 是否可以为null
		/// </summary>
		public bool MaybeNull { get; set; }
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
			return Required switch
			{
				true => $"\t\t{Content("")};",
				false => $"\t\tif ({VariableStyleName} is not null) {Content(".Value")};",

			};
			string Content(string s) => Type switch
			{
				"Stream" => $"content.Add({VariableStyleName}{s}.content, nameof({VariableStyleName}), {VariableStyleName}{s}.fileName)",
				"string" => $"content.Add(new StringContent({VariableStyleName}), nameof({VariableStyleName}))",
				"bool" or "int" or "long" => $"content.Add(JsonContent.Create({VariableStyleName}{s}), nameof({VariableStyleName}))",
				_ => $"content.Add(JsonContent.Create({VariableStyleName}), nameof({VariableStyleName}))",
			};
		}
		/// <summary>
		/// AddContent内容
		/// </summary>
		public string GetParameter()
		{
			return Required switch
			{
				true => $"{Content()} {VariableStyleName}",
				false => $"{Content()}? {VariableStyleName}",

			};
			string Content() => Type switch
			{
				"Stream" => $"(HttpContent content, string fileName)",
				_ => Type,
			};
		}
		#endregion

	}
	/// <summary>
	/// 字段信息
	/// </summary>
	public class Field
	{
		static char[] number = new char[] { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0' };
		/// <summary>
		/// 名称
		/// </summary>
		public required string Name { get; set; }
		/// <summary>
		/// 属性风格名称
		/// </summary>
		public string PropertyStyleName => (number.Contains(Name[0]) ? $"Field_{Name}" : Name).ToNamingConvention(NamingConvention.PascalCase);
		/// <summary>
		/// 变量风格名称
		/// </summary>
		public string VariableStyleName => (number.Contains(Name[0]) ? $"Field_{Name}" : Name).ToNamingConvention(NamingConvention.camelCase);

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
		public required string ResponseType { get; set; }
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
		public required string? Id { get; set; }
		public string MethodName { get => Id?.ToNamingConvention(NamingConvention.PascalCase) ?? Path.Replace('/', '_').ToNamingConvention(NamingConvention.PascalCase); }
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


		public string CreateMethod()
		{
			//组装请求url
			var url = Path;
			//更改路由参数命名规则
			foreach (var parameter in PathParameter)
				url = url.Replace($"{{{parameter.Name}}}", $"{{{parameter.Name.ToNamingConvention(NamingConvention.camelCase)}}}");
			//添加查询参数
			if (QueryParameter.Count > 0) url += "?";
			{
				var paraList = new List<string>();
				foreach (var item in QueryParameter)
					if (item.Required)
						paraList.Add($"{UrlEncoder.Default.Encode(item.Name)}={{{item.VariableStyleName}}}");
					else
						paraList.Add($"{{({item.VariableStyleName} == null ? \"\" : $\"{UrlEncoder.Default.Encode(item.Name)}={{{item.VariableStyleName}}}\")}}");
				url += string.Join('&', paraList);
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
						form.Required = false;
						para.Add(form);
					}
				}
			}
			//确定body内容
			var body = BodyParameter?.FirstOrDefault();
			var argument = string.Join(", ", para.Select(x => x.GetParameter()));
			return $$"""
					/// <summary>
					/// {{Summary}}
					/// </summary>
					/// <returns></returns>
				{{string.Concat(para.Select(x => $"	/// <param name=\"{x.Name.ToNamingConvention(NamingConvention.camelCase)}\">{x.Summary}</param>{Environment.NewLine}"))
				//拼接参数信息
				}}	public async Task<{{ResponseType}}{{(MaybeReturnNull ? "?" : "")}}> {{MethodName}}({{argument}})
					{
				{{(body != null
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
							RequestUri = new Uri($"{{url}}", UriKind.Relative),{{(body != null ? "\r\n\t\t\tContent = content," : "")}}
						};
						{{
						//如果返回的是流的话 默认提前响应
						ResponseType switch
						{
							"Stream" => $"var response = await httpClient.SendAsync(httpRequestMessage, HttpCompletionOption.ResponseHeadersRead);",
							_ => $"var response = await httpClient.SendAsync(httpRequestMessage);"
						}}}
						{{(MaybeReturnNull ? "if (response.StatusCode == System.Net.HttpStatusCode.NoContent) return null;" : "")}}
						{{ResponseType switch
						{
							"Stream" => $"return await response.Content.ReadAsStreamAsync();",
							"string" => $"return await response.Content.ReadAsStringAsync();",
							_ => $"return await GetResult<{ResponseType}>(httpRequestMessage, response);"
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
		public required string BodyType { get; set; }
		/// <summary>
		/// body的form参数
		/// </summary>
		public required List<ClassField> Form { get; set; }
	}

}
