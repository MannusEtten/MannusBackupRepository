using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MannusBackup.Configuration;
using MannusBackup.Database;
using MannusBackup.Tasks.Database;

namespace MannusBackup.Tasks
{
    internal class DatabaseTask : TaskBase 
    {
        private ProfileProperty _profileProperty;
        private ProfileConfiguration _profileConfiguration;

        public DatabaseTask(ProfileConfiguration profileConfiguration, ProfileProperty sqlYogProperty) : base(TaskType.Database)
        {
            _profileProperty = sqlYogProperty;
            _profileConfiguration = profileConfiguration;
        }

        protected override void Execute()
        {
            Console.WriteLine("backup database " + _profileConfiguration.Name);
            SqlYogDatabaseTask databaseTask = new SqlYogDatabaseTask(_profileConfiguration, _profileProperty);
            databaseTask.DumpDatabaseToFile();
        }
    }
}