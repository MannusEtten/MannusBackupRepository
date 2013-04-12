using System.Collections.Generic;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MannusBackup.UnitTests
{
    [TestClass]
    public class BackupNewTest
    {
        [TestMethod]
        public void Backup_Is_Succesfull()
        {
//            Assert.Inconclusive();
            BackupNew backuper = new BackupNew();
            backuper.Backup();
        }

        [TestMethod]
        public void SendMail()
        {
            BackupNew backuper = new BackupNew();
            List<string> xmlMessages = new List<string>();
            xmlMessages.Add("22:00:00");
            xmlMessages.Add("fout");
            xmlMessages.Add("b");
            xmlMessages.Add("c");
            xmlMessages.Add("d");
            backuper.SendMail(xmlMessages);
        }

        [TestMethod]
        public void Write_Text_To_Xml_File()
        {
            BackupNew backuper = new BackupNew();
            List<string> xmlMessages = new List<string>();
            xmlMessages.Add("22:00:00");
            xmlMessages.Add("fout");
            xmlMessages.Add("b");
            xmlMessages.Add("c");
            xmlMessages.Add("d");
            backuper.WriteMessagesToXmlFile(xmlMessages);
        }

        [TestMethod]
        public void DatabaseStarted_Test()
        {
            BackupNew backuper = new BackupNew();
            backuper.AddTasks();
            backuper.DatabaseStarted(null, null);
        }

        [TestMethod]
        public void GetSize()
        {
            BackupNew backuper = new BackupNew();
            long result = backuper.GetSize();
            long size2 = result;
            if (size2 > 1000)
            {
                size2 = result / 1000;
            }
            CultureInfo culture = new CultureInfo("nl-NL");
            string result2 = string.Format("grootte is: {0} mb\r\ngrootte is: {1} gb\r\n", result.ToString("0,###,###", culture), size2.ToString("0,###", culture));
        }
    }
}