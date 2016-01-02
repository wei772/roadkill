using Roadkill.Core.Security;

namespace Roadkill.Core.Configuration
{
	public interface IRoadkillConfiguration
	{
		/// <summary>
		/// Gets or sets the name of the admin role.
		/// </summary>
		string AdminRoleName { get; set; }

		/// <summary>
		/// Gets or sets the api keys (comma seperated) used for access to the REST api. If this is empty, then the REST api is disabled.
		/// </summary>
		string ApiKeys { get; set; }

		/// <summary>
		/// Gets or sets the attachments folder, which should begin with "~/".
		/// </summary>
		string AttachmentsFolder { get; set; }

		/// <summary>
		/// TODO: comments
		/// </summary>
		string AttachmentsRoutePath { get; set; }

		/// <summary>
		/// Gets or sets the name of the connection string in the connectionstrings section.
		/// </summary>
		string ConnectionStringName { get; set; }

		/// <summary>
		/// Gets or sets the name of the editor role.
		/// </summary>
		string EditorRoleName { get; set; }

		/// <summary>
		/// Whether errors in updating the lucene index throw exceptions or are just ignored.
		/// </summary>
		bool IgnoreSearchIndexErrors { get; set; }

		/// <summary>
		/// Gets or sets whether this roadkill instance has been installed.
		/// </summary>
		bool Installed { get; set; }

		/// <summary>
		/// Whether the site is public, i.e. all pages are visible by default. The default is true,
		/// and this is optional.
		/// </summary>
		bool IsPublicSite { get; set; }

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
		bool UseHtmlWhiteList { get; set; }

		/// <summary>
		/// Whether to enabled Windows and Active Directory authentication.
		/// </summary>
		bool UseWindowsAuthentication { get; set; }

		/// <summary>
		/// The type used for the managing users, in the format "MyNamespace.Type".
		/// This class should inherit from the <see cref="UserServiceBase"/> class or a one of its derived types.
		/// </summary>
		string UserServiceType { get; set; }

		/// <summary>
		/// Indicates whether server-based page object caching is enabled.
		/// </summary>
		bool UseObjectCache { get; set; }

		/// <summary>
		/// Indicates whether page content should be cached, if <see cref="UseObjectCache"/> is true.
		/// </summary>
		bool UseBrowserCache { get; set; }

		/// <summary>
		/// The database type for Roadkill. This defaults to SQLServer2008 (MongoDB on Mono) if empty.
		/// </summary>
		string DatabaseName { get; set; }

		/// <summary>
		/// TODO: comments + tests
		/// </summary>
		bool UseAzureFileStorage { get; set; }

		/// <summary>
		/// TODO: comments + tests
		/// </summary>
		string AzureConnectionString { get; set; }

		/// <summary>
		/// TODO: comments + tests
		/// </summary>
		string AzureContainer { get; set; }
	}
}