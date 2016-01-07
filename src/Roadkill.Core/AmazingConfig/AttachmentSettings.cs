using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Roadkill.Core.AmazingConfig
{
	/// <summary>
	/// All settings for file attachments.
	/// </summary>
	public class AttachmentSettings
	{
		/// <summary>
		/// Gets or sets the attachments folder, which should begin with "~/".
		/// </summary>
		public string AttachmentsFolder { get; set; }

		/// <summary>
		/// TODO: comments
		/// </summary>
		public string AttachmentsRoutePath { get; set; }

		/// <summary>
		/// TODO: comments + tests
		/// </summary>
		public string AzureConnectionString { get; set; }

		/// <summary>
		/// TODO: comments + tests
		/// </summary>
		public string AzureContainer { get; set; }

		/// <summary>
		/// The files types allowed for uploading.
		/// </summary>
		public string AllowedFileTypes { get; set; }

		/// <summary>
		/// Whether files with the same name overwrite the existing file, or throw an error.
		/// </summary>
		public bool OverwriteExistingFiles { get; set; }

		/// <summary>
		/// TODO: comments + tests
		/// </summary>
		[JsonIgnore]
		public bool UseAzureFileStorage
		{
			get { return !string.IsNullOrEmpty(AzureConnectionString); }
		}

		/// <summary>
		/// Retrieves a list of the file extensions that are permitted for upload.
		/// </summary>
		[JsonIgnore]
		public List<string> AllowedFileTypesList
		{
			get
			{
				if (string.IsNullOrEmpty(AllowedFileTypes))
					return new List<string>();

				return new List<string>(AllowedFileTypes.Replace(" ", "").Split(','));
			}
		}

		/// <summary>
		/// Additional custom settings.
		/// </summary>
		public Dictionary<string, string> Settings { get; set; }

		public AttachmentSettings()
		{
			AttachmentsRoutePath = "Attachments";
			AttachmentsFolder = "~/App_Data/Attachments";
			AllowedFileTypes = "jpg, png, gif";
			OverwriteExistingFiles = false;

			Settings = new Dictionary<string, string>();
		}
	}
}
