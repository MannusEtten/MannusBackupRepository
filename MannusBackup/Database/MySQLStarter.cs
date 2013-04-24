using System;
using System.ServiceProcess;
using ESRINederland.Framework.Logging;

namespace MannusBackup.Database
{
    internal class MySQLStarter
    {
        private string _serviceName;
        private ILogger _logger;
        public event EventHandler<TaskFinishedEventArgs> TaskIsFinished;

        public MySQLStarter(string serviceName)
        {
            _serviceName = serviceName;
            _logger = Logger.GetLogger();
        }

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