using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using NUnit.Framework;
using Roadkill.Core.AmazingConfig;
using Roadkill.Core.Cache;
using Roadkill.Tests.Unit.StubsAndMocks;

namespace Roadkill.Tests.Unit.Cache
{
	[TestFixture]
	[Category("Unit")]
	public class SiteCacheTests
	{
		[Test]
		public void addmenu_should_cache_html()
		{
			// Arrange
			CacheMock cache = new CacheMock();
			SiteCache siteCache = new SiteCache(cache);

			// Act
			siteCache.AddMenu("some html");

			// Assert
			Assert.That(cache.Count(), Is.EqualTo(1));
			IEnumerable<string> keys = cache.Select(x => x.Key);
			Assert.That(keys, Contains.Item(CacheKeys.MenuKey()));
		}

		[Test]
		public void addadminmenu_should_cache_html()
		{
			// Arrange
			CacheMock cache = new CacheMock();
			SiteCache siteCache = new SiteCache(cache);

			// Act
			siteCache.AddAdminMenu("some html");

			// Assert
			Assert.That(cache.Count(), Is.EqualTo(1));
			IEnumerable<string> keys = cache.Select(x => x.Key);
			Assert.That(keys, Contains.Item(CacheKeys.AdminMenuKey()));
		}

		[Test]
		public void addloggedinmenu_should_cache_html()
		{
			// Arrange
			CacheMock cache = new CacheMock();
			SiteCache siteCache = new SiteCache(cache);

			// Act
			siteCache.AddLoggedInMenu("some html");

			// Assert
			Assert.That(cache.Count(), Is.EqualTo(1));
			IEnumerable<string> keys = cache.Select(x => x.Key);
			Assert.That(keys, Contains.Item(CacheKeys.LoggedInMenuKey()));
		}

		[Test]
		public void getmenu_should_return_correct_html()
		{
			// Arrange
			string expectedHtml = "some html";

			CacheMock cache = new CacheMock();
			SiteCache siteCache = new SiteCache(cache);
			siteCache.AddMenu(expectedHtml);

			// Act
			string actualHtml = siteCache.GetMenu();

			// Assert
			Assert.That(actualHtml, Is.EqualTo(expectedHtml));
		}

		[Test]
		public void getadminmenu_should_return_correct_html()
		{
			// Arrange
			string expectedHtml = "some html";

			CacheMock cache = new CacheMock();
			SiteCache siteCache = new SiteCache(cache);
			siteCache.AddAdminMenu(expectedHtml);

			// Act
			string actualHtml = siteCache.GetAdminMenu();

			// Assert
			Assert.That(actualHtml, Is.EqualTo(expectedHtml));
		}

		[Test]
		public void getloggedinmenu_should_return_correct_html()
		{
			// Arrange
			string expectedHtml = "some html";
			CacheMock cache = new CacheMock();
			SiteCache siteCache = new SiteCache(cache);
			siteCache.AddLoggedInMenu(expectedHtml);

			// Act
			string actualHtml = siteCache.GetLoggedInMenu();

			// Assert
			Assert.That(actualHtml, Is.EqualTo(expectedHtml));
		}

		[Test]
		public void removemenucacheitems_should_clear_cache_items()
		{
			// Arrange
			CacheMock cache = new CacheMock();

			SiteCache siteCache = new SiteCache(cache);
			siteCache.AddMenu("menu html");
			siteCache.AddLoggedInMenu("logged in menu html");
			siteCache.AddAdminMenu("admin menu html");

			// Act
			siteCache.RemoveMenuCacheItems();

			// Assert
			Assert.That(cache.Count(), Is.EqualTo(0));
		}

		[Test]
		public void removeall_should_remove_sitecache_keys_only()
		{
			// Arrange
			CacheMock cache = new CacheMock();
			cache.Add("list.blah", "xyz", new CacheItemPolicy());
			SiteCache siteCache = new SiteCache(cache);

			siteCache.AddMenu("menu html");
			siteCache.AddLoggedInMenu("logged in menu html");
			siteCache.AddAdminMenu("admin menu html");

			// Act
			siteCache.RemoveAll();

			// Assert
			Assert.That(cache.Count(), Is.EqualTo(1));
		}
	}
}
