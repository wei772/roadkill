using System.Collections.Generic;
using AutoMapper;

namespace Roadkill.Core.Configuration
{
	public class RoadkillConfigurationConverter
	{
		static RoadkillConfigurationConverter()
		{
			Mapper.Initialize(cfg =>
			{
				cfg.CreateMap<IRoadkillConfiguration, ApplicationSettings>()
					.ForAllMembers(opt => opt.Condition(source => !source.IsSourceValueNull));
			});
		}

		public ApplicationSettings ToApplicationSettings(IRoadkillConfiguration configuration)
		{
			ApplicationSettings appSettings = Mapper.Map<IRoadkillConfiguration, ApplicationSettings>(configuration);
			appSettings.ApiKeys = ParseApiKeys(configuration.ApiKeys);


			// These properties aren't required
			//if (!string.IsNullOrEmpty(configuration.AttachmentsFolder))
			//	appSettings.AttachmentsFolder = configuration.AttachmentsFolder;

			//if (!string.IsNullOrEmpty(configuration.AttachmentsRoutePath))
			//	appSettings.AttachmentsRoutePath = configuration.AttachmentsRoutePath;

			//if (!string.IsNullOrEmpty(configuration.DatabaseName))
			//	appSettings.DatabaseName = configuration.DatabaseName;

			//appSettings.AdminRoleName = configuration.AdminRoleName;
			//appSettings.ApiKeys = ParseApiKeys(configuration.ApiKeys);

			//appSettings.ConnectionString = configuration.ConnectionString;
			//appSettings.EditorRoleName = configuration.EditorRoleName;
			//appSettings.Installed = configuration.Installed;
			//appSettings.UserServiceType = configuration.UserServiceType;
			//appSettings.UseWindowsAuthentication = configuration.UseWindowsAuthentication;


			// Optional settings

			//appSettings.IgnoreSearchIndexErrors = configuration.IgnoreSearchIndexErrors ?? true;
			//appSettings.IsPublicSite = configuration.IsPublicSite ?? true;
			//appSettings.UseObjectCache = configuration.UseObjectCache ?? true;
			//appSettings.UseBrowserCache = configuration.UseBrowserCache ?? true;
			//appSettings.UseHtmlWhiteList = configuration.UseHtmlWhiteList ?? true;

			//appSettings.LdapConnectionString = configuration.LdapConnectionString ?? "";
			//appSettings.LdapUsername = configuration.LdapUsername ?? "";
			//appSettings.LdapPassword = configuration.LdapPassword ?? "";

			//appSettings.AzureConnectionString = configuration.AzureConnectionString;
			//appSettings.AzureContainer = configuration.AzureContainer;
			//appSettings.UseAzureFileStorage = configuration.UseAzureFileStorage ?? false;

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
