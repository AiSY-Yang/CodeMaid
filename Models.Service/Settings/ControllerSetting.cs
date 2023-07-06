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
	public class ControllerSetting
	{
		/// <summary>
		/// 默认值
		/// </summary>
		public static readonly ControllerSetting Default = new()
		{
			ServicesDirectory = "Api/Services"
		};
		public required string ServicesDirectory { get; set; }
	}
}
