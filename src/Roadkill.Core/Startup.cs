using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Owin;
using Roadkill.Core.AmazingConfig;
using Roadkill.Core.Attachments;
using Roadkill.Core.Configuration;
using Roadkill.Core.DependencyResolution;
using Roadkill.Core.Logging;
using Roadkill.Core.Mvc.Setup;
using Roadkill.Core.Owin;
using Roadkill.Core.Services;

namespace Roadkill.Core
{
	public class Startup
	{
		// See LocatorStartup for lots of pre-startup IoC setup that's performed.

		public void Configuration(IAppBuilder app)
		{
			var configurationStore = LocatorStartup.Locator.GetInstance<IConfigurationStore>();
			var configuration = configurationStore.Load();

			// Check Roadkill is installed, redirect if it's not.
			app.Use<InstallCheckMiddleware>(configuration);

			// Register the "/Attachments/" route handler. This needs to be called before the other routing setup.
			if (configuration.Installed)
			{
				// InstallService.Install also performs this
				var fileService = LocatorStartup.Locator.GetInstance<IFileService>();
				AttachmentRouteHandler.RegisterRoute(configurationStore, RouteTable.Routes, fileService);
			}

			// Filters
			GlobalFilters.Filters.Add(new HandleErrorAttribute());

			// Areas are used for Site settings (for a cleaner view structure)
			// This should be called before the other routes, for some reason.
			AreaRegistration.RegisterAllAreas();

			// Register WebApi/MVC routes, including Swashbuckle
			if (configuration.SecuritySettings.IsRestApiEnabled)
			{
				Routing.RegisterWebApi(GlobalConfiguration.Configuration);
			}

			// Register MVC routes, including Swashbuckle
			Routing.Register(RouteTable.Routes);

			// Custom view engine registration (to add directory search paths for Theme views)
			ExtendedRazorViewEngine.Register();

			// Self-hosting setup for WebApi (WebApi Owin Self Host).
			if (configuration.SecuritySettings.IsRestApiEnabled)
			{
				app.UseWebApi(new HttpConfiguration());
			}

			Log.Information("Roadkill started");
		}
	}
}
