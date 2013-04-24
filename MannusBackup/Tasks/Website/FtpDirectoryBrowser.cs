using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using ESRINederland.Framework.Logging;
using MannusBackup.Configuration;

namespace MannusBackup.Tasks.Website
{
    internal class FtpDirectoryBrowser
    {
        private bool TestHasRun = false;
        private Uri ftpLocation;
        private NetworkCredential credentials;
        private List<string> ftpFiles = new List<string>();
        private ILogger _logger;

        public FtpDirectoryBrowser(Uri ftpLocation, NetworkCredential credentials)
        {
            this.ftpLocation = ftpLocation;
            this.credentials = credentials;
            _logger = Logger.GetLogger();
        }

        public List<string> GetAllFilesFromFtpLocation()
        {
            GetFilesFromFtpLocation();
            return ftpFiles;
        }

        private List<string> GetFilesFromFtpLocation()
        {
            string[] fileList = GetFileListFromDirectory(ftpLocation.AbsoluteUri);
            foreach (string filename in fileList)
            {
                bool conditionOne = MannusBackupFtpIndexerConfiguration.GetConfig().TestSituation;
                bool conditionTwo = !TestHasRun;
                bool situationOne = conditionOne && conditionTwo;
                bool situationTwo = !conditionOne;
                if (situationOne || situationTwo)
                {
                    if (filename.Contains("."))
                    {
                        string fileName = string.Format("{0}/{1}", ftpLocation.AbsoluteUri, filename);
                        ftpFiles.Add(filename);
                        _logger.LogDebug(filename);
                    }
                    else
                    {
                        GetFilesFromDirectory(string.Format("{0}/{1}", ftpLocation.AbsoluteUri, filename));
                    }
                }
                TestHasRun = true;
            }
            return ftpFiles;
        }

        internal FtpWebResponse CreateFtpResponse(string directory)
        {
            FtpWebRequest reqFTP;
            reqFTP = (FtpWebRequest)FtpWebRequest.Create(directory);
            reqFTP.Method = WebRequestMethods.Ftp.ListDirectory;
            reqFTP.UseBinary = true;
            reqFTP.Credentials = credentials;
            try { return (FtpWebResponse)reqFTP.GetResponse(); }
            catch (WebException ex)
            {
                string message = string.Format("{0} : {1}", directory, ex.Message);
                _logger.LogError(message);
                return null;
            }
        }

        internal long GetFileSize(string filename)
        {
            FtpWebRequest reqFTP = FtpWebRequest.Create(filename) as FtpWebRequest;
            reqFTP.Method = WebRequestMethods.Ftp.GetFileSize;
            reqFTP.UseBinary = true;
            reqFTP.Credentials = credentials;
            try
            {
                FtpWebResponse respSize = reqFTP.GetResponse() as FtpWebResponse;
                long size = respSize.ContentLength;
                respSize.Close();
                return size;
            }
            catch (WebException ex)
            {
                return 0;
            }
        }

        internal string[] GetFileListFromDirectory(string directory)
        {
            string directoryListingRootAsString = string.Empty;
            FtpWebResponse webresponse = CreateFtpResponse(directory);
            if (webresponse != null)
            {
                using (FtpWebResponse response = webresponse)
                {
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(responseStream))
                        {
                            directoryListingRootAsString = reader.ReadToEnd();
                        }
                    }
                }
            }
            else
            {
                //                return null;
            }
            return directoryListingRootAsString.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
        }

        private void GetFilesFromDirectory(string ftpDirectory)
        {
            Stack<string> stack = new Stack<string>();
            stack.Push(ftpDirectory);
            while (stack.Count > 0)
            {
                string dir = stack.Pop();
                try
                {
                    ftpFiles.AddRange(GetNamesFromDirectory(dir, false));
                    GetNamesFromDirectory(dir, true).ForEach(n => stack.Push(n));
                }
                catch (WebException e)
                {
                    _logger.LogError(e.Message);
                }
            }
        }

        internal List<string> GetNamesFromDirectory(string directory, bool directoryNames)
        {
            List<string> result = new List<string>();
            string[] fileList = GetFileListFromDirectory(directory);
            foreach (string filename in fileList)
            {
                if (directoryNames && !filename.Contains("."))
                {
                    string directoryname = string.Format("{0}/{1}", directory, filename);
                    long fileSize = GetFileSize(directoryname);
                    if (fileSize == 0)
                    {
                        result.Add(directoryname);
                    }
                }
                else if (!directoryNames && filename.Contains("."))
                {
                    result.Add(string.Format("{0}/{1}", directory, filename));
                }
                else if (!directoryNames && !filename.Contains("."))
                {
                    string fileName = string.Format("{0}/{1}", directory, filename);
                    long fileSize = GetFileSize(fileName);
                    if (fileSize > 0)
                    {
                        result.Add(fileName);
                    }
                }
            }
            return result;
        }
    }
}