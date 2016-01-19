using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Roadkill.Core.AmazingConfig;
using Roadkill.Core.Configuration;
using Roadkill.Core.Converters;
using Roadkill.Core.Services;
using StructureMap;
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
