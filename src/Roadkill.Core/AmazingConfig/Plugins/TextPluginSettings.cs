using System.Collections.Generic;

namespace Roadkill.Core.AmazingConfig
{
	/// <summary>
	/// Settings for an individual TextPlugin.
	/// </summary>
	public class TextPluginSettings
	{
		/// <summary>
		/// A unique id for the plugin, usually just the name i.e. "ResizeImages"
		/// </summary>
		public string Id { get; set; }

		/// <summary>
		/// The full type name of the plugin, in short format or long format (long format is prefered for setting upgrades):
		/// - MyNamespace.MyPlugin, MyPlugin
		/// - MyNamespace.MyPlugin, MyPlugin, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31BF3856AD364E35
		/// </summary>
		public string TypeName { get; set; }

		/// <summary>
		/// Whether the plugin is enabled in the system.
		/// </summary>
		public bool IsEnabled { get; set; }

		/// <summary>
		/// Additional custom settings for the plugin.
		/// </summary>
		public List<SettingValue> Settings { get; set; }

		public TextPluginSettings()
		{
			Settings = new List<SettingValue>();
		}
	}
}