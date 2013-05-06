using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MannusBackup.Configuration;
using MannusBackup.Tasks.Website;
using System.Net;

namespace MannusBackup.Tasks
{
    internal class WebsitesTask<T> : OldTaskBase where T : MannusBackupElement, new() 
    {
        private Uri ftpUrl { get; set; }
        private NetworkCredential credentials { get; set; }
        private List<string> filesOnFtpLocation { get; set; }

        public WebsitesTask() : base(TaskType.Website) {}

        protected override void Execute()
        {
            FtpSiteElement configuration = Configuration as FtpSiteElement;
            Console.WriteLine("backup website " + configuration.Key + " " + configuration.UserName);
            ftpUrl = new Uri(string.Format(@"ftp://{0}/www", configuration.FtpAddress));
            credentials = new NetworkCredential(configuration.UserName, configuration.Password);
            GetFilesToDownload(configuration);
            SetTaskDirectory(configuration.Key);
            DownloadFiles();
        }

        private void GetFilesToDownload(FtpSiteElement ftpSite)
        {
            FtpIndexer.FtpIndexer indexer = new FtpIndexer.FtpIndexer();
            filesOnFtpLocation = indexer.GetFtpFiles(ftpSite);
        }

        private void DownloadFiles()
        {
            FtpDownload download = new FtpDownload(ftpUrl, credentials, TaskDirectory);
            download.DownloadFiles(filesOnFtpLocation);
        }
    }
}