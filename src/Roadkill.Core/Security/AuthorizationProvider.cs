using System;
using System.Security.Principal;
using Roadkill.Core.AmazingConfig;

namespace Roadkill.Core.Security
{
	public class AuthorizationProvider : IAuthorizationProvider
	{
		private readonly IConfiguration _configuration;
		private readonly UserServiceBase _userService;

		public AuthorizationProvider(IConfigurationStore configurationStore, UserServiceBase userService)
		{
			if (configurationStore == null)
				throw new ArgumentNullException(nameof(configurationStore));

			if (userService == null)
				throw new ArgumentNullException(nameof(userService));

			_configuration = configurationStore.Load();
			_userService = userService;
		}

		public virtual bool IsAdmin(IPrincipal principal)
		{
			IIdentity identity = principal.Identity;

			if (!identity.IsAuthenticated)
			{
				return false;
			}

			// An empty admin role name implies everyone is an admin - there's roles at all.
			if (string.IsNullOrEmpty(_configuration.SecuritySettings.AdminRoleName))
				return true;

			// For custom IIdentity implementations, check the name (for Windows this should never happen)
			if (string.IsNullOrEmpty(identity.Name))
				return false;

			if (_userService.IsAdmin(identity.Name))
				return true;
			else
				return false;
		}

		public virtual bool IsEditor(IPrincipal principal)
		{
			IIdentity identity = principal.Identity;

			if (!identity.IsAuthenticated)
			{
				return false;
			}

			// An empty editor role name implies everyone is an editor - there's no page security.
			if (string.IsNullOrEmpty(_configuration.SecuritySettings.EditorRoleName))
				return true;

			// Same as IsAdmin - for custom IIdentity implementations, check the name (for Windows this should never happen)
			if (string.IsNullOrEmpty(identity.Name))
				return false;

			if (_userService.IsAdmin(identity.Name) || _userService.IsEditor(identity.Name))
				return true;
			else
				return false;
		}

		public virtual bool IsViewer(IPrincipal principal)
		{
			return true;
		}
	}
}
