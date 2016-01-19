using Roadkill.Core.Import;
using Roadkill.Core.Services;

namespace Roadkill.Tests.Unit.StubsAndMocks
{
	public class WikiImporterMock : IWikiImporter
	{
		public bool ImportedFromSql { get; set; }
		public bool UpdatedSearch { get; set; }

		public void ImportFromSqlServer(string connectionString)
		{
			ImportedFromSql = true;
		}

		public void UpdateSearchIndex(SearchService searchService)
		{
			UpdatedSearch = true;
		}
	}
}
