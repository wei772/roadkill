using System.Collections.Generic;
using Roadkill.Core.Database;
using Roadkill.Core.Mvc.ViewModels;

namespace Roadkill.Core.Services
{
	public interface IInstallationService
	{
		IEnumerable<RepositoryInfo> GetSupportedDatabases();
		void Install(ConfigurationViewModel model);
		void SetUninstalled();
		void ReloadAppDomain();
	}
}