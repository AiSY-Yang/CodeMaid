using System.ComponentModel;

using Models.CodeMaid;

namespace Models.CodeMaid
{
	/// <summary>
	/// 功能
	/// </summary>
	/// <remarks>0-配置同步功能,1-DTO同步</remarks>
	public enum MaidWork
	{
		/// <summary>
		/// 配置同步功能
		/// </summary>
		[Description]
		ConfigurationSync,
		/// <summary>
		/// DTO同步
		/// </summary>      
		DtoSync,
		/// <summary>
		/// DTO同步
		/// </summary>      
		HttpClientSync,
	}
}