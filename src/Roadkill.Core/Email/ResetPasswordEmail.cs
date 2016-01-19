using Roadkill.Core.AmazingConfig;
using Roadkill.Core.Mvc.ViewModels;

namespace Roadkill.Core.Email
{
	/// <summary>
	/// The template for password reset emails.
	/// </summary>
	public class ResetPasswordEmail : EmailTemplate
	{
		private static string _htmlContent;
		private static string _plainTextContent;

		public ResetPasswordEmail(IConfigurationStore configurationStore, IEmailClient emailClient)
			: base(configurationStore, emailClient)
		{
		}

		public override void Send(UserViewModel model)
		{
			// Thread safety should not be an issue here
			if (string.IsNullOrEmpty(_plainTextContent))
				_plainTextContent = ReadTemplateFile("ResetPassword.txt");

			if (string.IsNullOrEmpty(_htmlContent))
				_htmlContent = ReadTemplateFile("ResetPassword.html");

			PlainTextView = _plainTextContent;
			HtmlView = _htmlContent;

			base.Send(model);
		}
	}
}
