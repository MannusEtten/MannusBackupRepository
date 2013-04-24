using Microsoft.VisualStudio.TestTools.UnitTesting;
using MannusBackup.BackupResults;
namespace MannusBackup.UnitTests.Service
{
    [TestClass]
    public class BackupResultsWriterTest
    {
        BackupResultsWriter _logwriter;

        [TestInitialize]
        public void Setup()
        {
            _logwriter = new BackupResultsWriter();
        }

        [TestMethod]
        public void WriteLogToDatabase()
        {
            _logwriter.WriteLogToDatabase();
        }
    }
}