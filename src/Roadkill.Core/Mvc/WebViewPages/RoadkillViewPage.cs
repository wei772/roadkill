using System.Web.Mvc;
using Roadkill.Core.AmazingConfig;
using Roadkill.Core.Converters;
using StructureMap.Attributes;

namespace Roadkill.Core.Mvc.WebViewPages
{
	public abstract class RoadkillViewPage<T> : WebViewPage<T>
	{
		[SetterProperty]
		public IConfigurationStore ConfigurationStore { get; set; }
		
		[SetterProperty]
		public IUserContext RoadkillContext { get; set; }
		
		[SetterProperty]
		public MarkupConverter MarkupConverter { get; set; }
	}
}
