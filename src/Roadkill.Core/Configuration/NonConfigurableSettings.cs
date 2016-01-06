using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Web;
using Newtonsoft.Json;
using Roadkill.Core.Database;

namespace Roadkill.Core.Configuration
{
	public class NonConfigurableSettings
	{
		/// <summary>
		/// The path to the App_Data folder.
		/// </summary>
		public string AppDataPath { get; set; }

		/// <summary>
		/// The path to the App_Data/Internal folder (used by roadkill only, no user files are stored here).
		/// </summary>
		public string AppDataInternalPath { get; private set; }

		/// <summary>
		/// The file path for the custom tokens file.
		/// </summary>
		public string CustomTokensPath { get; set; }

		/// <summary>
		/// The path to the email templates folder, ~/App_Data/EmailTemplates/ by default.
		/// </summary>
		public string EmailTemplateFolder { get; set; }

		/// <summary>
		/// The file path for the html element white list file.
		/// </summary>
		public string HtmlElementWhiteListPath { get; set; }

		/// <summary>
		/// The number of characters each password should be.
		/// </summary>
		public int MinimumPasswordLength { get; set; }

		/// <summary>
		/// The full path to the nlog.config file - this defaults to ~/App_Data/NLog.Config (the ~ is replaced with the base web directory).
		/// </summary>
		public string NLogConfigFilePath { get; set; }

		/// <summary>
		/// The full path to the text plugins directory. This is where plugins are stored after 
		/// download (including their nuget files), and are copied to the bin folder.
		/// </summary>
		public string PluginsPath { get; internal set; }

		/// <summary>
		/// The directory within the /bin folder that the plugins are stored. They are 
		/// copied here on application start, so they can be loaded into the application domain with shadow 
		/// copy support and also monitored by the ASP.NET file watcher.
		/// </summary>
		public string PluginsBinPath { get; internal set; }

		/// <summary>
		/// The path to the folder that contains the Lucene index - ~/App_Data/Internal/Search.
		/// </summary>
		public string SearchIndexPath { get; set; }

		public static bool IsDemoSite
		{
			get
			{
				return ConfigurationManager.AppSettings["DemoSite"] == "true";
			}
		}

		/// <summary>
		/// The human-friendly current Roadkill product version, e.g. "1.7.0-Beta3".
		/// </summary>
		public static string ProductVersion
		{
			get
			{
				return FileVersionInfo.GetVersionInfo(typeof(ApplicationSettings).Assembly.Location).ProductVersion;
			}
		}

		/// <summary>
		/// The file version of the Roadkill product version, e.g. "1.7.0.0"
		/// </summary>
		public static string FileVersion
		{
			get
			{
				return FileVersionInfo.GetVersionInfo(typeof(ApplicationSettings).Assembly.Location).FileVersion;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ApplicationSettings"/> class.
		/// </summary>
		public NonConfigurableSettings()
		{
			AppDataPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data");
			AppDataInternalPath = Path.Combine(AppDataPath, "Internal");
			CustomTokensPath = Path.Combine(AppDataPath, "customvariables.xml");
			EmailTemplateFolder = Path.Combine(AppDataPath, "EmailTemplates");
			HtmlElementWhiteListPath = Path.Combine(AppDataInternalPath, "htmlwhitelist.xml");
			MinimumPasswordLength = 6;
			NLogConfigFilePath = "~/App_Data/NLog.config";
			PluginsBinPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin", "Plugins");
			PluginsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Plugins");
			SearchIndexPath = Path.Combine(AppDataInternalPath, "Search");
		}
	}
}