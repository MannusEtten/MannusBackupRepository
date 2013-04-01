using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MannusBackup.UnitTests
{
    [TestClass]
    public class DateTimeCreatorTest
    {
        private DateTimeCreator _dateCreator;
        private DateTime _defaultDate;

        [TestInitialize]
        public void Setup()
        {
            _dateCreator = new DateTimeCreator();
            _defaultDate = new DateTime(1900, 1, 1);
        }

        [TestMethod]
        public void FromDateStamp_With_Value_Backup_27May2012_And_Directory_From_Base_Is_True_Is_27_05_2012()
        {
            string directoryName = "backup_27May2012";
            var result = _dateCreator.FromDateStamp(directoryName, true);
            DateTime expected = new DateTime(2012, 5, 27);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void FromDateStamp_With_Value_Backup_27May2012_And_Directory_From_Base_Is_False_Is_27_05_2012()
        {
            string directoryName = "27May2012";
            var result = _dateCreator.FromDateStamp(directoryName, false);
            DateTime expected = new DateTime(2012, 5, 27);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
      public void FromDateStamp_With_Value_Backup_Laptop_01april2013()
        {
            string directoryName = "backup_laptop_01April2013";
            var result = _dateCreator.FromDateStamp(directoryName, false);
            DateTime expected = new DateTime(2013, 4, 01);
            Assert.AreEqual(expected, result);
        }
        [TestMethod]
        public void FromDateStamp_And_Directory_Not_From_Base_With_Invalid_Date_Returns_1_1_1900()
        {
            string directoryName = "Mannus";
            var result = _dateCreator.FromDateStamp(directoryName);
            Assert.AreEqual(_defaultDate, result);
        }

        [TestMethod]
        public void FromDateStamp_And_Directory_From_Base_With_Invalid_Date_Returns_1_1_1900()
        {
            string directoryName = "Mannus";
            var result = _dateCreator.FromDateStamp(directoryName, true);
            Assert.AreEqual(_defaultDate, result);
        }

        [TestMethod]
        public void FromDateStamp_And_DirectoryName_Has_Invalid_Day_Base_Is_False_Returns_1_1_1900()
        {
            FromDateStamp_Expect_1_1_1900("32May2012", false);
        }

        [TestMethod]
        public void FromDateStamp_And_DirectoryName_Has_Invalid_Day_Base_Is_True_Returns_1_1_1900()
        {
            FromDateStamp_Expect_1_1_1900("32May2012", true);
        }

        private void FromDateStamp_Expect_1_1_1900(string date, bool fromBase)
        {
            var result = _dateCreator.FromDateStamp(date, fromBase);
            Assert.AreEqual(_defaultDate, result);
        }
    }
}