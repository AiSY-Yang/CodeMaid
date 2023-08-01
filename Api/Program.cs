using System.Text.Json;
using System.Text.Json.Serialization;

using Api.Middleware;
using Api.Worker;

using MaidContexts;

using MassTransit;

using MasstransitModels;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

using OpenTelemetry.Exporter;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

using Serilog;

using ServicesModels.Results;
using ServicesModels.Settings;

using Swashbuckle.AspNetCore.SwaggerGen;

namespace Api
{
	/// <summary>
	/// 非顶级语句可以方便查看git记录 同时顶级语句会出现诡异的intelliCode错误
	/// </summary>
	public class Program
	{
		const string ServiceName = "CodeMaid";
		/// <summary>
		/// root ServiceProvider
		/// </summary>
		public static IServiceProvider Services { get; private set; } = null!;

		/// <summary>
		/// main函数
		/// </summary>
		/// <param name="args"></param>
		public static async Task Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);
			var openTelemetryEndpoint = builder.Configuration.GetValue<string>("OpenTelemetryEndpoint")!;
			//配置使用Serilog记录日志
			builder.Host.UseSerilog((context, services, config) =>
			{
				//从配置文件读取日志规则
				config.ReadFrom.Configuration(context.Configuration)
				//写入OpenTelemetry
				.WriteTo.OpenTelemetry(x =>
				{
					x.Endpoint = openTelemetryEndpoint;
					x.IncludedData = Serilog.Sinks.OpenTelemetry.IncludedData.TraceIdField | Serilog.Sinks.OpenTelemetry.IncludedData.TraceIdField;
				})
				;
			});
			//配置服务器
			builder.WebHost.UseKestrel((c, x) =>
			{
				x.ListenAnyIP(5241);
				x.AddServerHeader = false;
			});
			//添加映射配置
			builder.Services.AddMapster();
			//添加数据库
			string? connectionString = builder.Configuration.GetConnectionString("MaidContext");
			builder.Services.AddDbContextPool<MaidContext>((serviceProvider, x) =>
			x.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString), x => x.EnableRetryOnFailure(3).EnableIndexOptimizedBooleanColumns())
#if DEBUG
			.EnableSensitiveDataLogging()
#endif
			.UseInternalServiceProvider(serviceProvider)
			);
			builder.Services.AddEntityFrameworkMySql();
			//添加仓储
			builder.Services.AddMaidRepository();

			//添加控制器
			builder.Services.AddControllers(x =>
			{
				//将业务异常转化为自定义信息
				x.Filters.Add<HttpResponseExceptionFilter>();
				//支持从body直接接收string参数
				x.InputFormatters.Add(new TextFormatter());
			})
				.ConfigureApiBehaviorOptions(x =>
				{
					x.InvalidModelStateResponseFactory = x =>
					{
						var error = new ExceptionResult("ParameterError", string.Join(';', x.ModelState.Values.Select(x => x.Errors.FirstOrDefault()?.ErrorMessage).ToList()));
						return new ObjectResult(error) { StatusCode = 400 };
					};
				})
				//配置json解析
				.AddJsonOptions(options =>
				{
					options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
					options.JsonSerializerOptions.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
					//options.JsonSerializerOptions.Converters.Add(new Core.Converter.NullableDateOnlyJsonConverter());
					options.JsonSerializerOptions.ReadCommentHandling = System.Text.Json.JsonCommentHandling.Skip;
					options.JsonSerializerOptions.AllowTrailingCommas = true;
				});
#if DEBUG
			//调试环境下跳过授权
			builder.Services.AddSingleton<IAuthorizationMiddlewareResultHandler, SkipAuthorizationMiddleware>();
#endif
			//跨域
			builder.Services.AddCors();
			//响应缓存控制
			builder.Services.AddResponseCaching();
			//添加swagger
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen(x =>
			{
				//支持可空引用类型
				x.SupportNonNullableReferenceTypes();
				//自定义schema名称 RapiDoc不支持使用带+号的schemaName
				x.CustomSchemaIds((x) => x.FullName!.Replace("+", "_"));
				//自定义唯一ID
				x.CustomOperationIds(apiDescription =>
				{
					return apiDescription.TryGetMethodInfo(out System.Reflection.MethodInfo methodInfo) ? methodInfo.Name : null;
				});
				//添加XML注释
				foreach (var item in Directory.GetFiles(AppContext.BaseDirectory, "*.xml", SearchOption.TopDirectoryOnly))
					try
					{
						x.IncludeXmlComments(item);
					}
					catch (Exception)
					{
					}
				x.OperationFilter<AddBusinessExceptionResponse>();
				x.OperationFilter<Add204ResponseWhenReturnMaybeNull>();
				x.SchemaFilter<EnumSchemaFilter>();
				x.SchemaFilter<InheritInterfaceXmlCommentSchemaFilter>();
				//参数采用小驼峰
				x.DescribeAllParametersInCamelCase();
				//添加文档
				x.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo() { Title = "默认文档" });
				//添加DataOnly和TimeOnly的字符串支持
				x.UseDateOnlyTimeOnlyStringConverters();
			});
			//文件提供器
			Directory.CreateDirectory("/files");
			var f = new PhysicalFileProvider("/files");
			builder.Services.AddSingleton(f);
			//添加消息总线
			builder.Services.AddMassTransit(x =>
			{
				//添加所有消费者
				x.AddConsumers(System.Reflection.Assembly.GetExecutingAssembly());
				//使用内存队列
				x.UsingInMemory((context, cfg) =>
				{
					cfg.ConfigureEndpoints(context);
				});
			});
			//添加后台服务
			builder.Services.AddHostedService<TimedHostedService>();
			//添加OpenTelemetry
			var otlpOptions = new Action<OtlpExporterOptions>(opt =>
			{
				opt.Endpoint = new Uri(openTelemetryEndpoint);
			});
			builder.Services.AddOpenTelemetry()
				.ConfigureResource(x => x.AddService(ServiceName, serviceInstanceId: Environment.MachineName))
				.WithTracing(config =>
				{
					//记录对外Httpclient请求
					config.AddHttpClientInstrumentation(options =>
					{
						var maxLength = 10 * 1024;
						options.RecordException = true;
						//记录请求
						options.EnrichWithHttpRequestMessage = (activity, httpRequestMessage) =>
						{
							if (httpRequestMessage.Content != null)
							{
								var uri = httpRequestMessage.RequestUri;
								var headers = httpRequestMessage.Content.Headers;
								// 过滤过长或文件类型
								var contentLength = headers.ContentLength ?? 0;
								var contentType = headers.ContentType?.ToString();
								if (contentLength <= maxLength && (contentType == null || !contentType.Contains("multipart/form-data")))
								{
									var body = httpRequestMessage.Content.ReadAsStringAsync().Result;
									activity.SetTag("requestBody", body);
								}
							}
						};
						//记录响应
						options.EnrichWithHttpResponseMessage = (activity, httpResponseMessage) =>
						{
							if (httpResponseMessage.Content != null)
							{
								var uri = httpResponseMessage.RequestMessage?.RequestUri;
								var headers = httpResponseMessage.Content.Headers;
								// 过滤过长或文件类型
								var contentLength = headers.ContentLength ?? 0;
								var contentType = headers.ContentType?.ToString();
								if (contentLength <= maxLength && (httpResponseMessage.Content.Headers.ContentType?.MediaType) != "application/octet-stream")
								{
									// 不要使用await:The stream was already consumed. It cannot be read again
									var body = httpResponseMessage.Content.ReadAsStringAsync().Result;
									activity.SetTag("responseBody", body);
								}
							}
						};
						//记录异常
						options.EnrichWithException = (activity, exception) =>
						{
							activity.SetTag("message", exception.Message);
							activity.SetTag("stackTrace", exception.StackTrace);
						};

					});
					//记录请求
					config.AddAspNetCoreInstrumentation(options =>
					{
						options.RecordException = true;
						options.EnrichWithHttpRequest = async (activity, request) =>
						{
							// 此处过滤文件或过长的内容
							var contentLength = request.ContentLength ?? 0;
							var contentType = request.ContentType ?? string.Empty;
							if (contentLength <= 10 * 1024 && !contentType.Contains("multipart/form-data"))
							{
								request.EnableBuffering();
								request.Body.Position = 0;
								var reader = new StreamReader(request.Body);
								activity.SetTag("requestBody", await reader.ReadToEndAsync());
								request.Body.Position = 0;
							}
						};

						options.EnrichWithException = (activity, exception) =>
						{
							activity.SetTag("stackTrace", exception.StackTrace);
							activity.SetTag("message", exception.Message);
						};
					});
					config.AddSource(MassTransit.Logging.DiagnosticHeaders.DefaultListenerName);
					config.AddEntityFrameworkCoreInstrumentation();
					config.AddOtlpExporter(otlpOptions);
				})
				.WithMetrics(config =>
				{
					config.AddMeter(MassTransit.Monitoring.InstrumentationOptions.MeterName);
					config.AddAspNetCoreInstrumentation();
					config.AddHttpClientInstrumentation();
					config.AddEventCountersInstrumentation(x =>
					{
						//https://learn.microsoft.com/zh-cn/dotnet/core/diagnostics/available-counters
						x.AddEventSources("Microsoft-AspNetCore-Server-Kestrel");
						x.AddEventSources("Microsoft.AspNetCore.Hosting");
						x.AddEventSources("System.Net.Http");
					});
					config.AddRuntimeInstrumentation();
					config.AddOtlpExporter(otlpOptions);
				})
				;
			var app = builder.Build();
			//保存容器为全局变量 以便手动创建DI对象
			Services = app.Services;
			//开发环境显示swagger文档
			if (app.Environment.IsDevelopment())
			{
				//swagger配置
				app.UseSwagger();
				app.UseSwaggerUI();
			}
			//HTTPS重定向
			//app.UseHttpsRedirection();
			//添加静态文件支持
			app.UseDefaultFiles();
			app.UseStaticFiles(new StaticFileOptions()
			{
				OnPrepareResponse = context =>
				{
					//处理点击劫持漏洞
					context.Context.Response.Headers.Add("X-Frame-Options", "SAMEORIGIN");
					//CSP防止XSS
					context.Context.Response.Headers.Add("Content-Security-Policy", "script-src 'self'");
				}
			});
			//添加路由
			app.UseRouting();
			//本地化
			//app.UseRequestLocalization();
			//跨域配置
			app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod());
			//身份认证
			app.UseAuthentication();
			app.UseAuthorization();
			//响应压缩
			//app.UseResponseCompression();
			//响应缓存
			//app.UseResponseCaching();
			//使用控制器
			app.MapControllers();
			//版本信息
			app.MapGet("/version", () => $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Version}");
			//数据库迁移
			await Task.Run(() =>
				{
					var scope = app.Services.CreateScope();
					var context = scope.ServiceProvider.GetRequiredService<MaidContext>();
					context.Database.Migrate();
				});
			//初始化服务
			await Task.Run(async () =>
				{
					await InitServices.Init(app.Services);
				});
#if DEBUG
			await Task.Run(async () =>
					{
						var scope = app.Services.CreateScope();
						var context = scope.ServiceProvider.GetRequiredService<MaidContext>();
						var x = context.Maids.First(x => x.Id == 16);
						x.Setting = JsonSerializer.Deserialize<JsonElement>(JsonSerializer.Serialize(ControllerSetting.Default));
						context.SaveChanges();

						await Console.Out.WriteLineAsync("2");
						//File.WriteAllText("D:\\Code\\Template.WebApi\\Api\\Controllers\\ProjectController.cs", " " + File.ReadAllText("D:\\Code\\Template.WebApi\\Api\\Controllers\\ProjectController.cs"));
						File.Delete("D:\\Code\\Test\\WebDemo\\ProjectService.cs");
						File.Delete("D:\\Code\\Test\\WebDemo\\ProjectController.cs");
						await scope.ServiceProvider.GetRequiredService<IPublishEndpoint>().Publish(new ControllerCreateEvent()
						{
							MaidId = 16,
							ServicePath = "D:\\Code\\Test\\WebDemo\\ProjectService.cs",
							ControllerPath = "D:\\Code\\Test\\WebDemo\\ProjectController.cs",
							EntityName = "Project",
							EntityPath = "D:\\Code\\Template.WebApi\\Models.DbContext\\Project.cs",
						});

					});
#endif

			//开始运行
			app.Run();
			//程序结束的时候刷新日志
			Log.CloseAndFlush();
		}
	}
}