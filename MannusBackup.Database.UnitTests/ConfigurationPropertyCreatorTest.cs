using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MannusBackup.Database.UnitTests
{
    [TestClass]
    public class ConfigurationPropertyCreatorTest
    {
        private ConfigurationPropertyCreator _creator;

        [TestInitialize]
        public void Setup()
        {
            _creator = new ConfigurationPropertyCreator();
        }

        [TestMethod]
        public void CreateConfigurationProperty()
        {
            Assert.Inconclusive();
            _creator.CreateConfigurationProperty("SqlYog");
        }
    }
}