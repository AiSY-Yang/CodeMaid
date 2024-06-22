using Models.CodeMaid;

namespace MasstransitModels
{
	/// <summary>
	/// enum update
	/// </summary>
	public record EnumUpdateEvent
	{
		/// <summary>
		/// 枚举Id
		/// </summary>
		public required long EnumId { get; set; }
		public required long ProjectId { get; set; }
	}
}