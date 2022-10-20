using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Mapster;

using Microsoft.Extensions.DependencyInjection;

using Models.CodeMaid;

namespace Microsoft.Extensions.DependencyInjection
{
	public static class MapsterExtensions
	{
		public static void AddMapster(this IServiceCollection services)
		{
			new MyRegister().Register();
		}
	}
	class MyRegister
	{
		public void Register()
		{
			TypeAdapterConfig<DatabaseEntity, DatabaseEntity>.ForType().Ignore(x => x.Id).Ignore(x => x.CreateTime).Ignore(x => x.UpdateTime).Ignore(x => x.IsDeleted);
			TypeAdapterConfig<NameSpaceDefinition, NameSpaceDefinition>.ForType().Ignore(x => x.Classes);
			TypeAdapterConfig<ClassDefinition, ClassDefinition>.ForType().Ignore(x => x.Properties);

		}
	}
}
