using Models.CodeMaid;

namespace MasstransitModels
{
	public class FileChangeEvent
	{
		public string FilePath { get; set; }
		public long MaidId { get; set; }
	}
}