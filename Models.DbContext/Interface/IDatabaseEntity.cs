using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Models.Database.Interface
{
	/// <summary>
	/// 数据库对象
	/// </summary>
	public interface IDatabaseEntity
	{
		/// <summary>
		/// 唯一ID
		/// </summary>
		public long Id { get; }
		/// <summary>
		/// 创建时间
		/// </summary>
		public DateTimeOffset CreateTime { get; }
		/// <summary>
		/// 更新时间
		/// </summary>
		public DateTimeOffset UpdateTime { get; }
		/// <summary>
		/// 是否有效
		/// </summary>
		public bool IsDeleted { get; set; }
	}
}
