using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MannusBackup.UnitTests
{
    [TestClass]
    public class DiskSpaceCheckerTest
    {
        [TestMethod]
        public void CheckAvailableDiskspace_With_Minimum_Of_1_Gb_C_Drive_Is_True()
        {
            var diskChecker = GetDiskSpaceChecker(@"c:\", 1);
            var result = diskChecker.CheckAvailableDiskspace();
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CheckAvailableDiskspace_With_Minimum_Of_250_Gb_C_Drive_Is_False()
        {
            var diskChecker = GetDiskSpaceChecker(@"c:\", 250);
            var result = diskChecker.CheckAvailableDiskspace();
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void CheckAvailableDiskspace_With_Invalid_Drive_Is_False()
        {
            var diskChecker = GetDiskSpaceChecker(@"z:\", 1);
            var result = diskChecker.CheckAvailableDiskspace();
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void CheckAvailableDiskspace_With_Invalid_Directory_Minimum_Of_1_Is_False()
        {
            var diskChecker = GetDiskSpaceChecker(@"c:\abacadrabahieperdepiep", 1);
            var result = diskChecker.CheckAvailableDiskspace();
            Assert.IsFalse(result);
        }

        private DiskSpaceChecker GetDiskSpaceChecker(string directory, long minimumSpace)
        {
            var diskChecker = new DiskSpaceChecker(directory, minimumSpace);
            return diskChecker;
        }
    }
}