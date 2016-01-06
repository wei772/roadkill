using System.IO;
using System.Web;

namespace Roadkill.Core.AmazingConfig
{
	internal class AttachmentPathsResolver
	{
		private readonly IConfiguration _configuration;
		private readonly HttpContextWrapper _httpContext;

		public AttachmentPathsResolver(IConfiguration configuration)
		{
			if (HttpContext.Current != null)
				_httpContext = new HttpContextWrapper(HttpContext.Current);

			_configuration = configuration;
		}

		/// <summary>
		/// The absolute file path for the attachments folder. If the AttachmentsFolder uses "~/" then the path is 
		/// translated into one that is relative to the site root, otherwise it is assumed to be an absolute file path.
		/// This property always contains a trailing slash (or / on Unix based systems).
		/// </summary>
		public string GetAttachmentsDirectoryPath()
		{
			string path = "";

			if (_configuration.AttachmentsFolder.StartsWith("~") && _httpContext != null)
			{
				path = _httpContext.Server.MapPath(_configuration.AttachmentsFolder);
			}
			else
			{
				path = _configuration.AttachmentsFolder;
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
			string attachmentsPath = "/" + _configuration.AttachmentsRoutePath;

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