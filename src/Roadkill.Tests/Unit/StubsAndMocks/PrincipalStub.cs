using System.Security.Principal;

namespace Roadkill.Tests.Unit.StubsAndMocks
{
	public class PrincipalStub : IPrincipal
	{
		public IIdentity Identity { get; set; }
		public bool IsInRoleResult { get; set; }

		public bool IsInRole(string role)
		{
			return IsInRoleResult;
		}

		public void SetAuthenticate(bool authenticated)
		{
			((IdentityStub)Identity).IsAuthenticated = authenticated;
		}
	}
}
