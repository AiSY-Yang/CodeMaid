using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Api.Tools
{
	public static class FileTools
	{
		public static async Task Write(string path, CompilationUnitSyntax oldCompilationUnitSyntax, CompilationUnitSyntax newCompilationUnitSyntax)
		{
			if (oldCompilationUnitSyntax.ToFullString() != newCompilationUnitSyntax.ToFullString())
			{
				await File.WriteAllTextAsync(path, newCompilationUnitSyntax.ToFullString());
			}
		}
	}
}
