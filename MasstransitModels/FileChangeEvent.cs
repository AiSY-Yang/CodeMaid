﻿using Models.CodeMaid;

namespace MasstransitModels
{
	public record FileChangeEvent
	{
		public string FilePath { get; set; }
		public long MaidId { get; set; }
	}
}