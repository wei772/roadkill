using Roadkill.Core.AmazingConfig;

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

		public void WriteForFormsAuth()
		{
		}

		public void WriteForWindowsAuth()
		{
		}

		public string IsWriteable()
		{
			return "";
		}
	}
}
