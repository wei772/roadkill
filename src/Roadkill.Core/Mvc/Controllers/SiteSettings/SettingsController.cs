using System;
using System.Web.Mvc;
using Roadkill.Core.AmazingConfig;
using Roadkill.Core.Cache;
using Roadkill.Core.Configuration;
using Roadkill.Core.Mvc.Attributes;
using Roadkill.Core.Mvc.ViewModels;
using Roadkill.Core.Security;
using Roadkill.Core.Services;

namespace Roadkill.Core.Mvc.Controllers
{
	/// <summary>
	/// Provides functionality for the settings page including tools and user management.
	/// </summary>
	/// <remarks>All actions in this controller require admin rights.</remarks>
	[AdminRequired]
	public class SettingsController : ControllerBase
	{
		private readonly SiteCache _siteCache;
		private readonly IConfigReaderWriter _configReaderWriter;

		public SettingsController(IConfigurationStore configurationStore, UserServiceBase userManager, 
			IUserContext context, SiteCache siteCache, IConfigReaderWriter configReaderWriter)
			: base(configurationStore, userManager, context) 
		{
			_siteCache = siteCache;
			_configReaderWriter = configReaderWriter;
		}

		/// <summary>
		/// The default settings page that displays the current Roadkill settings.
		/// </summary>
		/// <returns>A <see cref="SettingsViewModel"/> as the model.</returns>
		public ActionResult Index()
		{
			throw new InvalidOperationException("TODO");

			//SettingsViewModel model = new SettingsViewModel(Configuration, siteSettings);
			//model.SetSupportedDatabases(SettingsService.GetSupportedDatabases());
			//return View(model);

			return View();
		}

		/// <summary>
		/// Saves the <see cref="SettingsViewModel"/> that is POST'd to the action.
		/// </summary>
		/// <param name="model">The settings to save to the web.config/database.</param>
		/// <returns>A <see cref="SettingsViewModel"/> as the model.</returns>
		[HttpPost]
		[ValidateInput(false)]
		public ActionResult Index(SettingsViewModel model)
		{
			if (ModelState.IsValid)
			{
				_configReaderWriter.Save(model);

				throw new InvalidOperationException("TODO");
				//_settingsService.SaveSiteSettings(model);
				_siteCache.RemoveMenuCacheItems();

				// Refresh the AttachmentsDirectoryPath using the absolute attachments path, as it's calculated in the constructor
				ApplicationSettings appSettings = _configReaderWriter.GetApplicationSettings();
				model.FillFromApplicationSettings(appSettings);
				model.UpdateSuccessful = true;
			}

			return View(model);
		}
	}
}
