using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
	public class FunctionController : ApiControllerBase
	{
		/// <summary>
		/// set remote ssh cert auth
		/// </summary>
		/// <returns></returns>
		[HttpPost("[action]")]
		public bool SetRemoteSSHCertAuth()
		{
			var publicKey = System.IO.File.ReadAllText(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".ssh\\id_rsa.pub"));
			Renci.SshNet.SshClient sshClient = new("1.1.1.1", "root", "123456");
			sshClient.Connect();
			sshClient.RunCommand($"echo \"{publicKey}\" >> ~/.ssh/authorized_keys");
			sshClient.Disconnect();
			return true;
		}
	}
}
