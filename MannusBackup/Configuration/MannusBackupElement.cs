using System.Configuration;
using ESRINederland.Framework.Configuration;

namespace MannusBackup.Configuration
{
    public class MannusBackupElement : ConfigurationElementBase
    {
        [ConfigurationProperty("timeout", DefaultValue = 0)]
        public int TimeOut
        {
            get { return int.Parse(this["timeout"].ToString()); }
        }

        [ConfigurationProperty("removeaftercompletion", DefaultValue = false)]
        public bool RemoveDirectoryAfterCompletion
        {
            get { return bool.Parse(this["removeaftercompletion"].ToString()); }
        }

        public override string ElementName
        {
            get { return "MannusBackupElement"; }
        }
    }
}