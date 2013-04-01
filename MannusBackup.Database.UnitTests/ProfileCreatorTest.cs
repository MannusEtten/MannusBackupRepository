using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MannusBackup.Database.UnitTests
{
    [TestClass]
    public class ProfileCreatorTest
    {
        private ProfileCreator _profileCreator;

        [TestInitialize]
        public void Setup()
        {
            _profileCreator = new ProfileCreator();
        }

        [TestMethod]
        public void CreateProfile()
        {
            _profileCreator.CreateProfile(EnumProfileType.Client);
        }
    }
}
