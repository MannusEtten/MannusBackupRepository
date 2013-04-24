using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MannusBackup.BackupResults;

namespace MannusBackup.UnitTests.BackupResults
{
    [TestClass]
    public class BackupResultsDatabaseHandlerTest
    {
        private BackupResultsDatabaseHandler _databaseHandler;

        [TestInitialize]
        public void Setup()
        {
            _databaseHandler = new BackupResultsDatabaseHandler();
        }

        [TestMethod]
        public void SaveNewResults_Adds_One_Result()
        {
            var list = new List<BackupResult>() { new BackupResult() { Datum = DateTime.Now, Host = "Laptop", Naam = "Mannus", Password = "Etten", Size = "2225", SizeInGb = "2.2", Status = "Afgerond", Tijd = DateTime.Now } };
            var itemsBefore = _databaseHandler.GetResults().Count();
            _databaseHandler.SaveNewResults(list);
            var itemsAfter = _databaseHandler.GetResults().Count();
            var result = itemsAfter - itemsBefore;
            var expected = 1;
            Assert.AreEqual(expected, result);

        }
    }
}
