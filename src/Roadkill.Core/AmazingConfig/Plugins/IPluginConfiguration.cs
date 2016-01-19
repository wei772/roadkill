using System;
using System.Collections.Generic;

namespace Roadkill.Core.AmazingConfig
{
	public interface IPluginConfiguration
	{
		IList<PluginSettings> PluginSettings { get; set; }
	}
}
