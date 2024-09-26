using System.Diagnostics;
using System.Text;

using Api.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TaskService.Controller;

/// <summary>
/// 命令行相关
/// </summary>
public class CommandController : CommonController
{
	/// <summary>
	/// 任务信息
	/// </summary>
	public class CommandInfo
	{
		/// <summary>
		/// 程序名称
		/// </summary>
		public required string FileName { get; set; }
		/// <summary>
		/// 工作目录
		/// </summary>
		public string? WorkingDirectory { get; set; }
		/// <summary>
		/// 命令行
		/// </summary>
		public required List<string> Commands { get; set; }
	}
	/// <summary>
	/// 命令输出
	/// </summary>
	public class CommandOutPut
	{
		/// <summary>
		/// 输出
		/// </summary>
		public required string StandardOutput { get; set; }
		/// <summary>
		/// 错误
		/// </summary>
		public required string StandardError { get; set; }
	}

	/// <summary>
	/// 命令行
	/// </summary>
	/// <param name="commandInfo"></param>
	/// <returns></returns>
	[HttpPost("Command")]
	public async Task<ActionResult<CommandOutPut>> Command(CommandInfo commandInfo)
	{
		ProcessStartInfo processStartInfo;
		processStartInfo = new();
		processStartInfo.FileName = commandInfo.FileName;
		processStartInfo.RedirectStandardInput = true;
		processStartInfo.RedirectStandardOutput = true;
		processStartInfo.RedirectStandardError = true;
		processStartInfo.EnvironmentVariables["PATH"] = "/data/genarsa_project/bioinfo_miniconda3/envs/snakemake_python3.9.0/bin:/data/genarsa_project/bioinfo_miniconda3/condabin:/opt/tools/singularity/3.8.1/bin:/opt/tools/jdk-11.0.18/bin:/home/bioworker/bin:/usr/local/sbin:/usr/local/bin:/usr/sbin:/usr/bin:/sbin:/bin:/usr/games:/usr/local/games:/snap/bin:/home/bioworker/.dotnet/tools";
		if (commandInfo.WorkingDirectory is not null) processStartInfo.WorkingDirectory = commandInfo.WorkingDirectory;
		using var process = new Process
		{
			StartInfo = processStartInfo,
			EnableRaisingEvents = true
		};
		var outputBuilder = new StringBuilder();
		var errorBuilder = new StringBuilder();
		process.OutputDataReceived += (sender, e) =>
		{
			if (!string.IsNullOrEmpty(e.Data))
			{
				outputBuilder.AppendLine(e.Data);
			}
		};

		process.ErrorDataReceived += (sender, e) =>
		{
			if (!string.IsNullOrEmpty(e.Data))
			{
				errorBuilder.AppendLine(e.Data);
			}
		};
		process.Start();
		process.BeginOutputReadLine();
		process.BeginErrorReadLine();
		foreach (var command in commandInfo.Commands)
		{
			await process.StandardInput.WriteLineAsync(command);
		}
		await process.StandardInput.WriteLineAsync($"exit");

		await process.WaitForExitAsync();
		var output = outputBuilder.ToString();
		var error = errorBuilder.ToString();
		return new CommandOutPut()
		{
			StandardOutput = output,
			StandardError = error,
		};
	}
}