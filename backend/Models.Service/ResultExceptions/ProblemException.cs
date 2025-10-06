using System.Net;

using ServicesModels.Results;

namespace ServicesModels.ResultExceptions
{
	/// <summary>
	/// 业务异常
	/// </summary>
	public class ProblemException : CustomException<Microsoft.AspNetCore.Mvc.ProblemDetails>
	{
		/// <inheritdoc/>
		public ProblemException(string? message) : base(message)
		{
			Data = new Microsoft.AspNetCore.Mvc.ProblemDetails();
			Data.Title = message;
		}
		/// <inheritdoc/>
		public ProblemException(string? message, string detail) : this(message)
		{
			Data.Detail = detail;
		}
	}
}
