using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MannusBackup.Database
{
    public class SqlYogConfiguration
    {
        public IEnumerable<ProfileConfigurationGroup> Configurations { get; set; }

        public ProfileProperty SqlYogProfileProperty { get; set; }
    }
}