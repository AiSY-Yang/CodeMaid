using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using ExtensionMethods;
using System;
using Models.CodeMaid;

using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Api.Extensions
{
	/// <summary>
	/// Roslyn扩展
	/// </summary>
	public static class RoslynExtension
	{
		/// <summary>
		/// 获取编译单元下的所有指定的类型的对象 忽略命名空间层级
		/// </summary>
		/// <param name="compilationUnit"></param>
		/// <returns></returns>
		public static List<T> GetDeclarationSyntaxes<T>(this CompilationUnitSyntax compilationUnit) where T : BaseTypeDeclarationSyntax
		{
			//没有命名空间的类型
			var declarationSyntax = compilationUnit.ChildNodes().Where(x => x is T).ToList();
			//如果原有的文件有命名空间 取命名空间下的成员
			var declarationSyntax2 = compilationUnit.ChildNodes()
				.Where(x => x is NamespaceDeclarationSyntax || x is FileScopedNamespaceDeclarationSyntax).SelectMany(x => x.ChildNodes())
					   .Where(x => x is T).ToList();
			return declarationSyntax.Union(declarationSyntax2).Select(x => x as T).ToList()!;
		}
		/// <summary>
		/// 获取备注
		/// </summary>
		/// <param name="memberDeclarationSyntax"></param>
		/// <returns><c>remarks</c>标签内的信息 如果不存在<c>remarks</c>标签返回<c>null</c></returns>
		public static string? GetRemark(this MemberDeclarationSyntax memberDeclarationSyntax)
		{
			var xml = memberDeclarationSyntax.GetLeadingTrivia().Select(x => x.GetStructure()).OfType<DocumentationCommentTriviaSyntax>().FirstOrDefault();
			var remarkNode = xml?.ChildNodes().OfType<XmlElementSyntax>().FirstOrDefault(x => x.StartTag.Name.ToString() == "remarks");
			var contentNode = remarkNode?.ChildNodes().OfType<XmlTextSyntax>().FirstOrDefault();
			return contentNode?.GetText().ToString();
		}
		/// <summary>
		/// 获取注释 如果是多行的话会合并成一行 中间空格隔开
		/// </summary>
		/// <param name="memberDeclarationSyntax"></param>
		/// <returns><c>summary</c>标签内的信息 如果不存在<c>summary</c>标签返回<c>null</c></returns>
		public static string? GetSummary(this MemberDeclarationSyntax memberDeclarationSyntax)
		{
			var xml = memberDeclarationSyntax.GetLeadingTrivia().Select(x => x.GetStructure()).OfType<DocumentationCommentTriviaSyntax>().FirstOrDefault();
			var summaryNode = xml?.ChildNodes().OfType<XmlElementSyntax>().FirstOrDefault(x => x.StartTag.Name.ToString() == "summary");
			var contentNode = summaryNode?.ChildNodes().OfType<XmlTextSyntax>().FirstOrDefault();
			if (contentNode is null)
			{
				return null;
			}
			return string.Join(' ', contentNode.TextTokens.Where(x => x.IsKind(SyntaxKind.XmlTextLiteralToken)).Select(x => x.Text.Trim()).Where(x => !x.IsNullOrWhiteSpace()).ToList());
		}
		/// <summary>
		/// 获取指定名称的Attribute
		/// </summary>
		/// <param name="memberDeclarationSyntax"></param>
		/// <param name="attributeName"></param>
		/// <returns></returns>
		public static AttributeSyntax? GetAttribute(this MemberDeclarationSyntax memberDeclarationSyntax, string attributeName)
		{
			return memberDeclarationSyntax.AttributeLists.SelectMany(x => x.Attributes).FirstOrDefault(x => x.Name.ToString() == attributeName);
		}
		/// <summary>
		/// 获取指定名称的Attribute的参数
		/// </summary>
		/// <param name="memberDeclarationSyntax"></param>
		/// <param name="attributeName"></param>
		/// <returns></returns>
		public static string? GetAttributeArguments(this MemberDeclarationSyntax memberDeclarationSyntax, string attributeName)
		{
			var attr = memberDeclarationSyntax.GetAttribute(attributeName);
			if (attr?.ArgumentList == null) return null;
			return string.Join(", ", attr.ArgumentList.Arguments.Select(x => x.ToString()));
		}
		/// <summary>
		/// 添加或者替换属性
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="typeDeclarationSyntax"></param>
		/// <param name="attribute"></param>
		/// <returns></returns>
		public static T AddOrReplaceAttribute<T>(this T typeDeclarationSyntax, AttributeDefinition attribute) where T : MemberDeclarationSyntax
		{
			var attributeOld = typeDeclarationSyntax.AttributeLists.SelectMany(x => x.Attributes).FirstOrDefault(x => x.Name.ToString() == attribute.Name);
			if (attributeOld != null)
			{
				if (attribute.ArgumentsText is null)
				{
					return typeDeclarationSyntax;
				}
				else
				{
					return typeDeclarationSyntax.ReplaceNode(attributeOld, attributeOld.WithArgumentList(ParseAttributeArgumentList(attribute.ArgumentsText)));
				}
			}
			else
			{
				var name = ParseName(attribute.Name);
				var arguments = attribute.ArgumentsText is null ? null : ParseAttributeArgumentList(attribute.ArgumentsText);
				//是否是顶级的元素
				var isTop = true;
				//获取原来的缩进
				var indentation = typeDeclarationSyntax.GetLeadingTrivia().Last();
				if (indentation.ToString().IsNullOrWhiteSpace())
				{
					isTop = false;
				}
				if (isTop)
				{
					return (T)typeDeclarationSyntax.WithoutLeadingTrivia()
						.AddAttributeLists(AttributeList().AddAttributes(Attribute(name, arguments)))
						.WithLeadingTrivia(typeDeclarationSyntax.GetLeadingTrivia())
						;
				}
				else
				{
					//如果不是顶级元素的话 需要在member前面加上缩进和换行
					if (typeDeclarationSyntax.AttributeLists.Any())
					{
						//当有Attribute的时候 新增的Attribute需要
						return (T)typeDeclarationSyntax.WithoutLeadingTrivia()
							.AddAttributeLists(AttributeList()
													.AddAttributes(Attribute(name, arguments))
													.WithLeadingTrivia(indentation)
													.WithTrailingTrivia(SyntaxTrivia(SyntaxKind.EndOfLineTrivia, Environment.NewLine))
													)
							.WithLeadingTrivia(typeDeclarationSyntax.GetLeadingTrivia());
					}
					else
					{
						return (T)typeDeclarationSyntax
							.WithoutLeadingTrivia()
							.WithLeadingTrivia(SyntaxTrivia(SyntaxKind.EndOfLineTrivia, Environment.NewLine), typeDeclarationSyntax.GetLeadingTrivia().Last())
							.AddAttributeLists(AttributeList().AddAttributes(Attribute(name, arguments)))
							.WithLeadingTrivia(typeDeclarationSyntax.GetLeadingTrivia())
							;
					}
				;
				}
			}
		}

		/// <summary>
		/// 添加或者替换XML节点内容
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="typeDeclarationSyntax"></param>
		/// <param name="tagName"></param>
		/// <param name="text"></param>
		/// <returns></returns>
		private static T AddOrReplaceXmlContent<T>(T typeDeclarationSyntax, string tagName, string text) where T : MemberDeclarationSyntax
		{
			//是否是顶级的元素
			var isTop = true;
			//获取原来的缩进
			var indentation = typeDeclarationSyntax.GetLeadingTrivia().Last().ToString();
			if (indentation.IsNullOrWhiteSpace())
			{
				isTop = false;
			}
			//如果是summary 则内容要修改成多行的格式
			var content = tagName == "summary" ? $"{Environment.NewLine}{indentation}/// {text}{Environment.NewLine}{indentation}/// " : text;
			//拿到这个tag
			var xml = typeDeclarationSyntax.GetLeadingTrivia()
				.Select(x => x.GetStructure())
				.OfType<DocumentationCommentTriviaSyntax>()
				.FirstOrDefault()?.ChildNodes()
				.OfType<XmlElementSyntax>()
				.FirstOrDefault(x => x.StartTag.Name.ToString() == tagName);
			if (xml is null)
			{
				//新增标签 前面要有缩进 后面要有换行 且保留原来的trivia
				var tag = string.Empty;
				if (isTop)
				{
					tag = $"/// <{tagName}>{content}</{tagName}>{Environment.NewLine}";
				}
				else
				{
					tag = $"/// <{tagName}>{content}</{tagName}>{Environment.NewLine}{indentation}";
				}
				return typeDeclarationSyntax.WithLeadingTrivia(
					typeDeclarationSyntax.GetLeadingTrivia()
					.AddRange(SyntaxTriviaList.Create(SyntaxTrivia(SyntaxKind.SingleLineCommentTrivia, tag)))
					);
			}
			else
			{
				//替换标签
				var contentNode = xml.ChildNodes().OfType<XmlTextSyntax>().FirstOrDefault();
				var newContentNode = XmlText(content);
				return typeDeclarationSyntax.ReplaceNode(xml, xml.ReplaceNode(contentNode!, newContentNode));
			}
		}

		/// <summary>
		/// 添加或者替换remarks标签
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="typeDeclarationSyntax"></param>
		/// <param name="remark"></param>
		/// <returns></returns>
		public static T AddOrReplaceRemark<T>(this T typeDeclarationSyntax, string remark) where T : MemberDeclarationSyntax
		{
			//当原来没有注释的时候不添加新的内容 防止破坏警告信息
			var summary = typeDeclarationSyntax.GetSummary();
			if (summary is null) return typeDeclarationSyntax;
			return AddOrReplaceXmlContent(typeDeclarationSyntax, "remarks", remark);
		}

		/// <summary>
		/// 添加或者替换summary标签
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="typeDeclarationSyntax"></param>
		/// <param name="summary">注释</param>
		/// <returns></returns>
		public static T AddOrReplaceSummary<T>(this T typeDeclarationSyntax, string? summary) where T : MemberDeclarationSyntax
		{
			var oldsummary = typeDeclarationSyntax.GetSummary();
			if (oldsummary == summary || summary is null) return typeDeclarationSyntax;
			return AddOrReplaceXmlContent(typeDeclarationSyntax, "summary", summary);
		}

		/// <summary>
		/// 移除类里面指定类型的属性
		/// </summary>
		/// <param name="classDeclarationSyntax"></param>
		/// <param name="propertyType">属性类型</param>
		/// <returns></returns>
		public static ClassDeclarationSyntax RemovePropertyByType(this ClassDeclarationSyntax classDeclarationSyntax, string propertyType)
		{
			var propertyDeclaration = classDeclarationSyntax.ChildNodes()
				.OfType<PropertyDeclarationSyntax>()
				.Where(p => p.Type.ToString() == propertyType)
				.ToList();
			return classDeclarationSyntax.RemoveNodes(propertyDeclaration, SyntaxRemoveOptions.KeepNoTrivia)!;
		}

	}
}
