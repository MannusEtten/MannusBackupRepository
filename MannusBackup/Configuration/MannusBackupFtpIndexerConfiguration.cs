using System.Configuration;
using ESRINederland.Framework.Configuration;

namespace MannusBackup.Configuration
{
    public class MannusBackupFtpIndexerConfiguration : ConfigurationSection
    {
        public static MannusBackupFtpIndexerConfiguration GetConfig()
        {
            return ConfigurationManager.GetSection("MannusBackupFtpIndexer") as MannusBackupFtpIndexerConfiguration;
        }

        [ConfigurationProperty("test", IsRequired = true)]
        public bool TestSituation
        {
            get { return (bool)this["test"]; }
        }

        [ConfigurationProperty("FtpSites")]
        public GenericConfigurationElementCollection<FtpSiteElement> FtpSites
        {
            get { return (GenericConfigurationElementCollection<FtpSiteElement>)this["FtpSites"]; }
        }
    }
}