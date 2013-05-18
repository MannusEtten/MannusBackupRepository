using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ESRINederland.Framework.Logging;
using MannusBackup.Database;
using MannusBackup.Interfaces;

namespace MannusBackup.Tasks
{
    public abstract class TaskBase
    {
        protected ConfigurationRepository _configurationRepository;
        protected ILogger _logger;

        protected TaskBase(TaskType typeOfTask)
        {
            Type = typeOfTask;
            _logger = Logger.GetLogger();
            TimeOut = 30;
            _configurationRepository = new ConfigurationRepository();
            //! TODO: iets maken om uit de configuratie de time-out time op te halen
            //! TODO: groep-settings maken voor configuratie
        }

        public int TimeOut { get; set; }

        public TaskType Type { get; private set; }

        protected internal abstract void Execute();
    }
}