using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using MannusBackup.Tasks;

namespace MannusBackup.Configuration
{
    public enum ExclusionType
    {
        Directory,
        File,
        FileExtension
    }
    public class ExclusionElement : MannusBackupElement
    {
        [ConfigurationProperty("type")]
        public TaskType Type
        {
            get { return  (TaskType)Enum.Parse(typeof(TaskType), this["type"].ToString());}
        }
 
        [ConfigurationProperty("childkey")]
        public string ChildKey
        {
            get { return this["childkey"] as string; }
        }

        [ConfigurationProperty("value")]
        public string Value
        {
            get { return this["value"] as string; }
        }

        [ConfigurationProperty("exclusiontype")]
        public ExclusionType TypeOfExclusion
        {
            get { return (ExclusionType)Enum.Parse(typeof(ExclusionType), this["exclusiontype"].ToString()); }
        }

        public override string ElementName
        {
            get { return "Exclusion"; }
        }
    }
}