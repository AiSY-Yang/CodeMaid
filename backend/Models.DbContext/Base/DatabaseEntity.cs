using System.ComponentModel.DataAnnotations;

using Mapster;

namespace Models.CodeMaid
{
	/// <summary>
	/// 所有数据库对象的基类
	/// </summary>
	public abstract class DatabaseEntity
	{
		/// <summary>
		/// 唯一ID1
		/// </summary>
		[Key]
		public long Id { get; }
		/// <summary>
		/// 创建时间
		/// </summary>
		[AdaptIgnore(MemberSide.Destination)]
		public DateTimeOffset CreateTime { get; set; } = DateTimeOffset.Now;
		/// <summary>
		/// 更新时间
		/// </summary>
		[AdaptIgnore(MemberSide.Destination)]
		public DateTimeOffset UpdateTime { get; set; } = DateTimeOffset.Now;
		/// <summary>
		/// 是否有效
		/// </summary>
		public bool IsDeleted { get; set; }
	}
}