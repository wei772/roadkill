using System.Web.Mvc;
using NUnit.Framework;
using Roadkill.Core;
using Roadkill.Core.AmazingConfig;
using Roadkill.Core.Database;
using Roadkill.Core.Mvc.Controllers;
using Roadkill.Core.Security;
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

		private IUserContext _context;
		private UserServiceMock _userService;

		private WebConfigManagerStub _webConfigManager;

		private ControllerBaseStub _controller;
		private DatabaseTesterMock _databaseTester;
		private InstallationService _installationService;

		[SetUp]
		public void Setup()
		{
			_container = new MocksAndStubsContainer();
			_configurationStore = _container.ConfigurationStoreMock;
			_configuration = _container.Configuration;

			_context = _container.UserContext;
			_userService = _container.UserService;

			_controller = new ControllerBaseStub(_configurationStore, _userService, _context);
			_controller.SetFakeControllerContext("~/");

			// InstallController
			_webConfigManager = new WebConfigManagerStub();
			_databaseTester = _container.DatabaseTester;
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
			InstallControllerStub installController = new InstallControllerStub(_configurationStore, _webConfigManager, _installationService, _databaseTester);
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
			Assert.That(_controller.ViewBag.Config, Is.EqualTo(_configurationStore));
		}
	}


	//
	// These stubs let the tests call OnActionExecuting (without resorting to a tangle of Moq setups)
	//

	public class ControllerBaseStub : Roadkill.Core.Mvc.Controllers.ControllerBase
	{
		public ControllerBaseStub(IConfigurationStore configurationStore, UserServiceBase userService, IUserContext context) : base(configurationStore, userService, context)
		{

		}

		public void CallOnActionExecuting(ActionExecutingContext filterContext)
		{
			base.OnActionExecuting(filterContext);
		}
	}

	internal class InstallControllerStub : InstallController
	{
		public InstallControllerStub(IConfigurationStore configurationStore, IWebConfigManager webConfigManager, IInstallationService installationService, IDatabaseTester databaseTester)
			: base(configurationStore, webConfigManager, installationService)
		{

		}

		public void CallOnActionExecuting(ActionExecutingContext filterContext)
		{
			base.OnActionExecuting(filterContext);
		}
	}
}