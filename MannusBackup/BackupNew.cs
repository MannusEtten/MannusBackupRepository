using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using ESRINederland.Framework.Configuration;
using ESRINederland.Framework.Logging;
using MannusBackup.BackupResults;
using MannusBackup.Configuration;
using MannusBackup.Database;
using MannusBackup.Outlook;
using MannusBackup.Tasks;
using MannusBackup.Tasks.Tasks;
using MySql.Data.MySqlClient;
using MannusBackup.Mail;
using MannusBackup.Storage;

namespace MannusBackup
{
    public class BackupNew
    {
        private int finishedTasks = 0;
        private ILogger logger;
        public event EventHandler<TaskFinishedEventArgs> TaskIsFinished;
        public event EventHandler<BackupFinishedEventArgs> BackupIsFinished;

        internal List<IBackupTask> Tasks { get; private set; }

        public BackupNew()
        {
            logger = Logger.GetLogger();
            Tasks = new List<IBackupTask>();
        }

        public void PrepareBackupOutlookArchives()
        {
            ArchiveBackupPreparer preparer = new ArchiveBackupPreparer();
            preparer.PrepareBackupMailArchives();
        }

        public void Backup()
        {
            var repository = new Repository();
            // TODO property ook in MannusConfiguration plaatsen
            // TODO profile ID uit configuratie halen
            var profile = repository.All<Profile>().Where(p => p.Id == 2).First();
            // TODO extension maken op Profile zodat we snel een specifieke property er uit kunnen halen en kunnen casten
            var numberOfBackups = int.Parse(profile.Properties.Where(p => p.Name.Equals("NumberOfLocalBackups")).First().Value);
            var localStorage = new LocalStorageManager();
            localStorage.DeleteBackupDirectories(numberOfBackups);
            logger.LogDebug("create base directory");
            try
            {
                DirectoryManager.CreateBaseDirectory();
            }
            catch (Exception e)
            {
                logger.LogException(e);
                return;
            }
            logger.LogDebug("add all backup tasks");
            AddTasks();
            logger.LogDebug("start MySQL-database");
            MySQLStarter databaseStarter = new MySQLStarter(MannusBackupConfiguration.GetConfig().MySQLServiceName);
            databaseStarter.TaskIsFinished += new EventHandler<TaskFinishedEventArgs>(DatabaseStarted);
            databaseStarter.StartDatabase();
            logger.LogDebug("execute all backup tasks");
            ExecuteAllTasks();
        }

        private void ExecuteAllTasks()
        {
            logger.LogDebug("execute all tasks");
            Tasks.ForEach(t => t.TaskIsFinished += new EventHandler<TaskFinishedEventArgs>(TaskIsFinishedHandler));
            Tasks.ForEach(t => t.ExecuteBackupTask());
        }

        internal void DatabaseStarted(object sender, TaskFinishedEventArgs e)
        {
            GenericConfigurationElementCollection<DatabaseElement> databases = MannusBackupConfiguration.GetConfig().Databases;
            foreach (DatabaseElement database in databases)
            {
                bool removeTask = TestDatabase(database);
                if (removeTask)
                {
                    string name = string.Format("Database - {0}", database.Key);
                    var item = from task in Tasks
                               where task.TaskName.Equals(name)
                               select task;
                    Tasks.Remove(item.First());
                }
            }
        }

        private bool TestDatabase(DatabaseElement database)
        {
            string connectionString = "Server={0};Database={1};Uid={2};Pwd={3}";
            connectionString = string.Format(connectionString, database.Host, database.Database, database.UserName, database.Password);
            try
            {
                logger.LogDebug(connectionString);
                string commandText = "SELECT COUNT(*) AS number_of_tables FROM information_schema.tables;";
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    using (MySqlCommand command = new MySqlCommand(commandText, connection))
                    {
                        if (connection.State != ConnectionState.Open)
                        {
                            connection.Open();
                        }
                        int result = int.Parse(command.ExecuteScalar().ToString());
                        return (result == 0);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogException(ex);
                return true;
            }
        }

        private void TaskIsFinishedHandler(object sender, TaskFinishedEventArgs e)
        {
            if (e.Count)
            {
                Interlocked.Increment(ref finishedTasks);
            }
            if (TaskIsFinished != null)
            {
                TaskIsFinished(this, e);
            }
            if (finishedTasks == Tasks.Count)
            {
                if (BackupIsFinished != null)
                {
                    BackupIsFinished(this, new BackupFinishedEventArgs());
                }
            }
        }

        internal void AddTasks()
        {
            logger.LogDebug("add tasks");
            GenericConfigurationElementCollection<DirectoryElement> directories = MannusBackupConfiguration.GetConfig().BackupLocations;
            BackupTask<DirectoryElement, XCopyTask<DirectoryElement>> task3 = new BackupTask<DirectoryElement, XCopyTask<DirectoryElement>>(directories, "XCopy");
            //GoogleElement googleDocsConfiguration = MannusBackupConfiguration.GetConfig().GoogleDocsConfiguration;
            //            BackupTask<GoogleElement, GoogleDocsTask<GoogleElement>> googleDocsTask = new BackupTask<GoogleElement, GoogleDocsTask<GoogleElement>>(googleDocsConfiguration, "GoogleDocs");
            GenericConfigurationElementCollection<FtpSiteElement> ftpSites = MannusBackupConfiguration.GetConfig().FtpSites;
            BackupTask<FtpSiteElement, WebsitesTask<FtpSiteElement>> task = new BackupTask<FtpSiteElement, WebsitesTask<FtpSiteElement>>(ftpSites, "Websites");
            GenericConfigurationElementCollection<DatabaseElement> databases = MannusBackupConfiguration.GetConfig().Databases;
            foreach (DatabaseElement database in databases)
            {
                string name = string.Format("Database - {0}", database.Key);
                BackupTask<DatabaseElement, DatabaseTask<DatabaseElement>> databaseTask = new BackupTask<DatabaseElement, DatabaseTask<DatabaseElement>>(database, name);
                Tasks.Add(databaseTask as IBackupTask);
            }
            //            Tasks.Add(googleDocsTask as IBackupTask);
            Tasks.Add(task as IBackupTask);
            Tasks.Add(task3 as IBackupTask);
        }

        private string CreateMessage(List<string> messages)
        {
            StringBuilder builder = new StringBuilder();
            messages.ForEach(s => builder.AppendLine(s));
            return builder.ToString();
        }

        public void Cleanup()
        {
            logger.LogDebug("clean the base directory");
            DirectoryManager.DeleteBaseDirectory();
            DeleteEmptyZipFiles();
            FileInfo[] files = new DirectoryInfo(MannusBackupConfiguration.BackupDirectory).GetFiles("*.zip");
            if (!MannusBackupConfiguration.GetConfig().TestSituation)
            {
                int numberOfFilesForCleanup = MannusBackupConfiguration.GetConfig().MinimumFilesToCleanup;
                if (files.Length <= numberOfFilesForCleanup)
                {
                    logger.LogDebug(string.Format("delete files because less than {0} available", numberOfFilesForCleanup));
                    Directory.Delete(MannusBackupConfiguration.BackupDirectory, true);
                }
            }
        }

        private void DeleteEmptyZipFiles()
        {
            logger.LogDebug("delete empty zip files");
            FileInfo[] files = new DirectoryInfo(MannusBackupConfiguration.BackupDirectory).GetFiles();
            foreach (FileInfo f in files)
            {
                logger.LogDebug(f.FullName);
                if (f.Length < 1000)
                {
                    logger.LogDebug("te kleine zip: " + f.FullName);
                    f.Delete();
                }
            }
        }

        public void WriteMessagesToFile(List<string> messages)
        {
            string baseDirectory = MannusBackupConfiguration.GetConfig().BaseBackupDirectory;
            string path = string.Format(@"{0}\backup_results", baseDirectory);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string filename = string.Format(@"{0}\{1:ddMMMMyyyy}.txt", path, DateTime.Now);
            if (File.Exists(filename))
            {
                File.Delete(filename);
            }
            File.WriteAllText(filename, CreateMessage(messages));
        }

        public void WriteMessagesToXmlFile(List<string> xmlMessages)
        {
            logger.LogDebug("schrijf resultaten weg naar xml");
            var result = BackupResult.FromXml(xmlMessages);
            var xmlFileHandler = new BackupResultsXmlFileHandler();
            xmlFileHandler.SaveResult(result);
        }

        public long GetSize()
        {
            FileInfo[] files = new DirectoryInfo(MannusBackupConfiguration.BackupDirectory).GetFiles();
            long result = 0;
            foreach (FileInfo f in files)
            {
                logger.LogDebug(f.FullName);
                result += f.Length;
            }
            return result / 1000;
        }

        public void WriteLogsToDatabase()
        {
            BackupResultsWriter logwriter = new BackupResultsWriter();
            logwriter.WriteLogToDatabase();
        }

        public void SendMail(List<string> xmlMessages)
        {
            // TODO: mail voorzien van logo Mannus.nl
            // TODO: implementeren
            BackupResultMailSender mailSender = new BackupResultMailSender();
            var backupResult = BackupResult.FromXml(xmlMessages);
            mailSender.SendMail(backupResult);
        }
    }
}