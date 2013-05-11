using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
        private IEnumerable<T> _tasksToRun;
        private List<Thread> _runningThreads;
        private List<TaskStatus> _taskStates;

        public BackupTask(IEnumerable<T> tasksToRun, string name)
        {
            _tasksToRun = tasksToRun;
            _runningThreads = new List<Thread>();
            _taskStates = new List<TaskStatus>();
            TaskName = name;
        }

        /// <summary>
        /// Treedt op als taak is uitgevoerd, kan een collectie zijn van meerdere specifieke taken
        /// </summary>
        public event EventHandler<TaskFinishedEventArgs> TaskIsFinished;

        /// <summary>
        /// Naam van de taak zodat die opgenomen kan worden in de logging en console
        /// </summary>
        public string TaskName { get; private set; }

        /// <summary>
        /// Starttijd van de taak
        /// </summary>
        public DateTime StartTime { get; private set; }

        /// <summary>
        /// Uitvoeren van de taak
        /// </summary>
        public void ExecuteBackupTask()
        {
            StartTime = DateTime.Now;
            foreach (var task in _tasksToRun)
            {
                ThreadStart threadStart = new ThreadStart(task.Execute);
                _runningThreads.Add(new Thread(threadStart));
                _taskStates.Add(TaskStatus.Running);
            }
            _runningThreads.ForEach(t => t.Start());
            CheckForTimeOut();
        }

        private void CheckForTimeOut()
        {
            int count = _tasksToRun.Count();
            var checkIfAllThreadsAreStopped = _runningThreads.TrueForAll(t => t.ThreadState == ThreadState.Stopped);
            while (!checkIfAllThreadsAreStopped)
            {
                for(int i = 0; i < _tasksToRun.Count(); i++)
                {
                    TimeSpan timeDifference = DateTime.Now - StartTime;
                    int differenceInMinutes = timeDifference.Minutes;
                    var task = _tasksToRun.ToList()[i];
                    if (task.TimeOut <= differenceInMinutes)
                    {
                        Thread thread = _runningThreads[i];
                        if (thread.ThreadState == ThreadState.Running)
                        {
                            _runningThreads[i].Abort();
                            _taskStates[i] = TaskStatus.Aborted;
                        }
                    }
                }
                Thread.Sleep(10000);
            }
            RaiseEvent();
        }

        private string CreateMessage()
        {
            StringBuilder builder = new StringBuilder();
            int count = _tasksToRun.Count();
            for (int i = 0; i < count; i++)
            {
                if (_taskStates[i] == TaskStatus.Running)
                {
                    _taskStates[i] = TaskStatus.Finished;
                }
                builder.AppendFormat("taak {2} key: {0} heeft de status {1}\r\n", _tasksToRun.ToList()[i].Type, _taskStates[i].ToString(), TaskName);
            }
            builder.AppendLine(string.Empty);
            return builder.ToString();
        }

        private void RaiseEvent()
        {
            TaskFinishedEventArgs args = new TaskFinishedEventArgs();
            args.StartTime = StartTime;
            args.EndTime = DateTime.Now;
            args.Message = CreateMessage();
            if (TaskIsFinished != null)
            {
                TaskIsFinished(this, args);
            }
        }
    }
}