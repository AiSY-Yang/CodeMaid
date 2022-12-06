namespace Models.Service
{
	/// <summary>
	/// 业务异常
	/// </summary>
	public class BusinessException : Exception
	{
		/// <summary>
		/// 业务异常结果
		/// </summary>
		public BusinessExceptionResult Result = new();
		/// <summary>
		/// 业务异常返回的结果模型
		/// </summary>
		public class BusinessExceptionResult
		{
			/// <summary>
			/// HTTP错误码
			/// </summary>
			public int Code { get; set; }
			/// <summary>
			/// 异常消息
			/// </summary>
			public string Msg { get; set; } = null!;

		}
		/// <summary>
		/// 按照400返回错误信息
		/// </summary>
		/// <param name="s"></param>
		public BusinessException(string s)
		{
			Result.Code = 400;
			Result.Msg = s;
		}
		/// <summary>
		/// 按照指定的错误码返回错误信息
		/// </summary>
		/// <param name="code"></param>
		/// <param name="s"></param>
		public BusinessException(int code, string s)
		{
			Result.Code = code;
			Result.Msg = s;
		}
	}
}