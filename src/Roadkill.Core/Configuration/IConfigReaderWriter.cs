using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Configuration;
using System.Xml.Linq;
using Roadkill.Core.Mvc.ViewModels;

namespace Roadkill.Core.Configuration
{
	/// <summary>
	/// Defines a class responsible for reading and writing application settings/configuration.
	/// </summary>
	public interface IConfigReaderWriter
	{
		/// <summary>
		/// Updates the current UI language in the globalization section and saves the configuration file.
		/// </summary>
		/// <param name="uiLanguageCode">The UI language code, e.g. fr for French.</param>
		/// <exception cref="T:Roadkill.Core.ConfigurationException">An exception occurred while updating the UI language.</exception>
		void UpdateLanguage(string uiLanguageCode);

		/// <summary>
		/// Saves the configuration settings. This will save a subset of the <see cref="T:Roadkill.Core.Mvc.ViewModels.SettingsViewModel" /> based on 
		/// the values that match those found in the <see cref="T:Roadkill.Core.RoadkillSection" />
		/// </summary>
		/// <param name="settings">The application settings.</param>
		/// <exception cref="T:Roadkill.Core.InstallerException">An exception occurred while updating the settings.</exception>
		void Save(SettingsViewModel settings);

		/// <summary>
		/// Loads the Roadkill-specific configuration settings.
		/// </summary>
		/// <returns>A <see cref="T:Roadkill.Core.RoadkillSection" /> instance with the settings.</returns>
		IRoadkillConfiguration Load();

		/// <summary>
		/// Gets the current application settings, which is usually cached settings from the <see cref="M:Roadkill.Core.Configuration.ConfigReaderWriter.Load" /> method.
		/// </summary>
		/// <returns>A new <see cref="T:Roadkill.Core.Configuration.Settings" /> instance</returns>
		ApplicationSettings GetApplicationSettings();

		/// <summary>
		/// Resets the state the configuration file/store so the 'installed' property is false.
		/// </summary>
		/// <exception cref="T:Roadkill.Core.InstallerException">An web.config related error occurred while reseting the install state.</exception>
		void ResetInstalledState();

		/// <summary>
		/// Tests the app.config or web.config file to ensure that it can be written to.
		/// </summary>
		/// <returns>An empty string if no error occurred; otherwise the error message.</returns>
		string TestSaveWebConfig();
	}
}
