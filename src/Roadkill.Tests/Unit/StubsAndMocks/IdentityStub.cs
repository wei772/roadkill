using System.Security.Principal;

namespace Roadkill.Tests.Unit.StubsAndMocks
{
	public class IdentityStub : IIdentity
	{
		public string AuthenticationType { get; set; }
		public bool IsAuthenticated { get; set; }
		public string Name { get; set; }
	}
}
