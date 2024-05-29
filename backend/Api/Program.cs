using System.Linq;
using System.Net;
using System.Text.Json;

using Api.Controllers;
using Api.Job;
using Api.Middleware;
using Api.Middleware.Swagger;
using Api.Worker;

using MaidContexts;

using MassTransit;

using MasstransitModels;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Models.CodeMaid;

using OpenTelemetry.Exporter;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

using Serilog;

using ServicesModels.Results;

using Swashbuckle.AspNetCore.SwaggerGen;


namespace Api
{
	/// <summary>
	/// 非顶级语句可以方便查看git记录 同时顶级语句会出现诡异的intelliCode错误
	/// </summary>
	public class Program
	{
		const string ServiceName = "CodeMaid";
		static readonly string[] I18N = ["zh-cn", "en-us"];
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
			var openTelemetryLogsEndpoint = openTelemetryEndpoint.TrimEnd('/') + "/v1/logs";
			var openTelemetryTracesEndpoint = openTelemetryEndpoint.TrimEnd('/') + "/v1/traces";
			var openTelemetryMetricsEndpoint = openTelemetryEndpoint.TrimEnd('/') + "/v1/metrics";
			//配置使用Serilog记录日志
			Serilog.Debugging.SelfLog.Enable(Console.Error);
			builder.Host.UseSerilog((context, services, config) =>
			{
				//从配置文件读取日志规则
				config.ReadFrom.Configuration(context.Configuration)
				//写入OpenTelemetry
				.WriteTo.OpenTelemetry(sinkOptions =>
				{
					sinkOptions.Endpoint = openTelemetryLogsEndpoint;
					//sinkOptions.IncludedData = Serilog.Sinks.OpenTelemetry.IncludedData.TraceIdField | Serilog.Sinks.OpenTelemetry.IncludedData.SpanIdField;
					sinkOptions.HttpMessageHandler = new HttpClientHandler()
					{
						ServerCertificateCustomValidationCallback = (a, b, c, d) => true,
						UseProxy = true,
						Proxy = new WebProxy() { BypassProxyOnLocal = true }
					};
					sinkOptions.Protocol = Serilog.Sinks.OpenTelemetry.OtlpProtocol.HttpProtobuf;
				})
				;
			});
			builder.Services.AddHttpLogging(x => { });

			//配置服务器
			builder.WebHost.UseKestrel((context, options) =>
			{
				//放开请求体大小限制
				options.Limits.MaxRequestBodySize = null;
				options.Limits.MinRequestBodyDataRate = null;
				options.ListenAnyIP(5241);
				options.AddServerHeader = false;
			});
			builder.Services.Configure<FormOptions>(options => options.MultipartBodyLengthLimit = long.MaxValue);
			//添加映射配置
			builder.Services.AddMapster();
			//添加数据库
			string? connectionString = builder.Configuration.GetConnectionString("MaidContext");
			builder.Services.AddDbContextPool<MaidContext>((serviceProvider, dbContextBuilder) =>
							dbContextBuilder.UseNpgsql(connectionString
#if DEBUG
							, builder => builder.EnableRetryOnFailure(0))
#else
							)
#endif
#if DEBUG
							.EnableDetailedErrors()
							.EnableSensitiveDataLogging()
#endif
							.UseInternalServiceProvider(serviceProvider)
			);
			//添加控制器
			builder.Services.AddControllers(options =>
			{
				//将业务异常转化为自定义信息
				options.Filters.Add<HttpResponseExceptionFilter>();
				//支持从body直接接收string参数
				options.InputFormatters.Add(new TextFormatter());
			})
				.ConfigureApiBehaviorOptions(options =>
				{
					options.InvalidModelStateResponseFactory = x =>
					{
						var error = new ExceptionResult("ParameterError", string.Join(';', x.ModelState.Values.Select(x => x.Errors.FirstOrDefault()?.ErrorMessage).ToList()));
						return new ObjectResult(error) { StatusCode = 400 };
					};
				})
				//配置json解析
				.AddJsonOptions(options =>
				{
					options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
					options.JsonSerializerOptions.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
					options.JsonSerializerOptions.ReadCommentHandling = System.Text.Json.JsonCommentHandling.Skip;
					options.JsonSerializerOptions.AllowTrailingCommas = true;
				}).AddDataAnnotationsLocalization();
			//添加响应压缩
			builder.Services.AddResponseCompression();
			//i18n
			builder.Services.AddLocalization();
			builder.Services.AddRequestLocalization(x =>
			{
				x.ApplyCurrentCultureToResponseHeaders = true;
				x.FallBackToParentCultures = true;
				x.FallBackToParentUICultures = true;
				x.AddSupportedUICultures(I18N).AddSupportedCultures(I18N);
			});

			//添加基础组件
			builder.Services.AddEntityFrameworkNpgsql();
			builder.Services.AddHttpClient();
			builder.Services.AddHttpClient("ignoreCertificate")
				.ConfigurePrimaryHttpMessageHandler(x => new HttpClientHandler() { ServerCertificateCustomValidationCallback = (a, b, c, d) => true });
			//添加后台服务
			builder.Services.AddHostedService<TimedHostedService>();
			//添加仓储
			builder.Services.AddMaidRepository();
			//添加Job
			builder.Services.AddScoped<HttpClientGenerator>();
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
			builder.Services.AddSwaggerGen(options =>
			{
				//支持可空引用类型
				options.SupportNonNullableReferenceTypes();
				//自定义schema名称 RapiDoc不支持使用带+号的schemaName
				options.CustomSchemaIds(type => type.FullName!.Replace("+", "_"));
				//自定义唯一ID
				options.CustomOperationIds(apiDescription =>
				{
					return apiDescription.TryGetMethodInfo(out System.Reflection.MethodInfo methodInfo) ? methodInfo.Name : null;
				});
				//添加XML注释
				foreach (var item in Directory.GetFiles(AppContext.BaseDirectory, "*.xml", SearchOption.TopDirectoryOnly))
					try
					{
						options.IncludeXmlComments(item);
					}
					catch (Exception)
					{
					}
				options.ParameterFilter<SwaggerNullableQueryParameterFilter>();
				options.RequestBodyFilter<SwaggerNullableBodyFilter>();
				options.SchemaFilter<EnumSchemaFilter>();
				options.SchemaFilter<InheritInterfaceXmlCommentSchemaFilter>();
				options.OperationFilter<AddBusinessExceptionResponse>();
				options.OperationFilter<Add204ResponseWhenReturnMaybeNull>();
				//参数采用小驼峰
				options.DescribeAllParametersInCamelCase();
				//添加文档
				options.SwaggerDoc("default", new Microsoft.OpenApi.Models.OpenApiInfo() { Title = "默认文档" });
				options.SwaggerDoc("test", new Microsoft.OpenApi.Models.OpenApiInfo() { Title = "默认文档" });
				//添加DataOnly和TimeOnly的字符串支持
				options.UseDateOnlyTimeOnlyStringConverters();
			});
			//文件提供器
			Directory.CreateDirectory("/files");
			var fileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider("/files");
			builder.Services.AddSingleton(fileProvider);
			//添加消息总线
			builder.Services.AddMassTransit(configurator =>
			{
				//添加所有消费者
				configurator.AddConsumers(System.Reflection.Assembly.GetExecutingAssembly());
				//使用内存队列
				configurator.UsingInMemory((context, cfg) =>
				{
					cfg.ConfigureEndpoints(context);
				});
			});
			//添加OpenTelemetry
			void openTelemetryTracesOptions(OtlpExporterOptions x) { x.Protocol = OtlpExportProtocol.HttpProtobuf; x.Endpoint = new Uri(openTelemetryTracesEndpoint); };
			void openTelemetryMetricsOptions(OtlpExporterOptions x) { x.Protocol = OtlpExportProtocol.HttpProtobuf; x.Endpoint = new Uri(openTelemetryMetricsEndpoint); };
			void resourceBuilder(ResourceBuilder x) => x.AddService(ServiceName, serviceInstanceId: Environment.MachineName);
			builder.Services.AddOpenTelemetry()
				.ConfigureResource(resourceBuilder)
				.WithTracing(config =>
				{
					//记录对外HttpClient请求
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
						options.Filter = httpContent => !httpContent.Request.Path.StartsWithSegments("/swagger");
						options.RecordException = true;
						options.EnrichWithException = (activity, exception) =>
						{
							activity.SetTag("stackTrace", exception.StackTrace);
							activity.SetTag("message", exception.Message);
						};
					});
					config.AddSource(MassTransit.Logging.DiagnosticHeaders.DefaultListenerName);
					config.AddEntityFrameworkCoreInstrumentation();
					config.AddOtlpExporter(openTelemetryTracesOptions);
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
					config.AddOtlpExporter(openTelemetryMetricsOptions);
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
				app.UseSwaggerUI(x =>
				{
					foreach (var doc in app.Services.GetRequiredService<SwaggerGeneratorOptions>().SwaggerDocs)
						x.SwaggerEndpoint($"/swagger/{doc.Key}/swagger.json", doc.Key);
				});
			}
			//请求日志记录
			app.Use(async (context, next) =>
			{
				int maxLength = 20 * 1024;
				long contentLength = context.Request.ContentLength ?? 0;
				string? contentType = context.Request.ContentType ?? "";
				if (contentLength > 0 && contentLength < maxLength && contentType.Contains("json"))
				{
					var logger = context.RequestServices.GetRequiredService<Microsoft.Extensions.Logging.ILogger<HttpRequest>>();
					context.Request.EnableBuffering();
					var pool = System.Buffers.ArrayPool<byte>.Shared;
					var buffer = pool.Rent((int)contentLength);
					await context.Request.Body.ReadAsync(buffer);
					context.Request.Body.Seek(0, SeekOrigin.Begin);
					logger.LogInformation("requestBody:{requestBody}", System.Text.Encoding.UTF8.GetString(buffer, 0, (int)contentLength));
					pool.Return(buffer);
				}
				await next();
			});
			//HTTPS重定向
			//app.UseHttpsRedirection();
			//添加静态文件支持
			app.UseDefaultFiles();
			app.UseStaticFiles(new StaticFileOptions()
			{
				OnPrepareResponse = context =>
				{
					//处理点击劫持漏洞
					context.Context.Response.Headers.XFrameOptions = "SAMEORIGIN";
					//CSP防止XSS
					context.Context.Response.Headers.ContentSecurityPolicy = "script-src 'self'";
				}
			});
			//添加路由
			app.UseRouting();
			//i18n
			app.UseRequestLocalization();
			//跨域配置
			app.UseCors(x => x
				.AllowAnyOrigin()
				.AllowAnyMethod()
				.WithExposedHeaders("Content-Disposition")
				.SetPreflightMaxAge(TimeSpan.FromDays(1)));
			//身份认证
			app.UseAuthentication();
			app.UseAuthorization();
			//响应压缩
			app.UseResponseCompression();
			//响应缓存
			//app.UseResponseCaching();
			//使用控制器
			app.MapControllers();
			//版本信息
			app.MapGet("/version", () => $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Version}");
			//数据库迁移
			var scope = app.Services.CreateScope();
			var context = scope.ServiceProvider.GetRequiredService<MaidContext>();
			context.Database.Migrate();
			//初始化服务
			await Task.Run(async () =>
				{
					await InitServices.Init(app.Services);
				});
			//await Task.Run(async () =>
			//	{
			//		var scope = app.Services.CreateScope();
			//		var context = scope.ServiceProvider.GetRequiredService<MaidContext>();
			//		var data = context.Maids.Where(x => x.MaidWork == Models.CodeMaid.MaidWork.HttpClientSync)
			//		//.Where(x => x.DestinationPath != "")
			//		.ToList();
			//		foreach (var item in data)
			//		{
			//			var setting = item.Setting.Deserialize<HttpClientSyncSetting>();
			//			//setting.ClientPath = item.DestinationPath;
			//			//item.DestinationPath = "";
			//			item.Setting = JsonSerializer.SerializeToDocument(setting);
			//		}
			//		context.SaveChanges();
			//	});
#if DEBUG
			Task.Run(async () =>
					{
						try
						{
							await Task.Delay(1000);
							var scope = app.Services.CreateScope();
							var context = scope.ServiceProvider.GetRequiredService<MaidContext>();
							var publish = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();
							var project = context.Projects.ToList();
							await publish.Publish<ProjectUpdateEvent>(new ProjectUpdateEvent() { ProjectId = 4 });



							context.SaveChanges();

						}
						catch (Exception)
						{
						}
					});
#endif

			//开始运行
			app.Run();
			//程序结束的时候刷新日志
			Log.CloseAndFlush();
		}
	}
}