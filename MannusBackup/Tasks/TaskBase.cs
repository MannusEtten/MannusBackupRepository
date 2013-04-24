using System.Collections.Generic;
using System.Linq;
using ESRINederland.Framework.Logging;
using MannusBackup.Configuration;
using MannusBackup.Tasks.Tasks;

namespace MannusBackup.Tasks
{
    public abstract class TaskBase
    {
        public string TaskDirectory { get; private set; }

        public MannusBackupElement Configuration { get; set; }

        public List<ExclusionElement> Exclusions { get; set; }

        protected ILogger _logger;

        protected TaskType Type { get; private set; }

        protected TaskBase(TaskType typeOfTask)
        {
            Type = typeOfTask;
            _logger = Logger.GetLogger();
        }

        protected void SetTaskDirectory()
        {
            TaskDirectory = DirectoryManager.GetTaskDirectory(TaskType.XCopy);
        }

        protected void SetTaskDirectory(string configurationKey)
        {
            TaskDirectory = DirectoryManager.GetTaskDirectory(Type, configurationKey);
        }

        public void ExecuteTask()
        {
            if (Type == TaskType.Zipfiles)
            {
                ZipFileElement z = Configuration as ZipFileElement;
                Exclusions = MannusBackupConfiguration.GetExclusionsForConfiguration(z.Type, z.Key);
            }
            Execute();
            if (Type != TaskType.XCopy)
            {
                ZipFiles();
            }
            if (Configuration.RemoveDirectoryAfterCompletion)
            {
                DirectoryManager.RemoveTaskDirectory(Type, Configuration.Key);
            }
        }

        protected abstract void Execute();

        private void ZipFiles()
        {
            ZipTask<ZipFileElement> zipper = new ZipTask<ZipFileElement>();
            zipper.Configuration = MannusBackupConfiguration.GetZipFileElement(Type, Configuration.Key);
            if (zipper.Configuration != null)
            {
                zipper.ExecuteTask();
            }
        }

        protected bool ValueIsExcluded(string value, ExclusionType typeOfExclusion)
        {
            return Exclusions.Any(e => e.Value.ToLowerInvariant().Equals(value.ToLowerInvariant()) && e.TypeOfExclusion == typeOfExclusion);
        }
    }
}