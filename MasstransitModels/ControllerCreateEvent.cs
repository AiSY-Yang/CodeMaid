using Models.CodeMaid;

namespace MasstransitModels
{
	/// <summary>
	/// 创建控制器
	/// </summary>
	public record ControllerCreateEvent
	{
		/// <summary>
		/// 1111111
		/// </summary>
		public required long MaidId { get; set; }
		public required string EntityName { get; set; }
		public required string EntityPath { get; set; }
		public required string ServicePath { get; set; }
		public required string ControllerPath { get; set; }
	}
}