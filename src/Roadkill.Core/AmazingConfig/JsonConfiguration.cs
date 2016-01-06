using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Roadkill.Core.Configuration;
using Roadkill.Core.Database;
using Roadkill.Core.Security;

namespace Roadkill.Core.AmazingConfig
{
	public class JsonConfiguration : IConfiguration
	{
		#region Required
		[JsonIgnore]
		public NonConfigurableSettings NonConfigurableSettings { get; set; }
		public Dictionary<string, string> Settings { get; set; }

		public string AdminRoleName { get; set; }
		public string ConnectionString { get; set; }
		public string EditorRoleName { get; set; }
		public bool Installed { get; set; }
		public bool UseWindowsAuthentication { get; set; }

		public string AttachmentsFolder { get; set; }
		public string AttachmentsRoutePath { get; set; }
		public string ApiKeys { get; set; }

		[JsonIgnore]
		public IEnumerable<string> ApiKeysList { get; private set; }

		[JsonIgnore]
		public bool IsRestApiEnabled
		{
			get
			{
				return ApiKeysList != null && ApiKeysList.Any();
			}
		}
		#endregion

		#region Optional
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public bool? IgnoreSearchIndexErrors { get; set; }

		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public string DatabaseProvider { get; set; }

		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public bool? IsPublicSite { get; set; }

		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public string LdapConnectionString { get; set; }

		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public string LdapUsername { get; set; }

		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public string LdapPassword { get; set; }

		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public bool? UseHtmlWhiteList { get; set; }

		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public bool? UseObjectCache { get; set; }

		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public bool? UseBrowserCache { get; set; }

		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public string UserServiceType { get; set; }
		#endregion

		#region Built in Preferences
		public string AllowedFileTypes { get; set; }
		public bool AllowUserSignup { get; set; }
		public bool IsRecaptchaEnabled { get; set; }
		public string MarkupType { get; set; }
		public string RecaptchaPrivateKey { get; set; }
		public string RecaptchaPublicKey { get; set; }
		public string SiteUrl { get; set; }
		public string SiteName { get; set; }
		public string Theme { get; set; }
		public bool OverwriteExistingFiles { get; set; }
		public string HeadContent { get; set; }
		public string MenuMarkup { get; set; }
		public DateTime PluginLastSaveDate { get; set; }

		public string AzureConnectionString { get; set; }
		public string AzureContainer { get; set; }

		[JsonIgnore]
		public bool UseAzureFileStorage
		{
			get { return !string.IsNullOrEmpty(AzureConnectionString); }
		}

		[JsonIgnore]
		public string ThemePath
		{
			get
			{
				return string.Format("~/Themes/{0}", Theme);
			}
		}

		[JsonIgnore]
		public List<string> AllowedFileTypesList
		{
			get
			{
				if (string.IsNullOrEmpty(AllowedFileTypes))
					return new List<string>();

				return new List<string>(AllowedFileTypes.Replace(" ", "").Split(','));
			}
		}
		#endregion

		public JsonConfiguration()
		{
			NonConfigurableSettings = new NonConfigurableSettings();
			Settings = new Dictionary<string, string>();

			ApiKeysList = new List<string>();
			AttachmentsRoutePath = "Attachments";
			AttachmentsFolder = "~/App_Data/Attachments";
			DatabaseProvider = SupportedDatabases.SqlServer2008.Id;

			// Optional
			IgnoreSearchIndexErrors = true;
			IsPublicSite = true;
			UseBrowserCache = true;
			UseHtmlWhiteList = true;
			UserServiceType = "";
			LdapConnectionString = "";
			LdapUsername = "";
			LdapPassword = "";

			// Preferences
			AllowedFileTypes = "jpg, png, gif";
			AllowUserSignup = false;
			IsRecaptchaEnabled = false;
			Theme = "Mediawiki";
			MarkupType = "Creole";
			SiteName = "Your site";
			SiteUrl = "";
			RecaptchaPrivateKey = "";
			RecaptchaPublicKey = "";
			OverwriteExistingFiles = false;
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
