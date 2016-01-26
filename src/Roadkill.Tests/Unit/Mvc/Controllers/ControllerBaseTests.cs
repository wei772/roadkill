using System.Web.Mvc;
using NUnit.Framework;
using Roadkill.Core;
using Roadkill.Core.AmazingConfig;
using Roadkill.Core.Services;
using Roadkill.Tests.Unit.StubsAndMocks;
using Roadkill.Tests.Unit.StubsAndMocks.Mvc;

namespace Roadkill.Tests.Unit.Mvc.Controllers
{
	[TestFixture]
	[Category("Unit")]
	public class ControllerBaseTests
	{
		private MocksAndStubsContainer _container;
		private IConfigurationStore _configurationStore;
		private IConfiguration _configuration;
		private WebConfigManagerStub _webconfigManager;

		private IUserContext _context;
		private UserServiceMock _userService;

		private ControllerBaseInvoker _controller;
		private InstallationService _installationService;

		[SetUp]
		public void Setup()
		{
			_container = new MocksAndStubsContainer();
			_configurationStore = _container.ConfigurationStoreMock;
			_configuration = _container.Configuration;
			_webconfigManager = _container.WebConfigManager;

			_context = _container.UserContext;
			_userService = _container.UserService;

			_controller = new ControllerBaseInvoker(_configurationStore, _userService, _context);
			_controller.SetFakeControllerContext("~/");

			// InstallController
			_installationService = _container.InstallationService;
		}

		[Test]
		public void should_redirect_when_installed_is_false()
		{
			// Arrange
			_configuration.Installed = false;
			ActionExecutingContext filterContext = new ActionExecutingContext();
			filterContext.Controller = _controller;

			// Act
			_controller.CallOnActionExecuting(filterContext);
			RedirectResult result = filterContext.Result as RedirectResult;

			// Assert
			Assert.That(result, Is.Not.Null, "RedirectResult");
			Assert.That(result.Url, Is.EqualTo("/install"));
		}

		[Test]
		public void should_not_redirect_when_installed_is_false_and_controller_is_installercontroller()
		{
			// Arrange
			_configuration.Installed = false;
			InstallControllerInvoker installController = new InstallControllerInvoker(_installationService, _configurationStore, _userService, _context, _webconfigManager);
			ActionExecutingContext filterContext = new ActionExecutingContext();
			filterContext.Controller = installController;

			// Act
			installController.CallOnActionExecuting(filterContext);

			// Assert
			Assert.That(filterContext.Result, Is.Null);
		}

		[Test]
		public void should_set_loggedin_user_and_viewbag_data()
		{
			// Arrange
			_configuration.Installed = true;
			_userService.LoggedInUserId = "mrblah";

			ActionExecutingContext filterContext = new ActionExecutingContext();
			filterContext.Controller = _controller;

			// Act
			_controller.CallOnActionExecuting(filterContext);

			// Assert
			Assert.That(_context.CurrentUsername, Is.EqualTo("mrblah"));
			Assert.That(_controller.ViewBag.Context, Is.EqualTo(_context));
			Assert.That(_controller.ViewBag.Config, Is.EqualTo(_configuration));
		}
	}
}