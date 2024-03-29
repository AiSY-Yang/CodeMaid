﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

using Repository;

namespace Microsoft.Extensions.DependencyInjection
{
	public static class IServiceCollectionExtensions
	{
		public static void AddMaidRepository(this IServiceCollection serviceCollection)
		{
			foreach (var item in typeof(IServiceCollectionExtensions).Assembly.GetTypes().Where(x=>!x.IsAbstract&& x.IsAssignableTo(typeof(IRepository))))
			{
				serviceCollection.AddScoped(item);
			}
		}
	}
}
