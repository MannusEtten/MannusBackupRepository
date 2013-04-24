using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace MannusBackup.Configuration
{
    public class FtpSiteElement : MannusBackupElement
    {
        [ConfigurationProperty("ftpaddress", IsRequired = true)]
        public string FtpAddress 
        {
            get { return this["ftpaddress"] as string; }
            set { this["ftpaddress"] = value; }
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
            get { return "FtpSite"; }
        }
    }
}