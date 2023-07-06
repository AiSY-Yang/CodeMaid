using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ServicesModels.Methods
{
	public interface Imethods
	{

	}
	public interface IAdd
	{
		public static HttpMethod Method = HttpMethod.Get;
		public static string Route = "[action]";
		public static string Suffix = "[action]";
	}
	public interface IAdd<TData, TResult> : IAdd
	{
	}

}
