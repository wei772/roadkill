using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Roadkill.Core.AmazingConfig;
using Roadkill.Core.Configuration;
using Roadkill.Core.Converters;
using Roadkill.Core.DependencyResolution;
using Roadkill.Core.Services;
using StructureMap;
using StructureMap.Attributes;

namespace Roadkill.Core.Mvc.WebViewPages
{
	// Layout pages aren't created using IDependencyResolver (as they're outside of MVC). So use bastard injection for them.
	public abstract class RoadkillLayoutPage : WebViewPage<object>
	{
		public IConfiguration Configuration { get; set; }
		public IUserContext RoadkillContext { get; set; }
		public MarkupConverter MarkupConverter { get; set; }

		public RoadkillLayoutPage()
		{
			IConfigurationStore configurationStore = LocatorStartup.Locator.GetInstance<IConfigurationStore>();
			Configuration = configurationStore.Load();

			RoadkillContext = LocatorStartup.Locator.GetInstance<IUserContext>();

			if (Configuration.Installed)
			{
				MarkupConverter = LocatorStartup.Locator.GetInstance<MarkupConverter>();
			}
		}
	}
}
