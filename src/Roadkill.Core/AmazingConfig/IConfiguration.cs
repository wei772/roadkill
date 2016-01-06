using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Newtonsoft.Json;
using Roadkill.Core.Configuration;
using Roadkill.Core.Security;

namespace Roadkill.Core.AmazingConfig
{
	public interface IConfiguration
	{
		#region Required settings
		NonConfigurableSettings NonConfigurableSettings { get; set; }
		Dictionary<string,string> Settings { get; set; }

		/// <summary>
		/// Gets or sets the name of the admin role.
		/// </summary>
		string AdminRoleName { get; set; }

		/// <summary>
		/// Gets or sets the api keys (comma seperated) used for access to the REST api. If this is empty, then the REST api is disabled.
		/// </summary>
		string ApiKeys { get; set; }

		/// <summary>
		/// Contains a list API keys for the REST api. If this is empty, then the REST api is disabled.
		/// </summary>
		IEnumerable<string> ApiKeysList { get; }

		/// <summary>
		/// Whether the REST api is available - if api keys are set in the config.
		/// </summary>
		bool IsRestApiEnabled { get; }

		/// <summary>
		/// The database connection string.
		/// </summary>
		string ConnectionString { get; set; }

		/// <summary>
		/// Gets or sets the name of the editor role.
		/// </summary>
		string EditorRoleName { get; set; }

		/// <summary>
		/// Gets or sets whether this roadkill instance has been installed.
		/// </summary>
		bool Installed { get; set; }

		/// <summary>
		/// Whether to enabled Windows and Active Directory authentication.
		/// </summary>
		bool UseWindowsAuthentication { get; set; }
		#endregion

		#region Optional properties
		/// <summary>
		/// Whether errors in updating the lucene index throw exceptions or are just ignored.
		/// </summary>
		bool? IgnoreSearchIndexErrors { get; set; }

		/// <summary>
		/// Gets or sets the attachments folder, which should begin with "~/".
		/// </summary>
		string AttachmentsFolder { get; set; }

		/// <summary>
		/// TODO: comments
		/// </summary>
		string AttachmentsRoutePath { get; set; }

		/// <summary>
		/// The database type for Roadkill. This defaults to SQLServer2008 (MongoDB on Mono) if empty.
		/// </summary>
		string DatabaseProvider { get; set; }

		/// <summary>
		/// Whether the site is public, i.e. all pages are visible by default. The default is true, and this is optional.
		/// </summary>
		bool? IsPublicSite { get; set; }

		/// <summary>
		/// For example: LDAP://mydc01.company.internal
		/// </summary>
		string LdapConnectionString { get; set; }

		/// <summary>
		/// The username to authenticate against the AD with
		/// </summary>
		string LdapUsername { get; set; }

		/// <summary>
		/// The password to authenticate against the AD with
		/// </summary>
		string LdapPassword { get; set; }

		/// <summary>
		/// Whether to remove all HTML tags from the markup except those found in the whitelist.xml file,
		/// inside the App_Data folder.
		/// </summary>
		bool? UseHtmlWhiteList { get; set; }

		/// <summary>
		/// Indicates whether server-based page object caching is enabled.
		/// </summary>
		bool? UseObjectCache { get; set; }

		/// <summary>
		/// Indicates whether page content should be cached, if <see cref="UseObjectCache"/> is true.
		/// </summary>
		bool? UseBrowserCache { get; set; }

		/// <summary>
		/// The type used for the managing users, in the format "MyNamespace.Type".
		/// This class should inherit from the <see cref="UserServiceBase"/> class or a one of its derived types.
		/// </summary>
		string UserServiceType { get; set; }

		/// <summary>
		/// TODO: comments + tests
		/// </summary>
		bool UseAzureFileStorage { get; }

		/// <summary>
		/// TODO: comments + tests
		/// </summary>
		string AzureConnectionString { get; set; }

		/// <summary>
		/// TODO: comments + tests
		/// </summary>
		string AzureContainer { get; set; }
		#endregion

		#region Built in Preferences
		/// <summary>
		/// Retrieves a list of the file extensions that are permitted for upload.
		/// </summary>
		List<string> AllowedFileTypesList { get; }

		/// <summary>
		/// The files types allowed for uploading.
		/// </summary>
		string AllowedFileTypes { get; set; }

		/// <summary>
		/// Whether users can register themselves, or if the administrators should do it. 
		/// If windows authentication is enabled, this setting is ignored.
		/// </summary>
		bool AllowUserSignup { get; set; }

		/// <summary>
		/// Whether to Recaptcha is enabled for user signups and password resets.
		/// </summary>
		bool IsRecaptchaEnabled { get; set; }

		/// <summary>
		/// The type of markup used: Three available options are: Creole, Markdown, MediaWiki.
		/// The default is Creole.
		/// </summary>
		/// <remarks>This is a string because it's easier with the Javascript interaction.</remarks>
		string MarkupType { get; set; }

		/// <summary>
		/// The private key for the recaptcha service, if enabled. This is optained when you sign up for the free service at https://www.google.com/recaptcha/.
		/// </summary>
		string RecaptchaPrivateKey { get; set; }

		/// <summary>
		/// The public key for the recaptcha service, if enabled. This is optained when you sign up for the free service at https://www.google.com/recaptcha/.
		/// </summary>
		string RecaptchaPublicKey { get; set; }

		/// <summary>
		/// The full url of the site.
		/// </summary>
		string SiteUrl { get; set; }

		/// <summary>
		/// The title of the site.
		/// </summary>
		string SiteName { get; set; }

		/// <summary>
		/// The site theme, defaults to "Blackbar"
		/// </summary>
		string Theme { get; set; }

		/// <summary>
		/// An asp.net relativate path e.g. ~/Themes/ to the current theme directory. Does not include a trailing slash.
		/// </summary>
		[JsonIgnore]
		string ThemePath { get; }

		/// <summary>
		/// Whether files with the same name overwrite the existing file, or throw an error.
		/// </summary>
		bool OverwriteExistingFiles { get; set; }

		/// <summary>
		/// Extra HTML/Javascript that is added to the HTML head, for example Google analytics, web fonts.
		/// </summary>
		string HeadContent { get; set; }

		/// <summary>
		/// The left menu markup which is parsed and rendered.
		/// </summary>
		string MenuMarkup { get; set; }

		/// <summary>
		/// The last time a plugin was saved - this is used for 304 modified checks when browser caching is enabled.
		/// </summary>
		DateTime PluginLastSaveDate { get; set; }
		#endregion
	}
}