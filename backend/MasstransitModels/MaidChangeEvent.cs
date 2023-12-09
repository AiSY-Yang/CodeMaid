using Models.CodeMaid;

namespace MasstransitModels
{
	public record MaidChangeEvent
	{
		public required long MaidId { get; set; }
	}
}