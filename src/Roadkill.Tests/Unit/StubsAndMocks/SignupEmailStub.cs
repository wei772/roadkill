using Roadkill.Core.AmazingConfig;
using Roadkill.Core.Email;
using Roadkill.Core.Mvc.ViewModels;

namespace Roadkill.Tests.Unit.StubsAndMocks
{
	public class SignupEmailStub : SignupEmail
	{
		public bool IsSent { get; set; }
		public UserViewModel ViewModel { get; set; }

		public SignupEmailStub(IConfigurationStore configurationStore, IEmailClient emailClient)
			: base(configurationStore, emailClient)
		{
		}
		
		public override void Send(UserViewModel model)
		{
			ReplaceTokens(model, "{EMAIL}");
			IsSent = true;
			ViewModel = model;
		}
	}
}
