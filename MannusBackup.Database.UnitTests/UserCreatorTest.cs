using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MannusBackup.Database.UnitTests
{
    [TestClass]
    public class UserCreatorTest
    {
        private UserCreator _userCreator;

        [TestInitialize]
        public void Setup()
        {
            _userCreator = new UserCreator();
        }

        [TestMethod]
        public void CreateUser()
        {
            _userCreator.CreateUser("UnitTest");
        }
    }
}
