using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

using Mapster;

using Reponsitory;

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
