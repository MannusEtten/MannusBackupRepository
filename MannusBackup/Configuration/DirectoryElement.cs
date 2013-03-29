using System.Configuration;

namespace MannusBackup.Configuration
{
    public class DirectoryElement : MannusBackupElement
    {
        [ConfigurationProperty("location", IsRequired = true)]
        public string Location
        {
            get
            {
                var result = this["location"] as string;
                if (result.Contains("AND"))
                {
                    result = result.Replace("AND", "&");
                }
                return result;
            }
        }

        [ConfigurationProperty("drive", IsRequired = true)]
        public string Drive
        {
            get { return this["drive"] as string; }
        }

        public override string ElementName
        {
            get { return "Directory"; }
        }
    }
}