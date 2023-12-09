using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesModels.Filters
{
	/// <summary>
	/// 分页结果
	/// </summary>
	public record PagedFilter : IFilter
	{
		/// <summary>
		/// 页码
		/// </summary>
		public int PageIndex { get; init; }
		/// <summary>
		/// 页尺寸
		/// </summary>
		public int PageSize { get; init; }
	}
}
