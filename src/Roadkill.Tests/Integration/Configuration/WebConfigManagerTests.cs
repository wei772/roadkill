using System;
using System.IO;
using System.Web.Configuration;
using NUnit.Framework;
using Roadkill.Core.AmazingConfig;

namespace Roadkill.Tests.Integration.Configuration
{
	[TestFixture]
	[Description("Tests writing and reading of .config files.")]
	[Category("Integration")]
	[Parallelizable]
	public class WebConfigManagerTests
	{
		[SetUp]
		public void Setup()
		{
			// Copy the config files so they're fresh before each test
			string source = Path.Combine(TestConstants.ROOT_FOLDER, "src", "Roadkill.Tests", "Integration", "Configuration", "TestConfigs", "XML");
			string destination = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Integration", "Configuration", "TestConfigs", "XML");

			foreach (string filename in Directory.GetFiles(source))
			{
				FileInfo info = new FileInfo(filename);
				File.Copy(filename, Path.Combine(destination, info.Name), true);
			}
		}

		private string GetConfigPath(string filename)
		{
			return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Integration", "Configuration", "TestConfigs", "XML", filename);
		}

		[Test]
		public void updatelanguage_should_save_language_code_to_globalization_section()
		{
			// Arrange
			string configFilePath = GetConfigPath("test.config");

			// Act
			var webConfigManager = new WebConfigManager(configFilePath);
			webConfigManager.UpdateLanguage("fr-FR");

			// Assert
			System.Configuration.Configuration config = webConfigManager.Configuration;
			GlobalizationSection globalizationSection = config.GetSection("system.web/globalization") as GlobalizationSection;

			Assert.That(globalizationSection, Is.Not.Null);
			Assert.That(globalizationSection.UICulture, Is.EqualTo("fr-FR"));
		}

		[Test]
		public void should_return_configuration_for_exe_file()
		{
			// Arrange
			string configFilePath = GetConfigPath("test.config");

			// Act
			var webConfigManager = new WebConfigManager(configFilePath);
			System.Configuration.Configuration config = webConfigManager.Configuration;

			// Assert
			Assert.That(config, Is.Not.Null);
			Assert.That(config.FilePath, Is.EqualTo(configFilePath));
		}

		[Test]
		public void WriteForFormsAuth_should_add_formsauth_section_and_anonymousidentification()
		{
			// Arrange
			string configFilePath = GetConfigPath("test.config");

			// Act
			var webConfigManager = new WebConfigManager(configFilePath);
			webConfigManager.WriteForFormsAuth();

			// Assert
			System.Configuration.Configuration config = webConfigManager.Configuration;
			AuthenticationSection authSection = config.GetSection("system.web/authentication") as AuthenticationSection;

			Assert.That(authSection, Is.Not.Null);
			Assert.That(authSection.Mode, Is.EqualTo(AuthenticationMode.Forms));
			Assert.That(authSection.Forms.LoginUrl, Is.EqualTo("~/User/Login"));

			AnonymousIdentificationSection anonSection = config.GetSection("system.web/anonymousIdentification") as AnonymousIdentificationSection;
			Assert.That(anonSection, Is.Not.Null);
			Assert.That(anonSection.Enabled, Is.True);
		}

		[Test]
		public void writeconfigforwindowsauth_should_set_windowsauthmode_and_disable_anonymousidentification()
		{
			// Arrange
			string configFilePath = GetConfigPath("test.config");

			// Act
			var webConfigManager = new WebConfigManager(configFilePath);
			webConfigManager.WriteForWindowsAuth();

			// Assert
			System.Configuration.Configuration config = webConfigManager.Configuration;
			AuthenticationSection authSection = config.GetSection("system.web/authentication") as AuthenticationSection;

			Assert.That(authSection, Is.Not.Null);
			Assert.That(authSection.Mode, Is.EqualTo(AuthenticationMode.Windows));
			Assert.That(authSection.Forms.LoginUrl, Is.EqualTo("login.aspx")); // login.aspx is the default for windows auth

			AnonymousIdentificationSection anonSection = config.GetSection("system.web/anonymousIdentification") as AnonymousIdentificationSection;
			Assert.That(anonSection, Is.Not.Null);
			Assert.That(anonSection.Enabled, Is.False);
		}

		[Test]
		public void IsWriteable_should_return_no_errors_for_writeable_config_file()
		{
			// Arrange
			string configFilePath = GetConfigPath("test.config");

			// Act
			var webConfigManager = new WebConfigManager(configFilePath);
			string errorMessages = webConfigManager.IsWriteable();

			// Assert
			Assert.That(errorMessages, Is.Empty.Or.Null);
		}

		[Test]
		public void IsWriteable_should_return_errors_when_config_file_is_not_writeable()
		{
			// Arrange
			string configFilePath = GetConfigPath("test.config");

			try
			{
				File.SetAttributes(configFilePath, FileAttributes.ReadOnly);

				// Act
				var webConfigManager = new WebConfigManager(configFilePath);
				webConfigManager.Configuration.AppSettings.Settings.Add("test", "not writeable");

				string errorMessages = webConfigManager.IsWriteable();

				// Assert
				Assert.That(errorMessages, Is.Not.Empty.Or.Null);
			}
			finally
			{
				File.SetAttributes(configFilePath, FileAttributes.Normal);
			}
		}
	}
}