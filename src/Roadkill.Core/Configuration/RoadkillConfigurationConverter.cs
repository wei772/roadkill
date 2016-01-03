using System.Collections.Generic;

namespace Roadkill.Core.Configuration
{
	public class RoadkillConfigurationConverter
	{
		public ApplicationSettings ToApplicationSettings(IRoadkillConfiguration configuration)
		{
			ApplicationSettings appSettings = new ApplicationSettings();

			// These properties aren't required
			if (!string.IsNullOrEmpty(configuration.AttachmentsFolder))
				appSettings.AttachmentsFolder = configuration.AttachmentsFolder;

			if (!string.IsNullOrEmpty(configuration.AttachmentsRoutePath))
				appSettings.AttachmentsRoutePath = configuration.AttachmentsRoutePath;

			if (!string.IsNullOrEmpty(configuration.DatabaseName))
				appSettings.DatabaseName = configuration.DatabaseName;

			appSettings.AdminRoleName = configuration.AdminRoleName;
			appSettings.ApiKeys = ParseApiKeys(configuration.ApiKeys);
			appSettings.AzureConnectionString = configuration.AzureConnectionString;
			appSettings.AzureContainer = configuration.AzureContainer;
			appSettings.ConnectionString = configuration.ConnectionString;
			appSettings.UseObjectCache = configuration.UseObjectCache;
			appSettings.UseBrowserCache = configuration.UseBrowserCache;
			appSettings.EditorRoleName = configuration.EditorRoleName;
			appSettings.IgnoreSearchIndexErrors = configuration.IgnoreSearchIndexErrors;
			appSettings.IsPublicSite = configuration.IsPublicSite; // TODO
			appSettings.Installed = configuration.Installed;
			appSettings.LdapConnectionString = configuration.LdapConnectionString ?? "";
			appSettings.LdapUsername = configuration.LdapUsername ?? "";
			appSettings.LdapPassword = configuration.LdapPassword ?? "";
			appSettings.UseAzureFileStorage = configuration.UseAzureFileStorage;
			appSettings.UseHtmlWhiteList = configuration.UseHtmlWhiteList; // TODO
			appSettings.UserServiceType = configuration.UserServiceType;
			appSettings.UseWindowsAuthentication = configuration.UseWindowsAuthentication;

			return appSettings;
		}

		private IEnumerable<string> ParseApiKeys(string apiKeys)
		{
			if (string.IsNullOrEmpty(apiKeys))
				return new List<string>();

			var keyList = new List<string>();

			string[] keys = apiKeys.Split(',');
			foreach (string item in keys)
			{
				keyList.Add(item.Trim());
			}

			return keyList;
		}
	}
}
