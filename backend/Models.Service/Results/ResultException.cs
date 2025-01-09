using System.Net;
using System.Text.Json.Serialization;

namespace ServicesModels.Results
{
	public class ApiException<T> : Exception
	{
		public required T Model { get; set; }
	}
	public class RFC9457Exception : ApiException<RFC9457>
	{
	}

	/// <summary>
	/// A business exception with message
	/// </summary>
	public abstract class ResultException : Exception
	{
		/// <summary>
		/// Business exception with code
		/// </summary>
		/// <param name="code"></param>
		public ResultException(string code)
		{
			Code = code;
		}
		/// <summary>
		/// Business exception with code and message
		/// </summary>
		/// <param name="code"></param>
		/// <param name="msg"></param>
		public ResultException(string code, string? msg)
		{
			Code = code;
			Msg = msg;
		}
		/// <summary>
		/// Business exception with code and message,will be returned with the specified HTTP status code
		/// </summary>
		/// <param name="code"></param>
		/// <param name="msg"></param>
		/// <param name="httpStatusCode"></param>
		public ResultException(string code, string? msg, HttpStatusCode httpStatusCode)
		{
			Code = code;
			Msg = msg;
			HttpStatusCode = httpStatusCode;
		}
		/// <summary>
		/// HTTP status code
		/// </summary>
		[JsonIgnore]
		public virtual HttpStatusCode HttpStatusCode { get; set; } = HttpStatusCode.BadRequest;
		/// <inheritdoc/>
		public string Code { get; set; } = "Error";

		/// <inheritdoc/>
		public virtual string? Msg { get; set; }
	}
	/// <summary>
	/// a business exception
	/// </summary>
	public class I18nResultException : Exception
	{
		/// <summary>
		/// business exception with no parameter i18n message
		/// </summary>
		/// <param name="code"></param>
		public I18nResultException(string code)
		{
			Code = code;
		}
		/// <inheritdoc/>
		public I18nResultException(string code, HttpStatusCode httpStatusCode)
		{
			Code = code;
			HttpStatusCode = httpStatusCode;
		}
		/// <summary>
		/// business exception with parameter i18n message
		/// </summary>
		/// <param name="code"></param>
		/// <param name="args"></param>
		public I18nResultException(string code, object[]? args)
		{
			Code = code;
			Args = args;
		}
		/// <inheritdoc/>
		public I18nResultException(string code, object[]? args, HttpStatusCode httpStatusCode)
		{
			Code = code;
			Args = args;
			HttpStatusCode = httpStatusCode;
		}
		/// <summary>
		/// HTTP status code
		/// </summary>
		[JsonIgnore]
		public virtual HttpStatusCode HttpStatusCode { get; set; } = HttpStatusCode.BadRequest;

		/// <inheritdoc/>
		public virtual string Code { get; set; } = "Error";
		/// <summary>
		/// Arguments provided to the <see cref="string.Format(string, object?[])"/> method
		/// </summary>
		public virtual object[]? Args { get; set; }
	}
}