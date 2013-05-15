using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using MannusBackup.Configuration;
using MannusBackup.Database;
using MannusBackup.Interfaces;

namespace MannusBackup.Tasks
{
    internal class DatabaseTask : TaskBase
    {
        private IEnumerable<ProfileConfiguration> _profileConfiguration;
        private ProfileProperty _profileProperty;

        public DatabaseTask()
            : base(TaskType.Database)
        {
        }

        internal void SetConfiguration(IEnumerable<ProfileConfiguration> sqlYogConfigurationProperties, ProfileProperty profileProperty)
        {
            _profileProperty = profileProperty;
            _profileConfiguration = sqlYogConfigurationProperties;
        }

        protected internal override void Execute()
        {
            var arguments = GetBatchFileArguments();
            ProcessStartInfo processStartInformation = new ProcessStartInfo(_profileProperty.Value, arguments);
            processStartInformation.RedirectStandardOutput = true;
            processStartInformation.WindowStyle = ProcessWindowStyle.Hidden;
            processStartInformation.UseShellExecute = false;
            try
            {
                Process dumpProcess = Process.Start(processStartInformation);
                while (!dumpProcess.HasExited)
                {
                    dumpProcess.WaitForExit(5000);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
            }
        }

        private string GetBatchFileArguments()
        {
            string batchfileArguments = "{0} {1} {2}";
            string jobfile = _profileConfiguration.Where(p => p.backup_profile_configuration_group.Name.Equals(ProfileSubProperties.SqlYogJobFileName)).First().Value;
            string logfile = _profileConfiguration.Where(p => p.backup_profile_configuration_group.Name.Equals(ProfileSubProperties.SqlYogLogFileName)).First().Value;
            string sessionfile = _profileConfiguration.Where(p => p.backup_profile_configuration_group.Name.Equals(ProfileSubProperties.SqlYogSessionFileName)).First().Value;
            return string.Format(batchfileArguments, jobfile, logfile, sessionfile);
        }
    }
}