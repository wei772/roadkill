using System.Web.Mvc;
using Roadkill.Core;
using Roadkill.Core.AmazingConfig;
using Roadkill.Core.Security;

namespace Roadkill.Tests.Unit.Mvc.Controllers
{
	//
	// These invokers let the tests call OnActionExecuting (without resorting to a tangle of Moq setups)
	//
	public class ControllerBaseInvoker : Roadkill.Core.Mvc.Controllers.ControllerBase
	{
		public ControllerBaseInvoker(IConfigurationStore configurationStore, UserServiceBase userService, IUserContext context) : base(configurationStore, userService, context)
		{

		}

		public void CallOnActionExecuting(ActionExecutingContext filterContext)
		{
			base.OnActionExecuting(filterContext);
		}
	}
}