using Roadkill.Core.AmazingConfig;
using Roadkill.Core.Configuration;
using Roadkill.Core.Security;
using Roadkill.Core.Services;

namespace Roadkill.Core.Mvc.Controllers
{
	public interface IRoadkillController
	{
		IConfigurationStore ConfigurationStore { get; }
		UserServiceBase UserService { get; }
		IUserContext Context { get; }
	}
}