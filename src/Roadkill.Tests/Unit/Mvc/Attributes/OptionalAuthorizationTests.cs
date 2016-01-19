using System;
using System.Web;
using NUnit.Framework;
using Roadkill.Core;
using Roadkill.Core.AmazingConfig;
using Roadkill.Core.Database;
using Roadkill.Core.Mvc.Attributes;
using Roadkill.Tests.Unit.StubsAndMocks;
using Roadkill.Tests.Unit.StubsAndMocks.Mvc;

namespace Roadkill.Tests.Unit.Mvc.Attributes
{
	/// <summary>
	/// Setup-heavy tests for the AdminRequired attribute.
	/// </summary>
	[TestFixture]
	[Category("Unit")]
	public class OptionalAuthorizationTests
	{
		private MocksAndStubsContainer _container;
		private ConfigurationStoreMock _configurationStore;
		private IConfiguration _configuration;

		private UserServiceMock _userService;

		[SetUp]
		public void Setup()
		{
			_container = new MocksAndStubsContainer();
			_configurationStore = _container.ConfigurationStoreMock;
			_configuration = _container.Configuration;
			_configuration.SecuritySettings.AdminRoleName = "Admin";
			_configuration.SecuritySettings.EditorRoleName = "Editor";

			_userService = _container.UserService;
		}

		[Test]
		public void should_return_true_if_installed_is_false()
		{
			// Arrange
			_configuration.Installed = false;

			OptionalAuthorizationAttributeMock attribute = new OptionalAuthorizationAttributeMock();
			attribute.AuthorizationProvider = new AuthorizationProviderMock();
			attribute.ConfigurationStore = _configurationStore;
			attribute.UserService = _userService;

			IdentityStub identity = new IdentityStub() { Name = Guid.NewGuid().ToString(), IsAuthenticated = true };
			PrincipalStub principal = new PrincipalStub() { Identity = identity };
			HttpContextBase context = GetHttpContext(principal);

			// Act
			bool isAuthorized = attribute.CallAuthorize(context);

			// Assert
			Assert.That(isAuthorized, Is.True);
		}

		[Test]
		public void should_return_true_if_publicsite_is_true()
		{
			// Arrange
			_configuration.IsPublicSite = true;

			OptionalAuthorizationAttributeMock attribute = new OptionalAuthorizationAttributeMock();
			attribute.AuthorizationProvider = new AuthorizationProviderMock();
			attribute.ConfigurationStore = _configurationStore;
			attribute.UserService = _userService;

			IdentityStub identity = new IdentityStub() { Name = Guid.NewGuid().ToString(), IsAuthenticated = true };
			PrincipalStub principal = new PrincipalStub() { Identity = identity };
			HttpContextBase context = GetHttpContext(principal);

			// Act
			bool isAuthorized = attribute.CallAuthorize(context);

			// Assert
			Assert.That(isAuthorized, Is.True);
		}

		[Test]
		public void should_use_authorizationprovider_for_editors_when_publicsite_is_false()
		{
			// Arrange
			User editorUser = CreateEditorUser();

			OptionalAuthorizationAttributeMock attribute = new OptionalAuthorizationAttributeMock();
			attribute.AuthorizationProvider = new AuthorizationProviderMock() { IsEditorResult = true };
			attribute.ConfigurationStore = _configurationStore;
			attribute.UserService = _userService;

			IdentityStub identity = new IdentityStub() { Name = editorUser.Id.ToString(), IsAuthenticated = true };
			PrincipalStub principal = new PrincipalStub() { Identity = identity };
			HttpContextBase context = GetHttpContext(principal);

			// Act
			bool isAuthorized = attribute.CallAuthorize(context);

			// Assert
			Assert.That(isAuthorized, Is.True);
		}

		[Test]
		public void should_use_authorizationprovider_for_admin_when_publicsite_is_false()
		{
			// Arrange
			User adminUser = CreateAdminUser();

			OptionalAuthorizationAttributeMock attribute = new OptionalAuthorizationAttributeMock();
			attribute.AuthorizationProvider = new AuthorizationProviderMock() { IsEditorResult = true };
			attribute.ConfigurationStore = _configurationStore;
			attribute.UserService = _userService;

			IdentityStub identity = new IdentityStub() { Name = adminUser.Id.ToString(), IsAuthenticated = true };
			PrincipalStub principal = new PrincipalStub() { Identity = identity };
			HttpContextBase context = GetHttpContext(principal);

			// Act
			bool isAuthorized = attribute.CallAuthorize(context);

			// Assert
			Assert.That(isAuthorized, Is.True);
		}

		[Test]
		[ExpectedException(typeof(SecurityException))]
		public void Should_Throw_SecurityException_When_AuthorizationProvider_Is_Null()
		{
			// Arrange
			OptionalAuthorizationAttributeMock attribute = new OptionalAuthorizationAttributeMock();
			attribute.AuthorizationProvider = null;

			IdentityStub identity = new IdentityStub() { Name = Guid.NewGuid().ToString(), IsAuthenticated = true };
			PrincipalStub principal = new PrincipalStub() { Identity = identity };
			HttpContextBase context = GetHttpContext(principal);

			// Act + Assert
			attribute.CallAuthorize(context);
		}

		protected HttpContextBase GetHttpContext(PrincipalStub principal)
		{
			MvcMockContainer container = new MvcMockContainer();
			HttpContextBase context = MvcMockHelpers.FakeHttpContext(container);
			container.Context.SetupProperty(x => x.User, principal);

			return context;
		}

		private User CreateAdminUser()
		{
			_userService.AddUser("admin@localhost", "admin", "password", true, false);
			User user = _userService.GetUser("admin@localhost", false);
			user.IsActivated = true;

			return user;
		}

		private User CreateEditorUser()
		{
			_userService.AddUser("editor@localhost", "editor", "password", false, true);
			User user = _userService.GetUser("editor@localhost", false);
			user.IsActivated = true;

			return user;
		}
	}

	internal class OptionalAuthorizationAttributeMock : OptionalAuthorizationAttribute
	{
		public bool CallAuthorize(HttpContextBase context)
		{
			return base.AuthorizeCore(context);
		}
	}
}