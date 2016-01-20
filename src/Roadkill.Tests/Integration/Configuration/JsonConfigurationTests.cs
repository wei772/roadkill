using NUnit.Framework;
using Roadkill.Core.AmazingConfig;

namespace Roadkill.Tests.Integration.Configuration
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
			Assert.That(configuration.AllowUserSignup, Is.False);
			Assert.That(configuration.IsRecaptchaEnabled, Is.False);

			Assert.That(configuration.Theme, Is.EqualTo("Mediawiki"));
			Assert.That(configuration.MarkupType, Is.EqualTo("Markdown"));
			Assert.That(configuration.HeadContent, Is.EqualTo(""));
			Assert.That(configuration.RecaptchaPrivateKey, Is.EqualTo(""));
			Assert.That(configuration.RecaptchaPublicKey, Is.EqualTo(""));
			Assert.That(configuration.MenuMarkup, Is.Not.Null.Or.Empty);
		}
	}
}