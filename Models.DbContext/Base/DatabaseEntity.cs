using Models.Database.Interface;

namespace Models.Database
{
	/// <summary>
	/// 所有数据库对象的基类
	/// </summary>
	public class DatabaseEntity : IDatabaseEntity
	{
		///<inheritdoc cref="IDatabaseEntity.Id"/>
		public long Id { get; }
		///<inheritdoc cref="IDatabaseEntity.CreateTime"/>
		public DateTimeOffset CreateTime { get; }
		///<inheritdoc cref="IDatabaseEntity.UpdateTime"/>
		public DateTimeOffset UpdateTime { get; set; }
		///<inheritdoc cref="IDatabaseEntity.IsDeleted"/>
		public bool IsDeleted { get; set; }
	}
}