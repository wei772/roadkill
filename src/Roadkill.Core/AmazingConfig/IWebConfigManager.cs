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

		void WriteForFormsAuth();
		void WriteForWindowsAuth();

		string IsWriteable();
	}
}