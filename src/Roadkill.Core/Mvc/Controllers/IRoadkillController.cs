using Roadkill.Core.AmazingConfig;
using Roadkill.Core.Security;

namespace Roadkill.Core.Mvc.Controllers
{
	public interface IRoadkillController
	{
		IConfigurationStore ConfigurationStore { get; }
		UserServiceBase UserService { get; }
		IUserContext Context { get; }
	}
}