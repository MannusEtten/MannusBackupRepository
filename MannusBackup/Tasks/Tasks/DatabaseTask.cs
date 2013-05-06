using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MannusBackup.Configuration;
using MannusBackup.Database;
using MannusBackup.Interfaces;
using MannusBackup.Tasks.Database;

namespace MannusBackup.Tasks
{
    internal class DatabaseTask : TaskBase
    {
        private ProfileConfiguration _profileConfiguration;
        private ProfileProperty _profileProperty;

        public DatabaseTask()
            : base(TaskType.Database)
        {
        }

        public void SetConfiguration(ProfileConfiguration profileConfiguration, ProfileProperty sqlYogProperty)
        {
            _profileProperty = sqlYogProperty;
            _profileConfiguration = profileConfiguration;
        }

        protected internal override void Execute()
        {
            Console.WriteLine("backup database " + _profileConfiguration.Name);
            SqlYogDatabaseTask databaseTask = new SqlYogDatabaseTask(_profileConfiguration, _profileProperty);
            databaseTask.DumpDatabaseToFile();
        }
    }
}