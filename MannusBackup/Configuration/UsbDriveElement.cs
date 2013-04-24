using System.Configuration;
using ESRINederland.Framework.Configuration;

namespace MannusBackup.Configuration
{
    public class UsbDriveElement : ConfigurationElementBase
    {
        public override string ElementName
        {
            get
            {
                return "UsbDrive";
            }
        }

        [ConfigurationProperty("numberofbackups", IsRequired = true)]
        public int NumberOfBackups
        {
            get { return (int)this["numberofbackups"]; }
        }

        [ConfigurationProperty("backupdirectory")]
        public string BackupDirectory
        {
            get { return this["backupdirectory"].ToString(); }
        }
    }
}