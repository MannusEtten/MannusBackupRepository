using System;
using System.Configuration;
using System.IO;
using System.Linq;
using MannusBackup.Database;
using MannusBackup.Interfaces;
using MannusBackup.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MannusBackup.UnitTests.Tasks
{
    [TestClass]
    public class DatabaseTaskTest
    {
        private DatabaseTask _databaseTask;
        private string _dropboxFolder = @"C:\Dropbox\backup\";

        [TestMethod]
        public void Execute()
        {
            if (!Directory.Exists(_dropboxFolder))
            {
                Assert.Inconclusive();
            }
            var fileNames = Directory.GetFiles(_dropboxFolder);
            fileNames.ToList().ForEach(f => File.Delete(f));
            var task = _databaseTask;
            task.Execute();
            fileNames = Directory.GetFiles(_dropboxFolder);
            var file = fileNames.ToList().Find(f => f.Contains("basketbalnieuws.sql"));
            Assert.IsNotNull(file);
            Assert.AreEqual(1, fileNames.Length);
        }

        [TestInitialize]
        public void Setup()
        {
            var _repository = new Repository();
            var _profile = _repository.All<Profile>().Where(p => p.Id == 2).First();
            var configuration = new ConfigurationRepository(_profile).GetDatabaseTaskConfiguration();
            _databaseTask = new DatabaseTask();
            var properties = configuration.Configurations.ToList()[0].Configuration;
            _databaseTask.SetConfiguration(properties, configuration.SqlYogProfileProperty);
        }
    }
}