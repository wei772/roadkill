using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Roadkill.Core.AmazingConfig;

namespace Roadkill.Core.Owin
{
	public class InstallCheckMiddleware : OwinMiddleware
	{
		private readonly IConfiguration _configuration;

		public InstallCheckMiddleware(OwinMiddleware next, IConfiguration configuration) : base(next)
		{
			_configuration = configuration;
		}

		public override async Task Invoke(IOwinContext context)
		{
			if (_configuration.Installed == false && IsOnInstallPage(context) == false && IsHtmlRequest(context))
			{
				context.Response.Redirect("/Install/");
			}
			else
			{
				await Next.Invoke(context);
			}
		}

		private static bool IsHtmlRequest(IOwinContext context)
		{
			return !string.IsNullOrEmpty(context.Request.Accept) && context.Request.Accept.Contains("text/html");
		}

		private bool IsOnInstallPage(IOwinContext context)
		{
			return context.Request.Uri.PathAndQuery.StartsWith("/Install/", StringComparison.InvariantCultureIgnoreCase);
		}
	}
}