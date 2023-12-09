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
		/// 是否创建模型
		/// </summary>
		public bool CreateModel { get; set; }
		/// <summary>
		/// 模型文件路径
		/// </summary>
		public string? ModelPath { get; set; }
	}
}
