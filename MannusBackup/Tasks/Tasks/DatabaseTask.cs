using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MannusBackup.Configuration;
using MannusBackup.Tasks.Database;

namespace MannusBackup.Tasks
{
    internal class DatabaseTask<T> : TaskBase where T : MannusBackupElement, new() 
    {
        public DatabaseTask() : base(TaskType.Database) {}

        protected override void Execute()
        {
            DatabaseElement configuration = Configuration as DatabaseElement;
            Console.WriteLine("backup database " + configuration.Key + " " + configuration.UserName);
            SetTaskDirectory(configuration.Key);
            DatabaseDumper dumper = new DatabaseDumper(configuration, TaskDirectory);
            dumper.DumpDatabaseToFile();
        }
    }
}