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
        }

        protected TaskType Type { get; private set; }

        protected internal abstract void Execute();
    }
}