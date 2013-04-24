using System;

namespace MannusBackup.Tasks
{
    public interface IBackupTask
    {
        #region Data Members (3)

        event EventHandler<TaskFinishedEventArgs> TaskIsFinished;

        #endregion Data Members

        #region Operations (1)

        void ExecuteBackupTask();

        #endregion Operations

        string TaskName { get; }
    }
}