using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesModels.Settings
{
	/// <summary>
	/// Dto同步的设置
	/// </summary>
	public class DtoSyncSetting
	{
		/// <summary>
		/// 是否创建目录 如果为false则生成单个文件 里面有多个类 如果为true则生成文件夹 每个类生成和类同名的文件
		/// </summary>
		public bool CreateDirectory { get; set; }
		/// <summary>
		/// 文件或者目录的后缀
		/// </summary>
		public string? Suffix { get; set; }
		/// <summary>
		/// Dto同步的设置
		/// </summary>
		public List<DtoSyncSettingItem> DtoSyncSettings { get; set; } = null!;
		/// <summary>
		/// Dto同步的设置
		/// </summary>
		public class DtoSyncSettingItem
		{
			/// <summary>
			/// 类后缀
			/// </summary>
			public string Suffix { get; set; } = null!;
			/// <summary>
			/// 排除列表属性
			/// </summary>
			public bool ExcludeList { get; set; }
			/// <summary>
			/// 排除复杂属性
			/// </summary>
			public bool ExcludeComplexTypes { get; set; }
			/// <summary>
			/// 转换为可为空属性 一般用于查询条件
			/// </summary>
			public bool ConvertToNullable { get; set; }
			/// <summary>
			/// 排除属性
			/// </summary>
			public List<string> ExcludeProperties { get; set; } = new List<string>();
			/// <summary>
			/// 仅包含属性
			/// </summary>
			public List<string> JustInclude { get; set; } = new List<string>();
			/// <summary>
			/// 需要扁平化映射的属性
			/// </summary>
			public List<string> FlatteningProperties { get; set; } = new List<string>();
		}
	}
}
