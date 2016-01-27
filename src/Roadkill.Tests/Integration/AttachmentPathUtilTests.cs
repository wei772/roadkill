using System;
using System.IO;
using NUnit.Framework;
using Roadkill.Core.AmazingConfig;
using Roadkill.Core.Attachments;
using Roadkill.Tests.Unit;
using Roadkill.Tests.Unit.StubsAndMocks;

namespace Roadkill.Tests.Integration
{
	[TestFixture]
	[Parallelizable(ParallelScope.None)]
	[Category("Unit")]
	public class AttachmentPathUtilTests
	{
		private MocksAndStubsContainer _container;
		private ConfigurationStoreMock _configurationStore;
		private IConfiguration _configuration;

		private AttachmentPathUtil _attachmentPathUtil;

		[SetUp]
		public void Setup()
		{
			_container = new MocksAndStubsContainer();
			_configurationStore = _container.ConfigurationStoreMock;
			_configuration = _container.Configuration;
			_configuration.AttachmentSettings.AttachmentsFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Attachments");

			_attachmentPathUtil = new AttachmentPathUtil(_configurationStore);

			try
			{
				// Delete any existing attachments folder

				// Remove the files 1st
				if (Directory.Exists(_configuration.AttachmentSettings.AttachmentsFolder))
				{
					DirectoryInfo directoryInfo = new DirectoryInfo(_configuration.AttachmentSettings.AttachmentsFolder);
					foreach (FileInfo file in directoryInfo.GetFiles())
					{
						File.Delete(file.FullName);
					}

					if (directoryInfo.Exists)
					{
						directoryInfo.Attributes = FileAttributes.Normal;
						directoryInfo.Delete(true);
					}
				}
				Directory.CreateDirectory(_configuration.AttachmentSettings.AttachmentsFolder);
			}
			catch (IOException e)
			{
				Assert.Fail("Unable to delete the attachments folder " + _configuration.AttachmentSettings.AttachmentsFolder + ", does it have a lock?" + e.ToString());
			}
		}

		[Test]
		[TestCase("")]
		[TestCase("/")]
		[TestCase("///")]
		public void ConvertUrlPathToPhysicalPath_Should_Strip_Empty_And_Redundant_Seperators(string relativePath)
		{
			// Arrange
			string expectedPath = _configuration.AttachmentSettings.GetAttachmentsDirectoryPath();

			// Act
			string actualPath = _attachmentPathUtil.ConvertUrlPathToPhysicalPath(relativePath);

			// Assert
			Assert.That(actualPath, Is.EqualTo(expectedPath));
		}

		[Test]
		[TestCase("/folder1", @"folder1\")]
		[TestCase("/folder1/folder2", @"folder1\folder2\")]
		[TestCase("/folder1/folder2/", @"folder1\folder2\")]
		public void ConvertUrlPathToPhysicalPath_Should_Combine_Paths_And_Contain_Trailing_Slash(string relativePath, string expectedPath)
		{
			// Arrange
			expectedPath = Path.Combine(_configuration.AttachmentSettings.GetAttachmentsDirectoryPath(), expectedPath);
			Directory.CreateDirectory(expectedPath);

			// Act
			string actualPath = _attachmentPathUtil.ConvertUrlPathToPhysicalPath(relativePath);

			// Assert
			Assert.That(actualPath, Is.EqualTo(expectedPath));
		}

		[Test]
		public void isattachmentpathvalid_should_be_true_for_valid_subdirectory()
		{
			// Arrange
			string physicalPath = Path.Combine(_configuration.AttachmentSettings.GetAttachmentsDirectoryPath(), "images", "test");
			Directory.CreateDirectory(physicalPath);
			bool expectedResult = true;

			// Act
			bool actualResult = _attachmentPathUtil.IsAttachmentPathValid(physicalPath);

			// Assert
			Assert.That(actualResult, Is.EqualTo(expectedResult));
		}

		[Test]
		public void isattachmentpathvalid_should_be_false_for_valid_path_that_does_not_exist()
		{
			// Arrange
			string physicalPath = Path.Combine(_configuration.AttachmentSettings.GetAttachmentsDirectoryPath(), "folder100", "folder99");
			bool expectedResult = false;

			// Act
			bool actualResult = _attachmentPathUtil.IsAttachmentPathValid(physicalPath);

			// Assert
			Assert.That(actualResult, Is.EqualTo(expectedResult));
		}

		[Test]
		public void isattachmentpathvalid_should_be_case_insensitive()
		{
			// Arrange
			string physicalPath = Path.Combine(_configuration.AttachmentSettings.GetAttachmentsDirectoryPath(), "images", "test");
			physicalPath = physicalPath.Replace("Attachments", "aTTacHMentS");
			Directory.CreateDirectory(physicalPath);

			bool expectedResult = true;

			// Act
			bool actualResult = _attachmentPathUtil.IsAttachmentPathValid(physicalPath);

			// Assert
			Assert.That(actualResult, Is.EqualTo(expectedResult));
		}

		[Test]
		[TestCase("{attachmentsfolder}", true)]
		[TestCase(@"{attachmentsfolder}folder1", true)]
		[TestCase(@"{attachmentsfolder}folder1\folder2", true)]
		public void IsAttachmentPathValid_Should_Be_True_For_Valid_Paths(string physicalPath, bool expectedResult)
		{
			// Arrange
			physicalPath = physicalPath.Replace("{attachmentsfolder}", _configuration.AttachmentSettings.GetAttachmentsDirectoryPath());
			Directory.CreateDirectory(physicalPath);

			// Act
			bool actualResult = _attachmentPathUtil.IsAttachmentPathValid(physicalPath);

			// Assert
			Assert.That(actualResult, Is.EqualTo(expectedResult));
		}

		[Test]
		public void isattachmentpathvalid_should_be_true_for_emptystring()
		{
			// Arrange
			bool expectedResult = true;

			// Act
			bool actualResult = _attachmentPathUtil.IsAttachmentPathValid("");

			// Assert
			Assert.That(actualResult, Is.EqualTo(expectedResult));
		}

		[Test]
		[TestCase(@".\", false)]
		[TestCase(@"\.", false)]
		[TestCase(@"\..", false)]
		[TestCase(@"..\", false)]
		[TestCase(@"{attachmentsfolder}.\", false)]
		[TestCase(@"{attachmentsfolder}\.", false)]
		[TestCase(@"{attachmentsfolder}\..", false)]
		[TestCase(@"{attachmentsfolder}..\", false)]
		[TestCase(@"{attachmentsfolder}.\..\", false)]
		[TestCase(@"{attachmentsfolder}..\.\", false)]
		[TestCase(@"./../", false)]
		[TestCase("/", false)]
		[TestCase("/folder1", false)]
		[TestCase("/folder1/folder2", false)]
		public void IsAttachmentPathValid_Should_Be_False_For_Invalid_Paths(string physicalPath, bool expectedResult)
		{
			// Arrange
			physicalPath = physicalPath.Replace("{attachmentsfolder}", _configuration.AttachmentSettings.GetAttachmentsDirectoryPath());

			// Act
			bool actualResult = _attachmentPathUtil.IsAttachmentPathValid(physicalPath);

			// Assert
			Assert.That(actualResult, Is.EqualTo(expectedResult));
		}

		[Test]
		public void attachmentfolderexistsandwriteable_should_return_empty_string_for_writeable_folder()
		{
			// Arrange
			string directory = AppDomain.CurrentDomain.BaseDirectory;
			string expectedMessage = "";

			// Act
			string actualMessage = AttachmentPathUtil.AttachmentFolderExistsAndWriteable(directory, null);

			// Assert
			Assert.That(actualMessage, Is.EqualTo(expectedMessage));
		}

		[Test]
		public void attachmentfolderexistsandwriteable_should_return_error_for_empty_folder()
		{
			// Arrange
			string directory = "";
			string expectedMessage = "The folder name is empty";

			// Act
			string actualMessage = AttachmentPathUtil.AttachmentFolderExistsAndWriteable(directory, null);

			// Assert
			Assert.That(actualMessage, Is.EqualTo(expectedMessage));
		}

		[Test]
		public void attachmentfolderexistsandwriteable_should_return_error_for_missing_folder()
		{
			// Arrange
			string directory = @"c:\87sd9f7dssdds3232";
			string expectedMessage = "The directory does not exist, please create it first";

			// Act
			string actualMessage = AttachmentPathUtil.AttachmentFolderExistsAndWriteable(directory, null);

			// Assert
			Assert.That(actualMessage, Is.EqualTo(expectedMessage));
		}
	}
}
