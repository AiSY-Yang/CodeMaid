using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesModels
{
	/// <summary>
	/// 配置同步的设置
	/// </summary>
	public class ConfigurationSyncSetting
	{
		/// <summary>
		/// DbContext文件的路径
		/// </summary>
		public string? ContextPath { get; set; }
	}
}
