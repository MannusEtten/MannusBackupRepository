using System.Configuration;
using System.Diagnostics;
using MannusBackup;

namespace OutLookBackupPrepareTool
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Program p = new Program();
            p.Start();
        }

        private void Start()
        {
            BackupNew backupTool = new BackupNew();
            backupTool.PrepareBackupOutlookArchives();
            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = ConfigurationManager.AppSettings["outlook"];
            process.StartInfo = startInfo;
            process.Start();
        }
    }
}