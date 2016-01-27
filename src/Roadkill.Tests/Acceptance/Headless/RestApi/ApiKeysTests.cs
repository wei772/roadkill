using System;
using System.IO;
using System.Net;
using NUnit.Framework;
using RestSharp;
using Roadkill.Core.AmazingConfig;

namespace Roadkill.Tests.Acceptance.Headless.RestApi
{
	[TestFixture]
	[Category("Acceptance")]
	[Parallelizable(ParallelScope.Fixtures)]
	public class ApiKeysTests : WebApiTestBase
	{
		private static void RemoveApiKeys()
		{
			try
			{
				string configPath = Path.Combine(TestConstants.WEB_PATH, "App_Data", "configuration.json");
				var configStore = new JsonConfigurationStore(configPath, "");

				IConfiguration config = configStore.Load();
				config.SecuritySettings.ApiKeys = "";

				configStore.Save(config);
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}
		}

		[Test]
		public void wrong_apikey_in_header_should_return_401_unauthorized()
		{
			// Arrange
			WebApiClient apiclient = new WebApiClient();
			apiclient.ApiKey = "bad api key";

			// Act
			WebApiResponse response = apiclient.Get("User");

			// Assert
			Assert.That(response.HttpStatusCode, Is.EqualTo(HttpStatusCode.Unauthorized), response);
		}

		[Test]
		public void missing_apikey_in_header_should_return_400_badrequest()
		{
			// Arrange
			WebApiClient apiclient = new WebApiClient();
			apiclient.ApiKey = "";

			// Act
			WebApiResponse response = apiclient.Get("User");

			// Assert
			Assert.That(response.HttpStatusCode, Is.EqualTo(HttpStatusCode.BadRequest), response);
		}

		[Test]
		public void blank_apikeys_config_setting_should_disable_rest_api()
		{
			// Arrange
			RemoveApiKeys();
			WebApiClient apiclient = new WebApiClient();

			// Act
			WebApiResponse response = apiclient.Get("User");

			// Assert
			Assert.That(response.HttpStatusCode, Is.EqualTo(HttpStatusCode.NotFound), response);
		}

		[Test]
		public void blank_apikeys_config_setting_should_disable_swashbuckle()
		{
			// Arrange
			RemoveApiKeys();
			var client = new RestClient(TestConstants.WEB_BASEURL);
			var restRequest = new RestRequest("/swagger/");

			// Act
			IRestResponse response = client.Get(restRequest);

			// Assert
			Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound), response.Content);
		}
	}
}
