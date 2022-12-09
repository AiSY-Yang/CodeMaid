using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using ExtensionMethods;

namespace Api
{
	public static class RoslynExtension
	{
		/// <summary>
		/// 获取备注
		/// </summary>
		/// <param name="trivias"></param>
		/// <returns></returns>
		/// <remarks>r1</remarks>
		public static string? GetRemark(this MemberDeclarationSyntax memberDeclarationSyntax)
		{
			var xml = memberDeclarationSyntax.GetLeadingTrivia().Select(x => x.GetStructure()).OfType<DocumentationCommentTriviaSyntax>().FirstOrDefault();
			var remarkNode = xml?.ChildNodes().OfType<XmlElementSyntax>().FirstOrDefault(x => x.StartTag.Name.ToString() == "remarks");
			var contentNode = remarkNode?.ChildNodes().OfType<XmlTextSyntax>().FirstOrDefault();
			return contentNode?.GetText().ToString();
		}
		/// <summary>
		/// 获取注释
		/// </summary>
		/// <param name="trivias"></param>
		/// <returns></returns>
		public static string? GetSummay(this MemberDeclarationSyntax memberDeclarationSyntax)
		{
			var xml = memberDeclarationSyntax.GetLeadingTrivia().Select(x => x.GetStructure()).OfType<DocumentationCommentTriviaSyntax>().FirstOrDefault();
			var remarkNode = xml?.ChildNodes().OfType<XmlElementSyntax>().FirstOrDefault(x => x.StartTag.Name.ToString() == "summary");
			var contentNode = remarkNode?.ChildNodes().OfType<XmlTextSyntax>().FirstOrDefault();
			if (contentNode is null)
			{
				return null;
			}
			return string.Join(' ', contentNode.TextTokens.Where(x => x.IsKind(SyntaxKind.XmlTextLiteralToken)).Select(x => x.Text.Trim()).Where(x => !x.IsNullOrWhiteSpace()).ToList());
		}
	}
}
