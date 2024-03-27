using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Api.Tools
{
	/// <summary>
	/// 文件服务
	/// </summary>
	public static class FileTools
	{
		/// <summary>
		/// 更新文件
		/// </summary>
		/// <param name="path"></param>
		/// <param name="oldCompilationUnitSyntax"></param>
		/// <param name="newCompilationUnitSyntax"></param>
		/// <returns></returns>
		public static async Task Write(string path, CompilationUnitSyntax oldCompilationUnitSyntax, CompilationUnitSyntax newCompilationUnitSyntax)
		{
			var dirPath = Path.GetDirectoryName(path);
			if (dirPath != null && !Directory.Exists(dirPath)) Directory.CreateDirectory(dirPath);
			if (oldCompilationUnitSyntax.IsEquivalentTo(newCompilationUnitSyntax))
				if (File.Exists(path))
					return;
			await File.WriteAllTextAsync(path, newCompilationUnitSyntax.ToFullString());
		}
	}
}
