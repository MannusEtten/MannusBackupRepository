using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace MannusBackup.Configuration
{
    public class DatabaseElement : MannusBackupElement
    {
        [ConfigurationProperty("database", IsRequired = true)]
        public string Database
        {
            get { return this["database"] as string; }
            set { this["database"] = value; }
        }
        [ConfigurationProperty("host", IsRequired = true)]
        public string Host
        {
            get { return this["host"] as string; }
            set { this["host"] = value; }
        }
        [ConfigurationProperty("username", IsRequired = true)]
        public string UserName
        {
            get { return this["username"] as string; }
            set { this["username"] = value; }
        }
        [ConfigurationProperty("password", IsRequired = true)]
        public string Password
        {
            get { return this["password"] as string; }
            set { this["password"] = value; }
        }
        public override string ElementName
        {
            get { return "Database"; }
        }
    }
}