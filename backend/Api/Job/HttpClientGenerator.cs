using System.Collections.Concurrent;
using System.ComponentModel;
using System.Net;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;

using Api.Extensions;
using Api.Tools;

using ExtensionMethods;

using Grpc.Net.Client.Configuration;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Readers;

using Models.CodeMaid;

using Serilog;

using ServicesModels.Settings;

using static Api.Job.NamingConvert;

namespace Api.Job
{
	/// <summary>
	/// Http客户端生成器
	/// </summary>
	public class HttpClientGenerator
	{
		static readonly ConcurrentDictionary<(long, string), string> Md5s = new();
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
			try
			{
				var setting = maid.Setting.Deserialize<HttpClientSyncSetting>()!;
				string json;
				var url = setting.SourceUrl;
				string? host = null;
				if (url is not null)
				{
					host = new Uri(url).Host;
					try
					{
						var res = await httpClient.GetAsync(url);
						if (!res.IsSuccessStatusCode)
						{
							logger.LogError("{maid}生成HTTP客户端错误,{url}返回状态码为{StatusCode}", maid.Name, url, res.StatusCode);
							return;
						}
						json = await res.Content.ReadAsStringAsync();
					}
					catch (Exception ex)
					{
						logger.LogError(ex, "{maid}生成HTTP客户端错误,{url}", maid.Name, url);
						return;
					}
				}
				else
				{
					json = setting.SwaggerDocument!;
				}
				var md5 = json.Hash(HashOption.MD5_32);
				if (Md5s.TryGetValue((maid.Id, maid.SourcePath), out string? lastMd5) && lastMd5 == md5)
				{
					return;
				}
				else
				{
					Md5s[(maid.Id, maid.SourcePath)] = md5;
				}
				OpenApiDocument openApiDocument = new OpenApiStringReader().Read(json, out var diagnostic);
				RestfulApiDocument restfulApiDocument = new(openApiDocument);
				if (!setting.RenameClient.IsNullOrEmpty()) restfulApiDocument.Title = setting.RenameClient.Replace("*", restfulApiDocument.Title);


				#region create model file
				var needCreateModel = !setting.ModelPath.IsNullOrEmpty();
				if (needCreateModel)
				{
					var modelDirectory = Path.Combine(maid.Project.Path, setting.ModelPath!);
					Directory.CreateDirectory(modelDirectory);
					//模型文件路径
					var modelPath = setting.HttpClientType switch
					{
						HttpClientType.CSharp => Path.Combine(modelDirectory, restfulApiDocument.PropertyStyleName + "Model.cs"),
						HttpClientType.TypeScript => Path.Combine(modelDirectory, restfulApiDocument.PropertyStyleName + "Model.ts"),
						_ => throw new InvalidEnumArgumentException()
					};
					switch (setting.HttpClientType)
					{
						case HttpClientType.CSharp:
							{
								CompilationUnitSyntax modelUnit;
								if (File.Exists(modelPath))
									modelUnit = CSharpSyntaxTree.ParseText(File.ReadAllText(modelPath)).GetCompilationUnitRoot();
								else
									modelUnit = CSharpSyntaxTree.ParseText($$"""
				#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。
				using System.Text.Json.Serialization;
				namespace RestfulClient.{{restfulApiDocument.PropertyStyleName}}Model;
				#pragma warning restore CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。

				""").GetCompilationUnitRoot();
								var modelUnitNew = modelUnit;
								foreach (var item in restfulApiDocument.Models)
								{
									var obj = modelUnitNew.GetDeclarationSyntaxes<BaseTypeDeclarationSyntax>().Where(x => x.Identifier.Text == PropertyStyle(item.Name)).FirstOrDefault();
									if (obj == null)
										modelUnitNew = modelUnitNew.AddMembers(SyntaxFactory.ParseMemberDeclaration(item.ToCode())!);
									else
										modelUnitNew = modelUnitNew.ReplaceNode(obj, SyntaxFactory.ParseMemberDeclaration(item.ToCode())!);
								}
								await FileTools.Write(modelPath, modelUnit, modelUnitNew);
							}
							break;
						case HttpClientType.TypeScript:
							{
								StringBuilder stringBuilder = new StringBuilder();
								foreach (var item in restfulApiDocument.Models)
								{
									var text = item.ToTsCode();
									stringBuilder.Append(text);
								}
								await File.WriteAllTextAsync(modelPath, stringBuilder.ToString());
							}
							break;
						default:
							break;
					}
				}
				#endregion
				var needCreateOptions = !setting.OptionsPath.IsNullOrEmpty();
				if (needCreateOptions)
				{
					var optionsDirectory = Path.Combine(maid.Project.Path, setting.OptionsPath!);
					Directory.CreateDirectory(optionsDirectory);

					switch (setting.HttpClientType)
					{
						case HttpClientType.CSharp:
							{
								var optionsPath = Path.Combine(optionsDirectory, restfulApiDocument.PropertyStyleName + "Options.cs");
								if (!File.Exists(optionsPath))
								{
									File.WriteAllText(optionsPath, $$"""
namespace RestfulClient;

public class {{restfulApiDocument.PropertyStyleName}}Options
{
	/// <summary>
	/// 服务地址
	/// </summary>
	public required string Url { get; set; }
	/// <summary>
	/// 超时时间
	/// </summary>
	public double TimeoutSecond { get; set; } = 30;
}
""");
								}
								break;
							}
						case HttpClientType.TypeScript:
							{
								var optionsPath = Path.Combine(optionsDirectory, restfulApiDocument.PropertyStyleName + "Options.ts");
								if (!File.Exists(optionsPath))
								{
									File.WriteAllText(optionsPath, $$"""
class {{restfulApiDocument.PropertyStyleName}}Option {
	baseURL = '{{host ?? "http://localhost"}}'
}
const option = new {{restfulApiDocument.PropertyStyleName}}Option()
export default option
""");
								}
								break;
							}
						default:
							break;
					}

				}
				#region Create Client
				var clientDirectory = Path.Combine(maid.Project.Path, setting.ClientPath);
				Directory.CreateDirectory(clientDirectory);
				switch (setting.HttpClientType)
				{
					case HttpClientType.CSharp:
						{
							//文件路径
							var PATH = Path.Combine(clientDirectory, restfulApiDocument.PropertyStyleName + ".cs");
							CompilationUnitSyntax unit;
							CompilationUnitSyntax unitNew;
							if (File.Exists(PATH))
							{
								unit = CSharpSyntaxTree.ParseText(File.ReadAllText(PATH)).GetCompilationUnitRoot();
								unitNew = unit;
							}
							else
							{
								var text = $$"""
using System.CodeDom.Compiler;
using System.Net.Http.Json;
using System.Text.Encodings.Web;
using System.Text.Json;
{{(needCreateModel ? $"using RestfulClient.{restfulApiDocument.PropertyStyleName}Model;\r\n" : "")}}

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace RestfulClient;

public partial class {{restfulApiDocument.PropertyStyleName}}
{
	private readonly HttpClient httpClient;
	private readonly ILogger<{{restfulApiDocument.PropertyStyleName}}> logger;

	public {{restfulApiDocument.PropertyStyleName}}(HttpClient httpClient, ILogger<{{restfulApiDocument.PropertyStyleName}}> logger, IOptions<{{restfulApiDocument.PropertyStyleName}}Options> options)
	{
		var url = options.Value.Url;
		var timeout = options.Value.TimeoutSecond;
		httpClient.BaseAddress = new Uri(url);
		httpClient.Timeout = TimeSpan.FromSeconds(timeout);
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
			logger.LogError(ex, "{name}请求失败:请求地址{address},状态码{statusCode},参数{body}响应{s2}"
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
								unit = CSharpSyntaxTree.ParseText("").GetCompilationUnitRoot();
								unitNew = CSharpSyntaxTree.ParseText(text).GetCompilationUnitRoot();
							}
							var c = unitNew.GetDeclarationSyntaxes<ClassDeclarationSyntax>().First();
							var newC = c;

							foreach (var api in restfulApiDocument.Api)
							{
								var methodName = api.MethodName;
								var m = newC.ChildNodes().OfType<MethodDeclarationSyntax>().ToList().FirstOrDefault(x => x.Identifier.Text == methodName);
								if (m != null)
								{
									if (m.AttributeLists.Any(x => x.Attributes.Any(x => x.Name.ToString() == "GeneratedCode")))
										newC = newC.ReplaceNode(m, (MethodDeclarationSyntax)SyntaxFactory.ParseMemberDeclaration(api.CreateMethod())!);
									//当已经存在方法的时候跳过
									continue;
								}
								else
								{
									newC = newC.AddMembers((MethodDeclarationSyntax)SyntaxFactory.ParseMemberDeclaration(api.CreateMethod())!);
								}
							}
							//移除已被删除的接口
							var methodNode = newC.ChildNodes().OfType<MethodDeclarationSyntax>().ToList().Where(m => m.AttributeLists.Any(x => x.Attributes.Any(x => x.Name.ToString() == "GeneratedCode")));
							var allMethodName = methodNode.Select(x => x.Identifier.Text).ToList();
							var removedMethod = methodNode.Where(node => !restfulApiDocument.Api.Any(x => x.MethodName == node.Identifier.Text)).ToList();
							newC = newC.RemoveNodes(removedMethod, SyntaxRemoveOptions.KeepNoTrivia);

							await FileTools.Write(PATH, unit, unitNew.ReplaceNode(c, newC!));
						}
						break;
					case HttpClientType.TypeScript:
						{
							var filePpath = Path.Combine(clientDirectory, restfulApiDocument.PropertyStyleName + ".ts");
							StringBuilder stringBuilder = new StringBuilder();
							stringBuilder.AppendLine($$"""
							import { $Fetch, ofetch } from 'ofetch'
							import option from './{{restfulApiDocument.PropertyStyleName}}Options'
							import * as Model from './{{restfulApiDocument.PropertyStyleName}}Model'

							class {{restfulApiDocument.PropertyStyleName}} {
								/** The client for making HTTP requests */
								client: $Fetch
								constructor() {
									this.client = ofetch.create({ baseURL: option.baseURL })
								}
							""");
							foreach (var api in restfulApiDocument.Api)
							{
								var parameter = new List<string>();
								var parameterType = new List<string>();
								//解析查询参数类型
								if (api.QueryParameter.Count > 0)
								{
									var getParamsTypeBuilder = new StringBuilder();
									var queryList = new List<string>();
									foreach (var item in api.QueryParameter)
										queryList.Add($"{item.Name}: {(item.Type.ReferenceId is not null ? "Model." : "")}{item.Type.TypeScriptType}");
									getParamsTypeBuilder.Append(string.Join("; ", queryList));
									parameter.Add("params");
									parameterType.Add($"params: {{{getParamsTypeBuilder}}}");
								}
								if (api.BodyParameter?.Count > 0)
								{
									parameter.Add("body");
									parameterType.Add($"body: any");
								}
								var parameterCodeText = $"{{ {string.Join(", ", parameter)} }} : {{ {string.Join("; ", parameterType)} }}";
								var responseType = api.RestfulApiResponses.First().ResponseType;
								var responseCodeText = responseType?.TypeScriptType is null ? "Blob"
									: responseType.ReferenceId is not null ? $"Model.{responseType.TypeScriptType}"
									: responseType.TypeScriptType;
								//Console.WriteLine(api.Id);
								//var methodName = api.Method + api.Path.Replace('/', '_').ToNamingConvention(NamingConvention.PascalCase).TrimStart("Api");
								var methodName = api.Method + (api.Id ?? api.Path.Replace("/", "").ToNamingConvention(NamingConvention.PascalCase).TrimStart("Api"));
								stringBuilder.AppendLine($$"""	{{methodName}}({{parameterCodeText}}): Promise<{{responseCodeText}}> {""");
								stringBuilder.AppendLine($$"""		return this.client('{{api.Path}}', {""");
								stringBuilder.AppendLine($$"""			method: '{{api.Method}}',""");
								if (api.QueryParameter.Count > 0) stringBuilder.AppendLine($$"""			params: params,""");
								if (api.BodyParameter?.Count > 0) stringBuilder.AppendLine($$"""			body: body,""");
								stringBuilder.AppendLine($$"""		})""");
								stringBuilder.AppendLine($$"""	}""");
							}
							stringBuilder.AppendLine("}");
							stringBuilder.AppendLine($"const {restfulApiDocument.PropertyStyleName}Client = new {restfulApiDocument.PropertyStyleName}()");
							stringBuilder.AppendLine($"export default {restfulApiDocument.PropertyStyleName}Client");
							await File.WriteAllTextAsync(filePpath, stringBuilder.ToString());

						}
						break;
					default:
						break;
				}
				#endregion
				logger.LogInformation("{maid}生成{name}Http客户端", maid.Name, restfulApiDocument.PropertyStyleName);
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "客户端生成错误");
			}
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
					if (!operation.Value.Responses.Any(x => x.IsSuccessResponse()))
					{
						//log
						continue;
					}
					var restfulApiResponses = new List<RestfulApiResponse>();
					foreach (var res in operation.Value.Responses)
					{
						var content = res.Value.Content.FirstOrDefault().Value;
						if (content is null)
							restfulApiResponses.Add(new RestfulApiResponse()
							{
								HttpStatusCode = res.Key,
								ResponseType = null,
							});
						else
							restfulApiResponses.Add(new RestfulApiResponse() { HttpStatusCode = res.Key, ResponseType = res.Value.Content.FirstOrDefault().Value?.Schema?.GetTypeInfo() });
					}
					RestfulApiModel restfulApiModel = new()
					{
						Path = item.Key,
						Method = operation.Key,
						RestfulApiResponses = restfulApiResponses,
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
					if (restfulApiModel.Method == OperationType.Get && restfulApiModel.BodyParameter is not null)
						Log.Warning("方法{MethodName}是Get请求,但是body不为空", restfulApiModel.MethodName);

					Api.Add(restfulApiModel);
				}
			}
		}
		/// <summary>
		/// 标题
		/// </summary>
		public string Title { get; set; }
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
		public override string ToCode()
		{
			var className = PropertyStyle(Name);
			StringBuilder stringBuilder = new();
			stringBuilder.AppendLine($"/// <summary>");
			if (Summary is null)
				stringBuilder.AppendLine($"/// ");
			else
				Summary.Split('\n').ToList().ForEach(x => stringBuilder.AppendLine($"/// {x}"));
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
		public override string ToTsCode()
		{
			StringBuilder stringBuilder = new StringBuilder();
			AppendTsSummary(stringBuilder, Summary, "");
			stringBuilder.AppendLine($$"""export interface {{PropertyStyle(Name)}} {""");
			foreach (var item in ClassFields)
			{
				AppendTsSummary(stringBuilder, item.Summary, "\t");
				stringBuilder.AppendLine($$"""	{{TsStyle(item.Name)}}: {{item.Type.TypeScriptType}},""");
			}
			stringBuilder.AppendLine($$"""}""");
			return stringBuilder.ToString();
		}
	}
	class RestfulEnumModel : RestfulModel
	{
		public required List<EnumField> EnumFields { get; set; }
		public override string ToCode()
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
		public override string ToTsCode()
		{
			StringBuilder stringBuilder = new StringBuilder();
			AppendTsSummary(stringBuilder, Summary, "");
			stringBuilder.AppendLine($$"""enum {{PropertyStyle(Name)}} {""");
			foreach (var item in EnumFields)
			{
				stringBuilder.AppendLine($$"""	{{VariableStyle(item.Name)}}= {{item.Value}},""");
			}
			stringBuilder.AppendLine($$"""}""");
			return stringBuilder.ToString();
		}
	}

	abstract class RestfulModel : Field
	{
		public abstract string ToCode();
		public abstract string ToTsCode();
		/// <summary>
		/// 类型 如果是枚举的话可能自定义类型转换器
		/// </summary>
		public required string SchemaType { get; set; }
		/// <summary>
		/// 类或者枚举的注释
		/// </summary>
		public required string? Summary { get; set; }

		protected static void AppendTsSummary(StringBuilder stringBuilder, string? summary, string leader)
		{
			if (summary is null)
				stringBuilder.AppendLine($$"""{{leader}}/**  */""");
			else
			{
				var summaries = summary.Split('\n', StringSplitOptions.RemoveEmptyEntries)
									.Select(x => x.Trim())
									.ToList();
				if (summaries.Count == 1)
					stringBuilder.AppendLine($$"""{{leader}}/** {{summaries[0]}} */""");
				else
				{
					stringBuilder.AppendLine($$"""{{leader}}/**""");
					summaries.ForEach(x => stringBuilder.AppendLine($" *{x}"));
					stringBuilder.AppendLine($$"""{{leader}} */""");
				}
			}
		}
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
		/// <summary>
		/// 字段的注释
		/// </summary>
		public required string? Summary { get => summary?.Replace('\n', ' ').Replace('\r', ' '); set => summary = value; }
		private string? summary;

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
			string Content(string s) => Type.CSharpType switch
			{
				"Stream" => Type.CanBeNull ? $"content.Add({name}{s}.content, nameof({name}), {name}{s}.fileName)" : $"content.Add({name}{s}.content, nameof({name}), {name}{s}.fileName)",
				"Guid" => $"content.Add(new StringContent({name}{s}.ToString()), nameof({name}))",
				"string" => $"content.Add(new StringContent({name}), nameof({name}))",
				"bool" or "int" or "long" => $"content.Add(JsonContent.Create({name}{s}), nameof({name}))",
				_ => $"content.Add(JsonContent.Create({name}), nameof({name}))",
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
		/// typescript type definition
		/// </summary>
		public string TypeScriptType => ReferenceId is not null ? NamingConvert.PropertyStyle(ReferenceId) :
			Item is not null ? $"{Item.CSharpType}[]" :
			Type switch
			{
				"integer" => "number",
				null => "Blob",
				_ => Type,
			}
			+ (CanBeNull ? " | null" : "") + (!Required ? " | undefined" : "");
		/// <summary>
		/// 作为参数的类型 判断required
		/// </summary>
		public string ParameterCode => CSharpType + (CanBeNull || !Required ? "?" : "");
		/// <summary>
		/// 作为返回值的类型 不判断required
		/// </summary>
		/// <returns></returns>
		public string ReturnCode => CSharpType + (CanBeNull ? "?" : "");
		/// <summary>
		/// C#类型
		/// </summary>
		public string CSharpType => ReferenceId is not null ? NamingConvert.PropertyStyle(ReferenceId) :
			Item is not null ? $"List<{Item.CSharpType}>" :
			Type switch
			{
				"string" => Format switch
				{
					"uuid" => "Guid",
					"binary" => "Stream",
					"date" => "DateOnly",
					"date-time" => "DateTimeOffset",
					_ => "string",
				},
				"number" => Format switch
				{
					"float" => "float",
					"double" => "double",
					_ => "decimal",
				},
				"integer" => Format switch
				{
					"int64" => "long",
					"int32" => "int",
					_ => "int",
				},
				"boolean" => "bool",
				"array" => $"array",
				_ => "JsonElement",
			};

		/// <summary>
		/// 流类型
		/// </summary>
		public const string stream = "Stream";
		/// <summary>
		/// 引用对象的Id
		/// </summary>
		public required string? ReferenceId { get; set; }
		/// <summary>
		/// 类型
		/// </summary>
		public required string Type { get; set; }
		/// <summary>
		/// 格式
		/// </summary>
		public required string? Format { get; set; }
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
		/// <summary>
		/// 数组item的信息
		/// </summary>
		public OpenApiSchemaInfo? Item { get; internal set; }
	}
	/// <summary>
	/// 命名风格转换器
	/// </summary>
	public static class NamingConvert
	{
		static readonly char[] number = new char[] { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0' };
		static readonly string[] csharpKeywords = {
	"abstract", "as", "base", "bool", "break",
	"byte", "case", "catch", "char", "checked",
	"class", "const", "continue", "decimal", "default",
	"delegate", "do", "double", "else", "enum",
	"event", "explicit", "extern", "false", "finally",
	"fixed", "float", "for", "foreach", "goto",
	"if", "implicit", "in", "int", "interface",
	"internal", "is", "lock", "long", "namespace",
	"new", "null", "object", "operator", "out",
	"override", "params", "private", "protected", "public",
	"readonly", "ref", "return", "sbyte", "sealed",
	"short", "sizeof", "stackalloc", "static", "string",
	"struct", "switch", "this", "throw", "true",
	"try", "typeof", "uint", "ulong", "unchecked",
	"unsafe", "ushort", "using", "virtual", "void",
	"volatile", "while", "add", "alias", "ascending",
	"async", "await", "by", "dynamic", "equals",
	"from", "get", "global", "group", "into",
	"join", "let", "nameof", "on", "orderby",
	"partial", "remove", "select", "set", "value",
	"var", "when", "where", "yield"
};
		static readonly string[] tsKeywords = {
			    // JavaScript 保留字
    "break", "case", "catch", "class", "const",
	"continue", "debugger", "default", "delete", "do",
	"else", "export", "extends", "finally", "for",
	"function", "if", "import", "in", "instanceof",
	"new", "return", "super", "switch", "this",
	"throw", "try", "typeof", "var", "void",
	"while", "with", "yield", "async", "await",

    // TypeScript 额外保留字
    "implements", "interface", "namespace", "pure",
	"readonly", "type", "abstract", "as", "any",
	"boolean", "constructor", "declare", "enum",
	"infer", "is", "keyof", "let", "module",
	"never", "object", "override", "private",
	"protected", "public", "static", "string",
	"symbol", "unknown"
		};
		/// <summary>
		/// 属性风格名称
		/// </summary>
		public static string PropertyStyle(string s) => StringExtension.ToNamingConvention(number.Contains(s[0]) ? $"Field_{s}" : s, NamingConvention.PascalCase);
		/// <summary>
		/// 变量风格名称
		/// </summary>
		public static string VariableStyle(string s) => StringExtension.ToNamingConvention(number.Contains(s[0]) ? $"Field_{s}" : csharpKeywords.Contains(s) ? $"@{s}" : s, NamingConvention.camelCase);
		public static string TsStyle(string s) => StringExtension.ToNamingConvention(number.Contains(s[0]) ? $"Field_{s}" : tsKeywords.Contains(s) ? $"'{s}'" : s, NamingConvention.camelCase);


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
		public required List<RestfulApiResponse> RestfulApiResponses { get; set; }

		/// <summary>
		/// 注释
		/// </summary>
		public required string Summary { get; set; }
		/// <summary>
		/// body参数 不可为空的话则有required 没有nullable标记
		/// </summary>
		public required List<BodyContent>? BodyParameter { get; set; }

		/// <summary>
		/// Operation Id
		/// </summary>
		public required string? Id { get; set; }
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
		/// 查询参数 默认非required 不可为null
		/// </summary>
		public required List<ClassField> QueryParameter { get; internal set; }
		/// <summary>
		/// 路由参数 默认required 不可为null
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
				url = url.Replace($"{{{parameter.Name}}}", $"{{{VariableStyle(parameter.Name)}}}");
			//添加查询参数
			var queryParameters = new List<string>();
			foreach (var item in QueryParameter)
			{
				var name = VariableStyle(item.Name);
				var paraType = item.Type.Type switch
				{
					null => item.Type.Required ? $"(HttpContent content, string fileName)" : $"(HttpContent content, string fileName)?",
					_ => item.Type.ParameterCode,
				};
				if (paraType.StartsWith("List<"))
					queryParameters.Add($"queryParameters.Add(string.Join('&', {VariableStyle(item.Name)}.Select(x => $\"{VariableStyle(item.Name)}={{x}}\")));");
				else
					queryParameters.Add($"queryParameters.Add($\"{UrlEncoder.Default.Encode(item.Name)}={{{VariableStyle(item.Name)}}}\");");
				if (item.Type.CanBeNull || !item.Type.Required)
					queryParameters[^1] = $"if ({VariableStyle(item.Name)} != null) " + queryParameters[^1];
			}
			//合并所有参数 作为方法的入参
			List<(string type, string name, string? summary)> argument = new List<(string type, string name, string? summary)>();
			foreach (var item in PathParameter) argument.Add((item.Type.ParameterCode, VariableStyle(item.Name), item.Summary));
			foreach (var item in QueryParameter) argument.Add((item.Type.Required || item.Type.CanBeNull ? item.Type.CSharpType : item.Type.CSharpType + "?", VariableStyle(item.Name), item.Summary));
			//foreach (var item in HeaderParameter) argument.Add((item.Type.Required ? item.Type.Type : item.Type.Type + "?", VariableStyle(item.Name)));
			//此处只处理一种contentType
			if (BodyParameter != null)
			{
				var item = BodyParameter.First();
				if (item.HasJsonContentType)
				{
					argument.Add((item.BodyType.Required ? item.BodyType.CSharpType : item.BodyType.CSharpType + "?", "body", null));
				}
				else
				{
					foreach (var form in item.Form)
					{
						if (form.Type.CSharpType == "Stream")
							argument.Add(("(HttpContent content, string fileName)?", VariableStyle(form.Name), form.Summary));
						else
							argument.Add((form.Type.Required || form.Type.CanBeNull ? form.Type.CSharpType : form.Type.CSharpType + "?", VariableStyle(form.Name), form.Summary));
					}
				}
			}
			//确定body内容
			var body = BodyParameter?.FirstOrDefault();
			var argumentCode = string.Join(", ", argument.Select(x => $"{x.type} {x.name}"));
			var queryParametersCode = queryParameters.Count > 0 ? $"		var queryParameters = new List<string>();\r\n{string.Concat(queryParameters.Select(x => $"\t\t{x}\r\n"))}" : "";
			var urlQueryParametersCode = queryParameters.Count > 0 ? "{(queryParameters.Count > 0 ? \"?\" + string.Join('&', queryParameters) : \"\")}" : "";
			var paramNamesXml = string.Concat(argument.Select(x => $"	/// <param name=\"{x.name}\">{x.summary}</param>{Environment.NewLine}"));
			//解析响应类型
			var successResponses = this.RestfulApiResponses.OrderBy(x => x.HttpStatusCode).Where(x => x.HttpStatusCode.StartsWith("2")).ToList();
			var hasNullResponse = this.RestfulApiResponses.OrderBy(x => x.HttpStatusCode).Any(x => x.HttpStatusCode.StartsWith("204"));

			var ResponseType = successResponses.First().ResponseType;

			return $$"""
					/// <summary>
					/// {{Summary}}
					/// </summary>
					/// <returns></returns>
				{{paramNamesXml}}	[GeneratedCode("codeMaid", "1.0.0")]
					public async Task<{{ResponseType?.ReturnCode ?? "Stream"}}{{(hasNullResponse ? "?" : "")}}> {{MethodName}}({{argumentCode}})
					{
				{{queryParametersCode}}{{(body != null
				? body.HasJsonContentType
					? $"""
					var content = JsonContent.Create(body);

					"""
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
						ResponseType?.Type switch
						{
							null => $"var response = await httpClient.SendAsync(httpRequestMessage, HttpCompletionOption.ResponseHeadersRead);",
							_ => $"var response = await httpClient.SendAsync(httpRequestMessage);"
						}}}
						{{(hasNullResponse ? "if (response.StatusCode == System.Net.HttpStatusCode.NoContent) return null;" : "")}}
						{{ResponseType?.Type switch
						{
							null => $"return await response.Content.ReadAsStreamAsync();",
							"string" => $"return await response.Content.ReadAsStringAsync();",
							_ => $"return await GetResult<{ResponseType.ReturnCode}>(httpRequestMessage, response);"
						}}}
					}

				""";
		}
	}

	class RestfulApiResponse
	{
		public required string HttpStatusCode { get; set; }
		/// <summary>
		/// 响应类型
		/// </summary>
		public required OpenApiSchemaInfo? ResponseType { get; set; }

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
