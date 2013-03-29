using System;
using System.Timers;
using ESRINederland.Framework.Logging;
using MannusBackup.Configuration;

namespace MannusBackup
{
    internal class ServerPoller
    {
        private Timer m_ServiceTimer = null;
        private ILogger _logger;

        public ServerPoller()
        {
            _logger = Logger.GetLogger();
            m_ServiceTimer = new Timer(ServicePollIntervalInMinutes);
            m_ServiceTimer.Elapsed += new ElapsedEventHandler(serviceTimer_Elapsed);
            GC.KeepAlive(m_ServiceTimer);
        }

        private int ServicePollIntervalInMinutes
        {
            get
            {
                int second = 1000;
                int minute = 60;
                int time = MannusBackupServiceConfiguration.GetConfig().UsbListenTime;
                return time * minute * second;
            }
        }

        public void Start()
        {
            _logger.LogDebug("start");
            m_ServiceTimer.AutoReset = true;
            m_ServiceTimer.Enabled = true;
            m_ServiceTimer.Start();
        }

        public void Stop()
        {
            _logger.LogDebug("stop");
            m_ServiceTimer.AutoReset = false;
            m_ServiceTimer.Enabled = false;
        }

        public void Pause()
        {
            _logger.LogDebug("pause");
            m_ServiceTimer.Stop();
        }

        private void serviceTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            ReadServices();
        }

        private void ReadServices()
        {
            _logger.LogDebug("read services");
            m_ServiceTimer.Stop();
            try
            {
                BackupCopier copier = new BackupCopier();
                copier.CopyBackup();
            }
            catch (Exception e)
            {
                _logger.LogException(e);
            }
            _logger.LogDebug("start");
            m_ServiceTimer.Start();
        }
    }
}