using System.Collections.Generic;

namespace Roadkill.Core.AmazingConfig
{
	/// <summary>
	/// Settings for an individual plugin.
	/// </summary>
	public class PluginSettings
	{
		/// <summary>
		/// A unique id for the plugin, usually just the name i.e. "ResizeImages"
		/// </summary>
		public string Id { get; set; }
		public bool IsEnabled { get; set; }

		/// <summary>
		/// Additional custom settings for the plugin.
		/// </summary>
		public Dictionary<string, string> Settings { get; set; }

		public PluginSettings()
		{
			Settings = new Dictionary<string, string>();
		}
	}
}