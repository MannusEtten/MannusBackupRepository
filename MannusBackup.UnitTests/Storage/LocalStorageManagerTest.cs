using System;
using System.IO;
using MannusBackup.Configuration;
using MannusBackup.Storage;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MannusBackup.UnitTests
{
    [TestClass]
    public class LocalStorageManagerTest
    {
        private LocalStorageManager _localStorageManager;

        [TestInitialize]
        public void Setup()
        {
            _localStorageManager = new LocalStorageManager();
        }


        [TestMethod]
        public void GetBackupDirectories()
        {
            var baseDirectory = MannusBackupConfiguration.GetConfig().BaseBackupDirectory;
            DirectoryManager.CreateBaseDirectory();
           var count = _localStorageManager.GetBackupDirectories(baseDirectory).Count;
            Assert.AreEqual(1, count);
        }

        [TestMethod]
        public void DeleteBackupDirectories()
        {
            var baseDirectory = MannusBackupConfiguration.GetConfig().BaseBackupDirectory;
            DirectoryManager.CreateBaseDirectory();
            var backupDirectory = MannusBackupConfiguration.BackupDirectory;
            _localStorageManager.DeleteBackupDirectories(0);
            Assert.IsFalse(Directory.Exists(backupDirectory));
        }
    }
}
