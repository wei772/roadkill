using System;
using System.Collections.Generic;
using System.Web.Routing;
using Microsoft.Practices.ServiceLocation;
using Mindscape.LightSpeed;
using Roadkill.Core.AmazingConfig;
using Roadkill.Core.Attachments;
using Roadkill.Core.Database;
using Roadkill.Core.Database.MongoDB;
using Roadkill.Core.Database.Schema;
using Roadkill.Core.DependencyResolution;
using Roadkill.Core.Mvc.ViewModels;

namespace Roadkill.Core.Services
{
	/// <summary>
	/// Provides common tasks for changing the Roadkill application settings.
	/// </summary>
	public class InstallationService : IInstallationService
	{
		private readonly IConfigurationStore _configurationStore;
		private readonly IWebConfigManager _webConfigManager;
		private readonly Func<string, string, IInstallerRepository> _getRepositoryFunc;
		internal ServiceLocatorImplBase Locator { get; set; }

		public InstallationService(IConfigurationStore configurationStore, IWebConfigManager webConfigManager)
		{
			_configurationStore = configurationStore;
			_webConfigManager = webConfigManager;
			_getRepositoryFunc = GetRepository;
		}

		internal InstallationService(IConfigurationStore configurationStore, IWebConfigManager webConfigManager, Func<string, string, IInstallerRepository> getRepositoryFunc)
			: this(configurationStore, webConfigManager)
		{
			_getRepositoryFunc = getRepositoryFunc;
		}

		public IEnumerable<RepositoryInfo> GetSupportedDatabases()
		{
			return new List<RepositoryInfo>()
			{
				SupportedDatabases.MongoDB,
				SupportedDatabases.MySQL,
				SupportedDatabases.Postgres,
				SupportedDatabases.SqlServer2008
			};
		}

		public void Install(ConfigurationViewModel model)
		{
			try
			{
				IInstallerRepository installerRepository = _getRepositoryFunc(model.DatabaseProvider, model.ConnectionString);
				installerRepository.CreateSchema();

				if (model.UseWindowsAuth)
				{
					_webConfigManager.WriteForWindowsAuth();
				}
				else
				{
					_webConfigManager.WriteForFormsAuth();
					installerRepository.AddAdminUser(model.AdminEmail, "admin", model.AdminPassword);
				}

				var configuration = new JsonConfiguration();
				configuration.Installed = true;
				configuration.DatabaseProvider = model.DatabaseProvider;
				configuration.ConnectionString = model.ConnectionString;

				configuration.MarkupType = model.MarkupType;
				configuration.SiteUrl = model.SiteUrl;
				configuration.SiteName = model.SiteName;
				configuration.Theme = model.Theme;
				configuration.HeadContent = model.HeadContent;
				configuration.MenuMarkup = model.MenuMarkup;

				configuration.AttachmentSettings.AllowedFileTypes = model.AllowedFileTypes;
				configuration.AttachmentSettings.OverwriteExistingFiles = model.OverwriteExistingFiles;

				configuration.SecuritySettings.AllowUserSignup = model.AllowUserSignup;
				configuration.SecuritySettings.IsRecaptchaEnabled = model.IsRecaptchaEnabled;
				configuration.SecuritySettings.RecaptchaPrivateKey = model.RecaptchaPrivateKey;
				configuration.SecuritySettings.RecaptchaPublicKey = model.RecaptchaPublicKey;

				_configurationStore.Save(configuration);

				ReloadAppDomain();
			}
			catch (DatabaseException ex)
			{
				throw new DatabaseException(ex, "An exception occurred while saving the site configuration.");
			}
		}

		public void SetUninstalled()
		{
			IConfiguration configuration = _configurationStore.Load();
			configuration.Installed = false;

			_configurationStore.Save(configuration);
		}

		public void ReloadAppDomain()
		{
			System.Web.HttpRuntime.UnloadAppDomain();
		}

		internal IInstallerRepository GetRepository(string databaseName, string connectionString)
		{
			if (databaseName == SupportedDatabases.MongoDB)
			{
				return new MongoDbInstallerRepository(connectionString);
			}
			else if (databaseName == SupportedDatabases.MySQL)
			{
				return new LightSpeedInstallerRepository(DataProvider.MySql5, new MySqlSchema(), connectionString);
			}
			else if (databaseName == SupportedDatabases.Postgres)
			{
				return new LightSpeedInstallerRepository(DataProvider.PostgreSql9, new PostgresSchema(), connectionString);
			}
			else
			{
				return new LightSpeedInstallerRepository(DataProvider.SqlServer2008, new SqlServerSchema(), connectionString);
			}
		}
	}
}
