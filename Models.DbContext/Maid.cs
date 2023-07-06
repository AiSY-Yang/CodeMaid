using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Nodes;

using Mapster;

namespace Models.CodeMaid
{
	/// <summary>
	/// 功能
	/// </summary>
	public class Maid : DatabaseEntity
	{
		/// <summary>
		/// 名称
		/// </summary>
		public string Name { get; set; } = null!;
		/// <summary>
		/// 所属项目
		/// </summary>
		public required Project Project { get; set; }
		/// <summary>
		/// 功能
		/// </summary>
		/// <remarks>0-配置同步功能,1-DTO同步,2-枚举remarks标签同步</remarks>
		public MaidWork MaidWork { get; set; }
		/// <summary>
		/// 原路径
		/// </summary>
		public string SourcePath { get; set; } = null!;
		/// <summary>
		/// 目标路径
		/// </summary>
		public string DestinationPath { get; set; } = null!;
		/// <summary>
		/// 是否自动修复
		/// </summary>
		public bool Autonomous { get; set; }
		/// <summary>
		/// 设置
		/// </summary>
		public JsonElement Setting { get; set; }
		/// <summary>
		/// 包含的类
		/// </summary>
		[AdaptIgnore]
		public List<ClassDefinition> Classes { get; set; } = null!;
		/// <summary>
		/// 包含的枚举
		/// </summary>
		[AdaptIgnore]
		public List<EnumDefinition> Enums { get; set; } = null!;
	}
}