using System.Linq;
using NUnit.Framework;
using Roadkill.Core.AmazingConfig;

namespace Roadkill.Tests.Unit.Configuration
{
	public class JsonConfigurationTests
	{
		[Test]
		public void should_set_default_values_in_constructor()
		{
			// Arrange + Act
			var configuration = new JsonConfiguration();

			// Assert
			Assert.That(configuration.InternalSettings, Is.Not.Null);
			Assert.That(configuration.AttachmentSettings, Is.Not.Null);
			Assert.That(configuration.SecuritySettings, Is.Not.Null);
			Assert.That(configuration.Settings, Is.Not.Null);

			Assert.That(configuration.Installed, Is.False);
			Assert.That(configuration.ConnectionString, Is.EqualTo(""));
			Assert.That(configuration.DatabaseProvider, Is.EqualTo("SqlServer2008"));

			Assert.That(configuration.IgnoreSearchIndexErrors, Is.True);
			Assert.That(configuration.IsPublicSite, Is.True);
			Assert.That(configuration.UseObjectCache, Is.True);
			Assert.That(configuration.UseBrowserCache, Is.True);
			Assert.That(configuration.UseHtmlWhiteList, Is.True);

			Assert.That(configuration.AttachmentSettings.OverwriteExistingFiles, Is.False);
			Assert.That(configuration.AttachmentSettings.AllowedFileTypes, Is.EqualTo("jpg, png, gif"));
			Assert.That(configuration.AttachmentSettings.AllowedFileTypesList, Contains.Item("jpg"));
			Assert.That(configuration.AttachmentSettings.AllowedFileTypesList, Contains.Item("png"));
			Assert.That(configuration.AttachmentSettings.AllowedFileTypesList, Contains.Item("gif"));

			Assert.That(configuration.AttachmentSettings.AttachmentsRoutePath, Is.EqualTo("Attachments"));
			Assert.That(configuration.AttachmentSettings.AttachmentsFolder, Is.EqualTo("~/App_Data/Attachments"));

			Assert.That(configuration.SecuritySettings.AllowUserSignup, Is.False);
			Assert.That(configuration.SecuritySettings.AdminRoleName, Is.EqualTo("Admin"));
			Assert.That(configuration.SecuritySettings.EditorRoleName, Is.EqualTo("Editor"));
			Assert.That(configuration.SecuritySettings.IsRecaptchaEnabled, Is.False);
			Assert.That(configuration.SecuritySettings.RecaptchaPrivateKey, Is.EqualTo(""));
			Assert.That(configuration.SecuritySettings.RecaptchaPublicKey, Is.EqualTo(""));
			Assert.That(configuration.SecuritySettings.ApiKeys, Is.Not.Null);
			Assert.That(configuration.SecuritySettings.ApiKeys.Count(), Is.EqualTo(0));
			Assert.That(configuration.SecuritySettings.Settings, Is.Not.Null);

			Assert.That(configuration.Theme, Is.EqualTo("Mediawiki"));
			Assert.That(configuration.ThemePath, Is.EqualTo("~/Themes/Mediawiki"));

			Assert.That(configuration.MarkupType, Is.EqualTo("Markdown"));
			Assert.That(configuration.HeadContent, Is.EqualTo(""));
			Assert.That(configuration.SiteName, Is.EqualTo("Your site"));
			Assert.That(configuration.SiteUrl, Is.EqualTo("http://localhost"));
			Assert.That(configuration.MenuMarkup, Is.Not.Null.Or.Empty);
		}

		[Test]
		public void should_parse_api_keys()
		{
			// Arrange + Act
			var configuration = new JsonConfiguration();
			configuration.SecuritySettings.ApiKeys = "123,456";

			// Assert
			Assert.That(configuration.SecuritySettings.ApiKeysList.Count(), Is.EqualTo(2));
			Assert.That(configuration.SecuritySettings.ApiKeys, Is.EqualTo("123,456"));
		}

		[Test]
		public void GetAttachmentsDirectoryPath_should()
		{
			// Arrange

			// Act

			// Assert
			Assert.Fail("TODO");
		}

		[Test]
		public void GetAttachmentsUrlPath_should()
		{
			// Arrange

			// Act

			// Assert
			Assert.Fail("TODO");
		}
	}
}