using System.IO;
using NUnit.Framework;
using Roadkill.Core;
using Roadkill.Core.AmazingConfig;
using Roadkill.Tests.Unit.StubsAndMocks;

namespace Roadkill.Tests.Unit.Text
{
	[TestFixture]
	[Category("Unit")]
	public class CustomTokenParserTests
	{
		private MocksAndStubsContainer _container;
		private ConfigurationStoreMock _configurationStore;
		private IConfiguration _configuration;

		[TestFixtureSetUp]
		public void TestFixtureSetUp()
		{
			CustomTokenParser.CacheTokensFile = false;

			_container = new MocksAndStubsContainer();
			_configurationStore = _container.ConfigurationStoreMock;
			_configuration = _container.Configuration;
		}

		[Test]
		public void should_contain_empty_list_when_tokens_file_not_found()
		{
			// Arrange
			_configuration.InternalSettings.CustomTokensPath = Path.Combine(TestConstants.WEB_PATH, "doesntexist.xml");
			CustomTokenParser parser = new CustomTokenParser(_configurationStore);

			string expectedHtml = "@@warningbox:ENTER YOUR CONTENT HERE {{some link}}@@";

			// Act
			string actualHtml = parser.ReplaceTokensAfterParse("@@warningbox:ENTER YOUR CONTENT HERE {{some link}}@@");

			// Assert
			Assert.That(actualHtml, Is.EqualTo(expectedHtml));
		}

		[Test]
		public void should_contain_empty_list_when_when_deserializing_bad_xml_file()
		{
			// Arrange
			_configuration.InternalSettings.CustomTokensPath = Path.Combine(TestConstants.ROOT_FOLDER, "readme.md"); // use a markdown file
			string expectedHtml = "@@warningbox:ENTER YOUR CONTENT HERE {{some link}}@@";

			// Act
			CustomTokenParser parser = new CustomTokenParser(_configurationStore);
			string actualHtml = parser.ReplaceTokensAfterParse("@@warningbox:ENTER YOUR CONTENT HERE {{some link}}@@");

			// Assert
			Assert.That(actualHtml, Is.EqualTo(expectedHtml));
		}

		[Test]
		public void warningbox_token_should_return_html_fragment()
		{
			// Arrange
			_configuration.InternalSettings.CustomTokensPath = Path.Combine(TestConstants.WEB_PATH, "App_Data", "customvariables.xml");
			CustomTokenParser parser = new CustomTokenParser(_configurationStore);

			string expectedHtml = @"<div class=""alert alert-warning"">ENTER YOUR CONTENT HERE {{some link}}</div><br style=""clear:both""/>";

			// Act
			string actualHtml = parser.ReplaceTokensAfterParse("@@warningbox:ENTER YOUR CONTENT HERE {{some link}}@@");

			// Assert
			Assert.That(actualHtml, Is.EqualTo(expectedHtml));
		}
	}
}
