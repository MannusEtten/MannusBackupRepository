using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using ESRINederland.Framework.Logging;

namespace MannusBackup.Tasks.Website
{
    internal class FtpDownload
    {
        #region Fields (3)

        private NetworkCredential credentials;
        private string destinationDirectory;
        private Uri ftpLocation;

        #endregion Fields

        #region Constructors (1)

        public FtpDownload(Uri ftpLocation, NetworkCredential credentials, string destinationDirectory)
        {
            this.ftpLocation = ftpLocation;
            this.destinationDirectory = destinationDirectory;
            this.credentials = credentials;
        }

        #endregion Constructors

        #region Methods (5)

        // Public Methods (1) 

        public void DownloadFiles(List<string> filesToDownload)
        {
            try
            {
                filesToDownload.ForEach(f => DownloadFile(f));
            }
            catch (Exception e)
            {
                string s = e.Message;
            }
        }

        // Private Methods (4) 

        private void CheckForDirectoryExistence(string directory)
        {
            if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);
        }

        private void DownloadFile(string filename)
        {
            string destinationFile = GetDestinationFileName(filename);
            using (WebClient request = new WebClient())
            {
                request.Credentials = credentials;
                byte[] fileData = null;
                try { fileData = request.DownloadData(filename); }
                catch (WebException e)
                {
                    Logger.GetLogger().LogError(e.Message);
                }
                catch (ThreadAbortException e)
                {
                    string message = string.Format("{0} - {1}", e.Message, filename);
                    Logger.GetLogger().LogError(message);
                }
                if (fileData != null)
                {
                    using (FileStream file = File.Create(destinationFile))
                    {
                        file.Write(fileData, 0, fileData.Length);
                    }
                }
            }
        }

        private string GetDestinationFileName(string webSourceFileName)
        {
            if (!destinationDirectory.EndsWith(@"\")) destinationDirectory += @"\";
            webSourceFileName = webSourceFileName.Replace(ftpLocation.AbsoluteUri, "");
            string[] path = webSourceFileName.Split("/".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            string directory = destinationDirectory + GetDestinationDirectory(path);
            if (path.Length > 1)
            {
                CheckForDirectoryExistence(directory);
                return directory + path[path.Length - 1];
            }
            else { return directory; }
        }

        private string GetDestinationDirectory(string[] path)
        {
            if (path.Length == 1) return path[0];
            string directory = string.Empty;
            for (int i = 0; i < path.Length - 1; i++) directory += path[i] + @"\";
            if (!directory.EndsWith(@"\")) directory += @"\";
            return directory;
        }

        #endregion Methods
    }
}