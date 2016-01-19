using Roadkill.Core.Mvc.ViewModels;

namespace Roadkill.Core.AmazingConfig
{
	public interface IWebConfigManager
	{
		string ConfigFilePath { get; set; }

		/// <summary>
		/// Updates the current UI language in the globalization section and saves the configuration file.
		/// </summary>
		/// <param name="uiLanguageCode">The UI language code, e.g. fr for French.</param>
		/// <exception cref="System.Configuration.ConfigurationException">An exception occurred while updating the UI language in the web.config</exception>
		void UpdateLanguage(string uiLanguageCode);

		/// <summary>
		/// Saves the configuration settings. This will save a subset of the <see cref="SettingsViewModel" /> based on
		/// the values that match those found in the <see cref="RoadkillSection" />
		/// </summary>
		/// <param name="settings">The application settings.</param>
		/// <exception cref="InstallerException">An exception occurred while updating the settings to the web.config</exception>
		void Save(SettingsViewModel settings);
	}
}