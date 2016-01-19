using Roadkill.Core;
using Roadkill.Core.Database;

namespace Roadkill.Tests.Unit.StubsAndMocks
{
	public class InstallerRepositoryMock : IInstallerRepository
	{
		public string ConnectionString { get; set; }
		public string DatabaseName { get; set; }

		public bool Installed { get; set; }
		public bool ThrowInstallException { get; set; }

		public bool AddAdminUserCalled { get; set; }
		public bool CreateSchemaCalled { get; set; }
		public bool SaveSettingsCalled { get; set; }

		public void AddAdminUser(string email, string username, string password)
		{
			AddAdminUserCalled = true;

			if (ThrowInstallException)
				throw new DatabaseException("Something happened", null);
		}

		public void CreateSchema()
		{
			CreateSchemaCalled = true;

			if (ThrowInstallException)
				throw new DatabaseException("Something happened", null);
		}

		public void Dispose()
		{
		}
	}
}