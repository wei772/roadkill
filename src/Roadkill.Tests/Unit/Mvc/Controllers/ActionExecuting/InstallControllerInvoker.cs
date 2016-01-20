using System.Web.Mvc;
using Roadkill.Core.AmazingConfig;
using Roadkill.Core.Database;
using Roadkill.Core.Mvc.Controllers;
using Roadkill.Core.Services;

namespace Roadkill.Tests.Unit.Mvc.Controllers
{
	internal class InstallControllerInvoker : InstallController
	{
		public InstallControllerInvoker(IConfigurationStore configurationStore, IWebConfigManager webConfigManager, IInstallationService installationService, IDatabaseTester databaseTester)
			: base(configurationStore, webConfigManager, installationService)
		{

		}

		public void CallOnActionExecuting(ActionExecutingContext filterContext)
		{
			base.OnActionExecuting(filterContext);
		}
	}
}