using System.Web.Mvc;
using Roadkill.Core.Services;
using Roadkill.Core.Security;
using Roadkill.Core.Mvc.ViewModels;
using System.Linq;
using Roadkill.Core.AmazingConfig;

namespace Roadkill.Core.Mvc.Controllers
{
	/// <summary>
	/// Provides help for the 3 supported markup syntax.
	/// </summary>
	public class HelpController : ControllerBase
	{
		private readonly CustomTokenParser _customTokenParser;
		private readonly PageService _pageService;

		public HelpController(IConfigurationStore configurationStore, UserServiceBase userManager, IUserContext context, PageService pageService)
			: base(configurationStore, userManager, context) 
		{
			IConfiguration config = ConfigurationStore.Load();

			_customTokenParser = new CustomTokenParser(config.InternalSettings);
			_pageService = pageService;
		}

		public ActionResult Index()
		{
			return View();
		}

		public ActionResult About()
		{
			// Get the first page with an "about" tag
			PageViewModel model = _pageService.FindByTag("about").FirstOrDefault();
			if (model == null)
				return RedirectToAction("New", "Pages", new { title = "about", tags = "about" });
			else
				return View("../Wiki/Index", model);
		}

		public ActionResult CreoleReference()
		{
			return View(_customTokenParser.Tokens);
		}

		public ActionResult MediaWikiReference()
		{
			return View(_customTokenParser.Tokens);
		}

		public ActionResult MarkdownReference()
		{
			return View(_customTokenParser.Tokens);
		}
	}
}
