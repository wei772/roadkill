using System;

namespace Roadkill.Core.Database
{
	public interface IInstallerRepository : IDisposable
	{
		void AddAdminUser(string email, string username, string password);
		void CreateSchema();
	}
}
