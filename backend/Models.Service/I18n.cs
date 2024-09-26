
using System.Collections.Frozen;
using System.Reflection.Metadata.Ecma335;

namespace ServicesModels
{
	/// <summary>
	/// i18n data
	/// </summary>
	public static class I18n
	{
		/// <summary>
		/// default language list
		/// </summary>
		public static string[] DefaultLanguage { get; set; } = ["en-US"];
		/// <summary>
		/// language
		/// </summary>
		public static AsyncLocal<IEnumerable<string?>> Language { get; set; } = new AsyncLocal<IEnumerable<string?>>();
		/// <summary>
		/// error messages dictionary for i18n, key is language code, value is dictionary key is error code, value is error message
		/// </summary>
		public static FrozenDictionary<string, FrozenDictionary<string, string>> I18nDictionary { get; set; } = null!;
		/// <summary>
		/// get i18n message
		/// </summary>
		/// <param name="language"></param>
		/// <param name="key"></param>
		/// <returns></returns>
		public static string? GetI18nMessages(string language, string key) => I18nDictionary.FirstOrDefault(x => x.Key == language).Value.FirstOrDefault(x => x.Key == key).Value;
	}
}