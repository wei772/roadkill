using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Web.Mvc;
using NUnit.Framework;
using Roadkill.Core;
using Roadkill.Core.AmazingConfig;
using Roadkill.Core.Cache;
using Roadkill.Core.Mvc.Controllers;
using Roadkill.Core.Mvc.ViewModels;
using Roadkill.Core.Plugins;
using Roadkill.Tests.Unit.StubsAndMocks;

namespace Roadkill.Tests.Unit.Mvc.Controllers
{
	[TestFixture]
	[Category("Unit")]
	public class PluginSettingsControllerTests
	{
		private MocksAndStubsContainer _container;

		private IConfigurationStore _configurationStore;
		private IUserContext _context;

		private UserServiceMock _userService;
		private PluginFactoryMock _pluginFactory;
		private ListCache _listCache;
		private SiteCache _siteCache;
		private PageViewModelCache _pageViewModelCache;
		private MemoryCache _memoryCache;

		private PluginSettingsController _controller;
		private IConfiguration _configuration;

		[SetUp]
		public void Setup()
		{
			_container = new MocksAndStubsContainer(true);

			_configurationStore = _container.ConfigurationStoreMock;
			_configuration = _container.Configuration;
			_configuration.UseObjectCache = true;

			_context = _container.UserContext;

			_pluginFactory = _container.PluginFactory;
			_userService = _container.UserService;

			_listCache = _container.ListCache;
			_siteCache = _container.SiteCache;
			_pageViewModelCache = _container.PageViewModelCache;
			_memoryCache = _container.MemoryCache;

			_controller = new PluginSettingsController(_configurationStore, _userService, _context, _pluginFactory, _siteCache, _pageViewModelCache, _listCache);
		}

		[Test]
		public void index_should_return_viewresult_and_model_with_2_pluginmodels_ordered_by_name()
		{
			// Arrange
			TextPluginStub pluginB = new TextPluginStub("b id", "b name", "b desc");
			TextPluginStub pluginA = new TextPluginStub("a id", "a name", "a desc");

			_pluginFactory.RegisterTextPlugin(pluginB); // reverse the order to test the ordering
			_pluginFactory.RegisterTextPlugin(pluginA);

			// Act
			ViewResult result = _controller.Index() as ViewResult;

			// Assert
			Assert.That(result, Is.Not.Null);
			IEnumerable<PluginViewModel> pluginModels = result.ModelFromActionResult<IEnumerable<PluginViewModel>>();
			Assert.NotNull(pluginModels, "Null model");

			List<PluginViewModel> pageModelList = pluginModels.ToList();

			Assert.That(pageModelList.Count(), Is.EqualTo(2));
			Assert.That(pageModelList[0].Name, Is.EqualTo("a name"));
			Assert.That(pageModelList[1].Name, Is.EqualTo("b name"));
		}

		[Test]
		public void edit_get_should_return_viewresult_and_model_with_known_values()
		{
			// Arrange		
			TextPluginStub plugin = new TextPluginStub();

			_pluginFactory.RegisterTextPlugin(plugin);

			// Act
			ViewResult result = _controller.Edit(plugin.Id) as ViewResult;

			// Assert
			Assert.That(result, Is.Not.Null);
			PluginViewModel model = result.ModelFromActionResult<PluginViewModel>();
			Assert.NotNull(model, "Null model");

			Assert.That(model.Id, Is.EqualTo(plugin.Id));
			Assert.That(model.Name, Is.EqualTo(plugin.Name));
			Assert.That(model.Description, Is.EqualTo(plugin.Description));
		}

		[Test]
		public void edit_get_should_load_settings_from_repository()
		{
			// Arrange
			TextPluginStub plugin = new TextPluginStub();

			_pluginFactory.RegisterTextPlugin(plugin);

			// Act
			ViewResult result = _controller.Edit(plugin.Id) as ViewResult;

			// Assert
			PluginViewModel model = result.ModelFromActionResult<PluginViewModel>();
			Assert.That(model.SettingValues[0].Value, Is.EqualTo("value1"));
			Assert.That(model.SettingValues[1].Value, Is.EqualTo("value2"));
		}

		[Test]
		public void edit_get_should_use_default_plugin_settings_when_plugin_doesnt_exist_in_repository()
		{
			// Arrange
			TextPluginStub plugin = new TextPluginStub();

			_pluginFactory.RegisterTextPlugin(plugin);

			// Act
			ViewResult result = _controller.Edit(plugin.Id) as ViewResult;

			// Assert
			PluginViewModel model = result.ModelFromActionResult<PluginViewModel>();
			Assert.That(model.SettingValues[0].Value, Is.EqualTo("default-value1"));
			Assert.That(model.SettingValues[1].Value, Is.EqualTo("default-value2"));
		}

		[Test]
		public void edit_get_should_redirect_when_id_is_empty()
		{
			// Arrange

			// Act
			RedirectToRouteResult result = _controller.Edit("") as RedirectToRouteResult;

			// Assert
			Assert.That(result, Is.Not.Null);
		}

		[Test]
		public void edit_get_should_redirect_when_plugin_does_not_exist()
		{
			// Arrange

			// Act
			RedirectToRouteResult result = _controller.Edit("somepluginId") as RedirectToRouteResult;

			// Assert
			Assert.That(result, Is.Not.Null);
		}

		[Test]
		public void edit_post_should_save_setting_values_to_repository_from_model_and_clear_sitecache()
		{
			// Arrange
			_pageViewModelCache.Add(1, new PageViewModel()); // dummmy items
			_listCache.Add("a key", new List<string>() { "1", "2" });

			TextPluginStub plugin = new TextPluginStub();

			_pluginFactory.RegisterTextPlugin(plugin);

			PluginViewModel model = new PluginViewModel();
			model.Id = plugin.Id;
			model.SettingValues = new List<SettingValue>();
			model.SettingValues.Add(new SettingValue() { Name = "name1", Value = "new-value1" });
			model.SettingValues.Add(new SettingValue() { Name = "name2", Value = "new-value2" });

			// Act
			ViewResult result = _controller.Edit(model) as ViewResult;

			Assert.That(_memoryCache.Count(), Is.EqualTo(0));
		}

		[Test]
		public void edit_post_should_redirect_when_plugin_does_not_exist()
		{
			// Arrange

			// Act
			RedirectToRouteResult result = _controller.Edit("somepluginId") as RedirectToRouteResult;

			// Assert
			Assert.That(result, Is.Not.Null);
		}
	}
}
