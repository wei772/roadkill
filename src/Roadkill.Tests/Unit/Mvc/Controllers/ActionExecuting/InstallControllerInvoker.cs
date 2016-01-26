using System.Web.Mvc;
using Roadkill.Core;
using Roadkill.Core.AmazingConfig;
using Roadkill.Core.Database;
using Roadkill.Core.Mvc.Controllers;
using Roadkill.Core.Security;
using Roadkill.Core.Services;

namespace Roadkill.Tests.Unit.Mvc.Controllers
{
	internal class InstallControllerInvoker : InstallController
	{
		public InstallControllerInvoker(IInstallationService installationService, IConfigurationStore configurationStore, UserServiceBase userService, IUserContext context, IWebConfigManager configManager)
			: base(installationService, configurationStore, userService, context, configManager)
		{

		}

		public void CallOnActionExecuting(ActionExecutingContext filterContext)
		{
			base.OnActionExecuting(filterContext);
		}
	}
}