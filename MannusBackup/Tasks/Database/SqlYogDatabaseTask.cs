using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ESRINederland.Framework.Logging;
using MannusBackup.Database;

namespace MannusBackup.Tasks.Database
{
    internal class SqlYogDatabaseTask
    {
        private ProfileConfiguration _configuration;
        private ILogger _logger;
        private ProfileProperty _profileProperty;

        internal SqlYogDatabaseTask(ProfileConfiguration configuration, ProfileProperty sqlYogProfileProperty)
        {
            _logger = Logger.GetLogger();
            _configuration = configuration;
            _profileProperty = sqlYogProfileProperty;
        }

        private string BatchFileArguments
        {
            get
            {
                return string.Format("{0}", _configuration.Value);
            }
        }

        internal void DumpDatabaseToFile()
        {
            string argumentsString = BatchFileArguments;
            string sqlyogLocation = _profileProperty.Value;
            ProcessStartInfo processStartInformation = new ProcessStartInfo(sqlyogLocation, argumentsString);
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
    }
}