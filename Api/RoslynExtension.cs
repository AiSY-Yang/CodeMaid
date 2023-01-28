using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using ExtensionMethods;
using System;

namespace Api
{
	/// <summary>
	/// roslyn扩展
	/// </summary>
	public static class RoslynExtension
	{
		/// <summary>
		/// 获取备注
		/// </summary>
		/// <param name="memberDeclarationSyntax"></param>
		/// <returns></returns>
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
		/// <returns></returns>
		public static string? GetSummay(this MemberDeclarationSyntax memberDeclarationSyntax)
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
		/// 获取Attribute
		/// </summary>
		/// <param name="memberDeclarationSyntax"></param>
		/// <param name="attributeName"></param>
		/// <returns></returns>
		public static AttributeSyntax? GetAttribute(this MemberDeclarationSyntax memberDeclarationSyntax, string attributeName)
		{
			return memberDeclarationSyntax.AttributeLists.SelectMany(x => x.Attributes).FirstOrDefault(x => x.Name.ToString() == attributeName);
		}
		/// <summary>
		/// 获取Attribute
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
	}
}
