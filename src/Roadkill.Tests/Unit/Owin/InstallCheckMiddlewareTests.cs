using System;
using NUnit.Framework;
using Roadkill.Core.AmazingConfig;
using Roadkill.Core.Owin;
using Roadkill.Tests.Unit.StubsAndMocks.Owin;

namespace Roadkill.Tests.Unit.Owin
{
	public class InstallCheckMiddlewareTests
	{
		private MocksAndStubsContainer _container;
		private IConfiguration _configuration;

		[SetUp]
		public void Setup()
		{
			_container = new MocksAndStubsContainer();
			_configuration = _container.Configuration;
		}

		[Test]
		public void should_redirect_to_install_url_when_installed_is_false()
		{
			// Arrange
			var middleware = new InstallCheckMiddleware(null, _configuration);

			var context = new OwinContextStub();
			context.Request.Uri = new Uri("http://localhost/");
			context.Request.Accept = "text/html";

			// Act
			middleware.Invoke(context);

			// Assert
			Assert.That(context.Response.Headers["Location"], Is.EqualTo("/Install/"));
		}

		[Test]
		public void should_not_redirect_if_on_install_page()
		{
			// Arrange
			var middleware = new InstallCheckMiddleware(null, _configuration);

			var context = new OwinContextStub();
			context.Request.Uri = new Uri("http://localhost/Install/");
			context.Request.Accept = "text/html";

			// Act
			middleware.Invoke(context);

			// Assert
			Assert.That(context.Response.Headers["Location"], Is.Null.Or.Empty);
		}

		[Test]
		public void should_not_redirect_if_request_is_installer_asset()
		{
			// Arrange
			var middleware = new InstallCheckMiddleware(null, _configuration);

			var context = new OwinContextStub();
			context.Request.Uri = new Uri("http://localhost/Install/InstallerJsVars?version=2.0.400");
			context.Request.Accept = "application/javascript";

			// Act
			middleware.Invoke(context);

			// Assert
			Assert.That(context.Response.Headers["Location"], Is.Null.Or.Empty);
		}

		[Test]
		public void should_not_redirect_if_installed_is_true()
		{
			// Arrange
			var middleware = new InstallCheckMiddleware(null, _configuration);

			var context = new OwinContextStub();
			context.Request.Uri = new Uri("http://localhost/Install/");
			context.Request.Accept = "text/html";

			// Act
			middleware.Invoke(context);

			// Assert
			Assert.That(context.Response.Headers["Location"], Is.Null.Or.Empty);
		}
	}
}
