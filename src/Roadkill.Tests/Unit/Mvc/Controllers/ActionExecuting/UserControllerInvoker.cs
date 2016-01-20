using System.Web.Mvc;
using Roadkill.Core;
using Roadkill.Core.AmazingConfig;
using Roadkill.Core.Email;
using Roadkill.Core.Mvc.Controllers;
using Roadkill.Core.Security;

namespace Roadkill.Tests.Unit.Mvc.Controllers
{
	public class UserControllerInvoker : UserController
	{
		public UserControllerInvoker(IConfigurationStore configurationStore, UserServiceBase userService,
			IUserContext context, SignupEmail signupEmail, ResetPasswordEmail resetPasswordEmail)
			: base(configurationStore, userService, context, signupEmail, resetPasswordEmail)
		{
		}

		public void CallOnActionExecuting(ActionExecutingContext filterContext)
		{
			filterContext.Controller = this;
			OnActionExecuting(filterContext);
		}
	}
}