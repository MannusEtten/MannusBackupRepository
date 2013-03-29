using System.ServiceProcess;
using MannusBackup;
using MannusBackup.Configuration;

namespace MannusBackupService
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        private static void Main()
        {
            if (MannusBackupServiceConfiguration.GetConfig().TestSituation)
            {
                ServerPoller poller = new ServerPoller();
                poller.Start();
            }
            else
            {
                MannusBackupService bs = new MannusBackupService();
                ServiceBase[] ServicesToRun = new ServiceBase[] { bs };
                ServiceBase.Run(ServicesToRun);
            }
        }
    }
}