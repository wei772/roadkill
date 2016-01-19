using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Roadkill.Core.AmazingConfig;

namespace Roadkill.Tests.Unit.StubsAndMocks
{
	public class ConfigurationStoreMock : IConfigurationStore
	{
		public IConfiguration Configuration { get; set; }
		public IPluginConfiguration PluginConfiguration { get; set; }

		public ConfigurationStoreMock()
		{
			Configuration = new JsonConfiguration();
		}

		public IConfiguration Load()
		{
			return Configuration;
		}

		public IPluginConfiguration LoadPluginConfiguration()
		{
			return PluginConfiguration;
		}

		public void Save(IConfiguration configuration)
		{
		}

		public void SavePluginConfiguration(IPluginConfiguration configuration)
		{
		}
	}
}
