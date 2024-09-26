using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ServicesModels.Results.Exceptions
{
	/// <summary>
	/// 业务异常
	/// </summary>
	public class BusinessException : ResultException
	{
		/// <inheritdoc/>
		public BusinessException(string code) : base(code)
		{
		}

		/// <inheritdoc/>
		public BusinessException(string code, string? msg) : base(code, msg)
		{
		}

		/// <inheritdoc/>
		public BusinessException(string code, string? msg, HttpStatusCode httpStatusCode) : base(code, msg, httpStatusCode)
		{
		}
		/// <inheritdoc/>
		public override HttpStatusCode HttpStatusCode => HttpStatusCode.InternalServerError;

		/// <inheritdoc/>
		public override string Msg { get; set; } = "业务异常";
	}
}
