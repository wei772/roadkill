using NUnit.Framework;
using Roadkill.Plugins.Text.BuiltIn;

namespace Roadkill.Tests.Unit.Plugins
{
	[TestFixture]
	[Category("Unit")]
	public class JumbotronTests
	{
		private MocksAndStubsContainer _container;

		[SetUp]
		public void Setup()
		{
			_container = new MocksAndStubsContainer();
		}

		[Test]
		public void should_remove_jumbotron_tag_from_markup()
		{
			// Arrange
			string markup = "Here is some #Heading 1 markup\n[[[jumbotron=\n#Welcome\n##This the subheading]]]";
			Jumbotron jumbotron = new Jumbotron(_container.MarkupConverter);

			// Act
			string actualMarkup = jumbotron.BeforeParse(markup);

			// Assert
			Assert.That(actualMarkup, Is.EqualTo("Here is some #Heading 1 markup\n"));
		}

		[Test]
		public void should_parse_and_fill_precontainerhtml()
		{
			// Arrange
			string markup = "Here is some #Heading 1 markup\n[[[jumbotron=#Welcome\n##This the subheading]]]";
			string expectedHtml = Jumbotron.HTMLTEMPLATE.Replace("${inner}", "<h1>Welcome</h1>\n\n<h2>This the subheading</h2>\n");

			Jumbotron jumbotron = new Jumbotron(_container.MarkupConverter);

			// Act
			jumbotron.BeforeParse(markup);
			string actualHtml = jumbotron.GetPreContainerHtml();

			// Assert
			Assert.That(actualHtml, Is.EqualTo(expectedHtml));
		}
	}
}
