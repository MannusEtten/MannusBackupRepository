﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using ESRINederland.Framework.Logging;
using MannusBackup.Configuration;

namespace MannusBackup.Tasks.Tasks
{
    internal class XCopyTask<T> : TaskBase where T : MannusBackupElement, new()
    {
        public XCopyTask() : base(TaskType.XCopy) { }

        private Stack<DirectoryInfo> directories = new Stack<DirectoryInfo>();
        private int maximumNumberOfThreads = 3;
        private int numberOfDirectoriesCopied = 0;
        private int numberOfDirectories;

        protected override void Execute()
        {
            SetTaskDirectory();
            DirectoryElement element = Configuration as DirectoryElement;
            _logger.LogDebug(string.Format("{0} - backup xcopy ", element.Key));
            string location = GetLocation(element);
            if (string.IsNullOrEmpty(location))
            {
                _logger.LogInfo(string.Format("usb drive not attached so location '{0}' not available", element.Location));
                return;
            }
            DirectoryInfo root = new DirectoryInfo(location);
            DirectoryInfo[] directoriesInfo = null;
            _logger.LogDebug(string.Format("{0} - get root directories", element.Key));
            try
            {
                directoriesInfo = root.GetDirectories();
            }
            catch (DirectoryNotFoundException e)
            {
                _logger.LogError(e.Message);
                return;
            }
            _logger.LogDebug(string.Format("{0} - filter directories ", element.Key));
            directoriesInfo = FilterDirectories(directoriesInfo);
            _logger.LogDebug(string.Format("{0} - get zip files in root ", element.Key));
            FileInfo[] filesInRoot = root.GetFiles("*.zip");
            foreach (FileInfo file in filesInRoot)
            {
                file.Delete();
            }
            _logger.LogDebug(string.Format("{0} - add directory with files from root", element.Key));
            DirectoryInfo tempDocuDirectory = AddDirectoryWithFilesFromRootDirectory(root);
            if (MannusBackupConfiguration.GetConfig().TestSituation)
            {
                if (directoriesInfo.Length > 1)
                {
                    directories.Push(directoriesInfo[1]);
                }
            }
            else
            {
                _logger.LogDebug(string.Format("{0} - add directories to stack", element.Key));
                AddDirectoriesToStack(directoriesInfo);
            }
            if (maximumNumberOfThreads > directories.Count)
            {
                maximumNumberOfThreads = directories.Count;
            }
            numberOfDirectories = this.directories.Count();
            _logger.LogDebug(string.Format("{0} - start with first thread", element.Key));
            StartFirstThreads();
            while (numberOfDirectories != numberOfDirectoriesCopied)
            {
                Thread.Sleep(5000);
            }
            DeleteTemporaryDirectory(tempDocuDirectory, element.Key);
        }

        internal void DeleteTemporaryDirectory(DirectoryInfo directory, string key)
        {
            _logger.LogDebug(string.Format("{0} - delete temporary documents directory", key));
            try
            {
                RemoveReadonlyAttributeFromFilesInDirectory(directory);
                directory.Delete(true);
            }
            catch (UnauthorizedAccessException e)
            {
                _logger.LogError(string.Format("{0} - {1}", e.Message, directory.FullName));
            }
        }

        private void RemoveReadonlyAttributeFromFilesInDirectory(DirectoryInfo directory)
        {
            var files = directory.GetFiles("*", SearchOption.AllDirectories);
            foreach (FileInfo file in files)
            {
                file.Attributes = FileAttributes.Normal;
            }
        }

        internal string GetLocation(DirectoryElement element)
        {
            _logger.LogDebug("Directory Key: " + element.Key);
            _logger.LogDebug("Drive: " + element.Drive);
            _logger.LogDebug("Location: " + element.Location);
            DriveElement driveElement = MannusBackupConfiguration.GetConfig().Drives[element.Drive];
            _logger.LogDebug("Drive Key: " + driveElement.Key);
            string drive = MannusBackupConfiguration.FindDrive(driveElement);
            if (string.IsNullOrEmpty(drive))
            {
                return null;
            }
            return Path.Combine(drive, element.Location);
        }

        private DirectoryInfo[] FilterDirectories(DirectoryInfo[] directoriesInfo)
        {
            Exclusions = MannusBackupConfiguration.GetExclusionsForConfiguration(Type, Configuration.Key);
            foreach (ExclusionElement exclusion in Exclusions)
            {
                if (exclusion.TypeOfExclusion == ExclusionType.Directory && exclusion.Type == TaskType.XCopy)
                {
                    string name = exclusion.Value.ToLowerInvariant();
                    IEnumerable<DirectoryInfo> selected = directoriesInfo.Where(d => !d.Name.ToLower().Equals(name));
                    directoriesInfo = selected.ToArray();
                }
            }
            return directoriesInfo;
        }

        private DirectoryInfo AddDirectoryWithFilesFromRootDirectory(DirectoryInfo root)
        {
            if (Directory.Exists(Path.Combine(root.FullName, root.Name)))
            {
                try
                {
                    Directory.Delete(Path.Combine(root.FullName, root.Name), true);
                }
                catch (UnauthorizedAccessException e)
                {
                    _logger.LogError(string.Format("{0} - {1}", e.Message, Path.Combine(root.FullName, root.Name)));
                }
            }
            DirectoryInfo subDir = root.CreateSubdirectory(root.Name);
            FileInfo[] filesInRoot = root.GetFiles();
            foreach (FileInfo file in filesInRoot)
            {
                try
                {
                    file.CopyTo(string.Format(@"{0}\{1}", subDir.FullName, file.Name));
                }
                catch (IOException e)
                {
                    _logger.LogError(e.Message);
                }
            }
            directories.Push(subDir);
            return subDir;
        }

        private void StartFirstThreads()
        {
            for (int i = 0; i < maximumNumberOfThreads; i++)
            {
                StartThread();
            }
        }

        private void StartThread()
        {
            if (directories.Count > 0)
            {
                DirectoryInfo directory = directories.Pop();
                CopyTask task = new CopyTask(directory, Configuration.Key, TaskDirectory);
                task.TaskIsFinished += new EventHandler<TaskFinishedEventArgs>(DirectoryIsCopied);
                ThreadStart threadStart = new ThreadStart(task.Execute);
                Thread thread = new Thread(threadStart);
                thread.Start();
            }
        }

        private void DirectoryIsCopied(object sender, TaskFinishedEventArgs e)
        {
            Interlocked.Increment(ref numberOfDirectoriesCopied);
            StartThread();
        }

        private void AddDirectoriesToStack(DirectoryInfo[] directories)
        {
            foreach (DirectoryInfo directory in directories)
            {
                this.directories.Push(directory);
            }
        }
    }

    internal class CopyTask
    {
        DirectoryInfo directory;
        private string key;
        private string backupDirectory;
        public event EventHandler<TaskFinishedEventArgs> TaskIsFinished;

        public CopyTask(DirectoryInfo directory, string key, string backupDirectory)
        {
            this.directory = directory;
            this.key = key;
            this.backupDirectory = backupDirectory;
        }

        public void Execute()
        {
            ZipTask<ZipFileElement> zipTask = new ZipTask<ZipFileElement>();
            string zipFileName = string.Format(@"{0}\{1}.zip", directory.Parent.FullName, directory.Name);
            string prefix = MannusBackupConfiguration.GetConfig().HostName;
            string backupZipFileName = string.Format(@"{0}\{1}_{2}_{3}.zip", backupDirectory, prefix, key, directory.Name);
            zipTask.Exclusions = MannusBackupConfiguration.GetExclusionsForConfiguration(TaskType.XCopy, key);
            zipTask.ExecuteZipTask(directory.FullName, zipFileName);
            try
            {
                File.Move(zipFileName, backupZipFileName);
            }
            catch (Exception e)
            {
                Logger.GetLogger().LogError(string.Format("{0}, {1}, {2}, {3}, {4}", e.Message, key, backupDirectory, zipFileName, backupZipFileName));
            }
            TaskIsFinished(this, null);
        }
    }
}