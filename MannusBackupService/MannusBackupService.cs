using System.ServiceProcess;
using ESRINederland.Framework.Logging;
using MannusBackup;

namespace MannusBackupService
{
    public partial class MannusBackupService : ServiceBase
    {
        private ServerPoller poller = null;
        private ILogger _logger;

        public MannusBackupService()
        {
            InitializeComponent();
            _logger = Logger.GetLogger();
            poller = new ServerPoller();
        }

        protected override void OnStart(string[] args)
        {
            _logger.LogDebug("start");
            poller.Start();
        }

        protected override void OnStop()
        {
            _logger.LogDebug("stop");
            poller.Stop();
        }

        protected override void OnPause()
        {
            _logger.LogDebug("pause");
            poller.Pause();
        }

        protected override void OnContinue()
        {
            _logger.LogDebug("continue");
            poller.Start();
        }
    }
}