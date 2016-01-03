using System;
using System.Reflection;

namespace Roadkill.Core.Configuration
{
	public class RoadkillConfiguration : IRoadkillConfiguration
	{
		public string AdminRoleName { get; set; }
		public string ApiKeys { get; set; }
		public string AttachmentsFolder { get; set; }
		public string AttachmentsRoutePath { get; set; }
		public string ConnectionString { get; set; }
		public string EditorRoleName { get; set; }
		public bool IgnoreSearchIndexErrors { get; set; }
		public bool Installed { get; set; }
		public bool IsPublicSite { get; set; }
		public string LdapConnectionString { get; set; }
		public string LdapUsername { get; set; }
		public string LdapPassword { get; set; }
		public bool UseHtmlWhiteList { get; set; }
		public bool UseWindowsAuthentication { get; set; }
		public string UserServiceType { get; set; }
		public bool UseObjectCache { get; set; }
		public bool UseBrowserCache { get; set; }
		public string DatabaseName { get; set; }
		public bool UseAzureFileStorage { get; set; }
		public string AzureConnectionString { get; set; }
		public string AzureContainer { get; set; }

		public static RoadkillConfiguration Convert(IRoadkillConfiguration configToConvert)
		{
			RoadkillConfiguration newConfig = Activator.CreateInstance<RoadkillConfiguration>();
			PropertyInfo[] properties = typeof(IRoadkillConfiguration).GetProperties();

			foreach (PropertyInfo prop in properties)
			{
				var value = prop.GetValue(configToConvert);
				prop.SetValue(newConfig, value);
			}

			return newConfig;
		}
	}
}