using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Roadkill.Core.Database;

namespace Roadkill.Core.AmazingConfig
{
	public class JsonConfiguration : IConfiguration
	{
		#region Required
		public bool Installed { get; set; }
		public string ConnectionString { get; set; }

		[JsonIgnore]
		public InternalSettings InternalSettings { get; set; }

		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public SecuritySettings SecuritySettings { get; set; }

		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public AttachmentSettings AttachmentSettings { get; set; }

		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public Dictionary<string, string> Settings { get; set; }
		#endregion

		#region Optional
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public bool? IgnoreSearchIndexErrors { get; set; }

		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public string DatabaseProvider { get; set; }

		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public bool? IsPublicSite { get; set; }

		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public bool? UseHtmlWhiteList { get; set; }

		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public bool? UseObjectCache { get; set; }

		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public bool? UseBrowserCache { get; set; }
		#endregion

		#region Built in Preferences
		public bool AllowUserSignup { get; set; }
		public bool IsRecaptchaEnabled { get; set; }
		public string MarkupType { get; set; }
		public string RecaptchaPrivateKey { get; set; }
		public string RecaptchaPublicKey { get; set; }
		public string SiteUrl { get; set; }
		public string SiteName { get; set; }
		public string Theme { get; set; }
		public string HeadContent { get; set; }
		public string MenuMarkup { get; set; }
		public DateTime PluginLastSaveDate { get; set; }

		[JsonIgnore]
		public string ThemePath
		{
			get
			{
				return string.Format("~/Themes/{0}", Theme);
			}
		}
		#endregion

		public JsonConfiguration()
		{
			InternalSettings = new InternalSettings();
			AttachmentSettings = new AttachmentSettings();;
			SecuritySettings = new SecuritySettings();
			Settings = new Dictionary<string, string>();

			// Required
			ConnectionString = "";
			Installed = false;
			DatabaseProvider = SupportedDatabases.SqlServer2008.Id;

			// Optional
			IgnoreSearchIndexErrors = true;
			IsPublicSite = true;
			UseBrowserCache = true;
			UseHtmlWhiteList = true;

			// Preferences
			AllowUserSignup = false;
			IsRecaptchaEnabled = false;
			Theme = "Mediawiki";
			MarkupType = "Markdown";
			SiteName = "Your site";
			SiteUrl = "";
			RecaptchaPrivateKey = "";
			RecaptchaPublicKey = "";
			HeadContent = "";
			MenuMarkup = GetDefaultMenuMarkup();
			PluginLastSaveDate = DateTime.UtcNow;
		}

		internal string GetDefaultMenuMarkup()
		{
			return "* %mainpage%\r\n" +
					"* %categories%\r\n" +
					"* %allpages%\r\n" +
					"* %newpage%\r\n" +
					"* %managefiles%\r\n" +
					"* %sitesettings%\r\n\r\n";
		}
	}
}
