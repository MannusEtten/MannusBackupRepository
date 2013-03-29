using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using ESRINederland.Framework.Configuration;
using ESRINederland.Framework.Logging;
using MannusBackup.Tasks;

namespace MannusBackup.Configuration
{
    public sealed class MannusBackupConfiguration : ConfigurationSection
    {
        public static MannusBackupConfiguration GetConfig()
        {
            return ConfigurationManager.GetSection("MannusBackup") as MannusBackupConfiguration;
        }

        #region "app.config section implementation"

        [ConfigurationProperty("Configuration")]
        public GenericConfigurationElementCollection<ConfigurationPropertyElement> Configuration
        {
            get { return (GenericConfigurationElementCollection<ConfigurationPropertyElement>)this["Configuration"]; }
        }

        [ConfigurationProperty("Drives")]
        public GenericConfigurationElementCollection<DriveElement> Drives
        {
            get { return (GenericConfigurationElementCollection<DriveElement>)this["Drives"]; }
        }

        [ConfigurationProperty("Exclusions")]
        public GenericConfigurationElementCollection<ExclusionElement> Exclusions
        {
            get { return (GenericConfigurationElementCollection<ExclusionElement>)this["Exclusions"]; }
        }

        /*
        [ConfigurationProperty("Google")]
        public GoogleElement GoogleDocsConfiguration
        {
            get { return this[("Google")] as GoogleElement; }
        }
         */

        [ConfigurationProperty("FtpSites")]
        public GenericConfigurationElementCollection<FtpSiteElement> FtpSites
        {
            get { return (GenericConfigurationElementCollection<FtpSiteElement>)this["FtpSites"]; }
        }

        [ConfigurationProperty("Databases")]
        public GenericConfigurationElementCollection<DatabaseElement> Databases
        {
            get { return (GenericConfigurationElementCollection<DatabaseElement>)this["Databases"]; }
        }

        [ConfigurationProperty("BackupLocations")]
        public GenericConfigurationElementCollection<DirectoryElement> BackupLocations
        {
            get { return (GenericConfigurationElementCollection<DirectoryElement>)this["BackupLocations"]; }
        }

        [ConfigurationProperty("ZipFiles")]
        public GenericConfigurationElementCollection<ZipFileElement> ZipFiles
        {
            get { return (GenericConfigurationElementCollection<ZipFileElement>)this["ZipFiles"]; }
        }

        #endregion "app.config section implementation"

        /// <summary>
        /// Password waarmee de zip-files worden beveiligd
        /// </summary>
        public string PassWord
        {
            get
            {
                PasswordGenerator passwordGenerator = new PasswordGenerator();
                return passwordGenerator.GetPassword();
            }
        }

        public bool TestSituation
        {
            get { return bool.Parse(Configuration["test"].Value); }
        }

        public string MySQLServiceName
        {
            get { return Configuration["mysqlservice"].Value; }
        }

        public int MinimumAvailableDiskspaceInGB
        {
            get { return int.Parse(Configuration["minimumavailablediskspace"].Value); }
        }

        public string ConnectionStringName
        {
            get { return Configuration["connectionstring"].Value; }
        }

        public int MinimumFilesToCleanup
        {
            get { return int.Parse(Configuration["minimalfilesforcleanup"].Value); }
        }

        public string BaseBackupDirectory
        {
            get { return Configuration["basebackupdirectory"].Value; }
        }

        public string NameCDrive
        {
            get { return Configuration["nameofcdrive"].Value.ToLowerInvariant(); }
        }

                public string ToMailAddress
        {
            get
            {
                return Configuration["tomailaddress"].Value;
            }
        }

                        public string XsltMailTemplate
        {
            get
            {
                return Configuration["xsltmailtemplate"].Value;
            }
        }

        public string HostName
        {
            get
            {
                return Configuration["hostname"].Value;
            }
        }

        public static string BackupDirectory
        {
            get
            {
                string baseDirectory = MannusBackupConfiguration.GetConfig().BaseBackupDirectory;
                DateTime date = DateTime.Now;
                string datum = date.ToString("ddMMMMyyyy", CultureInfo.InvariantCulture);
                string prefix = MannusBackupConfiguration.GetConfig().HostName;
                return string.Format(@"{0}\backup_{1}_{2}", baseDirectory,prefix,datum);
            }
        }

        public static string FindDrive(DriveElement driveElement)
        {
            ILogger logger = Logger.GetLogger();
            if (driveElement == null)
            {
                logger.LogError("geen drive beschikbaar, geen driveelement gedefinieerd");
                return null;
            }
            string driveName = driveElement.Key.ToLowerInvariant();
            string cdriveName = MannusBackupConfiguration.GetConfig().NameCDrive;
            if (driveName.ToLowerInvariant().Equals(cdriveName))
            {
                driveName = string.Empty;
            }
            logger.LogDebug("Drivename: " + driveName);
            DriveInfo[] drives = DriveInfo.GetDrives();
            foreach (DriveInfo drive in drives)
            {
                try
                {
                    logger.LogDebug("drive volumelabel: " + drive.VolumeLabel);
                    if (drive.VolumeLabel.ToLowerInvariant().Equals(driveName))
                    {
                        return drive.RootDirectory.FullName;
                    }
                }
                catch (IOException e)
                {
                    logger.LogException(e);
                }
            }
            return null;
        }

        public static ZipFileElement GetZipFileElement(TaskType typeOfTask, string childKey)
        {
            foreach (ZipFileElement e in GetConfig().ZipFiles)
            {
                if (e.Type == typeOfTask && e.ChildKey.Equals(childKey))
                {
                    return e;
                }
            }
            return null;
        }

        public static List<ExclusionElement> GetExclusionsForConfiguration(TaskType typeOfTask, string childKey)
        {
            List<ExclusionElement> exclusions = new List<ExclusionElement>();
            foreach (ExclusionElement e in GetConfig().Exclusions)
            {
                if (e.Type == typeOfTask && e.ChildKey.Equals(childKey))
                {
                    exclusions.Add(e);
                }
            }
            return exclusions;
        }
    }
}