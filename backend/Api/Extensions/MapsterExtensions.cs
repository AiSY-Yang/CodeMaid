using Mapster;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// IServiceCollection扩展
/// </summary>
public static class MapsterExtensions
{
	static MapsterExtensions()
	{
		IgnoreNullAdaptRule = Mapster.TypeAdapterConfig.GlobalSettings.Default.Config.Clone().Default.IgnoreNullValues(true);
		NotIgnoreNullAdaptRule = Mapster.TypeAdapterConfig.GlobalSettings.Default.Config.Clone().Default.IgnoreNullValues(false);
	}
	static readonly TypeAdapterSetter IgnoreNullAdaptRule;
	static readonly TypeAdapterSetter NotIgnoreNullAdaptRule;
	/// <summary>
	/// 添加Mapster配置
	/// </summary>
	/// <param name="serviceCollection"></param>
	public static void AddMapster(this IServiceCollection serviceCollection)
	{
	}
}
