using Ionic.Zip;
using MannusBackup.Configuration;
using System.IO;

namespace MannusBackup.Tasks.Tasks
{
    internal class ZipTask<T> : OldTaskBase where T : MannusBackupElement, new()
    {
        public ZipTask() : base(TaskType.Zipfiles) { }

        /// <summary>
        /// create zipfile for any backup-task except the xcopy-task
        /// </summary>
        protected override void Execute()
        {
            ZipFileElement c = Configuration as ZipFileElement;
            string filesFromDirectory = string.Empty;
            filesFromDirectory = DirectoryManager.GetTaskDirectory(c.Type, c.ChildKey);
            string toDirectory = DirectoryManager.GetTaskDirectory(TaskType.Zipfiles, Configuration.Key);
            string prefix = MannusBackupConfiguration.GetConfig().HostName;
            string zipFileName = string.Format(@"{0}\{1}_{2}_{3}.zip", toDirectory, prefix, c.Type.ToString(), c.ChildKey);
            CreateZipFileForAllDirectories(filesFromDirectory, zipFileName);
        }

        /// <summary>
        /// create zipfile for a xcopy-task
        /// </summary>
        /// <param name="directoryToZip">directory to zip</param>
        /// <param name="zipFileName">name of the zip file</param>
        internal void ExecuteZipTask(string directoryToZip, string zipFileName)
        {
            CreateZipFileForAllDirectories(directoryToZip, zipFileName);
        }

        private void CreateZipFileForAllDirectories(string directoryToZip, string zipFileName)
        {
            if (File.Exists(zipFileName))
            {
                try
                {
                    File.Delete(zipFileName);
                }
                catch (IOException e)
                {
                    _logger.LogException(e);
                }
            }
            using (ZipFile zipFile = new ZipFile(zipFileName))
            {
                zipFile.UseZip64WhenSaving = Zip64Option.AsNecessary;
                zipFile.Password = MannusBackupConfiguration.GetConfig().PassWord;
                zipFile.AddDirectory(directoryToZip);
                zipFile.Save();
            }
        }
    }
}