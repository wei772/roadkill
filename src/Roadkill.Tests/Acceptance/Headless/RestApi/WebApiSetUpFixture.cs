using NUnit.Framework;

namespace Roadkill.Tests.Acceptance.Headless.RestApi
{
	[SetUpFixture]
	[Category("Acceptance")]
	public abstract class WebApiSetUpFixture
	{
		[OneTimeSetUp]
		public void TestFixtureSetUp()
		{
			TestHelpers.CreateIisTestSite();
		}
	}
}