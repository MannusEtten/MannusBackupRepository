using MannusBackup.Configuration;
using MannusBackup.FtpIndexer;

namespace MannusBackupFtpIndexer
{
    internal static class Program
    {
        private static void Main()
        {
            FtpIndexer indexer = new FtpIndexer();
            indexer.ClearIndex();
            foreach (FtpSiteElement ftpSite in MannusBackupFtpIndexerConfiguration.GetConfig().FtpSites)
            {
                indexer.IndexFtpSite(ftpSite);
            }
        }
    }
}