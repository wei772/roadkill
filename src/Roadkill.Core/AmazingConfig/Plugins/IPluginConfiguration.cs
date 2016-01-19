using System;
using System.Collections.Generic;

namespace Roadkill.Core.AmazingConfig
{
	public interface IPluginConfiguration
	{
		IList<TextPluginSettings> TextPluginSettings { get; set; }
	}
}
