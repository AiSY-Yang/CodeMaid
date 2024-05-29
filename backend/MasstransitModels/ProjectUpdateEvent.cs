using Models.CodeMaid;

namespace MasstransitModels
{
	public record ProjectUpdateEvent
	{
		public required long ProjectId { get; set; }
	}
}