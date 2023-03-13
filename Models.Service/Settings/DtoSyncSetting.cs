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
        /// 目录后缀
        /// </summary>
        public string DirectorySuffix { get; set; } = null!;
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
            /// 后缀
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
