using Roadkill.Core.AmazingConfig;
using Roadkill.Core.Email;
using Roadkill.Core.Mvc.ViewModels;

namespace Roadkill.Tests.Unit.StubsAndMocks
{
	public class ResetPasswordEmailStub : ResetPasswordEmail
	{
		public bool IsSent { get; set; }
		public UserViewModel Model { get; set; }

		public ResetPasswordEmailStub(IConfigurationStore configurationStore, IEmailClient emailClient)
			: base(configurationStore, emailClient)
		{
		}

		public override void Send(UserViewModel model)
		{
			ReplaceTokens(model, "{EMAIL}");
			IsSent = true;
			Model = model;
		}
	}
}