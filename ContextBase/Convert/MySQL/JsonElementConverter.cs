using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ContextBases.Convert.MySQL
{
	/// <summary>
	/// For Mysql Add JsonElement Support
	/// </summary>
	public sealed class JsonElementConverter : Microsoft.EntityFrameworkCore.Storage.ValueConversion.ValueConverter<JsonElement, string>
	{
		public JsonElementConverter() : base(v => v.ToString(), v => JsonSerializer.Deserialize<JsonElement>(v, new JsonSerializerOptions()))
		{
		}
	}
}
