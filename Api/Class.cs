using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Api
{
	public class TypeInferenceRewriter : CSharpSyntaxRewriter
	{
		private readonly SemanticModel SemanticModel;

		public TypeInferenceRewriter(SemanticModel semanticModel) => SemanticModel = semanticModel;
		public override SyntaxNode VisitLocalDeclarationStatement(LocalDeclarationStatementSyntax node)
		{
			if (node.Declaration.Variables.Count > 1)
			{
				return node;
			}
			if (node.Declaration.Variables[0].Initializer == null)
			{
				return node;
			}
			return node;
		}
	}
}
