using System;
using System.Collections.Generic;
using System.Linq;
using Roadkill.Core.AmazingConfig;
using Roadkill.Core.DependencyResolution.StructureMap;
using StructureMap;
using StructureMap.Pipeline;
using StructureMap.Query;

namespace Roadkill.Core.Plugins
{
	/// <summary>
	/// The default <see cref="IPluginFactory"/> implementation, that uses the <see cref="StructureMapServiceLocator"/> class.
	/// </summary>
	public class PluginFactory : IPluginFactory
	{
		private readonly IContainer _container;

		public PluginFactory(IContainer container)
		{
			_container = container;
		}

		/// <summary>
		/// Allows additional text plugins to be registered at runtime.
		/// </summary>
		public void RegisterTextPlugin(TextPlugin plugin)
		{
			_container.Configure(x => x.For<TextPlugin>().Add(plugin));
		}

		/// <summary>
		/// Retrieves all text plugins from the IoC container.
		/// </summary>
		public IEnumerable<TextPlugin> GetTextPlugins()
		{
			return _container.GetAllInstances<TextPlugin>();
		}

		/// <summary>
		/// Retrieves all text plugins from the IoC container that are enabled according to the plugin settings file.
		/// </summary>
		public IEnumerable<TextPlugin> GetEnabledTextPlugins()
		{
			var configurationStore = _container.GetInstance<IConfigurationStore>();
			IPluginConfiguration configuration = configurationStore.LoadPluginConfiguration();

			var list = new List<TextPlugin>();
			foreach (TextPluginSettings setting in configuration.TextPluginSettings.Where(x => x.IsEnabled))
			{
				InstanceRef instanceRef = _container.Model.InstancesOf(Type.GetType(setting.TypeName)).FirstOrDefault();

				if (instanceRef != null)
				{
					list.Add(_container.GetInstance<TextPlugin>(instanceRef.Instance));
				}
			}

			return list;
		}

		/// <summary>
		/// Case insensitive search for a text plugin. Returns null if it doesn't exist.
		/// </summary>
		public TextPlugin GetTextPlugin(string id)
		{
			return _container.GetAllInstances<TextPlugin>().FirstOrDefault(x => x.Id.Equals(id, StringComparison.InvariantCultureIgnoreCase));
		}

		public IEnumerable<SpecialPagePlugin> GetSpecialPagePlugins()
		{
			return _container.GetAllInstances<SpecialPagePlugin>();
		}

		/// <summary>
		/// Case insensitive search for a special page plugin. Returns null if it doesn't exist.
		/// </summary>
		public SpecialPagePlugin GetSpecialPagePlugin(string name)
		{
			return _container.GetAllInstances<SpecialPagePlugin>().FirstOrDefault(x => x.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
		}
	}
}