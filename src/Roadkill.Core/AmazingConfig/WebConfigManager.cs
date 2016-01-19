using System;
using System.Configuration;
using System.IO;
using System.Web.Configuration;
using Roadkill.Core.Mvc.ViewModels;

namespace Roadkill.Core.AmazingConfig
{
	/// <summary>
	/// Reads and write the application configuration settings, from a web.config or app.config file.
	/// </summary>
	public class WebConfigManager : IWebConfigManager
	{
		private readonly Configuration _config;

		public string ConfigFilePath { get;set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="WebConfigManager"/> class.
		/// The class will attempt to load the Roadkill section from the web.config when the class 
		/// is instantiated, and cache it for the lifetime of the <see cref="WebConfigManager"/> instance.
		/// </summary>
		public WebConfigManager()
			: this("")
		{
		}

		public WebConfigManager(string configFilePath)
		{
			if (string.IsNullOrEmpty(configFilePath))
			{
				_config = WebConfigurationManager.OpenWebConfiguration("~");
			}
			else
			{
				if (!File.Exists(configFilePath))
					throw new ConfigurationException(null, "The config file {0} could not be found", configFilePath);

				if (configFilePath.ToLower() == "app.config")
				{
					_config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
				}
				else
				{
					if (!File.Exists(configFilePath))
						throw new FileNotFoundException(string.Format("The config file {0} could not be found", configFilePath));

					ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();
					fileMap.ExeConfigFilename = configFilePath;
					_config = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
				}
			}

			ConfigFilePath = configFilePath;
		}

		public virtual void UpdateLanguage(string uiLanguageCode)
		{
			try
			{
				GlobalizationSection globalizationSection = _config.GetSection("system.web/globalization") as GlobalizationSection;
				globalizationSection.UICulture = uiLanguageCode;
				_config.Save(ConfigurationSaveMode.Minimal);
			}
			catch (ConfigurationErrorsException ex)
			{
				throw new ConfigurationException("An exception occurred while updating the UI language in the web.config", ex);
			}
		}

		public virtual void Save(SettingsViewModel settings)
		{
			try
			{
				if (settings.UseWindowsAuth)
					WriteConfigForWindowsAuth();
				else
					WriteConfigForFormsAuth();

				_config.Save(ConfigurationSaveMode.Minimal);
			}
			catch (ConfigurationErrorsException ex)
			{
				throw new InstallerException(ex, "An exception occurred while updating the settings to the web.config");
			}
		}

		/// <summary>
		/// Adds config settings for forms authentication.
		/// </summary>
		internal void WriteConfigForFormsAuth()
		{
			// Turn on forms authentication
			AuthenticationSection authSection = _config.GetSection("system.web/authentication") as AuthenticationSection;
			authSection.Mode = AuthenticationMode.Forms;
			authSection.Forms.LoginUrl = "~/User/Login";

			// Turn on anonymous auth
			AnonymousIdentificationSection anonSection = _config.GetSection("system.web/anonymousIdentification") as AnonymousIdentificationSection;
			anonSection.Enabled = true;
		}

		/// <summary>
		/// Adds web.config settings for windows authentication.
		/// </summary>
		internal void WriteConfigForWindowsAuth()
		{
			// Turn on Windows authentication
			AuthenticationSection authSection = _config.GetSection("system.web/authentication") as AuthenticationSection;
			authSection.Mode = AuthenticationMode.Windows;

			// Turn off anonymous auth
			AnonymousIdentificationSection anonSection = _config.GetSection("system.web/anonymousIdentification") as AnonymousIdentificationSection;
			anonSection.Enabled = false;
		}
	}
}
