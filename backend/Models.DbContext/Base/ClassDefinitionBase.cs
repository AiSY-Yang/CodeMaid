using Mapster;

using Models.CodeMaid;

namespace Models.CodeMaid
{
	/// <summary>
	/// 类定义
	/// </summary>
	public abstract class ClassDefinitionBase : DatabaseEntity
	{
		/// <summary>
		/// 命名空间
		/// </summary>
		public string? NameSpace { get; set; } = null!;
		/// <summary>
		/// 修饰符
		/// </summary>
		public string? Modifiers { get; set; } = null!;
		/// <summary>
		/// 是否是抽象类
		/// </summary>
		public bool IsAbstract => Modifiers?.Contains("abstract") ?? false;
		/// <summary>
		/// 类名
		/// </summary>
		public required string Name { get; set; }
		/// <summary>
		/// 注释
		/// </summary>
		public string? Summary { get; set; } = null!;
		/// <summary>
		/// 基类或者接口名称
		/// </summary>
		public string? Base { get; set; } = null!;
		/// <summary>
		/// 类引用的命名空间
		/// </summary>
		public string? Using { get; set; }
		/// <summary>
		/// 前导
		/// </summary>
		public string? LeadingTrivia { get; set; }
		/// <summary>
		/// 成员类型
		/// </summary>
		public required MemberType MemberType { get; set; }
	}
	/// <summary>
	/// 成员类型
	/// </summary>
	/// <remarks>0-类,1-接口,2-记录,3-结构体</remarks>
	public enum MemberType
	{
		/// <summary>
		/// 类
		/// </summary>
		ClassDeclarationSyntax,
		/// <summary>
		/// 接口
		/// </summary>
		InterfaceDeclarationSyntax,
		/// <summary>
		/// 记录
		/// </summary>
		RecordDeclarationSyntax,
		/// <summary>
		/// 结构体
		/// </summary>
		StructDeclarationSyntax,
	}
}