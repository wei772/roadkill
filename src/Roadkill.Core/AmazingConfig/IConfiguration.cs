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
		InternalSettings InternalSettings { get; set; }
		SecuritySettings SecuritySettings { get; set; }
		AttachmentSettings AttachmentSettings { get; set; }
		Dictionary<string,string> Settings { get; set; }

		/// <summary>
		/// The database connection string.
		/// </summary>
		string ConnectionString { get; set; }

		/// <summary>
		/// Gets or sets whether this roadkill instance has been installed.
		/// </summary>
		bool Installed { get; set; }
		#endregion

		#region Optional properties
		/// <summary>
		/// Whether errors in updating the lucene index throw exceptions or are just ignored.
		/// </summary>
		bool? IgnoreSearchIndexErrors { get; set; }

		/// <summary>
		/// The database type for Roadkill. This defaults to SQLServer2008 (MongoDB on Mono) if empty.
		/// </summary>
		string DatabaseProvider { get; set; }

		/// <summary>
		/// Whether the site is public, i.e. all pages are visible by default. The default is true, and this is optional.
		/// </summary>
		bool? IsPublicSite { get; set; }

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
		#endregion

		#region Built in Preferences
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