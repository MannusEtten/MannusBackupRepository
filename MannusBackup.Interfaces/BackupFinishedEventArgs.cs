using System;
using ESRINederland.Framework.Logging;

namespace MannusBackup.Interfaces
{
    public class BackupFinishedEventArgs : EventArgs
    {
        public override string ToString()
        {
            Logger.GetLogger().LogDebug("backup afgerond");
            return "backup afgerond";
        }
    }
}