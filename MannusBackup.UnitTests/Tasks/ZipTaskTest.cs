using System.Collections.Generic;
using MannusBackup.Configuration;
using MannusBackup.Tasks.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MannusBackup.UnitTests.Tasks
{
    [TestClass]
    public class ZipTaskTest
    {
        [TestMethod]
        public void Zip()
        {
            ZipTask<ZipFileElement> task = new ZipTask<ZipFileElement>();
            task.Exclusions = new List<ExclusionElement>();
            task.ExecuteZipTask(@"c:\dp3", @"c:\mannusbackup.zip");
        }
    }
}