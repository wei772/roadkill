using System.Linq;
using NUnit.Framework;
using Roadkill.Core.Configuration;

namespace Roadkill.Tests.Unit.Configuration
{
	[TestFixture]
	[Category("Integration")]
	public class RoadkillConfigurationConverterTests
	{
		[Test]
		public void ToApplicationSettings_should_convert_properties()
		{
			// Arrange
			var config = new RoadkillConfiguration()
			{
				AdminRoleName = "Admin - test",
				ApiKeys = "apikey1,apikey2,   apikey3",
				AttachmentsFolder = "/Attachments-test",
				AttachmentsRoutePath = "AttachmentsRoutePathTest",
				ConnectionString = "connectionstring-test",
				EditorRoleName = "Editor-test",
				IgnoreSearchIndexErrors = true,
				Installed = true,
				IsPublicSite = false,
				LdapConnectionString = "ldapstring-test",
				LdapUsername = "ldapusername-test",
				LdapPassword = "ldappassword-test",
				UseHtmlWhiteList = false,
				UseWindowsAuthentication = false,
				UserServiceType = "DefaultUserManager-test",
				UseObjectCache = true,
				UseBrowserCache = true,
				DatabaseName = "SqlServer2008",
				UseAzureFileStorage = false,
				AzureConnectionString = "Attachments",
				AzureContainer = "Attachments"
			};

			// Act
			var converter = new RoadkillConfigurationConverter();
			ApplicationSettings appSettings = converter.ToApplicationSettings(config);

			// Assert
			Assert.That(appSettings.AdminRoleName, Is.EqualTo(config.AdminRoleName), "AdminRoleName");
			Assert.That(appSettings.ApiKeys.Count(), Is.EqualTo(3), "ApiKeys");
			Assert.That(appSettings.AttachmentsRoutePath, Is.EqualTo(config.AttachmentsRoutePath), "AttachmentsRoutePath");
			Assert.That(appSettings.AttachmentsFolder, Is.EqualTo(config.AttachmentsFolder), "AttachmentsFolder");
			Assert.That(appSettings.UseObjectCache, Is.EqualTo(config.UseObjectCache), "UseObjectCache");
			Assert.That(appSettings.UseBrowserCache, Is.EqualTo(config.UseBrowserCache), "UseBrowserCache");
			Assert.That(appSettings.ConnectionString, Is.EqualTo(config.ConnectionString), "ConnectionString");
			Assert.That(appSettings.DatabaseName, Is.EqualTo(config.DatabaseName), "DatabaseName");
			Assert.That(appSettings.EditorRoleName, Is.EqualTo(config.EditorRoleName), "EditorRoleName");
			Assert.That(appSettings.IgnoreSearchIndexErrors, Is.EqualTo(config.IgnoreSearchIndexErrors), "IgnoreSearchIndexErrors");
			Assert.That(appSettings.Installed, Is.EqualTo(config.Installed), "Installed");
			Assert.That(appSettings.IsPublicSite, Is.EqualTo(config.IsPublicSite), "IsPublicSite");
			Assert.That(appSettings.LdapConnectionString, Is.EqualTo(config.LdapConnectionString), "LdapConnectionString");
			Assert.That(appSettings.LdapPassword, Is.EqualTo(config.LdapPassword), "LdapPassword");
			Assert.That(appSettings.LdapUsername, Is.EqualTo(config.LdapUsername), "LdapUsername");
			Assert.That(appSettings.UseHtmlWhiteList, Is.EqualTo(config.UseHtmlWhiteList), "UseHtmlWhiteList");
			Assert.That(appSettings.UserServiceType, Is.EqualTo(config.UserServiceType), "UserServiceType");
			Assert.That(appSettings.UseWindowsAuthentication, Is.EqualTo(config.UseWindowsAuthentication), "UseWindowsAuthentication");
		}

		[Test]
		public void ToApplicationSettings_should_use_default_values_when_optional_settings_have_missing_values()
		{
			// Arrange
			var config = new RoadkillConfiguration()
			{
				AdminRoleName = "Admin-test",
				AttachmentsFolder = "~/Attachments-test",
				ConnectionString = "Roadkill-test",
				EditorRoleName = "Editor-test",
				Installed = true,
				UseWindowsAuthentication = false
			};

			// Act
			var converter = new RoadkillConfigurationConverter();
			ApplicationSettings appSettings = converter.ToApplicationSettings(config);

			// Assert
			Assert.That(appSettings.AttachmentsRoutePath, Is.EqualTo("Attachments"), "AttachmentsRoutePath");
			Assert.That(appSettings.ApiKeys, Is.Not.Null.And.Empty, "ApiKeys");
			Assert.That(appSettings.DatabaseName, Is.EqualTo("SqlServer2008"), "DatabaseName");
			Assert.That(appSettings.IgnoreSearchIndexErrors, Is.False, "IgnoreSearchIndexErrors");
			Assert.That(appSettings.IsPublicSite, Is.True, "IsPublicSite");
			Assert.That(appSettings.LdapConnectionString, Is.EqualTo(""), "LdapConnectionString");
			Assert.That(appSettings.LdapPassword, Is.EqualTo(""), "LdapPassword");
			Assert.That(appSettings.LdapUsername, Is.EqualTo(""), "LdapUsername");
			Assert.That(appSettings.UseHtmlWhiteList, Is.True, "UseHtmlWhiteList");
			Assert.That(appSettings.UserServiceType, Is.EqualTo(""), "DefaultUserManager");
		}

		[Test]
		public void ToApplicationSettings_should_parse_api_keys()
		{
			// Arrange
			var config = new RoadkillConfiguration()
			{
				ApiKeys = "apikey1,apikey2,   apikey3"
			};

			// Act
			var converter = new RoadkillConfigurationConverter();
			ApplicationSettings appSettings = converter.ToApplicationSettings(config);

			// Assert
			Assert.That(appSettings.ApiKeys, Is.Not.Null, "ApiKeys");
			Assert.That(appSettings.ApiKeys.Count(), Is.EqualTo(3), "ApiKeys");
			Assert.That(appSettings.ApiKeys, Contains.Item("apikey1"), "Doesn't contain 'apikey1'");
			Assert.That(appSettings.ApiKeys, Contains.Item("apikey2"), "Doesn't contain 'apikey2'");
			Assert.That(appSettings.ApiKeys, Contains.Item("apikey3"), "Doesn't contain 'apikey3'");
		}
	}
}