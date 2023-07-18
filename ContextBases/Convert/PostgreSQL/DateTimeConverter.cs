using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Microsoft.EntityFrameworkCore.Storage.ValueConversion
{
	/// <summary>
	/// For PostgresSql Convert DateTime to UTC
	/// </summary>
	public sealed class DateTimeConverter : ValueConverter<DateTime, DateTime>
	{
		public DateTimeConverter() : base(v => v.ToUniversalTime(), v => v.ToLocalTime())
		{
		}
	}
}
