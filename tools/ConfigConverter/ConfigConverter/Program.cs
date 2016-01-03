using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Newtonsoft.Json;
using Roadkill.Core.Configuration;
using Formatting = Newtonsoft.Json.Formatting;

namespace ConfigConverter
{
	class Program
	{
		static void Main(string[] args)
		{
			ConvertAll(args);
			return;

			if (args.Length == 0)
			{
				Console.WriteLine("Usage: configconverter.exe [full path to web.config/roadkill.config]");
				Console.WriteLine();
				Console.WriteLine("This will write a file called roadkill.json to the same directory as the source file.");
				return;
			}

			string sourcePath = args[0];
			var info = new FileInfo(sourcePath);
			string outputPath = Path.Combine(info.DirectoryName, "roadkill.json");
			
			var reader = new FullTrustConfigReaderWriter(sourcePath);
			IRoadkillConfiguration config = reader.Load();

			var jsonConfigObj = RoadkillConfiguration.Convert(config);

			string json = JsonConvert.SerializeObject(jsonConfigObj, typeof(IRoadkillConfiguration), Formatting.Indented, new JsonSerializerSettings());
			File.WriteAllText(outputPath, json);

			Console.WriteLine("Success! All config settings are now in '{0}'", outputPath);
			Console.WriteLine("Now copy this file into your website App_Data folder.");
		}

		static void ConvertAll(string[] args)
		{
			if (args.Length == 0)
			{
				Console.WriteLine("Usage: configconverter.exe [full path to web.config/roadkill.config]");
				Console.WriteLine();
				Console.WriteLine("This will write a file called roadkill.json to the same directory as the source file.");
				return;
			}

			string sourceDir = args[0];
			var info = new DirectoryInfo(sourceDir);

			foreach (string file in Directory.GetFiles(info.FullName, "*.config"))
			{
				string outputPath = Path.Combine(sourceDir, file.Replace(".config", ".json"));

				var reader = new FullTrustConfigReaderWriter(file);
				IRoadkillConfiguration config = reader.Load();

				var jsonConfigObj = RoadkillConfiguration.Convert(config);

				string json = JsonConvert.SerializeObject(jsonConfigObj, typeof(IRoadkillConfiguration), Formatting.Indented, new JsonSerializerSettings());
				File.WriteAllText(outputPath, json);

				Console.WriteLine("Success! All config settings are now in '{0}'", outputPath);
				Console.WriteLine("Now copy this file into your website App_Data folder.");
			}
		}
	}
}
