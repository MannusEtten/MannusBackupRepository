using System.IO;
using MannusBackup.Configuration;
using MannusBackup.Tasks.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MannusBackup.UnitTests.Tasks
{
    [TestClass]
    public class XCopyTaskTest
    {
        private XCopyTask<DirectoryElement> _task;
        private string _path;

        [TestInitialize]
        public void Setup()
        {
            _task = new XCopyTask<DirectoryElement>();
            DirectoryElement element = MannusBackupConfiguration.GetConfig().BackupLocations["test"];
            _path = _task.GetLocation(element);
        }

        [TestMethod]
        public void RemoveReadonlyAttributesFromAllFiles_Is_True()
        {
            var directory = new DirectoryInfo(_path);
            _task.DeleteTemporaryDirectory(directory, "Test");
        }

        [TestMethod]
        public void documenten_Is_H_Documenten()
        {
            DirectoryElement directoryElement = MannusBackupConfiguration.GetConfig().BackupLocations[0];
            string result = _task.GetLocation(directoryElement);
            Assert.AreEqual(@"H:\documenten", result);
        }

        [TestMethod]
        public void desktop_Is_C_Documenten()
        {
            DirectoryElement directoryElement = MannusBackupConfiguration.GetConfig().BackupLocations[2];
            string result = _task.GetLocation(directoryElement);
            Assert.AreEqual(@"C:\Users\etten\Desktop", result);
        }

        [TestMethod]
        public void mailarchief_Is_D_mailarchief()
        {
            DirectoryElement directoryElement = MannusBackupConfiguration.GetConfig().BackupLocations[5];
            string result = _task.GetLocation(directoryElement);
            Assert.AreEqual(@"D:\Mannus\Mailarchieven\BackupMailArchieven", result);
        }

    }
}