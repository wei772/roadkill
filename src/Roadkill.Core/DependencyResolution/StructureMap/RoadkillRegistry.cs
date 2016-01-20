using System;
using System.IO;
using System.Runtime.Caching;
using System.Web.Http;
using Roadkill.Core.AmazingConfig;
using Roadkill.Core.Attachments;
using Roadkill.Core.Cache;
using Roadkill.Core.Converters;
using Roadkill.Core.Database;
using Roadkill.Core.Domain.Export;
using Roadkill.Core.Email;
using Roadkill.Core.Import;
using Roadkill.Core.Mvc.Attributes;
using Roadkill.Core.Mvc.Controllers;
using Roadkill.Core.Mvc.ViewModels;
using Roadkill.Core.Mvc.WebApi;
using Roadkill.Core.Mvc.WebViewPages;
using Roadkill.Core.Plugins;
using Roadkill.Core.Security;
using Roadkill.Core.Security.Windows;
using Roadkill.Core.Services;
using StructureMap;
using StructureMap.Building;
using StructureMap.Graph;
using StructureMap.Graph.Scanning;
using StructureMap.Pipeline;
using StructureMap.TypeRules;
using StructureMap.Web;
using UserController = Roadkill.Core.Mvc.Controllers.UserController;

namespace Roadkill.Core.DependencyResolution.StructureMap
{
	public class RoadkillRegistry : Registry
	{
		public IConfiguration Configuration { get; set; }

		public RoadkillRegistry(IConfigurationStore configurationStore)
		{
			Configuration = configurationStore.Load();

			Scan(ScanTypes);
			ConfigureInstances(configurationStore);
		}

		private static void CopyPlugins(InternalSettings settings)
		{
			string pluginsDestPath = settings.PluginsBinPath;
			if (!Directory.Exists(pluginsDestPath))
				Directory.CreateDirectory(pluginsDestPath);

			PluginFileManager.CopyPlugins(settings);
		}

		private void ScanTypes(IAssemblyScanner scanner)
		{
			scanner.TheCallingAssembly();
			scanner.AssembliesFromApplicationBaseDirectory(assembly => assembly.FullName.Contains("Roadkill"));
			scanner.SingleImplementationsOfInterface();
			scanner.WithDefaultConventions();

			// Scan plugins: this includes everything e.g repositories, UserService, FileService TextPlugins
			CopyPlugins(Configuration.InternalSettings);
			foreach (string subDirectory in Directory.GetDirectories(Configuration.InternalSettings.PluginsBinPath))
			{
				scanner.AssembliesFromPath(subDirectory);
			}

			// Plugins
			scanner.With(new AbstractClassConvention<TextPlugin>());
			scanner.With(new AbstractClassConvention<SpecialPagePlugin>());
            scanner.AddAllTypesOf<IPluginFactory>();

			// New config
			scanner.AddAllTypesOf<IConfiguration>();
			scanner.AddAllTypesOf<IConfigurationStore>();
			scanner.AddAllTypesOf<IWebConfigManager>();

			// UserContext
			scanner.AddAllTypesOf<IUserContext>();

			// Repositories
			scanner.AddAllTypesOf<IUserRepository>();
			scanner.AddAllTypesOf<IPageRepository>();

			// Services
			scanner.With(new AbstractClassConvention<UserServiceBase>());
			scanner.AddAllTypesOf<IPageService>();
			scanner.AddAllTypesOf<ISearchService>();
			scanner.AddAllTypesOf<IActiveDirectoryProvider>();
			scanner.AddAllTypesOf<IFileService>();
			scanner.AddAllTypesOf<IInstallationService>();

			// Text parsers
			scanner.AddAllTypesOf<MarkupConverter>();
			scanner.AddAllTypesOf<CustomTokenParser>();

			// MVC Related
			scanner.AddAllTypesOf<UserViewModel>();
			scanner.AddAllTypesOf<SettingsViewModel>();
			scanner.AddAllTypesOf<AttachmentRouteHandler>();
			scanner.AddAllTypesOf<ISetterInjected>();
			scanner.AddAllTypesOf<IAuthorizationAttribute>();
			scanner.AddAllTypesOf<RoadkillLayoutPage>();
			scanner.AddAllTypesOf(typeof(RoadkillViewPage<>));
			scanner.ConnectImplementationsToTypesClosing(typeof(RoadkillViewPage<>));

			// Emails
			scanner.AddAllTypesOf<SignupEmail>();
			scanner.AddAllTypesOf<ResetPasswordEmail>();

			// Cache
			scanner.AddAllTypesOf<ListCache>();
			scanner.AddAllTypesOf<PageViewModelCache>();

			// Export
			scanner.AddAllTypesOf<WikiExporter>();

			// Controllers
			scanner.AddAllTypesOf<IRoadkillController>();
			scanner.AddAllTypesOf<ControllerBase>();
			scanner.AddAllTypesOf<ApiController>();
			scanner.AddAllTypesOf<ConfigurationTesterController>();
		}

		private void ConfigureInstances(IConfigurationStore configurationStore)
		{
			// New config
			For<IConfigurationStore>().Use(configurationStore).Singleton();
			For<IWebConfigManager>().Use<WebConfigManager>().Singleton();

			// Repositories
			ConfigureRepositories();

			// AlwaysUnique: this is a work around for controllers that use RenderAction() needing to be unique
			// See https://github.com/webadvanced/Structuremap.MVC5/issues/3
			For<HomeController>().AlwaysUnique();
			For<UserController>().AlwaysUnique();
			For<ConfigurationTesterController>().AlwaysUnique();
			For<WikiController>().AlwaysUnique();

			// Plugins
			For<IPluginFactory>().Singleton().Use<PluginFactory>();

			// Screwturn importer
			For<IWikiImporter>().Use<ScrewTurnImporter>();

			// Emails
			For<SignupEmail>().Use<SignupEmail>();
			For<ResetPasswordEmail>().Use<ResetPasswordEmail>();

			// Cache
			For<ObjectCache>().Singleton().Use(new MemoryCache("Roadkill"));
			For<ListCache>().Singleton();
			For<SiteCache>().Singleton();
			For<PageViewModelCache>().Singleton();

			// Services
			For<IPageService>().HybridHttpOrThreadLocalScoped().Use<PageService>();
			For<IInstallationService>().HybridHttpOrThreadLocalScoped().Use<InstallationService>();

			// Security
			For<IAuthorizationProvider>().Use<AuthorizationProvider>();
			For<IUserContext>().HybridHttpOrThreadLocalScoped();
#if !MONO
			For<IActiveDirectoryProvider>().Use<ActiveDirectoryProvider>();
#endif
			// User service
			ConfigureUserService();

			// File service
			ConfigureFileService();

			// Setter injected classes
			ConfigureSetterInjection();
		}

		private void ConfigureRepositories()
		{
			// TODO: All services should take an IRepositoryFactory, no injection should be needed for IXYZRepository
			For<IRepositoryFactory>()
				.Singleton()
				.Use<RepositoryFactory>("IRepositoryFactory", x =>
				{
					var configurationStore = x.GetInstance<IConfigurationStore>();
					IConfiguration configuration = configurationStore.Load();

					return new RepositoryFactory(configuration.DatabaseProvider, configuration.ConnectionString);
				});

			For<IUserRepository>()
				.HybridHttpOrThreadLocalScoped()
				.Use("IUserRepository", x =>
				{
					var configurationStore = x.GetInstance<IConfigurationStore>();
					IConfiguration configuration = configurationStore.Load();

					return x.TryGetInstance<IRepositoryFactory>()
						.GetUserRepository(configuration.DatabaseProvider, configuration.ConnectionString);
				});

			For<IPageRepository>()
				.HybridHttpOrThreadLocalScoped()
				.Use("IPageRepository", x =>
				{
					var configurationStore = x.GetInstance<IConfigurationStore>();
					IConfiguration configuration = configurationStore.Load();

					return x.TryGetInstance<IRepositoryFactory>()
						.GetPageRepository(configuration.DatabaseProvider, configuration.ConnectionString);
				});
		}

		private void ConfigureSetterInjection()
		{
			Policies.SetAllProperties(x => x.OfType<TextPlugin>());
			Policies.SetAllProperties(x => x.OfType<ApiKeyAuthorizeAttribute>());
			Policies.SetAllProperties(x => x.OfType<ISetterInjected>());
			Policies.SetAllProperties(x => x.OfType<IAuthorizationAttribute>());
			Policies.SetAllProperties(x => x.TypeMatches(t => t == typeof (RoadkillViewPage<>)));
			Policies.SetAllProperties(x => x.TypeMatches(t => t == typeof (RoadkillLayoutPage)));
		}

		private void ConfigureFileService()
		{
			if (Configuration.AttachmentSettings.UseAzureFileStorage)
			{
				For<IFileService>()
					.HybridHttpOrThreadLocalScoped()
					.Use<AzureFileService>();
			}
			else
			{
				For<IFileService>()
					.HybridHttpOrThreadLocalScoped()
					.Use<LocalFileService>();
			}
		}

		private void ConfigureUserService()
		{
			// Windows authentication, custom or the default FormsAuth
			string userServiceTypeName = Configuration.SecuritySettings.UserServiceType;

			if (Configuration.SecuritySettings.UseWindowsAuthentication)
			{
#if !MONO
				For<UserServiceBase>()
					.HybridHttpOrThreadLocalScoped()
					.Use<ActiveDirectoryUserService>();
#endif
			}
			else if (!string.IsNullOrEmpty(userServiceTypeName))
			{
				try
				{
					Type userServiceType = Type.GetType(userServiceTypeName, false, false);
					if (userServiceType == null)
						throw new IoCException(null, "Unable to find UserService type {0}. Make sure you use the AssemblyQualifiedName.", userServiceTypeName);

					For<UserServiceBase>()
						.Use("Inject custom UserService", context =>
						{					
							return (UserServiceBase) context.GetInstance(userServiceType);
						});
				}
				catch (StructureMapBuildException)
				{
					throw new IoCException(null, "Unable to find UserService type {0}", userServiceTypeName);
				}
			}
			else
			{
				For<UserServiceBase>()
					.HybridHttpOrThreadLocalScoped()
					.Use<FormsAuthUserService>();
			}
		}

		private class AbstractClassConvention<T> : IRegistrationConvention
		{
			public void ScanTypes(TypeSet types, Registry registry)
			{
				foreach (Type type in types.FindTypes(TypeClassification.Concretes | TypeClassification.Closed))
				{
					if (type.CanBeCastTo<T>())
					{
						registry.For(typeof(T)).LifecycleIs(new UniquePerRequestLifecycle()).Add(type);
					}
				}
			}
		}
	}
}