﻿using Mapster;


namespace Microsoft.Extensions.DependencyInjection
{
	/// <summary>
	/// IServiceCollection扩展
	/// </summary>
	public static class IServiceCollectionExtensions
	{
		/// <summary>
		/// 添加Mapster配置
		/// </summary>
		/// <param name="serviceCollection"></param>
		public static void AddMapster(this IServiceCollection serviceCollection)
		{
			TypeAdapterConfig<int, int>.ForType();
		}
	}
}
