using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ESRINederland.Framework.Logging;
using MannusBackup.Interfaces;

namespace MannusBackup.Tasks
{
    public abstract class TaskBase
    {
        protected ILogger _logger;

        protected TaskBase(TaskType typeOfTask)
        {
            Type = typeOfTask;
            _logger = Logger.GetLogger();
            TimeOut = 30;
            //! TODO: iets maken om uit de configuratie de time-out time op te halen
            //! TODO: groep-settings maken voor configuratie
        }

        public TaskType Type { get; private set; }

        protected internal abstract void Execute();

        public int TimeOut { get; set; }
    }
}