using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MannusBackup.UnitTests
{
    [TestClass]
    public class MessageMailerTest
    {
        [TestMethod]
        public void SendMessage()
        {
            string receiver = "mannus@mannus.nl";
            string title = "titel";
            string text = "tekst";
            MessageMailer.SendMessage(receiver, title, text);
        }
    }
}