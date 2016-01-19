using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using Roadkill.Core.AmazingConfig;
using StructureMap.Attributes;

namespace Roadkill.Core.Mvc.WebApi
{
	public class ApiKeyAuthorizeAttribute : AuthorizeAttribute
	{
		[SetterProperty]
		public IConfigurationStore ConfigurationStore { get; set; }

		public static readonly string APIKEY_HEADER_KEY = "Authorization";

		public override void OnAuthorization(HttpActionContext actionContext)
		{
			if (!actionContext.Request.Headers.Contains(APIKEY_HEADER_KEY))
			{
				actionContext.Response = new HttpResponseMessage(HttpStatusCode.BadRequest);
				return;
			}

			IConfiguration configuration = ConfigurationStore.Load();
			string keyValue = actionContext.Request.Headers.GetValues(APIKEY_HEADER_KEY).First();

			if (!configuration.SecuritySettings.ApiKeys.Contains(keyValue))
				actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
		}
	}
}