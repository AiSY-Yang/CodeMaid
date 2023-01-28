using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ServicesModels.Exceptions
{
	/// <summary>
	/// 业务异常
	/// </summary>
	public class BusinessException : ResultException
	{

		/// <inheritdoc/>
		public BusinessException(string msg) : base(msg)
		{
		}

		/// <inheritdoc/>
		public BusinessException(int code, string msg) : base(code, msg)
		{
		}

		/// <inheritdoc/>
		public BusinessException(int code, string msg, HttpStatusCode httpStatusCode) : base(code, msg, httpStatusCode)
		{
		}

		/// <inheritdoc/>
		public override string Msg { get; set; } = "业务异常";
	}
}
