using System.Net;
using System.Text.Json.Serialization;

namespace ServicesModels.Results
{

	/// <summary>
	/// A business exception with message and code
	/// </summary>
	public abstract class ErrorException : CustomException<ErrorException.Error>
	{
		/// <summary>
		/// Error model
		/// </summary>
		public class Error
		{
			/// <summary>
			/// Error code
			/// </summary>
			public int Code { get; set; }
			/// <summary>
			/// Error message
			/// </summary>
			public required string Message { get; set; }
		}
		/// <summary>
		/// Business exception with code
		/// </summary>
		/// <param name="message"></param>
		public ErrorException(string message) : base(message)
		{
			Data = new Error()
			{
				Code = -1,
				Message = message
			};
		}
		/// <summary>
		/// Business exception with code and message
		/// </summary>
		/// <param name="code"></param>
		/// <param name="message"></param>
		public ErrorException(int code, string message) : base(message)
		{
			Data = new Error()
			{
				Code = code,
				Message = message
			};
		}
	}
}