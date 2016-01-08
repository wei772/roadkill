using System.Web.Mvc;
using Roadkill.Core.AmazingConfig;
using Roadkill.Core.Cache;
using Roadkill.Core.Mvc.Attributes;
using Roadkill.Core.Mvc.ViewModels;
using Roadkill.Core.Plugins;
using Roadkill.Core.Security;

namespace Roadkill.Core.Mvc.Controllers
{
	/// <summary>
	/// Provides functionality for the cache management for admins.
	/// </summary>
	/// <remarks>All actions in this controller require admin rights.</remarks>
	[AdminRequired]
	public class CacheController : ControllerBase
	{
		private readonly ListCache _listCache;
		private readonly PageViewModelCache _pageViewModelCache;
		private readonly SiteCache _siteCache;
		private IPluginFactory _pluginFactory;

		public CacheController(IConfigurationStore configurationStore, UserServiceBase userService, IUserContext context,
			ListCache listCache, PageViewModelCache pageViewModelCache, SiteCache siteCache)
			: base(configurationStore, userService, context) 
		{
			_listCache = listCache;
			_pageViewModelCache = pageViewModelCache;
			_siteCache = siteCache;
		}

		/// <summary>
		/// Displays all items in the cache
		/// </summary>
		/// <returns></returns>
		[ImportModelState]
		public ActionResult Index()
		{
			IConfiguration config = ConfigurationStore.Load();

			CacheViewModel viewModel = new CacheViewModel()
			{
				IsCacheEnabled = config.UseObjectCache.GetValueOrDefault(true),
				PageKeys = _pageViewModelCache.GetAllKeys(),
				ListKeys = _listCache.GetAllKeys(),
				SiteKeys = _siteCache.GetAllKeys()
			};

			return View(viewModel);
		}

		/// <summary>
		/// Clears all items in the database caches (not sitecache)
		/// </summary>
		/// <param name="clear">If not empty, then signals the action to clear the cache</param>
		/// <returns></returns>
		[ExportModelState]
		[HttpPost]
		public ActionResult Clear()
		{
			_pageViewModelCache.RemoveAll();
			_listCache.RemoveAll();
			_siteCache.RemoveAll();
			TempData["CacheCleared"] = true;

			return RedirectToAction("Index");
		}
	}
}
