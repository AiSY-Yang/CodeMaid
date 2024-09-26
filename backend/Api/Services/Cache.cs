using System.Text.Json;

using EasyCaching.Core;

namespace Api.Services
{
	/// <summary>
	/// cache service
	/// </summary>
	/// <param name="cachingProvider"></param>
	public class Cache(IRedisCachingProvider cachingProvider)
	{
		/// <summary>
		/// Serialize an object to json and save it to the cache.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="key"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public async Task<bool> Set<T>(string key, T value) => await cachingProvider.StringSetAsync(key, JsonSerializer.Serialize(value));
		/// <summary>
		/// get value from cache
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="key"></param>
		/// <returns></returns>
		public async Task<T?> Get<T>(string key)
		{
			var value = await cachingProvider.StringGetAsync(key);
			if (value is null)
			{
				return default;
			}
			else
				return JsonSerializer.Deserialize<T>(value);
		}
	}
}
