using System;
using System.Linq;
using System.Runtime.Caching;
using System.Web.Mvc;
using NUnit.Framework;
using Roadkill.Core;
using Roadkill.Core.AmazingConfig;
using Roadkill.Core.Cache;
using Roadkill.Core.Mvc.Controllers;
using Roadkill.Core.Mvc.ViewModels;
using Roadkill.Tests.Unit.StubsAndMocks;

namespace Roadkill.Tests.Unit.Mvc.Controllers.Admin
{
	[TestFixture]
	[Category("Unit")]
	public class SettingsControllerTests
	{
		private MocksAndStubsContainer _container;

		private IConfigurationStore _configurationStore;
		private IConfiguration _configuration;

		private IUserContext _context;
		private UserServiceMock _userService;
		private SiteCache _siteCache;
		private MemoryCache _cache;
		private WebConfigManagerStub _webConfigManager;

		private SettingsController _settingsController;

		[SetUp]
		public void Setup()
		{
			_container = new MocksAndStubsContainer();
			_container.ClearCache();

			_configurationStore = _container.ConfigurationStoreMock;
			_configuration = _container.Configuration;
			_configuration.AttachmentSettings.AttachmentsFolder = AppDomain.CurrentDomain.BaseDirectory;

			_context = _container.UserContext;
			_userService = _container.UserService;
			_siteCache = _container.SiteCache;
			_cache = _container.MemoryCache;
			_webConfigManager = new WebConfigManagerStub();

			_settingsController = new SettingsController(_configurationStore, _userService, _context, _siteCache);
		}

		[Test]
		public void index_get_should_return_view_and_viewmodel()
		{
			// Arrange

			// Act
			ViewResult result = _settingsController.Index() as ViewResult;

			// Assert
			Assert.That(result, Is.Not.Null, "ViewResult");
			SettingsViewModel model = result.ModelFromActionResult<SettingsViewModel>();
			Assert.That(model, Is.Not.Null, "model");
		}

		[Test]
		public void index_post_should_return_viewresult_and_save_settings()
		{
			// Arrange
			SettingsViewModel model = new SettingsViewModel();
			model.MenuMarkup = "some new markup";

			// Act
			ViewResult result = _settingsController.Index(model) as ViewResult;

			// Assert
			Assert.That(result, Is.Not.Null, "ViewResult");
			SettingsViewModel resultModel = result.ModelFromActionResult<SettingsViewModel>();
			Assert.That(resultModel, Is.Not.Null, "model");

			Assert.That(_configuration.MenuMarkup, Is.EqualTo("some new markup"));
		}

		[Test]
		public void index_post_should_accept_httppost_only()
		{
			// Arrange
			SettingsViewModel model = new SettingsViewModel();

			// Act
			ViewResult result = _settingsController.Index(model) as ViewResult;

			// Assert
			_settingsController.AssertHttpPostOnly(x => x.Index(model));
		}

		[Test]
		public void index_post_should_clear_site_cache()
		{
			// Arrange
			_siteCache.AddMenu("some menu");
			_siteCache.AddAdminMenu("admin menu");
			_siteCache.AddLoggedInMenu("logged in menu");

			SettingsViewModel model = new SettingsViewModel();

			// Act
			ViewResult result = _settingsController.Index(model) as ViewResult;

			// Assert
			Assert.That(_cache.Count(), Is.EqualTo(0));
		}
	}
}
