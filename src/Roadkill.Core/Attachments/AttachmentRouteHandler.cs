using Roadkill.Core.Services;
using System.Web;
using System.Web.Routing;
using Roadkill.Core.AmazingConfig;

namespace Roadkill.Core.Attachments
{
	/// <summary>
	/// A route handler for the attachments virtual folder. This route handler doesn't swallow MVC routes.
	/// </summary>
	public class AttachmentRouteHandler : IRouteHandler
	{
		private readonly IFileService _fileService;
		private readonly IConfigurationStore _configurationStore;

		/// <summary>
		/// Registers the attachments path route, using the configuration given in the application configuration.
		/// </summary>
		/// <param name="configurationStore">The configuration store.</param>
		/// <param name="routes">The routes.</param>
		/// <exception cref="ConfigurationException">
		/// The configuration is missing an attachments route path.
		/// or
		/// The attachmentsRoutePath in the config is set to 'files' which is not an allowed route path.
		/// </exception>
		public static void RegisterRoute(IConfigurationStore configurationStore, RouteCollection routes, IFileService fileService)
		{
			IConfiguration configuration = configurationStore.Load();

			if (string.IsNullOrEmpty(configuration.AttachmentSettings.AttachmentsRoutePath))
				throw new ConfigurationException("The configuration is missing an attachments route path, please enter one using attachmentsRoutePath=\"Attachments\"", null);

			if (configuration.AttachmentSettings.AttachmentsRoutePath.ToLower() == "files")
				throw new ConfigurationException("The attachmentsRoutePath in the config is set to 'files' which is not an allowed route path. Please change it to something else.", null);

			Route route = new Route(configuration.AttachmentSettings.AttachmentsRoutePath + "/{*filename}", new AttachmentRouteHandler(configurationStore, fileService));
			route.Constraints = new RouteValueDictionary();
			route.Constraints.Add("MvcContraint", new IgnoreMvcConstraint(configuration));

			routes.Add(route);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="AttachmentRouteHandler"/> class.
		/// </summary>
		/// <param name="configuration">The current application configuration.</param>
		public AttachmentRouteHandler(IConfigurationStore configurationStore, IFileService fileService)
		{
			if (configurationStore == null)
				throw new IoCException("The configurationStore parameter is null", null);

			if (fileService == null)
				throw new IoCException("The fileService parameter is null", null);

			_configurationStore = configurationStore;
			_fileService = fileService;
		}

		/// <summary>
		/// Provides the object that processes the request.
		/// </summary>
		/// <param name="requestContext">An object that encapsulates information about the request.</param>
		/// <returns>
		/// An object that processes the request.
		/// </returns>
		public IHttpHandler GetHttpHandler(RequestContext requestContext)
		{
			return new AttachmentFileHandler(_configurationStore, _fileService);
		}

		/// <summary>
		/// A route constraint for the attachments route, that ignores any controller or action route requests so only the
		/// /attachments/{filename} routes get through.
		/// </summary>
		public class IgnoreMvcConstraint : IRouteConstraint
		{
			private IConfiguration _configuration;

			public IgnoreMvcConstraint(IConfiguration configuration)
			{
				_configuration = configuration;
			}

			public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
			{
				if (routeDirection == RouteDirection.UrlGeneration)
					return false;
				if (values.ContainsKey("controller") || values.ContainsKey("action"))
					return false;

				if (route.Url.StartsWith(_configuration.AttachmentSettings.AttachmentsRoutePath +"/{*filename}"))
					return true;
				else
					return false;
			}
		}
	}
}
