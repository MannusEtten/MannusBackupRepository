using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MannusBackup.FtpIndexer;

namespace MannusBackup.UnitTests
{
    [TestClass]
    public class MySqlDatabaseTest
    {
        MySqlDatabase database = new MySqlDatabase();
        [TestMethod]
        public void Open_And_Close_Database()
        {
            database.OpenDatabase();
            database.CloseDatabase();
        }

        [TestMethod]
        public void Delete_Existing_Records()
        {
            database.OpenDatabase();
            database.DeleteExistingData();
            database.CloseDatabase();
        }

        [TestMethod]
        public void Add_Record_To_Table()
        {
            database.OpenDatabase();
            database.AddFileToTable("mannus", "mannus.mannus");
            database.CloseDatabase();
        }
    }
}