using System.IO;
using System.Web;
using Newtonsoft.Json;

namespace Roadkill.Core.AmazingConfig
{
	public class JsonConfigurationStore : IConfigurationStore
	{
		private string _configPath;
		private string _pluginConfigPath;
		private IConfiguration _cachedConfiguration;
		private IPluginConfiguration _cachedPluginConfiguration;

		public JsonConfigurationStore() : this("~/App_Data/configuration.json", "~/App_Data/plugins.json")
		{
		}

		public JsonConfigurationStore(string configPath, string pluginConfigPath)
		{
			_configPath = configPath;
			_pluginConfigPath = pluginConfigPath;

			ResolveConfigPaths();
		}

		private void ResolveConfigPaths()
		{
			HttpContextWrapper httpContext = null;

			if (HttpContext.Current != null)
				httpContext = new HttpContextWrapper(HttpContext.Current);

			if (_configPath.StartsWith("~") && httpContext != null)
			{
				_configPath = httpContext.Server.MapPath(_configPath);
			}

			if (_pluginConfigPath.StartsWith("~") && httpContext != null)
			{
				_pluginConfigPath = httpContext.Server.MapPath(_pluginConfigPath);
			}
		}

		public IConfiguration Load()
		{
			if (_cachedConfiguration == null)
			{
				string json = File.ReadAllText(_configPath);
				JsonConfiguration configuration = JsonConvert.DeserializeObject<JsonConfiguration>(json);

				if (configuration.Installed && string.IsNullOrEmpty(configuration.ConnectionString))
					throw new ConfigurationException(null, "The roadkill connection string setting is empty in '{0}'.", _configPath);

				_cachedConfiguration = configuration;
			}

			return _cachedConfiguration;
		}

		public IPluginConfiguration LoadPluginConfiguration()
		{
			if (_cachedPluginConfiguration == null)
			{
				string json = File.ReadAllText(_pluginConfigPath);
				JsonPluginConfiguration pluginConfiguration = JsonConvert.DeserializeObject<JsonPluginConfiguration>(json);

				_cachedPluginConfiguration = pluginConfiguration;
			}

			return _cachedPluginConfiguration;
		}

		public void Save(IConfiguration configuration)
		{
			string json = JsonConvert.SerializeObject(configuration, Formatting.Indented);
			File.WriteAllText(_configPath, json);

			_cachedConfiguration = null;
		}

		public void SavePluginConfiguration(IPluginConfiguration configuration)
		{
			string json = JsonConvert.SerializeObject(configuration, Formatting.Indented);
			File.WriteAllText(_pluginConfigPath, json);

			_cachedPluginConfiguration = null;
		}
	}
}