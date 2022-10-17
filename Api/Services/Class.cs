using System.Net;

namespace Api.Services
{
	public class Class
	{
		/// <summary>
		/// 修复谷歌翻译问题
		/// </summary>
		/// <returns></returns>
		public async Task<bool> SetGoogleTransalate()
		{
			string hostsPath = "";
			if (OperatingSystem.IsWindows())
			{
				hostsPath = "C:\\Windows\\System32\\drivers\\etc\\hosts";
			}
			if (OperatingSystem.IsIOS() || OperatingSystem.IsLinux())
			{
				hostsPath = "/etc/hosts";
			}
			string comment = "# Fix Google Translate";
			string target = "translate.googleapis.com";
			var lines = await File.ReadAllLinesAsync(hostsPath);
			var dns = Dns.GetHostAddresses("google.cn")[0];
			lines.Where(x => x != comment)
				.Where(x => !x.StartsWith('#') && x.Split(' ')[1] != target)
				.ToList();
			lines.Append(comment);
			lines.Append($"{dns} {target}");
			await File.WriteAllLinesAsync(hostsPath, lines);
			return true;
		}
	}
}
