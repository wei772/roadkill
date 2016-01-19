using Roadkill.Core.AmazingConfig;
using Roadkill.Core.Mvc.ViewModels;

namespace Roadkill.Tests.Unit.StubsAndMocks
{
	public class WebConfigManagerStub : IWebConfigManager
	{
		public string ConfigFilePath { get; set; }

		public string UILanguageCode { get; set; }
		public bool Saved { get; set; }
		public string TestWebConfigResult { get; set; }

		public WebConfigManagerStub()
		{
			TestWebConfigResult = "OK";
		}


		public virtual void UpdateLanguage(string uiLanguageCode)
		{
			UILanguageCode = uiLanguageCode;
		}

		public virtual void Save(SettingsViewModel settings)
		{
			Saved = true;
		}
	}
}
