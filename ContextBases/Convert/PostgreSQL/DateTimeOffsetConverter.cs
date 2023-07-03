using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ContextBases.Convert.MySQL
{
	/// <summary>
	/// For PostgresSql Convert DateTimeOffset to UTC
	/// </summary>
	public sealed class DateTimeOffsetConverter : Microsoft.EntityFrameworkCore.Storage.ValueConversion.ValueConverter<DateTimeOffset, DateTimeOffset>
	{
		public DateTimeOffsetConverter() : base(v => v, v => v.ToLocalTime())
		{
		}
	}
}
