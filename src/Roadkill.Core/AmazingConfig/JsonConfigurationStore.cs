using System.IO;
using System.Web;
using Newtonsoft.Json;

namespace Roadkill.Core.AmazingConfig
{
	public class JsonConfigurationStore : IConfigurationStore
	{
		private string _configPath;
		private IConfiguration _cachedConfiguration;

		public JsonConfigurationStore() : this("~/App_Data/configuration.json")
		{
		}

		public JsonConfigurationStore(string configPath)
		{
			_configPath = configPath;
			ResolveConfigPath();
		}

		private void ResolveConfigPath()
		{
			HttpContextWrapper httpContext = null;

			if (HttpContext.Current != null)
				httpContext = new HttpContextWrapper(HttpContext.Current);

			if (_configPath.StartsWith("~") && httpContext != null)
			{
				_configPath = httpContext.Server.MapPath(_configPath);
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

		public void Save(IConfiguration configuration)
		{
			string json = JsonConvert.SerializeObject(configuration);
			File.WriteAllText(_configPath, json);

			_cachedConfiguration = null;
		}
	}
}