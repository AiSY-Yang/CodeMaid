namespace Models.CodeMaid
{
	/// <summary>
	/// 项目结构
	/// </summary>
	public class ProjectStructure : DatabaseEntity
	{
		/// <summary>
		/// 文件
		/// </summary>
		public required ProjectDirectoryFile ProjectDirectoryFile { get; set; }
		/// <summary>
		/// 类
		/// </summary>
		public required ClassDefinition ClassDefinition { get; set; }
		/// <summary>
		/// 属性
		/// </summary>
		public required List<PropertyDefinition> PropertyDefinitions { get; set; }
	}
}