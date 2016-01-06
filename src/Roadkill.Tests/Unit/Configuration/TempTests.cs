using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NUnit.Framework;
using Roadkill.Core.AmazingConfig;

namespace Roadkill.Tests.Unit.Configuration
{
	public class TempTests
	{
		[Test]
		public void method_should()
		{
			// Arrange
			//string settings = JsonConvert.SerializeObject(new JsonConfiguration(), Formatting.Indented);
			var config = JsonConvert.DeserializeObject<JsonConfiguration>(File.ReadAllText(@"d:\test.txt"));

			// Act
			Console.WriteLine(config);

			// Assert
		}
	}
}
