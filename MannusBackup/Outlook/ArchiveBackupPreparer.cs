using System;
using System.Configuration;
using System.IO;
using ESRINederland.Framework.Logging;

namespace MannusBackup.Outlook
{
    internal class ArchiveBackupPreparer
    {
        private ILogger _logger;

        public ArchiveBackupPreparer()
        {
            _logger = Logger.GetLogger();
            _logger.LogApplicationSettings();
            ArchiveDirectory = ConfigurationManager.AppSettings["mailarchives"];
            BackupDirectory = ConfigurationManager.AppSettings["backuplocation"];
        }

        private string ArchiveDirectory { get; set; }

        private string BackupDirectory { get; set; }

        private bool CheckExistenceDirectories()
        {
            if (Directory.Exists(ArchiveDirectory))
            {
                if (!Directory.Exists(BackupDirectory))
                {
                    Directory.CreateDirectory(BackupDirectory);
                }
                return true;
            }
            return false;
        }

        public void PrepareBackupMailArchives()
        {
            if (CheckExistenceDirectories())
            {
                try
                {
                    CopyFiles("*.pst");
                    CopyFiles("*.msf");
                    CopyDirectories();
                }
                catch (Exception e)
                {
                    _logger.LogException(e);
                }
            }
        }

        private void CopyDirectories()
        {
            DirectoryInfo archiveDirectory = new DirectoryInfo(ArchiveDirectory);
            DirectoryInfo[] directories = archiveDirectory.GetDirectories();
            foreach (var directory in directories)
            {
                CopyDirectory(directory);
            }
        }

        private void CopyDirectory(DirectoryInfo directory)
        {
            string directoryName = directory.FullName.ToLowerInvariant();
            string backupName = BackupDirectory.ToLowerInvariant();
            if (!directoryName.Equals(backupName))
            {
                backupName = Path.Combine(backupName, directory.Name);
                DirectoryInfo backupDirectory = new DirectoryInfo(backupName);
                CopyAll(directory, backupDirectory);
            }
        }

        private void CopyAll(DirectoryInfo source, DirectoryInfo target)
        {
            // Check if the target directory exists, if not, create it.
            if (Directory.Exists(target.FullName) == false)
            {
                Directory.CreateDirectory(target.FullName);
            }

            // Copy each file into it’s new directory.
            foreach (FileInfo fi in source.GetFiles())
            {
                try
                {
                    fi.CopyTo(Path.Combine(target.ToString(), fi.Name), true);
                }
                catch (IOException e)
                {
                    _logger.LogException(e);
                }
            }

            // Copy each subdirectory using recursion.
            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir =
                    target.CreateSubdirectory(diSourceSubDir.Name);
                CopyAll(diSourceSubDir, nextTargetSubDir);
            }
        }

        private void CopyFiles(string extension)
        {
            DirectoryInfo archiveDirectory = new DirectoryInfo(ArchiveDirectory);
            FileInfo[] archives = archiveDirectory.GetFiles(extension);
            foreach (var fileInfo in archives)
            {
                CopyFile(fileInfo);
            }
        }

        private void CopyFile(FileInfo fileInfo)
        {
            string fileName = Path.Combine(BackupDirectory, fileInfo.Name);
            fileInfo.CopyTo(fileName, true);
        }
    }
}