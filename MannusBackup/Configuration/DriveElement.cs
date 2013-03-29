using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace MannusBackup.Configuration
{
    public class DriveElement : MannusBackupElement
    {
        public override string ElementName
        {
            get { return "Drive"; }
        }
    }
}