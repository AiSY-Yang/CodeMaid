using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ContextBases.Convert.MySQL
{
	/// <summary>
	/// For PostgresSql Convert DateTime to UTC
	/// </summary>
	public sealed class DateTimeConverter : Microsoft.EntityFrameworkCore.Storage.ValueConversion.ValueConverter<DateTime, DateTime>
	{
		public DateTimeConverter() : base(v => v.ToUniversalTime(), v => v.ToLocalTime())
		{
		}
	}
}
