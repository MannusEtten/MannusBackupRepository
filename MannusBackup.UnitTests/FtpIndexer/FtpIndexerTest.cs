using System;
using System.Linq;
using System.Net;
using MannusBackup.Configuration;
using MannusBackup.FtpIndexer;
using MannusBackup.Tasks.Website;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MannusBackup.UnitTests
{
    [TestClass]
    public class FtpIndexerTest
    {
        MySqlDatabase database = new MySqlDatabase();
        FtpIndexer.FtpIndexer indexer;

        [TestMethod]
        public void IndexFtpSite_Count_Greather_Than_Zero()
        {
            MannusBackupFtpIndexerConfiguration config = MannusBackupFtpIndexerConfiguration.GetConfig();
            indexer = new FtpIndexer.FtpIndexer();
            indexer.IndexFtpSite(config.FtpSites[0]);
            Assert.AreNotEqual(0, indexer.GetFtpFiles(config.FtpSites[0]).Count);
        }

        [TestMethod]
        public void GetFtpFiles_Count_Is_One()
        {
            MannusBackupFtpIndexerConfiguration config = MannusBackupFtpIndexerConfiguration.GetConfig();
            indexer = new FtpIndexer.FtpIndexer();
            database.OpenDatabase();
            database.DeleteExistingData();
            database.AddFileToTable(config.FtpSites[0].Key, "Mannus.Mannus");
            database.CloseDatabase();
            Assert.AreEqual(1, indexer.GetFtpFiles(config.FtpSites[0]).Count);
        }

        [TestMethod]
        public void GetFtpWebResponse_With_Directory_Whichs_Return_Error_In_Logging()
        {
            Uri ftpUrl = new Uri("ftp://ftp.mannus.nl/www");
            NetworkCredential credentials = new NetworkCredential("mannus.nl", "zdmsddb");
            FtpDirectoryBrowser browser = new FtpDirectoryBrowser(ftpUrl, credentials);
            var result = browser.CreateFtpResponse("ftp://ftp.mannus.nl/www/bug/core/phpmailer/README/README");
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetFileListFromDirectory_Count_Is_Six()
        {
            Uri ftpUrl = new Uri("ftp://ftp.mannus.nl/www");
            NetworkCredential credentials = new NetworkCredential("mannus.nl", "zdmsddb");
            FtpDirectoryBrowser browser = new FtpDirectoryBrowser(ftpUrl, credentials);
            var result = browser.GetFileListFromDirectory("ftp://ftp.mannus.nl/www/bug/core/phpmailer");
            Assert.AreEqual(6, result.Count());
        }

        [TestMethod]
        public void GetNamesFromDirectory_Count_Is_One()
        {
            Uri ftpUrl = new Uri("ftp://ftp.mannus.nl/www");
            NetworkCredential credentials = new NetworkCredential("mannus.nl", "zdmsddb");
            FtpDirectoryBrowser browser = new FtpDirectoryBrowser(ftpUrl, credentials);
            var result = browser.GetNamesFromDirectory("ftp://ftp.mannus.nl/www/bug/core/phpmailer", true);
            Assert.AreEqual(1, result.Count);
        }
    }
}