using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Roadkill.Core.AmazingConfig;
using Roadkill.Core.Cache;
using Roadkill.Core.Mvc.Attributes;
using Roadkill.Core.Mvc.ViewModels;
using Roadkill.Core.Plugins;
using Roadkill.Core.Security;

namespace Roadkill.Core.Mvc.Controllers
{
	[AdminRequired]
	public class PluginSettingsController : ControllerBase
	{
		private readonly IConfigurationStore _configurationStore;
		private readonly IPluginFactory _pluginFactory;
		private readonly SiteCache _siteCache;
		private readonly PageViewModelCache _viewModelCache;
		private readonly ListCache _listCache;

		public PluginSettingsController(IConfigurationStore configurationStore, UserServiceBase userService, IUserContext context, 
			IPluginFactory pluginFactory, SiteCache siteCache, PageViewModelCache viewModelCache, ListCache listCache)
			: base (configurationStore, userService, context)
		{
			_configurationStore = configurationStore;
			_pluginFactory = pluginFactory;
			_siteCache = siteCache;
			_viewModelCache = viewModelCache;
			_listCache = listCache;
		}

		public ActionResult Index()
		{
			IEnumerable<TextPlugin> plugins = _pluginFactory.GetTextPlugins().OrderBy(x => x.Name);
			List<PluginViewModel> modelList = new List<PluginViewModel>();

			foreach (TextPlugin plugin in plugins)
			{
				modelList.Add(new PluginViewModel(plugin));
			}

			return View(modelList);
		}

		public ActionResult Edit(string id)
		{
			throw new NotImplementedException();

			//// Guards
			//if (string.IsNullOrEmpty(id))
			//	return RedirectToAction("Index");

			//TextPlugin plugin = _pluginFactory.GetTextPlugin(id);
			//if (plugin == null)
			//	return RedirectToAction("Index");

			//PluginViewModel model = new PluginViewModel()
			//{
			//	Id = plugin.Id,
			//	DatabaseId = plugin.DatabaseId,
			//	Name = plugin.Name,
			//	Description = plugin.Description,
			//};

			//// Try to load the settings from the database, fall back to defaults
			//model.SettingValues = new List<SettingValue>(plugin.Settings.Values);
			//model.IsEnabled = plugin.Settings.IsEnabled;

			//return View(model);
		}

		[HttpPost]
		public ActionResult Edit(PluginViewModel model)
		{
			throw new NotImplementedException();

			//TextPlugin plugin = _pluginFactory.GetTextPlugin(model.Id);
			//if (plugin == null)
			//	return RedirectToAction("Index");

			//// Update the plugin settings with the values from the summary
			//plugin.Settings.IsEnabled = model.IsEnabled;

			//foreach (SettingValue summaryValue in model.SettingValues)
			//{
			//	SettingValue pluginValue = plugin.Settings.Values.FirstOrDefault(x => x.Name == summaryValue.Name);
			//	if (pluginValue != null)
			//		pluginValue.Value = summaryValue.Value;
			//}

			//// Update the plugin last saved date - this is important for 304 modified tracking
			//// when the browser caching option is turned on.
			//IConfiguration config = ConfigurationStore.Load();

			//throw new InvalidOperationException("TODO");
			////Configuration.SiteSettings settings = SettingsService.GetSiteSettings();
			////settings.PluginLastSaveDate = DateTime.UtcNow;
			////ConfigurationViewModel settingsViewModel = new ConfigurationViewModel(Configuration, settings);
			////SettingsService.SaveSiteSettings(settingsViewModel);

			//// Save and clear the cached settings

			//var configuration = _configurationStore.Load();
			////configuration.SaveTextPluginSettings(plugin);

			//_siteCache.RemovePluginSettings(plugin);
		
			//// Clear all other caches if the plugin has been enabled or disabled.
			//_viewModelCache.RemoveAll();
			//_listCache.RemoveAll();

			//return RedirectToAction("Index");
		}
	}
}