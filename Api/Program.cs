using System.Text.Json.Serialization;

using Api.Middleware;
using Api.Worker;

using MaidContexts;

using MassTransit;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

using Serilog;

using ServicesModels.Exceptions;

using Swashbuckle.AspNetCore.SwaggerGen;

namespace Api
{
	/// <summary>
	/// 非顶级语句可以方便查看git记录 同时顶级语句会出现诡异的intelliCode错误
	/// </summary>
	public class Program
	{
		/// <summary>
		/// root ServiceProvider
		/// </summary>
		public static IServiceProvider Services { get; private set; } = null!;

		/// <summary>
		/// main函数
		/// </summary>
		/// <param name="args"></param>
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);
			//配置使用Serilog记录日志
			builder.Host.UseSerilog((context, services, config) =>
			{
				//从配置文件读取日志规则
				config.ReadFrom.Configuration(context.Configuration)
				;
			});
			//配置服务器
			builder.WebHost.UseKestrel((c, x) =>
			{
				x.ListenAnyIP(5241);
				x.AddServerHeader = false;
			});
			//添加数据库
			string? connectionString = builder.Configuration.GetConnectionString("MaidContext");
			builder.Services.AddEntityFrameworkMySql();
			builder.Services.AddDbContextPool<MaidContext>((serviceProvider, x) =>
			x.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString), x => x.EnableRetryOnFailure(3).EnableIndexOptimizedBooleanColumns())
#if DEBUG
			.EnableSensitiveDataLogging()
#endif
			.UseInternalServiceProvider(serviceProvider)
			);
			//添加仓储
			builder.Services.AddMaidReponsitory();

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
						var error = new ExceptionResult(400, string.Join(';', x.ModelState.Values.Select(x => x.Errors.FirstOrDefault()?.ErrorMessage).ToList()));
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
				x.OperationFilter<BusinessExceptionFilter>();
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
			builder.Services.AddMassTransitHostedService();
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
			Task.Run(() =>
			{
				var scope = app.Services.CreateScope();
				var context = scope.ServiceProvider.GetRequiredService<MaidContext>();
				InitServices.Init(app.Services);
			});
			//开始运行
			app.Run();
			//程序结束的时候刷新日志
			Log.CloseAndFlush();
		}
	}
}