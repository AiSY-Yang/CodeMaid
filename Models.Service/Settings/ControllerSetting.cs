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
			Using = new List<string>(),
			MethodInterfaces = new List<MethodInterface>() {
				new() { MethodType = MethodType.Create, InterfaceName = "IAdd", MethodName = "Add", HttpMethod = "Get", Route = "", Parameters = "" },
				new() { MethodType = MethodType.Delete, InterfaceName = "IDelete", MethodName = "Delete", HttpMethod = "Delete", Route = "", Parameters = "" },
				new() { MethodType = MethodType.Delete, InterfaceName = "I"+nameof(MethodType.Put), MethodName = nameof(MethodType.Put), HttpMethod = "Delete", Route = "", Parameters = "" },
				new() { MethodType = MethodType.Delete, InterfaceName = "I"+nameof(MethodType.Patch), MethodName = nameof(MethodType.Patch), HttpMethod = "Delete", Route = "", Parameters = "" },
			},
		};
		public required List<string> Using { get; set; }
		public required List<MethodInterface> MethodInterfaces { get; set; }
		public class MethodInterface
		{
			/// <summary>
			/// 方法类型 决定了生成方式
			/// </summary>
			public required MethodType MethodType { get; set; }
			public string InterfaceName { get; set; }
			public string MethodName { get; set; }
			public string HttpMethod { get; set; }
			public string Route { get; set; }
			public string Parameters { get; set; }
		}
	}
	/// <summary>
	/// 方法类型
	/// </summary>
	public enum MethodType
	{
		/// <summary>
		/// 新增
		/// </summary>
		Create,
		/// <summary>
		/// 删除
		/// </summary>
		Delete,
		/// <summary>
		/// 修改
		/// </summary>
		Put,
		/// <summary>
		/// 部分修改
		/// </summary>
		Patch,
		/// <summary>
		/// 获取列表
		/// </summary>
		GetList,
		/// <summary>
		/// 获取明细
		/// </summary>
		GetPageList,
		/// <summary>
		/// 获取字典
		/// </summary>
		GetDictionary,
	}
	/// <summary>
	/// 文件类型
	/// </summary>
	public enum FileType
	{
		/// <summary>
		/// 控制器
		/// </summary>
		Controller,
		/// <summary>
		/// 服务层
		/// </summary>
		Service,
	}
}
