using System;
using MannusBackup.Configuration;
using MannusBackup.Database;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MannusBackup.UnitTests.Database
{
    [TestClass]
    public class MySQLStarterTest
    {
        private MySQLStarter _starter;
        private bool _eventHandled;

        [TestMethod]
        public void StartDatabase_With_Valid_ServiceName_Is_Ok()
        {
            _starter = new MySQLStarter(MannusBackupConfiguration.GetConfig().MySQLServiceName);
            _starter.TaskIsFinished += new EventHandler<TaskFinishedEventArgs>(_starter_TaskIsFinished);
            _starter.StartDatabase();
            Assert.IsTrue(_eventHandled);
        }

        [TestMethod]
        public void StartDatabase_With_Invalid_ServiceName_Is_Ok()
        {
            _starter = new MySQLStarter("PietjePuk");
            _starter.TaskIsFinished += new EventHandler<TaskFinishedEventArgs>(_starter_TaskIsFinished);
            _starter.StartDatabase();
            Assert.IsTrue(!_eventHandled);
        }

        private void _starter_TaskIsFinished(object sender, TaskFinishedEventArgs e)
        {
            _eventHandled = true;
        }
    }
}