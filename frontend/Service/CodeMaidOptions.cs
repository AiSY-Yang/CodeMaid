namespace RestfulClient;

public class CodeMaidOptions
{
	/// <summary>
	/// 服务地址
	/// </summary>
	public required string Url { get; set; }
	/// <summary>
	/// 超时时间
	/// </summary>
	public double TimeoutSecond { get; set; } = 30;
}