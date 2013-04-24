using System.Net.Mail;
using ESRINederland.Framework.Logging;

namespace MannusBackup
{
    /// <summary>
    /// Verzend emails
    /// </summary>
    public static class MessageMailer
    {
        /// <summary>
        /// Verzend een mail
        /// </summary>
        /// <param name="emailaddress">Emailadres van ontvanger</param>
        /// <param name="title">Titel</param>
        /// <param name="message">Inhoud</param>
        public static void SendMessage(string emailaddress, string title, string message)
        {
            MailMessage mailmessage = new MailMessage();
            mailmessage.Body = message;
            mailmessage.From = new MailAddress("mannusbackup@mannus.nl", "MannusBackup");
            mailmessage.IsBodyHtml = false;
            mailmessage.Subject = title;
            MailAddressCollection collection = new MailAddressCollection();
            mailmessage.To.Add(new MailAddress(emailaddress, "MannusBackup"));
            SmtpClient client = new SmtpClient();
            try
            {
                client.Send(mailmessage);
            }
            catch (SmtpException e)
            {
                ILogger logger = Logger.GetLogger();
                logger.LogException(e);
            }
        }
    }
}