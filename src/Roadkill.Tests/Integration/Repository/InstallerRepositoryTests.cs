using NUnit.Framework;
using Roadkill.Core;
using Roadkill.Core.Configuration;
using Roadkill.Core.Database;

namespace Roadkill.Tests.Integration.Repository
{
	[TestFixture]
	[Category("Integration")]
	public abstract class InstallerRepositoryTests
	{
		protected abstract string InvalidConnectionString { get; }
		protected abstract string ConnectionString { get; }

		protected abstract IInstallerRepository GetRepository(string connectionString);
		protected abstract void Clearup();
		protected abstract void CheckDatabaseProcessIsRunning();
		protected abstract bool HasAdminUser();
		protected abstract bool HasEmptyTables();

		[SetUp]
		public void Setup()
		{
			// Setup the repository
			Clearup();
		}

		[Test]
		public void AddAdminUser_should_add_user()
		{
			// Arrange
			var repository = GetRepository(ConnectionString);
			string username = "admin";
			string email = "email@example.com";
			string password = "password";

			// Act
			repository.AddAdminUser(email, username, password);

			// Assert
			Assert.True(HasAdminUser());
		}

		[Test]
		public void CreateSchema_should_create_and_clear_all_tables()
		{
			// Arrange
			var repository = GetRepository(ConnectionString);

			// Act
			repository.CreateSchema();

			// Assert
			Assert.True(HasEmptyTables());
		}

		[Test]
		public void AddAdminUser_And_CreateSchema_should_throw_databaseexception_with_invalid_connection_string()
		{
			// Arrange 
			var repository = GetRepository(InvalidConnectionString);

			// Act Assert
			Assert.Throws<DatabaseException>(() => repository.AddAdminUser("", "", ""));
			Assert.Throws<DatabaseException>(() => repository.CreateSchema());
		}
	}
}
