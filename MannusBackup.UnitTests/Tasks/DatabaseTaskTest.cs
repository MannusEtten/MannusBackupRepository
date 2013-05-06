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

        [TestMethod]
        public void Execute()
        {
            var task = _databaseTask;
            task.Execute();
            var fileNames = Directory.GetFiles(@"C:\Dropbox\backup\");
            var file = fileNames.ToList().Find(f => f.Contains("basketbalnieuws.sql"));
            Assert.IsNotNull(file);
        }

        [TestInitialize]
        public void Setup()
        {
            var _repository = new Repository();
            var _profile = _repository.All<Profile>().Where(p => p.Id == 2).First();
            var sqlYogProfileProperty = _profile.Properties.Where(p => p.Name.Equals(ProfileProperties.SqlYog.ToString())).First();
            var profilePropertiesTypeName = ProfileProperties.SqlYog.ToString();
            var sqlYogConfigurationProperty = _repository.All<MannusBackup.Database.ConfigurationProperty>().Where(p => p.Name.Equals(profilePropertiesTypeName)).First();
            var sqlYogDatabaseConfigurations = _profile.Configuration.Where(p => p.configurationid == sqlYogConfigurationProperty.id);
            _databaseTask = new DatabaseTask();
            _databaseTask.SetConfiguration(sqlYogDatabaseConfigurations.First(), sqlYogProfileProperty);
        }
    }
}