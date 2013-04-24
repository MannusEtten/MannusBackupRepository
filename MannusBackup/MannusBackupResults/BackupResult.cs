using System;
using System.Collections.Generic;
using MannusBackup.Configuration;
using System.Xml.Linq;
using System.Globalization;
namespace MannusBackup.BackupResults
{
    public class BackupResult
    {
        public DateTime Datum { get; set; }

        public string Host { get; set; }

        public DateTime Tijd { get; set; }

        public string Status { get; set; }

        public string Naam { get; set; }

        public string Password { get; set; }

        // TODO velden weer van double/int maken en opmaken middels string.format
        public string Size { get; set; }

        public string SizeInGb { get; set; }

        public static BackupResult FromXml(List<string> xmlMessages)
        {
            BackupResult result = new BackupResult();
            result.Datum = DateTime.Now;
            result.Host = MannusBackupConfiguration.GetConfig().HostName;
            result.Tijd = DateTime.Parse(xmlMessages[0]);
            result.Naam = MannusBackupConfiguration.BackupDirectory;
            result.Status = xmlMessages[1];
            result.Password = xmlMessages[2];
            result.Size = xmlMessages[3];
            result.SizeInGb = xmlMessages[4];
            return result;
        }

        internal static XElement ToXml(BackupResult backupresult)
        {
            XElement element = new XElement("resultaat");
            XElement datumElement = new XElement("datum");
            datumElement.Value = backupresult.Datum.ToString(CultureInfo.InvariantCulture);
            XElement timeElement = new XElement("totaletijd");
            timeElement.Value = backupresult.Tijd.ToString(CultureInfo.InvariantCulture);
            XElement resultElement = new XElement("status");
            resultElement.Value = backupresult.Status;
            XElement nameElement = new XElement("naam");
            nameElement.Value = backupresult.Naam;
            XElement pwdElement = new XElement("password");
            pwdElement.Value = backupresult.Password;
            XElement size = new XElement("size");
            size.Value = backupresult.Size;
            XElement sizeInGb = new XElement("sizeingb");
            sizeInGb.Value = backupresult.SizeInGb;
            XElement host = new XElement("hostname");
            host.Value = backupresult.Host;
            element.Add(host);
            element.Add(datumElement);
            element.Add(timeElement);
            element.Add(resultElement);
            element.Add(nameElement);
            element.Add(pwdElement);
            element.Add(size);
            element.Add(sizeInGb);
            return element;
        }

        internal static BackupResult FromXml(XElement resultaat)
        {
            BackupResult result = new BackupResult();
            result.Host = resultaat.Element("hostname").Value;
            result.Datum = DateTime.Parse(resultaat.Element("datum").Value, CultureInfo.InvariantCulture);
            result.Tijd = DateTime.Parse(resultaat.Element("totaletijd").Value, CultureInfo.InvariantCulture);
            result.Status = resultaat.Element("status").Value;
            result.Naam = resultaat.Element("naam").Value;
            result.Password = resultaat.Element("password").Value;
            result.Size = resultaat.Element("size").Value;
            result.SizeInGb = resultaat.Element("sizeingb").Value;
            return result;
        }
    }
}