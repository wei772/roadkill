using System;
using System.Web.Mvc;
using Roadkill.Core.Database;
using Roadkill.Core.Services;
using Roadkill.Core.Security;
using Roadkill.Core.Mvc.ViewModels;
using System.Collections.Generic;
using System.Threading;
using System.Globalization;
using System.Linq;
using Roadkill.Core.AmazingConfig;

namespace Roadkill.Core.Mvc.Controllers
{
	/// <summary>
	/// Provides functionality for the installation wizard.
	/// </summary>
	/// <remarks>If the web.config "installed" setting is "true", then all the actions in
	/// this controller redirect to the homepage</remarks>
	public class InstallController : Controller, IRoadkillController
	{
		private readonly IWebConfigManager _webConfigManager;
		private readonly IInstallationService _installationService;
		private static string _uiLanguageCode = "en";

		public IConfigurationStore ConfigurationStore { get; private set; }
		public UserServiceBase UserService { get; private set; }
		public IUserContext Context { get; private set; }

		/// <summary>
		/// 
		/// </summary>
		public InstallController(IInstallationService installationService, IConfigurationStore configurationStore, UserServiceBase userService, IUserContext context, IWebConfigManager webConfigManager)
		{
			_installationService = installationService;
			_webConfigManager = webConfigManager;

			ConfigurationStore = configurationStore;
			UserService = userService;
			Context = context;
		}

		protected override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			IConfiguration config = ConfigurationStore.Load();

			if (config.Installed)
				filterContext.Result = new RedirectResult(this.Url.Action("Index", "Home"));
		}

		/// <summary>
		/// Installs Roadkill with default settings and the provided datastory type and connection string.
		/// </summary>
		public ActionResult Unattended(string databaseName, string connectionString)
		{
			var model = new ConfigurationViewModel();
			model.DatabaseProvider = databaseName;
			model.ConnectionString = connectionString;
			model.AllowedFileTypes = "jpg,png,gif,zip,xml,pdf";
			model.AttachmentsFolder = "~/App_Data/Attachments";
			model.MarkupType = "Creole";
			model.Theme = "Responsive";
			model.UseObjectCache = true;
			model.UseBrowserCache = true;
			model.AdminEmail = "admin@localhost";
			model.AdminPassword = "Password1";
			model.AdminRoleName = "admins";
			model.EditorRoleName = "editors";
			model.SiteName = "my site";
			model.SiteUrl = "http://localhost";
			_installationService.Install(model);

			return Content("Unattended installation complete");
		}

		/// <summary>
		/// Returns Javascript 'constants' for the installer.
		/// </summary>
		public ActionResult InstallerJsVars()
		{
			return View();
		}

		/// <summary>
		/// Displays the language choice page.
		/// </summary>
		public ActionResult Index()
		{
			Thread.CurrentThread.CurrentUICulture = new CultureInfo("en");

			return View("Index", LanguageViewModel.SupportedLocales());
		}

		/// <summary>
		/// Resets the app pool and the installed state (for debugging /help + support).
		/// </summary>
		public ActionResult Reset()
		{
			_installationService.SetUninstalled();
			_installationService.ReloadAppDomain();

			return RedirectToAction("Index");
		}

		/// <summary>
		/// Displays the start page for the installer (step1).
		/// </summary>
		public ActionResult Step1(string language)
		{
			Thread.CurrentThread.CurrentUICulture = new CultureInfo(language);
			LanguageViewModel languageModel = LanguageViewModel.SupportedLocales().First(x => x.Code == language);

			return View(languageModel);
		}

		/// <summary>
		/// Displays the second step in the installation wizard (connection strings and site url/name).
		/// </summary>
		public ActionResult Step2(string language)
		{
			// Persist the language change now that we know the web.config can be written to.
			if (!string.IsNullOrEmpty(language))
			{
				Thread.CurrentThread.CurrentUICulture = new CultureInfo(language);
				_webConfigManager.UpdateLanguage(language);
			}

			var settingsModel = new ConfigurationViewModel();
			IEnumerable<RepositoryInfo> supportedDatabases = _installationService.GetSupportedDatabases();

			settingsModel.SetSupportedDatabases(supportedDatabases);

			return View(settingsModel);
		}

		/// <summary>
		/// Displays the authentication choice step in the installation wizard.
		/// </summary>
		/// <remarks>The <see cref="ConfigurationViewModel"/> object that is POST'd is passed to the next step.</remarks>
		[HttpPost]
		public ActionResult Step3(ConfigurationViewModel model)
		{
			return View(model);
		}

		/// <summary>
		/// Displays either the Windows Authentication settings view, or the DB settings view depending on
		/// the choice in Step3.
		/// </summary>
		/// <remarks>The <see cref="ConfigurationViewModel"/> object that is POST'd is passed to the next step.</remarks>
		[HttpPost]
		public ActionResult Step3b(ConfigurationViewModel model)
		{
			model.LdapConnectionString = "LDAP://";
			model.EditorRoleName = "Editor";
			model.AdminRoleName = "Admin";

			if (model.UseWindowsAuth)
				return View("Step3WindowsAuth", model);
			else
				return View("Step3Database",model);
		}

		/// <summary>
		/// Displays the final installation step, which provides choices for caching, themes etc.
		/// </summary>
		/// <remarks>The <see cref="ConfigurationViewModel"/> object that is POST'd is passed to the next step.</remarks>
		[HttpPost]
		public ActionResult Step4(ConfigurationViewModel model)
		{
			model.AllowedFileTypes = "jpg,png,gif,zip,xml,pdf";
			model.AttachmentsFolder = "~/App_Data/Attachments";
			model.MarkupType = "Creole";
			model.Theme = "Responsive";
			model.UseObjectCache = true;
			model.UseBrowserCache = false;

			return View(model);
		}

		/// <summary>
		/// Validates the POST'd <see cref="ConfigurationViewModel"/> object. If the settings are valid,
		/// an attempt is made to install using this.
		/// </summary>
		/// <returns>The Step5 view is displayed.</returns>
		[HttpPost]
		[ValidateInput(false)]
		public ActionResult Step5(ConfigurationViewModel model)
		{
			try
			{
				// Any missing values are handled by data annotations. Those that are missed
				// can be seen as fiddling errors which are down to the user.
				if (ModelState.IsValid)
				{
					_installationService.Install(model);
				}
			}
			catch (Exception e)
			{
				try
				{
					_installationService.SetUninstalled();
				}
				catch (Exception ex)
				{
					// TODO-translation
					ModelState.AddModelError("An error occurred rolling back the installation", ex.Message + e);
				}

				ModelState.AddModelError("An error occurred installing", e.Message + e);
			}

			return View(model);
		}
	}
}
