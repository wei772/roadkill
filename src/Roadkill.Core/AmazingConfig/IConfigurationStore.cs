using Roadkill.Core.Logging;

namespace Roadkill.Core.AmazingConfig
{
	public interface IConfigurationStore
	{
		IConfiguration Load();
		void Save(IConfiguration configuration);
	}
}
