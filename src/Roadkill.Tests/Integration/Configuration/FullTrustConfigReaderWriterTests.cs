using NUnit.Framework;

namespace Roadkill.Tests.Integration.Configuration
{
	[TestFixture]
	[Description("Tests writing and reading of XML based .config files.")]
	[Category("Integration")]
	public class FullTrustConfigReaderWriterTests
	{
		// TODO
		//[SetUp]
		//public void Setup()
		//{
		//	// Copy the config files so they're fresh before each test
		//	string source = Path.Combine(TestConstants.ROOT_FOLDER, "src", "Roadkill.Tests", "Integration", "Configuration", "TestConfigs", "XML");
		//	string destination = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Integration", "Configuration", "TestConfigs", "XML");

		//	foreach (string filename in Directory.GetFiles(source))
		//	{
		//		FileInfo info = new FileInfo(filename);
		//		File.Copy(filename, Path.Combine(destination, info.Name), true);
		//	}
		//}

		//[Test]
		//public void load_should_return_roadkillsection()
		//{
		//	// Arrange
		//	string configFilePath = GetConfigPath("test.config");

		//	// Act
		//	FullTrustWebConfigReaderWriter webConfigManager = new FullTrustWebConfigReaderWriter(configFilePath);
		//	IRoadkillConfiguration config = webConfigManager.Load();

		//	// Assert
		//	Assert.That(config.AdminRoleName, Is.EqualTo("Admin-test"), "AdminRoleName"); // basic check
		//}

		//[Test]
		//public void updatelanguage_should_save_language_code_to_globalization_section()
		//{
		//	// Arrange
		//	string configFilePath = GetConfigPath("test.config");

		//	// Act
		//	FullTrustWebConfigReaderWriter webConfigManager = new FullTrustWebConfigReaderWriter(configFilePath);
		//	webConfigManager.UpdateLanguage("fr-FR");

		//	// Assert
		//	System.Configuration.Configuration config = webConfigManager.GetConfiguration();
		//	GlobalizationSection globalizationSection = config.GetSection("system.web/globalization") as GlobalizationSection;

		//	Assert.That(globalizationSection, Is.Not.Null);
		//	Assert.That(globalizationSection.UICulture, Is.EqualTo("fr-FR"));
		//}

		//[Test]
		//public void resetinstalledstate_should_set_installed_to_false()
		//{
		//	// Arrange
		//	string configFilePath = GetConfigPath("test.config");

		//	// Act
		//	FullTrustWebConfigReaderWriter webConfigManager = new FullTrustWebConfigReaderWriter(configFilePath);
		//	webConfigManager.ResetInstalledState();

		//	// Assert
		//	RoadkillSection section = webConfigManager.Load() as RoadkillSection;
		//	Assert.That(section.Installed, Is.False);
		//}

		//[Test]
		//public void testsavewebconfig_should_return_empty_string_for_success()
		//{
		//	// Arrange
		//	string configFilePath = GetConfigPath("test.config");

		//	// Act
		//	FullTrustWebConfigReaderWriter webConfigManager = new FullTrustWebConfigReaderWriter(configFilePath);
		//	string result = webConfigManager.TestSaveWebConfig();

		//	// Assert
		//	Assert.That(result, Is.EqualTo(""));
		//}

		//[Test]
		//public void getconfiguration_should_return_configuration_for_exe_file()
		//{
		//	// Arrange
		//	string configFilePath = GetConfigPath("test.config");

		//	// Act
		//	FullTrustWebConfigReaderWriter webConfigManager = new FullTrustWebConfigReaderWriter(configFilePath);
		//	System.Configuration.Configuration config = webConfigManager.GetConfiguration();

		//	// Assert
		//	Assert.That(config, Is.Not.Null);
		//	Assert.That(config.FilePath, Is.EqualTo(configFilePath));
		//}

		//[Test]
		//public void writeconfigforformsauth_should_add_formsauth_section_and_anonymousidentification()
		//{
		//	// Arrange
		//	string configFilePath = GetConfigPath("test.config");

		//	// Act
		//	FullTrustWebConfigReaderWriter webConfigManager = new FullTrustWebConfigReaderWriter(configFilePath);
		//	webConfigManager.WriteConfigForFormsAuth();

		//	// Assert
		//	System.Configuration.Configuration config = webConfigManager.GetConfiguration();
		//	AuthenticationSection authSection = config.GetSection("system.web/authentication") as AuthenticationSection;

		//	Assert.That(authSection, Is.Not.Null);
		//	Assert.That(authSection.Mode, Is.EqualTo(AuthenticationMode.Forms));
		//	Assert.That(authSection.Forms.LoginUrl, Is.EqualTo("~/User/Login"));

		//	AnonymousIdentificationSection anonSection = config.GetSection("system.web/anonymousIdentification") as AnonymousIdentificationSection;
		//	Assert.That(anonSection.Enabled, Is.True);
		//}

		//[Test]
		//public void writeconfigforwindowsauth_should_set_windowsauthmode_and_disable_anonymousidentification()
		//{
		//	// Arrange
		//	string configFilePath = GetConfigPath("test.config");

		//	// Act
		//	FullTrustWebConfigReaderWriter webConfigManager = new FullTrustWebConfigReaderWriter(configFilePath);
		//	webConfigManager.WriteConfigForWindowsAuth();

		//	// Assert
		//	System.Configuration.Configuration config = webConfigManager.GetConfiguration();
		//	AuthenticationSection authSection = config.GetSection("system.web/authentication") as AuthenticationSection;

		//	Assert.That(authSection, Is.Not.Null);
		//	Assert.That(authSection.Mode, Is.EqualTo(AuthenticationMode.Windows));
		//	Assert.That(authSection.Forms.LoginUrl, Is.EqualTo("login.aspx")); // login.aspx is the default for windows auth

		//	AnonymousIdentificationSection anonSection = config.GetSection("system.web/anonymousIdentification") as AnonymousIdentificationSection;
		//	Assert.That(anonSection.Enabled, Is.False);
		//}

		//[Test]
		//public void getapplicationsettings_should_find_connection_value_from_connection_setting()
		//{
		//	// Arrange
		//	string configFilePath = GetConfigPath("test.config");

		//	// Act
		//	FullTrustWebConfigReaderWriter webConfigManager = new FullTrustWebConfigReaderWriter(configFilePath);
		//	ApplicationSettings appSettings = webConfigManager.GetApplicationSettings();

		//	// Assert
		//	Assert.That(appSettings.ConnectionString, Is.EqualTo("connectionstring-test"), "ConnectionStringName");
		//}

		//[Test]
		//[ExpectedException(typeof(ConfigurationErrorsException))]
		//public void RoadkillSection_Missing_Values_Throw_Exception()
		//{
		//	// Arrange
		//	string configFilePath = GetConfigPath("test-missing-values.config");

		//	// Act
		//	FullTrustWebConfigReaderWriter webConfigManager = new FullTrustWebConfigReaderWriter(configFilePath);
			
		//	// Assert
		//}

		//[Test]
		//[Description("Tests the save from both the settings page and installation")]
		//public void Save_Should_Persist_All_ApplicationSettings()
		//{
		//	// Arrange
		//	string configFilePath = GetConfigPath("test-empty.config");
		//	SettingsViewModel viewModel = new SettingsViewModel()
		//	{
		//		AdminRoleName = "admin role name",
		//		AttachmentsFolder = @"c:\AttachmentsFolder",
		//		UseObjectCache = true,
		//		UseBrowserCache = true,
		//		ConnectionString = "connection string",
		//		DatabaseProvider = "MongoDB",
		//		EditorRoleName = "editor role name",
		//		LdapConnectionString = "ldap connection string",
		//		LdapUsername = "ldap username",
		//		LdapPassword = "ldap password",
		//		UseWindowsAuth = true,
		//		IsPublicSite = false,
		//		IgnoreSearchIndexErrors = false
		//	};

		//	// Act
		//	FullTrustWebConfigReaderWriter webConfigManager = new FullTrustWebConfigReaderWriter(configFilePath);
		//	webConfigManager.Save(viewModel);

		//	ApplicationSettings appSettings = webConfigManager.GetApplicationSettings();

		//	// Assert
		//	Assert.That(appSettings.AdminRoleName, Is.EqualTo(viewModel.AdminRoleName), "AdminRoleName");
		//	Assert.That(appSettings.AttachmentsFolder, Is.EqualTo(viewModel.AttachmentsFolder), "AttachmentsFolder");
		//	Assert.That(appSettings.UseObjectCache, Is.EqualTo(viewModel.UseObjectCache), "UseObjectCache");
		//	Assert.That(appSettings.UseBrowserCache, Is.EqualTo(viewModel.UseBrowserCache), "UseBrowserCache");
		//	Assert.That(appSettings.ConnectionString, Is.EqualTo(viewModel.ConnectionString), "ConnectionStringName");
		//	Assert.That(appSettings.DatabaseName, Is.EqualTo("MongoDB"), "DatabaseProvider");
		//	Assert.That(appSettings.EditorRoleName, Is.EqualTo(viewModel.EditorRoleName), "EditorRoleName");
		//	Assert.That(appSettings.IgnoreSearchIndexErrors, Is.EqualTo(viewModel.IgnoreSearchIndexErrors), "IgnoreSearchIndexErrors");
		//	Assert.That(appSettings.IsPublicSite, Is.EqualTo(viewModel.IsPublicSite), "IsPublicSite");
		//	Assert.That(appSettings.LdapConnectionString, Is.EqualTo(viewModel.LdapConnectionString), "LdapConnectionString");
		//	Assert.That(appSettings.LdapPassword, Is.EqualTo(viewModel.LdapPassword), "LdapPassword");
		//	Assert.That(appSettings.LdapUsername, Is.EqualTo(viewModel.LdapUsername), "LdapUsername");
		//	Assert.That(appSettings.UseWindowsAuthentication, Is.EqualTo(viewModel.UseWindowsAuth), "UseWindowsAuthentication");
		//	Assert.That(appSettings.Installed, Is.True, "Installed");
		//}

		//private string GetConfigPath(string filename)
		//{
		//	return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Integration", "Configuration", "TestConfigs", "XML", filename);
		//}
	}
}