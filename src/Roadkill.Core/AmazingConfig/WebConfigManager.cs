using System;
using System.Configuration;
using System.IO;
using System.Web.Configuration;

namespace Roadkill.Core.AmazingConfig
{
	/// <summary>
	/// Reads and write the application configuration settings, from a web.config or app.config file.
	/// </summary>
	public class WebConfigManager : IWebConfigManager
	{
		internal Configuration Configuration { get; set; }
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
				Configuration = WebConfigurationManager.OpenWebConfiguration("~");
			}
			else
			{
				if (configFilePath.ToLower() == "app.config")
				{
					Configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
				}
				else
				{
					if (!File.Exists(configFilePath))
						throw new FileNotFoundException(string.Format("The config file {0} could not be found", configFilePath));

					ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();
					fileMap.ExeConfigFilename = configFilePath;
					Configuration = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
				}
			}

			ConfigFilePath = configFilePath;
		}

		public virtual void UpdateLanguage(string uiLanguageCode)
		{
			try
			{
				GlobalizationSection globalizationSection = Configuration.GetSection("system.web/globalization") as GlobalizationSection;
				globalizationSection.UICulture = uiLanguageCode;
				Configuration.Save(ConfigurationSaveMode.Minimal);
			}
			catch (ConfigurationErrorsException ex)
			{
				throw new ConfigurationException("An exception occurred while updating the UI language in the web.config", ex);
			}
		}

		/// <summary>
		/// Adds config settings for forms authentication.
		/// </summary>
		public void WriteForFormsAuth()
		{
			// TODO: Tests
			try
			{
				// Turn on forms authentication
				AuthenticationSection authSection = Configuration.GetSection("system.web/authentication") as AuthenticationSection;
				authSection.Mode = AuthenticationMode.Forms;
				authSection.Forms.LoginUrl = "~/User/Login";

				// Turn on anonymous auth
				AnonymousIdentificationSection anonSection = Configuration.GetSection("system.web/anonymousIdentification") as AnonymousIdentificationSection;
				anonSection.Enabled = true;

				Configuration.Save(ConfigurationSaveMode.Minimal);
			}
			catch (ConfigurationErrorsException ex)
			{
				throw new InstallerException(ex, "An exception occurred while updating the settings to the web.config");
			}
		}

		/// <summary>
		/// Adds web.config settings for windows authentication.
		/// </summary>
		public void WriteForWindowsAuth()
		{
			try
			{
				// Turn on Windows authentication
				AuthenticationSection authSection = Configuration.GetSection("system.web/authentication") as AuthenticationSection;
				authSection.Mode = AuthenticationMode.Windows;

				// Turn off anonymous auth
				AnonymousIdentificationSection anonSection = Configuration.GetSection("system.web/anonymousIdentification") as AnonymousIdentificationSection;
				anonSection.Enabled = false;

				Configuration.Save(ConfigurationSaveMode.Minimal);
			}
			catch (ConfigurationErrorsException ex)
			{
				throw new InstallerException(ex, "An exception occurred while updating the settings to the web.config");
			}
		}

		public string IsWriteable()
		{
			try
			{
				Configuration.Save(ConfigurationSaveMode.Minimal);
				return "";
			}
			catch (Exception e)
			{
				return e.ToString();
			}
		}
	}
}
