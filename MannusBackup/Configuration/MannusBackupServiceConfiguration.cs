using System.Configuration;
using System.IO;
using ESRINederland.Framework.Configuration;

namespace MannusBackup.Configuration
{
    public class MannusBackupServiceConfiguration : ConfigurationSection
    {
        public static MannusBackupServiceConfiguration GetConfig()
        {
            return ConfigurationManager.GetSection("MannusBackupService") as MannusBackupServiceConfiguration;
        }

        [ConfigurationProperty("basebackupdirectory", IsRequired = true)]
        public string BaseBackupDirectory
        {
            get { return string.Format(@"{0}\", this["basebackupdirectory"].ToString()); }
        }

        [ConfigurationProperty("usblistentime", IsRequired = true)]
        public int UsbListenTime
        {
            get { return (int)this["usblistentime"]; }
        }

        [ConfigurationProperty("UsbDrives")]
        public GenericConfigurationElementCollection<UsbDriveElement> UsbDrives
        {
            get { return (GenericConfigurationElementCollection<UsbDriveElement>)this["UsbDrives"]; }
        }

        [ConfigurationProperty("Hosts")]
        public GenericConfigurationElementCollection<HostElement> Hosts
        {
            get { return (GenericConfigurationElementCollection<HostElement>)this["Hosts"]; }
        }

        [ConfigurationProperty("test", IsRequired = true)]
        public bool TestSituation
        {
            get { return (bool)this["test"]; }
        }

        public static string FindDrive(UsbDriveElement driveElement)
        {
            string driveName = driveElement.Key.ToLowerInvariant();
            DriveInfo[] drives = DriveInfo.GetDrives();
            foreach (DriveInfo drive in drives)
            {
                try
                {
                    if (drive.VolumeLabel.ToLowerInvariant().Equals(driveName))
                    {
                        return drive.RootDirectory.FullName;
                    }
                }
                catch (IOException e) { }
            }
            return null;
        }
    }
}