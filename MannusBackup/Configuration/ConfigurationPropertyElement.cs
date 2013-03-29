using System.Configuration;
using ESRINederland.Framework.Configuration;

namespace MannusBackup.Configuration
{
    public class ConfigurationPropertyElement : ConfigurationElementBase
    {
        [ConfigurationProperty("value")]
        public string Value
        {
            get { return this["value"] as string; }
        }

        public override string ElementName
        {
            get { return "ConfigurationProperty"; }
        }
    }
}