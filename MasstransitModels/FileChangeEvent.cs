﻿using Models.CodeMaid;

namespace MasstransitModels
{
	public record FileChangeEvent
	{
		public required string FilePath { get; set; }
		public required long MaidId { get; set; }
		public required bool IsDelete { get; set; }
	}
}