using System;
using NUnit.Framework;
using Roadkill.Core;
using Roadkill.Core.AmazingConfig;

namespace Roadkill.Tests.Integration.Configuration
{
	public class JsonConfigurationStoreTests
	{
		private JsonConfigurationStore GetConfigurationStore(string configPath)
		{
			return new JsonConfigurationStore($@"Integration\Configuration\TestConfigs\JSON\{configPath}");
		}

		[Test]
		public void load_should_not_have_null_child_settings_when_they_are_missing()
		{
			// Arrange
			JsonConfigurationStore store = GetConfigurationStore("test-missing-child-settings.json");

			// Act
			IConfiguration actualConfiguration = store.Load();

			// Assert
			Assert.That(actualConfiguration.InternalSettings, Is.Not.Null);
			Assert.That(actualConfiguration.AttachmentSettings, Is.Not.Null);
			Assert.That(actualConfiguration.SecuritySettings, Is.Not.Null);
			Assert.That(actualConfiguration.Settings, Is.Not.Null);
		}

		[Test]
		public void empty_connection_string_should_load_when_not_installed()
		{
			// Arrange
			JsonConfigurationStore store = GetConfigurationStore("test-empty-connectionstring-not-installed.json");

			// Act
			IConfiguration actualConfiguration = store.Load();

			// Assert
			Assert.That(actualConfiguration.Installed, Is.False);
			Assert.That(actualConfiguration.ConnectionString, Is.Empty);
		}

		[Test]
		public void empty_connection_string_should_throw_exception_when_installed()
		{
			// Arrange
			JsonConfigurationStore store = GetConfigurationStore("test-empty-connectionstring-installed.json");

			// Act + Assert
			Assert.Throws<ConfigurationException>(() => store.Load());
		}

		[Test]
		public void missing_optional_settings_should_be_default_values()
		{
			// Arrange
			JsonConfigurationStore store = GetConfigurationStore("test-optional-values.json");

			// Act
			IConfiguration actualConfiguration = store.Load();

			// Assert
			Assert.That(actualConfiguration.Installed, Is.True);
			Assert.That(actualConfiguration.DatabaseProvider, Is.EqualTo("SqlServer2008"));
			Assert.That(actualConfiguration.AllowUserSignup, Is.False);
			Assert.That(actualConfiguration.IsRecaptchaEnabled, Is.False);
			Assert.That(actualConfiguration.Theme, Is.EqualTo("Mediawiki"));
			Assert.That(actualConfiguration.MarkupType, Is.EqualTo("Creole"));
			Assert.That(actualConfiguration.SiteName, Is.EqualTo("Your site"));
			Assert.That(actualConfiguration.SiteUrl, Is.EqualTo(""));
			Assert.That(actualConfiguration.RecaptchaPrivateKey, Is.EqualTo(""));
			Assert.That(actualConfiguration.RecaptchaPublicKey, Is.EqualTo(""));
			Assert.That(actualConfiguration.HeadContent, Is.EqualTo(""));
			Assert.That(actualConfiguration.MenuMarkup, Is.EqualTo("* %mainpage%\r\n* %categories%\r\n* %allpages%\r\n* %newpage%\r\n* %managefiles%\r\n* %sitesettings%\r\n\r\n"));

			Assert.That(actualConfiguration.SecuritySettings.IsRestApiEnabled, Is.False);
			Assert.That(actualConfiguration.SecuritySettings.ApiKeysList, Is.Not.Null);
			Assert.That(actualConfiguration.SecuritySettings.AdminRoleName, Is.EqualTo("Admin"));
			Assert.That(actualConfiguration.SecuritySettings.EditorRoleName, Is.EqualTo("Editor"));

			Assert.That(actualConfiguration.IgnoreSearchIndexErrors, Is.True);
			Assert.That(actualConfiguration.IsPublicSite, Is.True);
			Assert.That(actualConfiguration.UseBrowserCache, Is.True);
			Assert.That(actualConfiguration.UseHtmlWhiteList, Is.True);
		}

		[Test]
		public void load_should_return_cached_version()
		{
			// Arrange
			JsonConfigurationStore store = GetConfigurationStore("test.json");

			// Act
			IConfiguration actualConfiguration1 = store.Load();
			IConfiguration actualConfiguration2 = store.Load();

			// Assert
			Assert.True(Object.ReferenceEquals(actualConfiguration1, actualConfiguration2));
		}

		[Test]
		public void save_should_serialize_configuration()
		{
			// Arrange
			var configuration = new JsonConfiguration()
			{
				Installed = true,
				ConnectionString = "foo",
				DatabaseProvider = "MySql"
			};

			configuration.Settings["Hello"] = "World";

			JsonConfigurationStore store = new JsonConfigurationStore("configuration.json");

			// Act
			store.Save(configuration);

			// Assert
			var actualConfiguration = store.Load();

			Assert.That(actualConfiguration.Installed, Is.True);
			Assert.That(actualConfiguration.ConnectionString, Is.EqualTo("foo"));
			Assert.That(actualConfiguration.DatabaseProvider, Is.EqualTo("MySql"));
			Assert.That(actualConfiguration.Settings["Hello"], Is.EqualTo("World"));

			Assert.That(actualConfiguration.InternalSettings, Is.Not.Null);
			Assert.That(actualConfiguration.AttachmentSettings, Is.Not.Null);
			Assert.That(actualConfiguration.SecuritySettings, Is.Not.Null);
			Assert.That(actualConfiguration.Settings, Is.Not.Null);
		}
	}
}
