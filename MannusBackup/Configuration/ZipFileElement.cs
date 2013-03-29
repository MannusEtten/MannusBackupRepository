using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using MannusBackup.Tasks;

namespace MannusBackup.Configuration
{
    public class ZipFileElement : MannusBackupElement
    {
        [ConfigurationProperty("type", IsRequired = true)]
        public TaskType Type
        {
            get 
            {
                return (TaskType) Enum.Parse(typeof(TaskType), this["type"].ToString());
            }
        }

        [ConfigurationProperty("childkey", IsRequired = true)]
        public string ChildKey
        {
            get 
            {
                return this["childkey"] as string;
            }
        }

        public override string ElementName
        {
            get { return "ZipFile"; }
        }
    }
}