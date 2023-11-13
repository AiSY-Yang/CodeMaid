using System.ComponentModel;

using Models.CodeMaid;

namespace Models.CodeMaid
{
	/// <summary>
	/// 功能
	/// </summary>
	/// <remarks>0-配置同步功能,1-DTO同步,2-HTTP客户端生成,3-controller同步,4-生成 Masstransit Consumer</remarks>
	public enum MaidWork
	{
		/// <summary>
		/// 配置同步功能
		/// </summary>
		ConfigurationSync,
		/// <summary>
		/// DTO同步
		/// </summary>      
		DtoSync,
		/// <summary>
		/// HTTP客户端生成
		/// </summary>
		HttpClientSync,
		/// <summary>
		/// controller同步
		/// </summary>
		ControllerSync,
		/// <summary>
		/// 生成 Masstransit Consumer
		/// </summary>
		MasstransitConsumerSync,
	}
}