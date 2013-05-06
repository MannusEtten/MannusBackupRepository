using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using ESRINederland.Framework.Configuration;
using MannusBackup.Configuration;
using MannusBackup.Interfaces;

namespace MannusBackup.Tasks
{
    [Obsolete("niet meer gebruiken")]
    internal class BackupTaskOld<T, U> : IBackupTask
        where T : MannusBackupElement, new()
        where U : OldTaskBase, new()
    {
        private List<Thread> runningThreads = new List<Thread>();

        private List<TaskStatus> taskState = new List<TaskStatus>();

        public BackupTaskOld(GenericConfigurationElementCollection<T> configuration, string name)
        {
            Configuration = configuration;
            TaskName = name;
        }

        public BackupTaskOld(T configuration, string name)
        {
            OneItemConfiguration = configuration;
            TaskName = name;
        }

        public event EventHandler<TaskFinishedEventArgs> TaskIsFinished;

        public GenericConfigurationElementCollection<T> Configuration { get; private set; }

        public T OneItemConfiguration { get; private set; }

        #region Constructors (2)

        public DateTime StartTime { get; private set; }

        public string TaskName { get; private set; }

        #endregion Constructors

        #region Properties (7)

        public int TimeOutTime { get; private set; }

        #endregion Properties

        #region Delegates and Events (1)

        // Events (1) 
        #endregion Delegates and Events

        public void ExecuteBackupTask()
        {
            StartTime = DateTime.Now;
            if (Configuration == null)
            {
                CreateThread();
            }
            else
            {
                CreateMultipleThreads();
            }
            StartThreads();
            CheckForTimeOut();
        }

        private void CheckForTimeOut()
        {
            int count = (Configuration == null) ? 1 : Configuration.Count;
            while (!CheckIfAllThreadsAreStopped())
            {
                for (int i = 0; i < count; i++)
                {
                    TimeSpan timeDifference = DateTime.Now - StartTime;
                    int differenceInMinutes = timeDifference.Minutes;
                    T configuration = (Configuration == null) ? OneItemConfiguration : Configuration[i];
                    if (configuration.TimeOut <= differenceInMinutes)
                    {
                        Thread thread = runningThreads[i];
                        if (thread.ThreadState == ThreadState.Running)
                        {
                            runningThreads[i].Abort();
                            taskState[i] = TaskStatus.Aborted;
                        }
                    }
                }
                Thread.Sleep(10000);
            }
            RaiseEvent();
        }

        private bool CheckIfAllThreadsAreStopped()
        {
            return runningThreads.TrueForAll(t => t.ThreadState == ThreadState.Stopped);
        }

        private string CreateMessage()
        {
            StringBuilder builder = new StringBuilder();
            int count = (Configuration == null) ? 1 : Configuration.Count;
            for (int i = 0; i < count; i++)
            {
                if (taskState[i] == TaskStatus.Running)
                {
                    taskState[i] = TaskStatus.Finished;
                }
                T configuration = (Configuration == null) ? OneItemConfiguration : Configuration[i];
                builder.AppendFormat("taak {2} key: {0} heeft de status {1}\r\n", configuration.Key, taskState[i].ToString(), TaskName);
            }
            builder.AppendLine(string.Empty);
            return builder.ToString();
        }

        private void CreateMultipleThreads()
        {
            for (int i = 0; i < Configuration.Count; i++)
            {
                T element = Configuration[i];
                U task = new U();
                task.Configuration = element;
                ThreadStart threadStart = new ThreadStart(task.ExecuteTask);
                runningThreads.Add(new Thread(threadStart));
                taskState.Add(TaskStatus.Running);
            }
        }

        private void CreateThread()
        {
            T element = OneItemConfiguration;
            U task = new U();
            task.Configuration = element;
            ThreadStart threadStart = new ThreadStart(task.ExecuteTask);
            runningThreads.Add(new Thread(threadStart));
            taskState.Add(TaskStatus.Running);
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

        private void StartThreads()
        {
            runningThreads.ForEach(t => t.Start());
        }
    }
}