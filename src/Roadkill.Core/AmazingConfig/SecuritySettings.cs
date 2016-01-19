using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Roadkill.Core.Security;

namespace Roadkill.Core.AmazingConfig
{
	/// <summary>
	/// All security-related settings: Roles, Active Directory/LDAP settings, api keys.
	/// </summary>
	public class SecuritySettings
	{
		/// <summary>
		/// Gets or sets the api keys (comma seperated) used for access to the REST api. If this is empty, then the REST api is disabled.
		/// </summary>
		public string ApiKeys { get; set; }

		/// <summary>
		/// Contains a list API keys for the REST api. If this is empty, then the REST api is disabled.
		/// </summary>
		[JsonIgnore]
		public IEnumerable<string> ApiKeysList { get; }

		/// <summary>
		/// Whether the REST api is available - if api keys are set in the config.
		/// </summary>
		[JsonIgnore]
		public bool IsRestApiEnabled
		{
			get
			{
				return ApiKeysList != null && ApiKeysList.Any();
			}
		}

		/// <summary>
		/// Gets or sets the name of the admin role.
		/// </summary>
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public string AdminRoleName { get; set; }

		/// <summary>
		/// Gets or sets the name of the editor role.
		/// </summary>
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public string EditorRoleName { get; set; }

		/// <summary>
		/// Whether to enabled Windows and Active Directory authentication.
		/// </summary>
		public bool UseWindowsAuthentication { get; set; }

		/// <summary>
		/// Whether users can register themselves, or if the administrators should do it. 
		/// If windows authentication is enabled, this setting is ignored.
		/// </summary>
		public bool AllowUserSignup { get; set; }

		/// <summary>
		/// Whether to Recaptcha is enabled for user signups and password resets.
		/// </summary>
		public bool IsRecaptchaEnabled { get; set; }

		/// <summary>
		/// The private key for the recaptcha service, if enabled. This is optained when you sign up for the free service at https://www.google.com/recaptcha/.
		/// </summary>
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public string RecaptchaPrivateKey { get; set; }

		/// <summary>
		/// The public key for the recaptcha service, if enabled. This is optained when you sign up for the free service at https://www.google.com/recaptcha/.
		/// </summary>
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public string RecaptchaPublicKey { get; set; }

		/// <summary>
		/// The type used for the managing users, in the format "MyNamespace.Type".
		/// This class should inherit from the <see cref="UserServiceBase"/> class or a one of its derived types.
		/// </summary>
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public string UserServiceType { get; set; }

		/// <summary>
		/// For example: LDAP://mydc01.company.internal
		/// </summary>
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public string LdapConnectionString { get; set; }

		/// <summary>
		/// The username to authenticate against the AD with
		/// </summary>
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public string LdapUsername { get; set; }

		/// <summary>
		/// The password to authenticate against the AD with
		/// </summary>
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public string LdapPassword { get; set; }

		/// <summary>
		/// Additional custom settings.
		/// </summary>
		public Dictionary<string, string> Settings { get; set; }

		public SecuritySettings()
		{
			AllowUserSignup = false;
			AdminRoleName = "Admin";
			ApiKeysList = new List<string>();
			EditorRoleName = "Editor";
			RecaptchaPrivateKey = "";
			RecaptchaPublicKey = "";

			Settings = new Dictionary<string, string>();
		}
	}
}
