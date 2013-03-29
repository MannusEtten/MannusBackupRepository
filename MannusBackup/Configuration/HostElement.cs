using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace MannusBackup.Configuration
{
    public class HostElement : MannusBackupElement
    {
        [ConfigurationProperty("basebackupdirectory", IsRequired = true)]
        public int BaseBackupDirectory
        {
            get { return (int)this["basebackupdirectory"]; }
        }

        public override string ElementName
        {
            get { return "Host"; }
        }
    }
}