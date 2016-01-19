using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using Moq;
using NUnit.Framework;
using Roadkill.Core;
using Roadkill.Core.AmazingConfig;
using Roadkill.Core.Extensions;
using Roadkill.Core.Mvc.Controllers;
using Roadkill.Core.Mvc.ViewModels;
using Roadkill.Core.Services;
using Roadkill.Tests.Unit.StubsAndMocks;
using Roadkill.Tests.Unit.StubsAndMocks.Mvc;

namespace Roadkill.Tests.Unit.Extensions
{
	[TestFixture]
	[Category("Unit")]
	public class HtmlHelperLinkExtensionsTests
	{
		// Objects for the HtmlHelper
		private MocksAndStubsContainer _container;
		private IConfigurationStore _configurationStore;
		private IConfiguration _configuration;

		private IUserContext _context;
		private UserServiceMock _userService;
		private PageService _pageService;
		private WikiController _wikiController;
		private HtmlHelper _htmlHelper;
		private ViewContext _viewContext;

		[SetUp]
		public void Setup()
		{
			// WikiController setup (use WikiController as it's the one typically used by views)
			_container = new MocksAndStubsContainer();
			_configurationStore = _container.ConfigurationStoreMock;
			_configuration = _container.Configuration;

			_context = _container.UserContext;
			_userService = _container.UserService;
			_pageService = _container.PageService;

			_wikiController = new WikiController(_configurationStore, _userService, _pageService, _context);
			_wikiController.SetFakeControllerContext("~/wiki/index/1");

			// HtmlHelper setup
			var viewDataDictionary = new ViewDataDictionary();
			_viewContext = new ViewContext(_wikiController.ControllerContext, new Mock<IView>().Object, viewDataDictionary, new TempDataDictionary(), new StringWriter());
			var mockViewDataContainer = new Mock<IViewDataContainer>();
			mockViewDataContainer.Setup(v => v.ViewData).Returns(viewDataDictionary);

			_htmlHelper = new HtmlHelper(_viewContext, mockViewDataContainer.Object);
		}

		[Test]
		public void loginstatus_should_contain_link_and_username_in_text_when_user_is_loggedin()
		{
			// Arrange
			_context.CurrentUser = "editor";
			string expectedHtml = "<a href=\"/user/profile\">Logged in as editor</a>";

			// Act
			string actualHtml = _htmlHelper.LoginStatus().ToString();

			// Assert
			Assert.That(actualHtml, Is.EqualTo(expectedHtml));
		}

		[Test]
		public void loginstatus_should_contain_login_link_and_guest_in_text_when_user_is_anonymous()
		{
			// Arrange
			_context.CurrentUser = "";
			string expectedHtml = "Not logged in";

			// Act
			string actualHtml = _htmlHelper.LoginStatus().ToString();

			// Assert
			Assert.That(actualHtml, Is.EqualTo(expectedHtml));
		}

		[Test]
		public void settingslink_should_render_link_html_when_logged_in_as_admin()
		{
			// Arrange
			_userService.AddUser("admin@localhost", "admin", "password", true, true);
			Guid userId = _userService.ListAdmins().First().Id.Value;
			_context.CurrentUser = userId.ToString();

			string expectedHtml = "@<a href=\"/settings\">Site settings</a>~";

			// Act
			string actualHtml = _htmlHelper.SettingsLink("@","~").ToString();

			// Assert
			Assert.That(actualHtml, Is.EqualTo(expectedHtml));
		}

		[Test]
		public void settingslink_should_not_render_html_when_logged_in_as_editor()
		{
			// Arrange
			_userService.AddUser("editor@localhost", "admin", "password", false, true);
			Guid userId = _userService.ListEditors().First().Id.Value;
			_context.CurrentUser = userId.ToString();

			string expectedHtml = "";

			// Act
			string actualHtml = _htmlHelper.SettingsLink("@", "~").ToString();

			// Assert
			Assert.That(actualHtml, Is.EqualTo(expectedHtml));
		}

		[Test]
		public void settingslink_should_not_render_html_when_anonymous_user()
		{
			// Arrange
			_context.CurrentUser = "";
			string expectedHtml = "";

			// Act
			string actualHtml = _htmlHelper.SettingsLink("@", "~").ToString();

			// Assert
			Assert.That(actualHtml, Is.EqualTo(expectedHtml));
		}

		[Test]
		public void filemanagerlink_should_render_link_html_when_logged_in_as_admin()
		{
			// Arrange
			_userService.AddUser("admin@localhost", "admin", "password", true, true);
			Guid userId = _userService.ListAdmins().First().Id.Value;
			_context.CurrentUser = userId.ToString();

			string expectedHtml = "@<a href=\"/filemanager\">Manage files</a>~";

			// Act
			string actualHtml = _htmlHelper.FileManagerLink("@", "~").ToString();

			// Assert
			Assert.That(actualHtml, Is.EqualTo(expectedHtml));
		}

		[Test]
		public void filemanagerlink_should_render_link_html_when_logged_in_as_editor()
		{
			// Arrange
			_userService.AddUser("editor@localhost", "editor", "password", false, true);
			Guid userId = _userService.ListEditors().First().Id.Value;
			_context.CurrentUser = userId.ToString();

			string expectedHtml = "@<a href=\"/filemanager\">Manage files</a>~";

			// Act
			string actualHtml = _htmlHelper.FileManagerLink("@", "~").ToString();

			// Assert
			Assert.That(actualHtml, Is.EqualTo(expectedHtml));
		}

		[Test]
		public void filemanagerlink_should_not_render_html_when_anonymous_user()
		{
			// Arrange
			_context.CurrentUser = "";
			string expectedHtml = "";

			// Act
			string actualHtml = _htmlHelper.FileManagerLink("@", "~").ToString();

			// Assert
			Assert.That(actualHtml, Is.EqualTo(expectedHtml));
		}

		[Test]
		public void loginlink_should_render_login_html_when_anonymous_user()
		{
			// Arrange
			_context.CurrentUser = "";
			string expectedHtml = "@<a href=\"/user/login?returnurl=~%2fwiki%2findex%2f1\">Login</a>~";

			// Act
			string actualHtml = _htmlHelper.LoginLink("@", "~").ToString();

			// Assert
			Assert.That(actualHtml, Is.EqualTo(expectedHtml));
		}

		[Test]
		public void loginlink_should_render_logout_html_when_logged_in_as_editor()
		{
			// Arrange
			_userService.AddUser("editor@localhost", "editor", "password", false, true);
			Guid userId = _userService.ListEditors().First().Id.Value;
			_context.CurrentUser = userId.ToString();

			string expectedHtml = "@<a href=\"/user/logout\">Logout</a>~";

			// Act
			string actualHtml = _htmlHelper.LoginLink("@", "~").ToString();

			// Assert
			Assert.That(actualHtml, Is.EqualTo(expectedHtml));
		}

		[Test]
		public void loginlink_should_return_empty_string_when_windows_auth_is_enabled()
		{
			// Arrange
			_configuration.SecuritySettings.UseWindowsAuthentication = true;
			string expectedHtml = "";

			// Act
			string actualHtml = _htmlHelper.LoginLink("@", "~").ToString();

			// Assert
			Assert.That(actualHtml, Is.EqualTo(expectedHtml));
		}

		[Test]
		public void newpagelink_should_render_link_html_when_logged_in_as_admin()
		{
			// Arrange
			_userService.AddUser("admin@localhost", "admin", "password", true, true);
			Guid userId = _userService.ListAdmins().First().Id.Value;
			_context.CurrentUser = userId.ToString();

			string expectedHtml = "@<a href=\"/pages/new\">New page</a>~";

			// Act
			string actualHtml = _htmlHelper.NewPageLink("@", "~").ToString();

			// Assert
			Assert.That(actualHtml, Is.EqualTo(expectedHtml));
		}

		[Test]
		public void newpagelink_should_render_link_html_when_logged_in_as_editor()
		{
			// Arrange
			_userService.AddUser("editor@localhost", "editor", "password", false, true);
			Guid userId = _userService.ListEditors().First().Id.Value;
			_context.CurrentUser = userId.ToString();

			string expectedHtml = "@<a href=\"/pages/new\">New page</a>~";

			// Act
			string actualHtml = _htmlHelper.NewPageLink("@", "~").ToString();

			// Assert
			Assert.That(actualHtml, Is.EqualTo(expectedHtml));
		}

		[Test]
		public void newpagelink_should_not_render_html_when_anonymous_user()
		{
			// Arrange
			_context.CurrentUser = "";
			string expectedHtml = "";

			// Act
			string actualHtml = _htmlHelper.NewPageLink("@", "~").ToString();

			// Assert
			Assert.That(actualHtml, Is.EqualTo(expectedHtml));
		}

		[Test]
		public void mainpagelink_should_render_html_with_home_link()
		{
			// Arrange
			string expectedHtml = "@<a href=\"/\">the fun starts here</a>~";

			// Act
			string actualHtml = _htmlHelper.MainPageLink("the fun starts here", "@", "~").ToString();

			// Assert
			Assert.That(actualHtml, Is.EqualTo(expectedHtml));
		}


		[Test]
		public void pagelink_should_render_html_link_with_page_title_and_html_attributes()
		{
			// Arrange
			_pageService.AddPage(new PageViewModel() { Id = 7, Title = "Crispy Pancake Recipe" });
			string expectedHtml = "@<a data-merry=\"xmas\" href=\"/wiki/1/crispy%20pancake%20recipe\">captains log</a>~"; // the url will always be /wiki/1 because of the mock url setup

			// Act
			string actualHtml = _htmlHelper.PageLink("captains log", "Crispy Pancake Recipe", new { data_merry = "xmas" }, "@", "~", _pageService).ToString();

			// Assert
			Assert.That(actualHtml, Is.EqualTo(expectedHtml));
		}

		[Test]
		public void pagelink_should_render_html_with_no_link_when_page_does_not_exist()
		{
			// Arrange
			string expectedHtml = "captains log"; // the url will always be /wiki/1 because of the mock url setup

			// Act
			string actualHtml = _htmlHelper.PageLink("captains log", "Random page that doesnt exist", new { data_merry = "xmas" }, "@", "~", _pageService).ToString();

			// Assert
			Assert.That(actualHtml, Is.EqualTo(expectedHtml));
		}
	}
}
