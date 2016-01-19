using Roadkill.Core.Database;
using Roadkill.Core.Localization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Web;
using System.Web.Mvc;
using Roadkill.Core.AmazingConfig;

namespace Roadkill.Core.Mvc.ViewModels
{
	/// <summary>
	/// Represents settings for the site, some of which are stored in the web.config.
	/// </summary>
	[Serializable]
	public class SettingsViewModel
	{
		private static string _themesRoot;
		private List<SelectListItem> _supportedDatabasesSelectList;

		[Required(ErrorMessageResourceType = typeof(SiteStrings), ErrorMessageResourceName = "SiteSettings_Validation_MarkupTypeEmpty")]
		public string MarkupType { get; set; }

		[Required(ErrorMessageResourceType = typeof(SiteStrings), ErrorMessageResourceName = "SiteSettings_Validation_SiteNameEmpty")]
		public string SiteName { get; set; }

		[Required(ErrorMessageResourceType = typeof(SiteStrings), ErrorMessageResourceName = "SiteSettings_Validation_SiteUrlEmpty")]
		public string SiteUrl { get; set; }

		[Required(ErrorMessageResourceType = typeof(SiteStrings), ErrorMessageResourceName = "SiteSettings_Validation_AttachmentsEmpty")]
		[RegularExpression(@"^[^/Files].*", ErrorMessageResourceType = typeof(SiteStrings), ErrorMessageResourceName = "SiteSettings_Validation_AttachmentsReservedName")]
		public string AttachmentsFolder { get; set; }

		public string AzureConnectionString { get; set; }

		public string AzureContainer { get; set; }

		[Required(ErrorMessageResourceType = typeof(SiteStrings), ErrorMessageResourceName = "SiteSettings_Validation_ConnectionEmpty")]
		public string ConnectionString { get; set; }

		[Required(ErrorMessageResourceType = typeof(SiteStrings), ErrorMessageResourceName = "SiteSettings_Validation_ThemeEmpty")]
		public string Theme { get; set; }

		public string AdminEmail { get; set; }
		public string AdminPassword { get; set; }
		public string AdminRoleName { get; set; }
		public string AllowedFileTypes { get; set; }
		public bool AllowUserSignup { get; set; }
		public string AttachmentsDirectoryPath { get; set; }
		public bool UseObjectCache { get; set; }
		public bool UseBrowserCache { get; set; }
		public string DatabaseProvider { get; set; }
		public string EditorRoleName { get; set; }
		public bool IsRecaptchaEnabled { get; set; }
		public string LdapConnectionString { get; set; }
		public string LdapUsername { get; set; }
		public string LdapPassword { get; set; }
		public string RecaptchaPrivateKey { get; set; }
		public string RecaptchaPublicKey { get; set; }
		public bool UseAzureFileStorage { get; set; }
		public bool UseWindowsAuth { get; set; }
		
		// v2.0
		public bool OverwriteExistingFiles { get; set; }
		public string HeadContent { get; set; }
		public string MenuMarkup { get; set; }

		public bool IsPublicSite { get; set; }
		public bool IgnoreSearchIndexErrors { get; set; }

		/// <summary>
		/// True when the model was updated during postback
		/// </summary>
		public bool UpdateSuccessful { get; set; }


		/// <summary>
		/// Gets an IEnumerable{SelectListItem} from a the SettingsViewModel.DatabaseTypesAvailable, as a default
		/// SelectList doesn't add option value attributes.
		/// </summary>
		public List<SelectListItem> DatabaseTypesAsSelectList
		{
			get
			{
				return _supportedDatabasesSelectList;
			}
		}

		public IEnumerable<string> MarkupTypesAvailable
		{
			get
			{
				return new string[] { "Markdown" };
			}
		}

		public IEnumerable<string> ThemesAvailable
		{
			get
			{
				if (string.IsNullOrEmpty(_themesRoot))
				{
					_themesRoot = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Themes");
					if (!Directory.Exists(_themesRoot))
						throw new InvalidOperationException("The Themes directory could not be found");
				}

				foreach (string directory in Directory.GetDirectories(_themesRoot))
				{
					yield return new DirectoryInfo(directory).Name;
				}
			}
		}

		public string Version
		{
			get
			{
				return InternalSettings.ProductVersion;
			}
		}

		public SettingsViewModel()
		{
			if (HttpContext.Current != null)
			{
				// Default the site's url using the current request
				Uri uri = HttpContext.Current.Request.Url;

				string port = "";
				if (uri.Port != 80 && uri.Port != 443)
					port = ":" + uri.Port;

				SiteUrl = string.Format("{0}://{1}{2}", uri.Scheme, uri.Host, port);
			}
			else
			{
				SiteUrl = "http://localhost";
			}
		}

		/// <summary>
		/// Fills this instance of SettingsViewModel using the properties from the Settings 
		/// and the SiteSettings.
		/// </summary>
		public SettingsViewModel(IConfigurationStore configurationStore) : this()
		{
			IConfiguration configuration = configurationStore.Load();

			// Settings
			FillFromApplicationSettings(configuration);

			// SiteSettings
			AllowedFileTypes = string.Join(",", configuration.AttachmentSettings.AllowedFileTypesList);
			AllowUserSignup = configuration.SecuritySettings.AllowUserSignup;
			IsRecaptchaEnabled = configuration.SecuritySettings.IsRecaptchaEnabled;
			MarkupType = configuration.MarkupType;
			RecaptchaPrivateKey = configuration.SecuritySettings.RecaptchaPrivateKey;
			RecaptchaPublicKey = configuration.SecuritySettings.RecaptchaPublicKey;
			SiteName = configuration.SiteName;
			SiteUrl = configuration.SiteUrl;
			Theme = configuration.Theme;
			OverwriteExistingFiles = configuration.AttachmentSettings.OverwriteExistingFiles;
			HeadContent = configuration.HeadContent;
			MenuMarkup = configuration.MenuMarkup;
		}

		public void FillFromApplicationSettings(IConfiguration configuration)
		{
			AdminRoleName = configuration.SecuritySettings.AdminRoleName;
			AttachmentsFolder = configuration.AttachmentSettings.AttachmentsFolder;
			AttachmentsDirectoryPath = configuration.AttachmentSettings.GetAttachmentsDirectoryPath();
			ConnectionString = configuration.ConnectionString;
			DatabaseProvider = configuration.DatabaseProvider;
			EditorRoleName = configuration.SecuritySettings.EditorRoleName;
			IsPublicSite = configuration.IsPublicSite.GetValueOrDefault();
			IgnoreSearchIndexErrors = configuration.IgnoreSearchIndexErrors.GetValueOrDefault();
			LdapConnectionString = configuration.SecuritySettings.LdapConnectionString;
			LdapUsername = configuration.SecuritySettings.LdapUsername;
			LdapPassword = configuration.SecuritySettings.LdapPassword;
			UseWindowsAuth = configuration.SecuritySettings.UseWindowsAuthentication;
			UseObjectCache = configuration.UseObjectCache.GetValueOrDefault();
			UseBrowserCache = configuration.UseBrowserCache.GetValueOrDefault();
		}

		public void SetSupportedDatabases(IEnumerable<RepositoryInfo> repositoryInfos)
		{
			_supportedDatabasesSelectList = new List<SelectListItem>();

			foreach (RepositoryInfo info in repositoryInfos)
			{
				SelectListItem item = new SelectListItem();
				item.Value = info.Id;
				item.Text = info.Description;

				if (item.Value == DatabaseProvider)
					item.Selected = true;

				_supportedDatabasesSelectList.Add(item);
			}
		}
	}
}
