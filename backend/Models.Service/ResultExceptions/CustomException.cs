using System.Runtime.Serialization;

using Microsoft.AspNetCore.Mvc;

namespace ServicesModels.Results
{
	/// <inheritdoc/>
	public abstract class CustomException : Exception
	{
		/// <inheritdoc/>
		public CustomException() : base()
		{
		}

		/// <inheritdoc/>
		public CustomException(string? message) : base(message)
		{
		}
		/// <inheritdoc/>
		abstract public Microsoft.AspNetCore.Mvc.ObjectResult GetObjectResult();
	}
	/// <inheritdoc/>
	public class CustomException<T> : CustomException
	{
		/// <inheritdoc/>
		public CustomException(string? message) : base(message)
		{
			Data = default!;
		}

		/// <inheritdoc/>
		public new T Data { get; set; }
		/// <inheritdoc/>
		public override ObjectResult GetObjectResult()
		{
			return new Microsoft.AspNetCore.Mvc.ObjectResult(Data)
			{
				StatusCode = (int)System.Net.HttpStatusCode.InternalServerError,
			};
		}
	}
}