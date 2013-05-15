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
using MannusBackup.Interfaces;
using MannusBackup.Mail;
using MannusBackup.Outlook;
using MannusBackup.Storage;
using MannusBackup.Tasks;
using MannusBackup.Tasks.Tasks;
using MySql.Data.MySqlClient;

namespace MannusBackup
{
    public class BackupNew
    {
        private Profile _profile;
        private Repository _repository;
        private int finishedTasks = 0;
        private ILogger logger;

        public BackupNew()
        {
            logger = Logger.GetLogger();
            Tasks = new List<IBackupTask>();
            _repository = new Repository();
            // TODO property ook in MannusConfiguration plaatsen
            // TODO profile ID uit configuratie halen
            _profile = _repository.All<Profile>().Where(p => p.Id == 2).First();
        }

        public event EventHandler<BackupFinishedEventArgs> BackupIsFinished;

        public event EventHandler<TaskFinishedEventArgs> TaskIsFinished;

        internal List<IBackupTask> Tasks { get; private set; }

        public void Backup()
        {
            // TODO extension maken op Profile zodat we snel een specifieke property er uit kunnen halen en kunnen casten
            var numberOfBackups = int.Parse(_profile.Properties.Where(p => p.Name.Equals("NumberOfLocalBackups")).First().Value);
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
            logger.LogDebug("execute all backup tasks");
            ExecuteAllTasks();
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

        public void PrepareBackupOutlookArchives()
        {
            ArchiveBackupPreparer preparer = new ArchiveBackupPreparer();
            preparer.PrepareBackupMailArchives();
        }

        public void SendMail(List<string> xmlMessages)
        {
            // TODO: mail voorzien van logo Mannus.nl
            // TODO: implementeren
            BackupResultMailSender mailSender = new BackupResultMailSender();
            var backupResult = BackupResult.FromXml(xmlMessages);
            mailSender.SendMail(backupResult);
        }

        public void WriteLogsToDatabase()
        {
            BackupResultsWriter logwriter = new BackupResultsWriter();
            logwriter.WriteLogToDatabase();
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

        internal void AddDatabaseTasks()
        {
            var configurationRepository = new ConfigurationRepository(_profile);
            var sqlYogConfiguration = configurationRepository.GetDatabaseTaskConfiguration();
            var databaseTasks = new List<DatabaseTask>();
            foreach (var configurationGroup in sqlYogConfiguration.Configurations)
            {
                var sqlYogConfigurationProperties = configurationGroup.Configuration;
                DatabaseTask databaseTask = new DatabaseTask();
                databaseTask.SetConfiguration(sqlYogConfigurationProperties, sqlYogConfiguration.SqlYogProfileProperty);
                databaseTasks.Add(databaseTask);
                //!+ task maken die oude .sql-files en de session/log files opruimt
                //!+ in configuratie opnemen dat c:\dropbox\backup wordt meegenomen in de zip-files
            }
            BackupTask<DatabaseTask> databaseTaskContainer = new BackupTask<DatabaseTask>(databaseTasks, "database");
            Tasks.Add(databaseTaskContainer as IBackupTask);
        }

        internal void AddTasks()
        {
            logger.LogDebug("add tasks");
            GenericConfigurationElementCollection<DirectoryElement> directories = MannusBackupConfiguration.GetConfig().BackupLocations;
            BackupTaskOld<DirectoryElement, XCopyTask<DirectoryElement>> task3 = new BackupTaskOld<DirectoryElement, XCopyTask<DirectoryElement>>(directories, "XCopy");
            //GoogleElement googleDocsConfiguration = MannusBackupConfiguration.GetConfig().GoogleDocsConfiguration;
            //            BackupTask<GoogleElement, GoogleDocsTask<GoogleElement>> googleDocsTask = new BackupTask<GoogleElement, GoogleDocsTask<GoogleElement>>(googleDocsConfiguration, "GoogleDocs");
            GenericConfigurationElementCollection<FtpSiteElement> ftpSites = MannusBackupConfiguration.GetConfig().FtpSites;
            BackupTaskOld<FtpSiteElement, WebsitesTask<FtpSiteElement>> task = new BackupTaskOld<FtpSiteElement, WebsitesTask<FtpSiteElement>>(ftpSites, "Websites");

            AddDatabaseTasks();
            //! TODO: tasks aanmaken vanuit een andere class

            //            Tasks.Add(googleDocsTask as IBackupTask);
            Tasks.Add(task as IBackupTask);
            Tasks.Add(task3 as IBackupTask);
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

        private string CreateMessage(List<string> messages)
        {
            StringBuilder builder = new StringBuilder();
            messages.ForEach(s => builder.AppendLine(s));
            return builder.ToString();
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

        private void ExecuteAllTasks()
        {
            logger.LogDebug("execute all tasks");
            Tasks.ForEach(t => t.TaskIsFinished += new EventHandler<TaskFinishedEventArgs>(TaskIsFinishedHandler));
            Tasks.ForEach(t => t.ExecuteBackupTask());
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
    }
}