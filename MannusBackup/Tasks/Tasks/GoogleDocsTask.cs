namespace MannusBackup.Tasks
{
    /*
    internal class GoogleDocsTask<T> : TaskBase where T : MannusBackupElement, new()
    {
        public GoogleDocsTask() : base(TaskType.GoogleDocs) { }

        protected override void Execute()
        {
            GoogleElement configuration = Configuration as GoogleElement;
            Console.WriteLine("backup google docs " + configuration.Key);
            SetTaskDirectory(configuration.Key);
            try
            {
                GoogleDocsDownloader downloader = new GoogleDocsDownloader(configuration);
                downloader.DownloadDocuments(string.Format(@"{0}\Documenten", TaskDirectory));
                DuplicateFilesAndDirectoryRemover dadr = new DuplicateFilesAndDirectoryRemover();
                dadr.RemoveDuplicates(string.Format(@"{0}\Documenten", TaskDirectory));
                DirectoryInfo directory = new DirectoryInfo(string.Format(@"{0}\Documenten", TaskDirectory));
                foreach (FileInfo file in directory.GetFiles())
                {
                    file.MoveTo(string.Format(@"{0}\GoogleDocs\{1}", TaskDirectory, file.Name));
                }
                foreach (DirectoryInfo subdirectory in directory.GetDirectories())
                {
                    subdirectory.MoveTo(string.Format(@"{0}\GoogleDocs\{1}", TaskDirectory, subdirectory.Name));
                }
            }
            catch (Exception e)
            {
                _logger.LogException(e);
            }
        }
    }
     */
}