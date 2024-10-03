using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ServicesModels.Settings
{
	/// <summary>
	/// Http客户端生成的setting
	/// </summary>
	public class HttpClientSyncSetting
	{
		/// <summary>
		/// Url
		/// </summary>
		public string? SourceUrl { get; set; }
		/// <summary>
		/// swagger文档
		/// </summary>
		public string? SwaggerDocument { get; set; }
		/// <summary>
		/// 是否是手动执行
		/// </summary>
		public bool IsManual { get; set; }
		/// <summary>
		/// 客户端路径
		/// </summary>
		public required string ClientPath { get; set; }
		/// <summary>
		/// 模型文件路径
		/// </summary>
		public string? ModelPath { get; set; }
		/// <summary>
		/// 选项的路径
		/// </summary>
		public string? OptionsPath { get; set; }
		/// <summary>
		/// 重命名客户端
		/// </summary>
		public string? RenameClient { get; set; }
		/// <summary>
		/// 生成类型
		/// </summary>
		public HttpClientType HttpClientType { get; set; }

	}
	/// <summary>
	/// 客户端类型
	/// </summary>
	public enum HttpClientType
	{
		/// <summary>
		/// C#
		/// </summary>
		CSharp,
		/// <summary>
		/// TypeScript
		/// </summary>
		TypeScript,
	}
	class HttpClientTypeScriptGenerateSetting
	{

	}
}
