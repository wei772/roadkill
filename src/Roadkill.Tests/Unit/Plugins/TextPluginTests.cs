using System;
using System.Linq;
using Moq;
using NUnit.Framework;
using Roadkill.Core;
using Roadkill.Core.AmazingConfig;
using Roadkill.Core.Cache;
using Roadkill.Core.Plugins;
using Roadkill.Tests.Unit.StubsAndMocks;

namespace Roadkill.Tests.Unit.Plugins
{
	[TestFixture]
	[Category("Unit")]
	public class TextPluginTests
	{
		[Test]
		public void pluginvirtualpath_should_contain_plugin_id_and_no_trailing_slash()
		{
			// Arrange
			TextPluginStub plugin = new TextPluginStub("Plugin1", "name", "desc");

			// Act
			string virtualPath = plugin.PluginVirtualPath;

			// Assert
			Assert.That(virtualPath, Is.StringContaining("Plugin1"));
			Assert.That(virtualPath, Is.StringStarting("~/Plugins/"));
			Assert.That(virtualPath, Is.Not.StringEnding("/"));
		}

		[Test]
		[ExpectedException(typeof(PluginException))]
		public void PluginVirtualPath_Should_Throw_Exception_When_Id_Is_Empty()
		{
			// Arrange
			string id = "";
			TextPluginStub plugin = new TextPluginStub(id, "name", "description");

			// Act + Assert
			string path = plugin.PluginVirtualPath;
		}

		[Test]
		public void constructor_should_set_cacheable_to_true()
		{
			// Arrange
			TextPluginStub plugin = new TextPluginStub();

			// Act + Assert
			Assert.That(plugin.IsCacheable, Is.True);
		}

		[Test]
		public void getjavascripthtml_should_contain_scripts_with_headjs()
		{
			// Arrange
			TextPlugin plugin = new TextPluginStub();
			plugin.AddScript("pluginscript.js", "script1");
			string expectedHtml = @"<script type=""text/javascript"">" +
								@"head.js({ ""script1"", ""pluginscript.js"" },function() {  })" +
								"</script>\n";

			// Act
			string actualHtml = plugin.GetJavascriptHtml();

			// Assert
			Assert.That(actualHtml, Is.EqualTo(expectedHtml), actualHtml);
		}

		[Test]
		public void setheadjsonloadedfunction_should_be_added_to_javascript()
		{
			// Arrange
			TextPluginStub plugin = new TextPluginStub();
			plugin.AddScript("pluginscript.js", "script1");
			plugin.SetHeadJsOnLoadedFunction("alert('done')");

			string expectedHtml = @"<script type=""text/javascript"">" +
								@"head.js({ ""script1"", ""pluginscript.js"" },function() { alert('done') })" +
								"</script>\n";

			// Act
			string actualHtml = plugin.GetJavascriptHtml();

			// Assert
			Assert.That(actualHtml, Is.EqualTo(expectedHtml), actualHtml);
		}

		[Test]
		public void getcsslink_should_contain_file_and_expected_html()
		{
			// Arrange
			TextPluginStub plugin = new TextPluginStub("PluginId", "name", "desc");
			string expectedHtml = "\t\t" +
								 @"<link href=""~/Plugins/PluginId/file.css?version={PluginVersion}"" rel=""stylesheet"" type=""text/css"" />" +
								 "\n";

			expectedHtml = expectedHtml.Replace("{PluginVersion}", InternalSettings.ProductVersion);

			// Act
			string actualHtml = plugin.GetCssLink("file.css");

			// Assert
			Assert.That(actualHtml, Is.EqualTo(expectedHtml), actualHtml);
		}
	}
}