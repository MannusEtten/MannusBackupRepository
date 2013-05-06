using System;
using System.ServiceProcess;
using ESRINederland.Framework.Logging;
using MannusBackup.Interfaces;

namespace MannusBackup.Database
{
    internal class MySQLStarter
    {
        private ILogger _logger;
        private string _serviceName;

        public MySQLStarter(string serviceName)
        {
            _serviceName = serviceName;
            _logger = Logger.GetLogger();
        }

        public event EventHandler<TaskFinishedEventArgs> TaskIsFinished;

        public void StartDatabase()
        {
            ServiceController myService = new ServiceController();
            myService.ServiceName = _serviceName;
            try
            {
                if (myService.Status == ServiceControllerStatus.Stopped)
                {
                    myService.Start();
                }
                myService.WaitForStatus(ServiceControllerStatus.Running);
                RaiseEvent();
            }
            catch (InvalidOperationException e)
            {
                _logger.LogError("MySql-service niet gevonden en kan niet gestart worden");
            }
        }

        private void RaiseEvent()
        {
            TaskFinishedEventArgs args = new TaskFinishedEventArgs();
            args.Message = "MySQL is gestart";
            args.Count = false;
            if (TaskIsFinished != null)
            {
                TaskIsFinished(this, args);
            }
        }
    }
}