using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MannusBackup.Configuration;

namespace MannusBackup.Storage
{
    public class LocalStorageManager
    {
        /// <summary>
        /// Haalt alle directories op binnen een bepaalde directory en sorteer deze op datum
        /// </summary>
        /// <param name="directory">The directory.</param>
        /// <returns></returns>
        public SortedList<DateTime, DirectoryInfo> GetBackupDirectories(string directory)
        {
            DateTimeCreator dateTimeCreator = new DateTimeCreator(StorageLocation.Local);
            DirectoryInfo backupDirectories = new DirectoryInfo(directory);
            SortedList<DateTime, DirectoryInfo> backupDirectoriesNames = new SortedList<DateTime, DirectoryInfo>();
            foreach (DirectoryInfo subdir in backupDirectories.GetDirectories())
            {
                string subDirName = subdir.Name;
                DateTime dateStamp = dateTimeCreator.FromDateStamp(subDirName);
                if (!dateStamp.Year.Equals(1900))
                {
                    var fullDirectoryName = Path.Combine(MannusBackupConfiguration.GetConfig().BaseBackupDirectory, subDirName);
                    backupDirectoriesNames.Add(dateStamp, subdir);
                }
            }
            return backupDirectoriesNames;
        }

        /// <summary>
        /// verwijder directories indien er meer dan x zijn op de lokale harde schijf, de oudste wordt dan verwijderd
        /// </summary>
        /// <param name="numberOfBackups">The number of backups.</param>
        internal void DeleteBackupDirectories(int numberOfBackups)
        {
            var baseDirectory = MannusBackupConfiguration.GetConfig().BaseBackupDirectory;
            var backupDirectoriesSortedByDate = GetBackupDirectories(baseDirectory);
            if (backupDirectoriesSortedByDate.Count > numberOfBackups)
            {
                var directoriesToStay = backupDirectoriesSortedByDate.Take(numberOfBackups);
                var directoriesToDelete = backupDirectoriesSortedByDate.Except(directoriesToStay);
                directoriesToDelete.ToList().ForEach(d => d.Value.Delete(true));
            }
        }
    }
}
