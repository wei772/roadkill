using System;
using System.Web;
using System.Web.Mvc;
using Moq;
using NUnit.Framework;
using Roadkill.Core.AmazingConfig;
using Roadkill.Core.Cache;
using Roadkill.Core.Database;
using Roadkill.Core.Services;
using Roadkill.Core.Mvc.Attributes;
using Roadkill.Core.Mvc.Controllers;
using Roadkill.Tests.Unit.StubsAndMocks;
using Roadkill.Tests.Unit.StubsAndMocks.Mvc;

namespace Roadkill.Tests.Unit.Mvc.Attributes
{
	[TestFixture]
	[Category("Unit")]
	public class BrowserCacheAttributeTests
	{
		private MocksAndStubsContainer _container;
		private IConfigurationStore _configurationStore;
		private IConfiguration _configuration;

		private PluginFactoryMock _pluginFactory;
		private PageRepositoryMock _pageRepository;

		private readonly DateTime _pageCreatedDate = DateTime.Today;
		private readonly DateTime _pageModifiedDate = DateTime.Today;
		private readonly DateTime _pluginLastSavedDate = DateTime.Today;

		[SetUp]
		public void Setup()
		{
			_container = new MocksAndStubsContainer();
			_configurationStore = _container.ConfigurationStoreMock;
			_configuration = _container.Configuration;
			_configuration.PluginLastSaveDate = _pluginLastSavedDate;

			_pluginFactory = _container.PluginFactory;
			_pageRepository = _container.PageRepository;
		}

		[Test]
		public void should_not_set_viewresult_if_not_installed()
		{
			// Arrange
			BrowserCacheAttribute attribute = new BrowserCacheAttribute();
			attribute.ConfigurationStore = _configurationStore;

			WikiController controller = CreateWikiController(attribute);
			ResultExecutedContext filterContext = CreateContext(controller);

			_configuration.Installed = false;

			// Act
			attribute.OnResultExecuted(filterContext);

			// Assert
			Assert.That(filterContext.Result, Is.Not.TypeOf<HttpStatusCodeResult>());
		}

		[Test]
		public void should_not_set_viewresult_if_usebrowsercache_is_disabled()
		{
			// Arrange
			BrowserCacheAttribute attribute = new BrowserCacheAttribute();
			attribute.ConfigurationStore = _configurationStore;

			WikiController controller = CreateWikiController(attribute);
			ResultExecutedContext filterContext = CreateContext(controller);

			_configuration.UseBrowserCache = false;

			// Act
			attribute.OnResultExecuted(filterContext);

			// Assert
			Assert.That(filterContext.Result, Is.Not.TypeOf<HttpStatusCodeResult>());
		}

		[Test]
		public void should_not_set_viewresult_if_user_is_logged_in()
		{
			// Arrange
			BrowserCacheAttribute attribute = new BrowserCacheAttribute();
			attribute.ConfigurationStore = _configurationStore;

			WikiController controller = CreateWikiController(attribute);
			ResultExecutedContext filterContext = CreateContext(controller);

			attribute.Context.CurrentUser = Guid.NewGuid().ToString();

			// Act
			attribute.OnResultExecuted(filterContext);

			// Assert
			Assert.That(filterContext.Result, Is.Not.TypeOf<HttpStatusCodeResult>());
		}

		[Test]
		public void should_have_200_http_status_code_if_no_modified_since_header()
		{
			// Arrange
			BrowserCacheAttribute attribute = new BrowserCacheAttribute();
			attribute.ConfigurationStore = _configurationStore;

			WikiController controller = CreateWikiController(attribute);
			ResultExecutedContext filterContext = CreateContext(controller);

			// Act
			attribute.OnResultExecuted(filterContext);

			// Assert
			Assert.That(filterContext.HttpContext.Response.StatusCode, Is.EqualTo(200));
			Assert.That(filterContext.Result, Is.Not.TypeOf<HttpStatusCodeResult>());
		}

		[Test]
		public void should_have_200_http_status_code_if_pluginssaved_after_header_last_modified_date()
		{
			// Arrange
			BrowserCacheAttribute attribute = new BrowserCacheAttribute();
			attribute.ConfigurationStore = _configurationStore;
			_configuration.PluginLastSaveDate = DateTime.UtcNow;

			WikiController controller = CreateWikiController(attribute);
			ResultExecutedContext filterContext = CreateContext(controller);
			filterContext.HttpContext.Request.Headers.Add("If-Modified-Since", DateTime.Today.ToUniversalTime().ToString("r"));

			// Act
			attribute.OnResultExecuted(filterContext);

			// Assert
			Assert.That(filterContext.HttpContext.Response.StatusCode, Is.EqualTo(200));
			Assert.That(filterContext.Result, Is.Not.TypeOf<HttpStatusCodeResult>());
		}

		[Test]
		public void should_have_304_http_status_code_if_pluginssaved_is_equal_to_header_last_modified_date()
		{
			// Arrange
			BrowserCacheAttribute attribute = new BrowserCacheAttribute();
			attribute.ConfigurationStore = _configurationStore;
			_configuration.PluginLastSaveDate = DateTime.Today.ToUniversalTime().AddHours(1);

			WikiController controller = CreateWikiController(attribute);
			ResultExecutedContext filterContext = CreateContext(controller);
			filterContext.HttpContext.Request.Headers.Add("If-Modified-Since", DateTime.Today.AddHours(1).ToUniversalTime().ToString("r"));

			// Act
			attribute.OnResultExecuted(filterContext);

			// Assert
			Assert.That(filterContext.HttpContext.Response.StatusCode, Is.EqualTo(304));
			Assert.That(filterContext.Result, Is.TypeOf<HttpStatusCodeResult>());
		}

		[Test]
		public void should_have_304_http_status_code_if_response_has_modified_since_header_matching_page_modified_date()
		{
			// The file date and the browser date always match for a 304 status, the browser will never send back a more recent date,
			// i.e. "Has the file changed since this date I've stored for the last time it was changed?"
			// and *not* "has the file changed since the time right now?". 

			// Arrange
			BrowserCacheAttribute attribute = new BrowserCacheAttribute();

			WikiController controller = CreateWikiController(attribute);
			ResultExecutedContext filterContext = CreateContext(controller);

			// (page modified date is set in CreateWikiController)
			filterContext.HttpContext.Request.Headers.Add("If-Modified-Since", DateTime.Today.ToString("r"));

			// Act
			attribute.OnResultExecuted(filterContext);

			// Assert
			HttpStatusCodeResult result = filterContext.Result as HttpStatusCodeResult;
			Assert.That(filterContext.HttpContext.Response.StatusCode, Is.EqualTo(304));
			Assert.That(result.StatusCode, Is.EqualTo(304));
			Assert.That(result.StatusDescription, Is.EqualTo("Not Modified"));
		}

		private WikiController CreateWikiController(BrowserCacheAttribute attribute)
		{
			// Settings
			_configuration.Installed = true;
			_configuration.UseBrowserCache = true;
			UserContextStub userContext = new UserContextStub() { IsLoggedIn = false };

			// PageService
			PageViewModelCache pageViewModelCache = new PageViewModelCache(_configurationStore, CacheMock.RoadkillCache);
			ListCache listCache = new ListCache(_configurationStore, CacheMock.RoadkillCache);
			SiteCache siteCache = new SiteCache(CacheMock.RoadkillCache);
			SearchServiceMock searchService = new SearchServiceMock(_configurationStore, _pageRepository, _pluginFactory);
			PageHistoryService historyService = new PageHistoryService(_configurationStore, _pageRepository, userContext, pageViewModelCache, _pluginFactory);
			PageService pageService = new PageService(_configurationStore, _pageRepository, searchService, historyService, userContext, listCache, pageViewModelCache, siteCache, _pluginFactory);

			// WikiController
			var userManager = new UserServiceStub();
			var wikiController = new WikiController(_configurationStore, userManager, pageService, userContext);

			// Create a page that the request is for
			Page page = new Page() { Title = "title", ModifiedOn = _pageModifiedDate };
			_pageRepository.AddNewPage(page, "text", "user", _pageCreatedDate);

			// Update the BrowserCacheAttribute
			attribute.ConfigurationStore = _configurationStore;
			attribute.Context = userContext;
			attribute.PageService = pageService;

			return wikiController;
		}

		private ResultExecutedContext CreateContext(WikiController wikiController)
		{
			// HTTP Context
			ControllerContext controllerContext = new Mock<ControllerContext>().Object;
			MvcMockContainer container = new MvcMockContainer();
			HttpContextBase context = MvcMockHelpers.FakeHttpContext(container);
			controllerContext.HttpContext = context;

			// ResultExecutedContext
			ActionResult result = new ViewResult();
			Exception exception = new Exception();
			bool cancelled = true;

			ResultExecutedContext filterContext = new ResultExecutedContext(controllerContext, result, cancelled, exception);
			filterContext.Controller = wikiController;
			filterContext.RouteData.Values.Add("id", 1);
			filterContext.HttpContext = context;

			return filterContext;
		}
	}
}