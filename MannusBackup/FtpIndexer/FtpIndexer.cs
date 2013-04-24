using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MannusBackup.Configuration;
using MySql.Data.MySqlClient;
using System.Net;
using MannusBackup.Tasks.Website;

namespace MannusBackup.FtpIndexer
{
    public class FtpIndexer
    {
        private FtpSiteElement Configuration { get; set; }
        private List<string> Files { get; set; }

        public FtpIndexer()
        {
            Files = new List<string>();
        }

        public void IndexFtpSite(FtpSiteElement ftpSiteConfiguration)
        {
            Configuration = ftpSiteConfiguration;
            ReadFiles();
            AddFileInformationToDatabase();
        }

        public List<string> GetFtpFiles(FtpSiteElement ftpSiteConfiguration)
        {
            Configuration = ftpSiteConfiguration;
            List<string> fileNames = new List<string>();
            MySqlDatabase database = new MySqlDatabase();
            database.OpenDatabase();
            MySqlDataReader data = database.ReadData(Configuration.Key);
            if(data != null && data.HasRows)
            {
                while(data.Read())
                {
                    fileNames.Add(data.GetString(0));
                }
            }
            database.CloseDatabase();
            return fileNames;
        }

        private void AddFileInformationToDatabase()
        {
            MySqlDatabase database = new MySqlDatabase();
            database.OpenDatabase();
            Files.ForEach(f => database.AddFileToTable(Configuration.Key, f));
            database.CloseDatabase();
        }

        private void ReadFiles()
        {
            Uri ftpUrl = new Uri(string.Format(@"ftp://{0}/www", Configuration.FtpAddress));
            NetworkCredential credentials = new NetworkCredential(Configuration.UserName, Configuration.Password);
            FtpDirectoryBrowser browser = new FtpDirectoryBrowser(ftpUrl, credentials);
            Files = browser.GetAllFilesFromFtpLocation();
        }

        public void ClearIndex()
        {
            MySqlDatabase database = new MySqlDatabase();
            database.OpenDatabase();
            database.DeleteExistingData();
            database.CloseDatabase();
        }
    }
}