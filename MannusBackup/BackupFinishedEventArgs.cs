using System;
using ESRINederland.Framework.Logging;

namespace MannusBackup
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