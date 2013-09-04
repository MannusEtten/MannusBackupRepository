using System;
using System.Linq;
using MannusBackup.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MannusBackup.Database.UnitTests
{
    [TestClass]
    public class ConfigurationRepositoryTest
    {
        private ConfigurationRepository _configurationRepository;

        [TestMethod]
        public void GetDatabaseTaskConfiguration_Count_Is_Two()
        {
            var configuration = _configurationRepository.GetDatabaseTaskConfiguration();
            var result = configuration.Configurations.Count();
            var expected = 2;
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void GetProfileConfigurationValue_Is_Ok()
        {
            var configuration = _configurationRepository.GetDatabaseTaskConfiguration();
            var specificConfiguration = configuration.Configurations.ToList()[0];
            var result = _configurationRepository.GetProfileConfigurationValue(specificConfiguration.Configuration, ProfileSubProperties.BackupDirectory);
            var expected = @"C:\Dropbox\backup";
            Assert.AreEqual(expected, result);
        }

        [TestInitialize]
        public void Setup()
        {
            var repository = new Repository();
            var profile = repository.All<Profile>().Where(p => p.Id == 2).First();
            _configurationRepository = new ConfigurationRepository(profile);
        }
    }
}