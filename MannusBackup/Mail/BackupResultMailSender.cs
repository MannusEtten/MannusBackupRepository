using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MannusBackup.BackupResults;
using System.IO;
using System.Net.Mail;
using System.Globalization;
using ESRINederland.Framework.Logging;
using MannusBackup.Configuration;
using System.Net.Mime;
using System.Net;

namespace MannusBackup.Mail
{
    internal class BackupResultMailSender
    {
        private ILogger _logger;

        public BackupResultMailSender()
        {
            _logger = Logger.GetLogger();
        }
        
        public void SendMail(BackupResult backupresult)
        {
            string htmlFileName = Path.Combine(@"C:\", "MannusBackupResult.html");
            string xmlFileName = Path.Combine(@"C:\", "MannusBackupResult.xml");
            var xml = BackupResult.ToXml(backupresult);
            xml.Save(xmlFileName);
            XmlDataToHtmlCreator htmlCreator = new XmlDataToHtmlCreator();
            htmlCreator.ExportResultsToHtmlForEmail(htmlFileName, xmlFileName);
            AlternateView htmlView = CreateMailContent(htmlFileName);
            MailAddress from = new MailAddress("mannusbackup@mannus.nl", "Mannus backup");
            MailAddress to = new MailAddress(MannusBackupConfiguration.GetConfig().ToMailAddress);
            var dateString = GetDateString();
            string title = string.Format("resultaten backup {0} voor '{1}'", dateString, MannusBackupConfiguration.GetConfig().HostName);
            MailMessage mailMessage = CreateMailMessage(from, to, htmlView, title);
            SendMail(mailMessage);
            // TODO opruimen van bestanden, process lockt de files nu
//            File.Delete(xmlFileName);
//            File.Delete(htmlFileName);
        }

        private AlternateView CreateMailContent(string pathToHtmlFile)
        {
            string transformedXmlFile = pathToHtmlFile;
            ContentType mimeType = new ContentType("text/html");
            AlternateView htmlView = new AlternateView(transformedXmlFile, mimeType);
            // TODO eigen logo inbouwen
//            string appImage = WaterToetsConfiguration.Configuration.UpAndRunningCheckerSettings.CheckerSettings["applicationlogo"].Value;
//            string esriImage = WaterToetsConfiguration.Configuration.UpAndRunningCheckerSettings.CheckerSettings["esrilogo"].Value;
//            LinkedResource esriLogo = new LinkedResource(esriImage, MediaTypeNames.Image.Jpeg);
//            esriLogo.ContentId = "EsriLogo";
//            LinkedResource applicationLogo = new LinkedResource(appImage, MediaTypeNames.Image.Jpeg);
 //           applicationLogo.ContentId = "ApplicationLogo";
 //           htmlView.LinkedResources.Add(esriLogo);
 //           htmlView.LinkedResources.Add(applicationLogo);
            return htmlView;
        }

        private MailMessage CreateMailMessage(MailAddress fromMailAddress, MailAddress toMailAddress, AlternateView content, string title)
        {
            MailMessage mail = new MailMessage(fromMailAddress, toMailAddress);
            mail.AlternateViews.Add(content);
            mail.Subject = title;
            mail.Sender = fromMailAddress;
            return mail;
        }

        private void SendMail(MailMessage mailMessage)
        {
            SmtpClient smtpClient = new SmtpClient();
            try
            {
                smtpClient.Host = "smtp.vevida.com";
                smtpClient.Port = 25;
                smtpClient.EnableSsl = true;
                smtpClient.Credentials = new NetworkCredential("mannusbackup@mannus.nl", "5mF8hv#GL2!kH#2");
                smtpClient.Send(mailMessage);
            }
            catch (SmtpException e)
            {
                _logger.LogException(e);
            }
        }

        private string GetDateString()
        {
            var date = DateTime.Now.ToString("d", CultureInfo.CreateSpecificCulture("nl-NL"));
            return date;
        }

    }
}