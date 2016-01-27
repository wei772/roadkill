using System;
using System.Configuration;
using Mindscape.LightSpeed;
using NUnit.Framework;
using Roadkill.Core.Database;
using Roadkill.Core.Database.LightSpeed;

namespace Roadkill.Tests.Acceptance.Headless.RestApi
{
	[TestFixture]
	[Category("Acceptance")]
	public abstract class WebApiTestBase
	{
		protected static readonly string ADMIN_EMAIL = TestConstants.ADMIN_EMAIL;
		protected static readonly string ADMIN_PASSWORD = TestConstants.ADMIN_PASSWORD;
		protected static readonly Guid ADMIN_ID = TestConstants.ADMIN_ID;
		protected string BaseUrl;

		[SetUp]
		public void Setup()
		{
			string url = ConfigurationManager.AppSettings["url"];
			if (string.IsNullOrEmpty(url))
				url = TestConstants.WEB_BASEURL;

			BaseUrl = url;

			TestHelpers.CopyDevWebConfig();
			TestHelpers.CopyDevConfiguration();
			TestHelpers.SqlServerSetup.RecreateTables();
			TestHelpers.RestartAppPool();
		}

		protected IPageRepository GetRepository()
		{
			var context = new LightSpeedContext();
			context.ConnectionString = TestConstants.CONNECTION_STRING;
			context.DataProvider = DataProvider.SqlServer2008;

			LightSpeedPageRepository repository = new LightSpeedPageRepository(context.CreateUnitOfWork());
			return repository;
		}

		protected PageContent AddPage(string title, string content)
		{
			using (IPageRepository repository = GetRepository())
			{
				Page page = new Page();
				page.Title = title;
				page.Tags = "tag1, tag2";
				page.CreatedBy = "admin";
				page.CreatedOn = DateTime.UtcNow;
				page.ModifiedOn = DateTime.UtcNow;
				page.ModifiedBy = "admin";

				return repository.AddNewPage(page, content, "admin", DateTime.UtcNow);
			}
		}
	}
}
