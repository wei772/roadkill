using System;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;
using NUnit.Framework;
using Roadkill.Core;
using Roadkill.Core.Database;
using Roadkill.Core.DependencyResolution;
using Roadkill.Core.DependencyResolution.StructureMap;
using Roadkill.Core.Mvc.ViewModels;
using Roadkill.Tests.Unit.StubsAndMocks;
using StructureMap;

namespace Roadkill.Tests.Unit.DependencyResolution
{
	[TestFixture]
	[Category("Unit")]
	public class LocatorStartupTests
	{
		private MocksAndStubsContainer _container;
		private ConfigurationStoreMock _configurationStore;

		[SetUp]
		public void Setup()
		{
			_container = new MocksAndStubsContainer();
			_configurationStore = _container.ConfigurationStoreMock;

			MockServiceLocator();
		}

		[TearDown]
		public void TearDown()
		{
			try
			{
				// Clear down the Microsoft's statics
				ModelBinders.Binders.Clear();
				GlobalConfiguration.Configuration.Services.RemoveAll(typeof(IFilterProvider), o => true);
			}
			catch (Exception)
			{
			}
        }

		private void MockServiceLocator()
		{
			var registry = new RoadkillRegistry(_configurationStore);
			var container = new Container(registry);
			container.Configure(x =>
			{
				x.Scan(a => a.AssemblyContainingType<TestHelpers>());
				x.For<IPageRepository>().Use(new PageRepositoryMock());
				x.For<IUserContext>().Use(new UserContextStub());
			});

			LocatorStartup.Locator = new StructureMapServiceLocator(container, false);
			DependencyResolver.SetResolver(LocatorStartup.Locator);

			var all =
				container.Model.AllInstances.OrderBy(t => t.PluginType.Name)
					.Select(t => String.Format("{0}:{1}", t.PluginType.Name, t.ReturnedType.AssemblyQualifiedName));

			Console.WriteLine(String.Join("\n", all));
		}

		[Test]
		public void StartMVCInternal_should_create_service_locator_and_set_mvc_service_locator()
		{
			// Arrange
			var registry = new RoadkillRegistry(_configurationStore);

			// Act
			LocatorStartup.StartMVCInternal(registry, false);

			// Assert
			Assert.That(LocatorStartup.Locator, Is.Not.Null);
			Assert.That(DependencyResolver.Current, Is.EqualTo(LocatorStartup.Locator));
		}

		[Test]
		public void AfterInitializationInternal_should_register_webapi_servicelocator_and_attributeprovider()
		{
			// Arrange
			var registry = new RoadkillRegistry(_configurationStore);
			var container = new Container(registry);

			LocatorStartup.StartMVCInternal(registry, false); // needed to register LocatorStartup.Locator

			// Act
			LocatorStartup.AfterInitializationInternal(container);

			// Assert
			Assert.That(GlobalConfiguration.Configuration.DependencyResolver, Is.EqualTo(LocatorStartup.Locator));

			// Doesn't work...maybe it will work in 2016
			//Assert.That(GlobalConfiguration.Configuration.Services.GetService(typeof(System.Web.Http.Filters.IFilterProvider)), Is.TypeOf<MvcAttributeProvider>());
		}

		[Test]
		public void AfterInitializationInternal_should_register_mvc_attributes_and_modelbinders()
		{
			// Arrange
			var registry = new RoadkillRegistry(_configurationStore);
			var container = new Container(registry);

			LocatorStartup.StartMVCInternal(registry, false);

			// Act
			LocatorStartup.AfterInitializationInternal(container);

			// Assert
			Assert.True(ModelBinders.Binders.ContainsKey(typeof(SettingsViewModel)));
			Assert.True(ModelBinders.Binders.ContainsKey(typeof(UserViewModel)));
		}
	}
}