using MannusBackup.BackupResults;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MannusBackup.UnitTests.BackupResults
{
    [TestClass]
    public class BackupResultsXmlFileHandlerTest
    {
        [TestMethod]
        public void Create_File_Handler()
        {
            var handler = new BackupResultsXmlFileHandler();
            Assert.IsNotNull(handler);
        }
    }
}