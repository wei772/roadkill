using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;

namespace Roadkill.Core.AmazingConfig
{
	/// <summary>
	/// All settings for file attachments.
	/// </summary>
	public class AttachmentSettings
	{
		private readonly HttpContextWrapper _httpContext;

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
			if (HttpContext.Current != null)
				_httpContext = new HttpContextWrapper(HttpContext.Current);
		}

		/// <summary>
		/// The absolute file path for the attachments folder. If the AttachmentsFolder uses "~/" then the path is 
		/// translated into one that is relative to the site root, otherwise it is assumed to be an absolute file path.
		/// This property always contains a trailing slash (or / on Unix based systems).
		/// </summary>
		public string GetAttachmentsDirectoryPath()
		{
			string path = "";

			if (AttachmentsFolder.StartsWith("~") && _httpContext != null)
			{
				path = _httpContext.Server.MapPath(AttachmentsFolder);
			}
			else
			{
				path = AttachmentsFolder;
			}

			if (!path.EndsWith(Path.DirectorySeparatorChar.ToString()))
				path += Path.DirectorySeparatorChar.ToString();

			return path;
		}

		/// <summary>
		/// Gets the full path for the attachments folder, including any extra application paths from the url.
		/// Contains a "/" the start and does not contain a trailing "/".
		/// </summary>
		public string GetAttachmentsUrlPath()
		{
			string attachmentsPath = "/" + AttachmentsRoutePath;

			if (_httpContext != null)
			{
				string applicationPath = _httpContext.Request.ApplicationPath;
				if (!applicationPath.EndsWith("/"))
					applicationPath += "/";

				if (attachmentsPath.StartsWith("/"))
					attachmentsPath = attachmentsPath.Remove(0, 1);

				attachmentsPath = applicationPath + attachmentsPath;
			}

			return attachmentsPath;
		}
	}
}
