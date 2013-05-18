using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
                DeleteUnnecessaryFiles();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
            }
        }

        private void DeleteFile(string file)
        {
            if (!file.Contains(".sql"))
            {
                File.Delete(file);
            }
        }

        private void DeleteUnnecessaryFiles()
        {
            var backupDirectory = _configurationRepository.GetProfileConfigurationValue(_profileConfiguration, ProfileSubProperties.BackupDirectory);
            var filesInBackupDirectory = Directory.GetFiles(backupDirectory);
            foreach (var file in filesInBackupDirectory)
            {
                DeleteFile(file);
            }
        }

        private string GetBatchFileArguments()
        {
            string batchfileArguments = "{0} {1} {2}";
            var jobfile = _configurationRepository.GetProfileConfigurationValue(_profileConfiguration, ProfileSubProperties.SqlYogJobFileName);
            var logfile = _configurationRepository.GetProfileConfigurationValue(_profileConfiguration, ProfileSubProperties.SqlYogLogFileName);
            var sessionfile = _configurationRepository.GetProfileConfigurationValue(_profileConfiguration, ProfileSubProperties.SqlYogSessionFileName);
            return string.Format(batchfileArguments, jobfile, logfile, sessionfile);
        }
    }
}