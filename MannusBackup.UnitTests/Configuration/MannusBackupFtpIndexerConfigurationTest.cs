using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MannusBackup.Configuration;

namespace MannusBackup.UnitTests
{
    [TestClass]
    public class MannusBackupFtpIndexerConfigurationTest
    {
        [TestMethod]
        public void FtpSites_Count_Is_Two()
        {
            MannusBackupFtpIndexerConfiguration config = MannusBackupFtpIndexerConfiguration.GetConfig();
            Assert.AreEqual(2, config.FtpSites.Count);
        }
    }
}
