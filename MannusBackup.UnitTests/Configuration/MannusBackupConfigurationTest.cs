using System;
using System.Collections.Generic;
using System.Globalization;
using MannusBackup.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
namespace MannusBackup.UnitTests
{
    /// <summary>
    /// Summary description for MannusBackupConfigurationTest
    /// </summary>
    [TestClass]
    public class MannusBackupConfigurationTest
    {
        [TestMethod]
        public void UsbListenTime_Is_1()
        {
            Assert.AreEqual(MannusBackupServiceConfiguration.GetConfig().UsbListenTime, 10);
        }

        [TestMethod]
        public void Password_Does_Not_Change()
        {
            var pwd = MannusBackupConfiguration.GetConfig().PassWord;
            var pwd2 = MannusBackupConfiguration.GetConfig().PassWord;
            var pwd3 = MannusBackupConfiguration.GetConfig().PassWord;
            StringAssert.Equals(pwd, pwd2);
            StringAssert.Equals(pwd3, pwd2);
        }

        [TestMethod]
        public void BackupDirectory_Name_Does_Not_Contain_Dutch_MonthNames()
        {
            string backupDirectory = MannusBackupConfiguration.BackupDirectory;
            DateTimeFormatInfo dtfi = CultureInfo.InvariantCulture.DateTimeFormat;
            Assert.IsTrue(dtfi.MonthNames.Any(mn => backupDirectory.Contains(mn)));
        }

        [TestMethod]
        public void FindDrive_Does_Not_Crash()
        {
            DriveElement driveElement = MannusBackupConfiguration.GetConfig().Drives["Local Disk"];
            string driveLetter = MannusBackupConfiguration.FindDrive(driveElement);
            Assert.AreEqual(driveLetter, @"C:\");
        }

        [TestMethod]
        public void FindDrive_Local_Disk_Is_C()
        {
            DriveElement driveElement = MannusBackupConfiguration.GetConfig().Drives["Local Disk"];
            string driveLetter = MannusBackupConfiguration.FindDrive(driveElement);
            Assert.AreEqual(driveLetter, @"C:\");
        }

        [TestMethod]
        public void Find_Directory_With_Ampersand()
        {
            var directories = MannusBackupConfiguration.GetConfig().BackupLocations;
            var directory = directories["paenma1"];
            StringAssert.Contains(directory.Location, " & ");
        }
    }
}