namespace Roadkill.Core.AmazingConfig
{
	public interface IConfigurationStore
	{
		IConfiguration Load();
		IPluginConfiguration LoadPluginConfiguration();
		void Save(IConfiguration configuration);
		void SavePluginConfiguration(IPluginConfiguration configuration);
	}
}
