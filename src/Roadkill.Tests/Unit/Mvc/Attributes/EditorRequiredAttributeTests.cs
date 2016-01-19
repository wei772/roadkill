using System;
using System.Web;
using NUnit.Framework;
using Roadkill.Core;
using Roadkill.Core.AmazingConfig;
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
	public class EditorRequiredAttributeTests
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

			_userService = _container.UserService;

			_configuration.SecuritySettings.AdminRoleName = "Admin";
			_configuration.SecuritySettings.EditorRoleName = "Editor";
		}

		[Test]
		public void should_use_authorizationprovider()
		{
			// Arrange
			EditorRequiredAttributeMock attribute = new EditorRequiredAttributeMock();
			attribute.AuthorizationProvider = new AuthorizationProviderMock() { IsEditorResult = true };
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
		[ExpectedException(typeof(SecurityException))]
		public void Should_Throw_SecurityException_When_AuthorizationProvider_Is_Null()
		{
			// Arrange
			EditorRequiredAttributeMock attribute = new EditorRequiredAttributeMock();
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
	}

	internal class EditorRequiredAttributeMock : EditorRequiredAttribute
	{
		public bool CallAuthorize(HttpContextBase context)
		{
			return base.AuthorizeCore(context);
		}
	}
}