using Roadkill.Core.AmazingConfig;
using Roadkill.Core.Plugins;

namespace Roadkill.Tests.Unit.StubsAndMocks
{
	public class TextPluginStub : TextPlugin
	{
		public override string Id { get; }
		public override string Name { get; }
		public override string Description { get; }

		public string HeadContent { get; set; }
		public string FooterContent { get; set; }
		public string PreContainerHtml { get; set; }
		public string PostContainerHtml { get; set; }

		public TextPluginStub()
		{
			Id = "Amazing plugin";
			Name = "An amazing plugin";
			Description = "Amazing stubbed plugin";
		}

		internal TextPluginStub(IConfigurationStore configurationStore) : base(configurationStore)
		{
			Id = "Amazing plugin";
			Name = "An amazing plugin";
			Description = "Amazing stubbed plugin";
		}

		public TextPluginStub(string id, string name, string description)
		{
			Id = id;
			Name = name;
			Description = description;
		}

		public override string BeforeParse(string markupText)
		{
			return markupText.Replace("~~~usertoken~~~", "<span>usertoken</span>");
		}

		public override string AfterParse(string html)
		{
			return html.Replace("<strong>", "<strong style='color:green'><iframe src='javascript:alert(test)'>");
		}

		public override string GetHeadContent()
		{
			return HeadContent;
		}

		public override string GetFooterContent()
		{
			return FooterContent;
		}

		public override string GetPreContainerHtml()
		{
			return PreContainerHtml;
		}

		public override string GetPostContainerHtml()
		{
			return PostContainerHtml;
		}
	}
}
		