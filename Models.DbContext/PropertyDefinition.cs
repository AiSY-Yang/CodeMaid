﻿using System.ComponentModel.DataAnnotations;

using Models.CodeMaid;

namespace Models.CodeMaid
{
	/// <summary>
	/// 类定义
	/// </summary>
	public class PropertyDefinition : DatabaseEntity
	{
		public ClassDefinition ClassDefinition { get; set; } = null!;
		/// <summary>
		/// 前导
		/// </summary>
		public string? LeadingTrivia { get; set; }
		/// <summary>
		/// 注释
		/// </summary>
		public string? Summary { get; set; }
		/// <summary>
		/// 完整文本内容
		/// </summary>
		public string FullText { get; set; } = null!;
		/// <summary>
		/// 修饰符
		/// </summary>
		public string Modifiers { get; set; } = null!;
		/// <summary>
		/// 初始化器
		/// </summary>
		public string? Initializer { get; set; }
		/// <summary>
		/// 属性名称
		/// </summary>
		[MaxLength(20)]
		public string Name { get; set; } = null!;
		/// <summary>
		/// Get方法体
		/// </summary>
		public string? Get { get; set; } = null!;
		/// <summary>
		/// Set方法体
		/// </summary>
		public string? Set { get; set; } = null!;
		/// <summary>
		/// 属性列表
		/// </summary>
		public List<AttributeDefinition> Attributes { get; set; } = null!;
	}
}