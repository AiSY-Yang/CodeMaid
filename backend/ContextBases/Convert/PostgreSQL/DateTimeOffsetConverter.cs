using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Microsoft.EntityFrameworkCore.Storage.ValueConversion
{
	/// <summary>
	/// For PostgresSql Convert DateTimeOffset to UTC
	/// </summary>
	public sealed class DateTimeOffsetConverter : ValueConverter<DateTimeOffset, DateTimeOffset>
	{
		public DateTimeOffsetConverter() : base(v => v.ToUniversalTime(), v => v.ToLocalTime())
		{
		}
	}
}
