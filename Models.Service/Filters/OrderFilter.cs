using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesModels.Filters
{
	/// <summary>
	/// 带排序的分页结果
	/// </summary>
	public record OrderFilter : PagedFilter
	{
		/// <summary>
		/// 排序
		/// </summary>
		public Dictionary<string, string>? OrderBy { get; init; }
	}
}
