using Roadkill.Core.AmazingConfig;
using Roadkill.Core.Mvc.ViewModels;

namespace Roadkill.Core.Email
{
	/// <summary>
	/// The template for signup emails.
	/// </summary>
	public class SignupEmail : EmailTemplate
	{
		private static string _htmlContent;
		private static string _plainTextContent;

		public SignupEmail(IConfigurationStore configurationStore, IEmailClient emailClient)
			: base(configurationStore, emailClient)
		{
		}

		public override void Send(UserViewModel model)
		{
			// Thread safety should not be an issue here
			if (string.IsNullOrEmpty(_plainTextContent))
				_plainTextContent = ReadTemplateFile("Signup.txt");

			if (string.IsNullOrEmpty(_htmlContent))
				_htmlContent = ReadTemplateFile("Signup.html");

			PlainTextView = _plainTextContent;
			HtmlView = _htmlContent;

			base.Send(model);
		}
	}
}
