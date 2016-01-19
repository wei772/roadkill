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
		public IList<TextPluginSettings> TextPluginSettings { get; set; }

		public JsonPluginConfiguration()
		{
			TextPluginSettings = new List<TextPluginSettings>();
		}
	}
}