using System.Collections.Generic;
using Newtonsoft.Json;

namespace Roadkill.Core.AmazingConfig
{
	/// <summary>
	/// 
	/// </summary>
	public class JsonPluginConfiguration : IPluginConfiguration
	{
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public IList<PluginSettings> PluginSettings { get; set; }

		public JsonPluginConfiguration()
		{
			PluginSettings = new List<PluginSettings>();
		}
	}
}