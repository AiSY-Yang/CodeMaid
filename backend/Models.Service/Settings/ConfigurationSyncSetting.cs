using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesModels.Settings
{
    /// <summary>
    /// 配置同步的设置
    /// </summary>
    public class ConfigurationSyncSetting
    {
        /// <summary>
        /// 源目录
        /// </summary>
        public required string SourceDirectory { get; set; }
        /// <summary>
        /// 目标目录
        /// </summary>
        public required string TargetDirectory { get; set; }
        /// <summary>
        /// DbContext文件的路径
        /// </summary>
        public string? ContextPath { get; set; }
    }
}
