using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using ESRINederland.Framework.Logging;
using MannusBackup.Configuration;

namespace MannusBackup.Tasks.Database
{
    internal class DatabaseDumper
    {
        private const string BATCHFILENAME = "mysqlbackup.bat";

        private DatabaseElement DatabaseConfiguration { get; set; }

        private string rootDirectory;

        internal DatabaseDumper(DatabaseElement databaseConfiguration, string rootDirectory)
        {
            this.DatabaseConfiguration = databaseConfiguration;
            this.rootDirectory = rootDirectory;
        }

        internal void DumpDatabaseToFile()
        {
            string argumentsString = BatchFileArguments;
            ProcessStartInfo processStartInformation = new ProcessStartInfo(BatchFileLocation, argumentsString);
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
                Logger.GetLogger().LogError(e.Message);
            }
        }

        private string BatchFileArguments
        {
            get
            {
                return string.Format("{0} {1} {2} {3} {4}", DatabaseConfiguration.Database, DatabaseConfiguration.UserName, DatabaseConfiguration.Password, GetDumpFileName(), DatabaseConfiguration.Host);
            }
        }

        private string GetDumpFileName()
        {
            return string.Format(@"{0}\{1}.sql", rootDirectory, DatabaseConfiguration.Key);
        }

        private string BatchFileLocation
        {
            get
            {
                string assemblyLocation = Assembly.GetExecutingAssembly().Location;
                return string.Format(@"{0}\{1}", Directory.GetParent(assemblyLocation).FullName, BATCHFILENAME);
            }
        }
    }
}