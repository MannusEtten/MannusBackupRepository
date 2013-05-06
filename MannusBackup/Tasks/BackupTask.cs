using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MannusBackup.Interfaces;

namespace MannusBackup.Tasks
{
    /// <summary>
    /// maak een 'container' aan voor een taak, deze container handelt de multi-threading af
    /// T is een type die TaskBase implementeert en dus een object is met specifieke instellingen over hoe
    /// de task uitgevoerd dient te worden
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class BackupTask<T> : IBackupTask
        where T : TaskBase, new()
    {
        private T _taskToRun;

        public BackupTask(T taskToRun, string name)
        {
            _taskToRun = taskToRun;
            TaskName = name;
        }

        public event EventHandler<TaskFinishedEventArgs> TaskIsFinished;

        public string TaskName { get; private set; }

        public void ExecuteBackupTask()
        {
            throw new NotImplementedException();
        }
    }
}