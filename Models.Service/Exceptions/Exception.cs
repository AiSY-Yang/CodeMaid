using System.Net;
using System.Text.Json.Serialization;

namespace ServicesModels.Exceptions
{
	/// <summary>
	/// 业务异常返回的结果模型
	/// </summary>
	public interface IExceptionResult
	{
		/// <summary>
		/// 错误码
		/// </summary>
		public int Code { get; set; }

		/// <summary>
		/// 异常消息
		/// </summary>
		public string Msg { get; set; }
	}

	/// <inheritdoc/>
	public abstract class ResultException : Exception, IExceptionResult
	{
		/// <inheritdoc/>
		protected ResultException(string msg)
		{
			Msg = msg;
		}
		/// <inheritdoc/>
		protected ResultException(int code, string msg)
		{
			Code = code;
			Msg = msg;
		}
		/// <inheritdoc/>
		protected ResultException(int code, string msg, HttpStatusCode httpStatusCode)
		{
			Code = code;
			Msg = msg;
			HttpStatusCode = httpStatusCode;
		}
		/// <summary>
		/// HTTP状态码
		/// </summary>
		[JsonIgnore]
		public virtual HttpStatusCode HttpStatusCode { get; set; } = HttpStatusCode.OK;

		/// <inheritdoc/>
		public virtual int Code { get; set; } = 1;

		/// <inheritdoc/>
		public abstract string Msg { get; set; }
	}

}