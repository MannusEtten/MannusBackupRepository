using System.IO;
using MannusBackup.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MannusBackup.UnitTests
{
    [TestClass]
    public class BackupCopierTest
    {
        private BackupCopier _copier;

        [TestInitialize]
        public void Setup()
        {
            _copier = new BackupCopier();
        }

        [TestMethod]
        public void GetLatestBackup_Is_2602_2011()
        {
            string directoryName = _copier.GetLatestBackup().FullName;
            StringAssert.Contains(directoryName, "12november2011");
        }

        [TestMethod]
        public void CopyBackup()
        {
            _copier.CopyBackup();
        }

        [TestMethod]
        public void LatestBackupAtUsbDrive_Is_False()
        {
            DirectoryInfo backupDirectory = _copier.GetLatestBackup();
            UsbDriveElement drive = MannusBackupServiceConfiguration.GetConfig().UsbDrives[0];
            string path = MannusBackupServiceConfiguration.FindDrive(drive);
            path = Path.Combine(path, drive.BackupDirectory);
            Assert.IsFalse(_copier.LatestBackupAtUsbDrive(path, backupDirectory));
        }

        [TestMethod]
        public void GetBackupDirectoriesFromBase_Count_Is_Ten()
        {
            var list = _copier.GetBackupDirectoriesFromBase(@"D:\backups");
            Assert.AreEqual(10, list.Count);
        }

        [TestMethod]
        public void DeleteOldBaseBackupDirectories()
        {
            _copier.DeleteOldBaseBackupDirectories();
            var list = _copier.GetBackupDirectoriesFromBase(@"D:\backups");
            Assert.AreEqual(10, list.Count);
        }
    }
}