using System;
using System.IO;
using ESRINederland.Framework.Logging;
using MannusBackup.Configuration;
using MannusBackup.Tasks;

namespace MannusBackup
{
    public class DirectoryManager
    {
        public static void CreateBaseDirectory()
        {
            DirectoryInfo directory = new DirectoryInfo(MannusBackupConfiguration.BackupDirectory);
            if (directory.Exists)
            {
                FileInfo[] files = directory.GetFiles();
                foreach (FileInfo file in files)
                {
                    file.Delete();
                }
                directory.Delete(true);
            }
            directory.Create();
            foreach (TaskType item in Enum.GetValues(typeof(TaskType)))
            {
                if (item != TaskType.Zipfiles && item != TaskType.XCopy)
                {
                    string directoryName = item.ToString();
                    string path = string.Format(@"{0}\{1}", directory.FullName, directoryName);
                    Directory.CreateDirectory(path);
                }
            }
        }

        public static void DeleteBaseDirectory()
        {
            DirectoryInfo directory = new DirectoryInfo(MannusBackupConfiguration.BackupDirectory);
            ILogger logger = Logger.GetLogger();
            foreach (DirectoryInfo subDir in directory.GetDirectories())
            {
                logger.LogDebug(subDir.FullName);
                subDir.Delete(true);
            }
        }

        internal static string GetTaskDirectory(TaskType taskType, string configurationKey)
        {
            string directoryName = taskType.ToString();
            directoryName = directoryName.Replace("Task", string.Empty);
            string path = string.Format(@"{0}\{1}\{2}", MannusBackupConfiguration.BackupDirectory, directoryName, configurationKey);
            if (taskType == TaskType.Zipfiles || taskType == TaskType.XCopy)//|| taskType == TaskType.GoogleDocs)
            {
                path = string.Format(@"{0}", MannusBackupConfiguration.BackupDirectory);
            }
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return path;
        }

        internal static string ZipDirectoryForGoogleDocs()
        {
            return string.Format(@"{0}\{1}", MannusBackupConfiguration.BackupDirectory, "GoogleDocs");
        }

        internal static void RemoveTaskDirectory(TaskType type, string key)
        {
            string taskDirectory = GetTaskDirectory(type, key);
            Directory.Delete(taskDirectory, true);
        }

        internal static string GetTaskDirectory(TaskType taskType)
        {
            return GetTaskDirectory(taskType, string.Empty);
        }
    }
}