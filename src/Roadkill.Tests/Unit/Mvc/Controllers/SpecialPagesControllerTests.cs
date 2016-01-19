using System.Web;
using System.Web.Mvc;
using NUnit.Framework;
using Roadkill.Core;
using Roadkill.Core.AmazingConfig;
using Roadkill.Core.Mvc.Controllers;
using Roadkill.Tests.Unit.StubsAndMocks;

namespace Roadkill.Tests.Unit.Mvc.Controllers
{
	[TestFixture]
	[Category("Unit")]
	public class SpecialPagesControllerTests
	{
		private MocksAndStubsContainer _container;
		private IConfigurationStore _configurationStore;

		private IUserContext _context;
		private UserServiceMock _userService;
		private PluginFactoryMock _pluginFactory;

		private SpecialPagesController _specialPagesController;

		[SetUp]
		public void Setup()
		{
			_container = new MocksAndStubsContainer();
			_configurationStore = _container.ConfigurationStoreMock;

			_context = _container.UserContext;
			_pluginFactory = _container.PluginFactory;
			_userService = _container.UserService;

			_specialPagesController = new SpecialPagesController(_configurationStore, _userService, _context, _pluginFactory);
		}

		[Test]
		public void index_should_call_plugin_getresult()
		{
			// Arrange
			_pluginFactory.SpecialPages.Add(new SpecialPageMock());

			// Act
			ContentResult result = _specialPagesController.Index("kay") as ContentResult;

			// Assert
			Assert.That(result, Is.Not.Null);
		}

		[Test]
		public void index_should_throw_httpexception_when_plugin_does_not_exist()
		{
			// Arrange + Act + Assert
			HttpException httpException = Assert.Throws<HttpException>(() => _specialPagesController.Index("badID"));
			Assert.That(httpException.GetHttpCode(), Is.EqualTo(404));
		}
	}
}
