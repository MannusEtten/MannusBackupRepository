using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading;
using ESRINederland.Framework.Logging;
using MannusBackup;
using MannusBackup.Configuration;
using MannusBackup.Interfaces;

namespace MannusBackupApplication
{
    public class Program
    {
        private static bool Finished = false;
        private static List<string> messages = new List<string>();

        private static DateTime start;

        public static void Main(string[] args)
        {
            long minimum = MannusBackupConfiguration.GetConfig().MinimumAvailableDiskspaceInGB;
            string root = Directory.GetDirectoryRoot(MannusBackupConfiguration.GetConfig().BaseBackupDirectory);
            DiskSpaceChecker diskSpaceChecker = new DiskSpaceChecker(root, minimum);
            bool diskHasEnoughSpace = diskSpaceChecker.CheckAvailableDiskspace();
            if (diskHasEnoughSpace)
            {
                NewBackupMethod();
            }
        }

        private static void backup_BackupIsFinished(object sender, BackupFinishedEventArgs e)
        {
            DateTime eind = DateTime.Now;
            TimeSpan verschil = eind - start;
            string schilString = string.Format("{0}:{1}:{2}", verschil.Hours, verschil.Minutes, verschil.Seconds);
            string message = string.Format("backup klaargezet op {0} en gereed op {1} ({2})\r\n", start, eind, schilString);
            Console.WriteLine(message);
            messages.Add(message);
        }

        private static void backup_taskIsFinished(object sender, TaskFinishedEventArgs e)
        {
            Finished = true;
            Console.WriteLine(e.ToString());
            messages.Add(e.ToString());
        }

        private static string FormatSizeInGb(long size)
        {
            long size2 = size;
            if (size2 > 1000)
            {
                size2 = size / 1000;
            }
            CultureInfo culture = new CultureInfo("nl-NL");
            string result = string.Format("grootte is: {0} gb\r\n", size2.ToString("0,###", culture));
            return result;
        }

        private static string FormatSizeInMb(long size)
        {
            CultureInfo culture = new CultureInfo("nl-NL");
            string result = string.Format("grootte is: {0} mb\r\n", size.ToString("0,###,###", culture));
            return result;
        }

        private static void NewBackupMethod()
        {
            start = DateTime.Now;
            MannusBackupConfiguration config = MannusBackupConfiguration.GetConfig();
            BackupNew backup = new BackupNew();
            backup.TaskIsFinished += new EventHandler<TaskFinishedEventArgs>(backup_taskIsFinished);
            backup.BackupIsFinished += new EventHandler<BackupFinishedEventArgs>(backup_BackupIsFinished);
            bool error = false;
            try
            {
                backup.Backup();
            }
            catch (Exception e)
            {
                Logger.GetLogger().LogException(e);
                error = true;
            }
            if (!error)
            {
                while (!Finished)
                {
                    Thread.Sleep(10000);
                }
                backup.Cleanup();
            }
            long size = backup.GetSize();
            DateTime eind = DateTime.Now;
            TimeSpan verschil = eind - start;
            string schilString = string.Format("{0}:{1}:{2}", verschil.Hours, verschil.Minutes, verschil.Seconds);
            string message = string.Format("backup gestart op {0} en gereed op {1} ({2})\r\n", start, eind, schilString);
            List<string> xmlMessages = new List<string>();
            Console.WriteLine(message);
            messages.Add(message);
            xmlMessages.Add(schilString);
            if (!Directory.Exists(MannusBackupConfiguration.BackupDirectory) || error)
            {
                messages.Add("!!! geen backup gemaakt !!!");
                xmlMessages.Add("fout");
            }
            else
            {
                xmlMessages.Add("afgerond");
            }
            message = string.Format("password is: '{0}'\r\n", MannusBackupConfiguration.GetConfig().PassWord);
            messages.Add(message);
            xmlMessages.Add(MannusBackupConfiguration.GetConfig().PassWord);
            Console.WriteLine(message);
            message = FormatSizeInMb(size);
            messages.Add(message);
            xmlMessages.Add(message.Replace("\r\n", string.Empty));
            Console.WriteLine(message);
            message = FormatSizeInGb(size);
            messages.Add(message);
            xmlMessages.Add(message.Replace("\r\n", string.Empty));
            Console.WriteLine(message);
            backup.WriteMessagesToFile(messages);
            backup.WriteMessagesToXmlFile(xmlMessages);
            backup.WriteLogsToDatabase();
            backup.SendMail(xmlMessages);
        }
    }
}