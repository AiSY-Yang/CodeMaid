using System.Reflection;
using System.Text.Json.Serialization;

using Api.Controllers;
using Api.MasstransitConsumer;
using Api.Middleware;

using MaidContexts;

using MassTransit;

using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

using Serilog;

using Swashbuckle.AspNetCore.SwaggerGen;

namespace Api
{
	/// <summary>
	/// 非顶级语句可以方便查看git记录 同时顶级语句会出现诡异的intelliCode错误
	/// </summary>
	public class Program
	{
		public static IServiceProvider Services = null!;
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);
			//配置使用serilog记录日志
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
			});
			//添加数据库
			string conn = builder.Configuration.GetConnectionString("MaidContext");
			builder.Services.AddDbContext<MaidContext>(x => x.UseMySql(conn, ServerVersion.AutoDetect(conn),
				x => x.EnableRetryOnFailure(3).EnableIndexOptimizedBooleanColumns()).EnableSensitiveDataLogging());
			//添加仓储
			builder.Services.AddMaidReponsitory();

			//添加控制器
			builder.Services.AddControllers(x =>
			{
				//将业务异常转化为自定义信息
				x.Filters.Add<HttpResponseExceptionFilter>();
				//支持从body直接接收string参数
				x.InputFormatters.Add(new TextFormatter());
				//添加DataOnly和TimeOnly的字符串支持
				x.UseDateOnlyTimeOnlyStringConverters();
			})
				//配置json解析
				.AddJsonOptions(options =>
				{
					options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
					options.JsonSerializerOptions.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
					//options.JsonSerializerOptions.Converters.Add(new Core.Converter.NullableDateOnlyJsonConverter());
					options.JsonSerializerOptions.ReadCommentHandling = System.Text.Json.JsonCommentHandling.Skip;
					options.JsonSerializerOptions.AllowTrailingCommas = true;
					//添加DataOnly和TimeOnly的字符串支持
					options.UseDateOnlyTimeOnlyStringConverters();
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
				//自定义schema名称 rapiDoc不支持使用带+号的schemaName
				x.CustomSchemaIds((x) => x.FullName!.Replace("+", "_"));
				//自定义唯一ID
				x.CustomOperationIds(apiDesc =>
				{
					return apiDesc.TryGetMethodInfo(out System.Reflection.MethodInfo methodInfo) ? methodInfo.Name : null;
				});
				//添加XML注释
				foreach (var item in Directory.GetFiles(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) ?? "./", "*.xml", SearchOption.TopDirectoryOnly))
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
			//添加mapster配置
			builder.Services.AddMapster();
			//添加消息总线
			builder.Services.AddMassTransitHostedService();
			builder.Services.AddMassTransit(x =>
			{
				x.AddConsumer<FileChangeEventConsumer, OrderEtoConsumerDefinition>();
				x.UsingInMemory((context, cfg) =>
				{

					cfg.ConfigureEndpoints(context);
				});
			});

			var app = builder.Build();
			
			//保存容器为全局变量 以便手动创建DI对象
			Services = app.Services;
			//开发环境显示swagger文档
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}
			//处理点击劫持漏洞
			app.Use(async (context, next) =>
			{
				context.Response.Headers.Add("X-Frame-Options", "SAMEORIGIN");
				await next();
			});
			//跨域配置
			app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod());
			//HTTPS重定向
			//app.UseHttpsRedirection();
			//swagger配置
			app.UseSwagger();
			app.UseRouting();
			app.UseAuthorization();
			//使用控制器
			app.MapControllers();
			//版本信息
			app.UseEndpoints(x =>
			{
				//x.MapGet("/version", () => $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Version}");
			});
			//添加静态文件支持
			app.UseDefaultFiles();
			app.UseStaticFiles();
			Task.Run(() =>
			{
				var scope = app.Services.CreateScope();
				var context = scope.ServiceProvider.GetRequiredService<MaidContext>();
				Maids.Init(app.Services);
				new TestClass(scope.ServiceProvider).Task();
			});
			//开始运行
			app.Run();
			//程序结束的时候刷新日志
			Log.CloseAndFlush();
		}
	}
}