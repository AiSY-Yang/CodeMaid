using System.Net;
using System.Text.Json.Serialization;

namespace ServicesModels.Results
{
	/// <summary>
	/// 业务异常返回的结果模型
	/// </summary>
	public interface IExceptionResult
	{
		/// <summary>
		/// 错误码
		/// </summary>
		public string Code { get; set; }

		/// <summary>
		/// 异常消息
		/// </summary>
		public string Msg { get; set; }
	}

	/// <summary>
	/// 业务异常返回的结果模型
	/// </summary>
	public class ExceptionResult : IExceptionResult
	{
		/// <inheritdoc/>
		public ExceptionResult(string code, string msg)
		{
			Code = code;
			Msg = msg;
		}

		/// <inheritdoc/>
		public ExceptionResult(ResultException resultException)
		{
			Code = resultException.Code;
			Msg = resultException.Msg;
		}

		/// <inheritdoc/>
		public string Code { get; set; }
		/// <inheritdoc/>
		public string Msg { get; set; }
	}

	/// <inheritdoc/>
	public class ResultException : Exception, IExceptionResult
	{
		/// <inheritdoc/>
		public ResultException(string msg)
		{
			Msg = msg;
		}
		/// <inheritdoc/>
		public ResultException(string code, string msg)
		{
			Code = code;
			Msg = msg;
		}
		/// <inheritdoc/>
		public ResultException(string code, string msg, HttpStatusCode httpStatusCode)
		{
			Code = code;
			Msg = msg;
			HttpStatusCode = httpStatusCode;
		}
		/// <summary>
		/// HTTP状态码
		/// </summary>
		[JsonIgnore]
		public virtual HttpStatusCode HttpStatusCode { get; set; } = HttpStatusCode.BadRequest;

		/// <inheritdoc/>
		public virtual string Code { get; set; } = "Error";

		/// <inheritdoc/>
		public virtual string Msg { get; set; }
	}

}