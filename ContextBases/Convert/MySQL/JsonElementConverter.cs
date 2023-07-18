using System.Text.Json;

namespace Microsoft.EntityFrameworkCore.Storage.ValueConversion
{
	/// <summary>
	/// For Mysql Add JsonElement Support
	/// </summary>
	public sealed class JsonElementConverter : ValueConverter<JsonElement, string>
	{
		public JsonElementConverter() : base(v => v.ToString(), v => JsonSerializer.Deserialize<JsonElement>(v, new JsonSerializerOptions()))
		{
		}
	}
}
