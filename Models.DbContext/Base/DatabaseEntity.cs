using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Mapster;

using Microsoft.EntityFrameworkCore;

using Models.CodeMaid.Interface;

namespace Models.CodeMaid
{
	/// <summary>
	/// 所有数据库对象的基类
	/// </summary>
	public abstract class DatabaseEntity : IDatabaseEntity
	{
		///<inheritdoc cref="IDatabaseEntity.Id"/>
		[Key]
		public long Id { get; }
		///<inheritdoc cref="IDatabaseEntity.CreateTime"/>
		[AdaptIgnore(MemberSide.Destination)]
		public DateTimeOffset CreateTime { get; set; } = DateTimeOffset.Now;
		///<inheritdoc cref="IDatabaseEntity.UpdateTime"/>
		[AdaptIgnore(MemberSide.Destination)]
		public DateTimeOffset UpdateTime { get; set; } = DateTimeOffset.Now;
		///<inheritdoc cref="IDatabaseEntity.IsDeleted"/>
		public bool IsDeleted { get; set; }
	}
}