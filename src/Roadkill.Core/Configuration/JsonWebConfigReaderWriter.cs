using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Roadkill.Core.Mvc.ViewModels;

namespace Roadkill.Core.Configuration
{
	public class JsonWebConfigReaderWriter : IWebConfigReaderWriter
	{
		private readonly string _configFilePath;
		private IRoadkillConfiguration _configuration;

		/// <summary>
		/// Initializes a new instance of the <see cref="JsonWebConfigReaderWriter"/> class.
		/// The class will attempt to load roadkill.json file when the class 
		/// is instantiated, and cache it for the lifetime of the <see cref="JsonWebConfigReaderWriter"/> instance,
		/// using ~/App_Data/roadkill.json as the file path.
		/// </summary>
		public JsonWebConfigReaderWriter() : this("~/App_Data/roadkill.json")
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="JsonWebConfigReaderWriter"/> class.
		/// The class will attempt to load roadkill.json file when the class 
		/// is instantiated, and cache it for the lifetime of the <see cref="JsonWebConfigReaderWriter"/> instance.
		/// </summary>
		/// <param name="configFilePath">The full path to the JSON configuration file.</param>
		public JsonWebConfigReaderWriter(string configFilePath)
		{
			_configFilePath = configFilePath;

			try
			{
				_configuration = JsonConvert.DeserializeObject<IRoadkillConfiguration>(_configFilePath);
			}
			catch (Exception e)
			{
				throw new ConfigurationException(e, "An error occurred loading the config file '{0}'", _configFilePath);
			}
		}

		public IRoadkillConfiguration Load()
		{
			return _configuration;
		}

		public void UpdateLanguage(string uiLanguageCode)
		{
			// Use the old ASP.NET way for now, until ASP.NET 5 changes this.
			var webconfigReader = new FullTrustWebConfigReaderWriter();
			webconfigReader.UpdateLanguage(uiLanguageCode);
		}

		public void Save(SettingsViewModel settings)
		{
		}

		public ApplicationSettings GetApplicationSettings()
		{
			var converter = new RoadkillConfigurationConverter();
			return converter.ToApplicationSettings(_configuration);
		}

		public void ResetInstalledState()
		{
		}

		public string TestSaveWebConfig()
		{
			try
			{
				ResetInstalledState();
				return "";
			}
			catch (Exception e)
			{
				return e.ToString();
			}
		}
	}
}
