using System.Collections.Generic;

namespace Roadkill.Core.Database
{
	public interface IRepositoryFactory
	{
		IUserRepository GetUserRepository(string databaseProviderName, string connectionString);
		IPageRepository GetPageRepository(string databaseProviderName, string connectionString);

		IEnumerable<RepositoryInfo> ListAll();
	}
}