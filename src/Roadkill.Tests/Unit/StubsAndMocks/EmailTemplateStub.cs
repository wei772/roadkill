using Roadkill.Core.AmazingConfig;
using Roadkill.Core.Email;

namespace Roadkill.Tests.Unit.StubsAndMocks
{
	public class EmailTemplateStub : EmailTemplate
	{
		public EmailTemplateStub(IConfigurationStore configurationStore, IEmailClient emailClient)
			: base(configurationStore, emailClient)
		{
			base.PlainTextView = "plaintextview";
			base.HtmlView = "htmlview";
		}

		public IEmailClient GetEmailClient()
		{
			return base.EmailClient;
		}
	}
}
