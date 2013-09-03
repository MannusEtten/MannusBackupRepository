using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using ESRINederland.Framework.Logging;
using MannusBackup.BackupResults;
using MannusBackup.Configuration;

namespace MannusBackup
{
	public class BackupCopier
	{
		#region Fields (2) 

		private DateTimeCreator _dateTimeCreator;
		private ILogger _logger;

		#endregion Fields 

		#region Constructors (1) 

		public BackupCopier()
		{
			_logger = Logger.GetLogger();
			_dateTimeCreator = new DateTimeCreator();
		}

		#endregion Constructors 

		#region Methods (8) 

		// Public Methods (1) 

		/// <summary>
		/// Kopieer de backup van 1 van de laptops naar een externe harde schijf
		/// </summary>
		public void CopyBackup()
		{
			// loop door elke host heen en kopieer voor iedere host de files naar alle usb drives
			foreach (var host in MannusBackupServiceConfiguration.GetConfig().Hosts)
			{
				CopyBackupToUsbDrives();
				DeleteOldBaseBackupDirectories();
			}
			//            LogWriter logwriter = new LogWriter();
			//            logwriter.WriteLogToDatabase();
		}

		private void CopyBackupToUsbDrives()
		{
			foreach (UsbDriveElement drive in MannusBackupServiceConfiguration.GetConfig().UsbDrives)
			{
				string path = MannusBackupServiceConfiguration.FindDrive(drive);
				_logger.LogDebug(path);
				if (!string.IsNullOrEmpty(path))
				{
					DirectoryInfo latestBackup = GetLatestBackup();
					_logger.LogDebug(latestBackup.FullName);
					path = Path.Combine(path, drive.BackupDirectory);
					if (!LatestBackupAtUsbDrive(path, latestBackup))
					{
						CreatePlaceForBackup(path, drive);
						CopyBackupToUsbDrive(path, latestBackup);
					}
				}
			}
		}
		// Private Methods (3) 

		private void CopyBackupToUsbDrive(string path, DirectoryInfo latestBackup)
		{
			string datePart = latestBackup.Name.Split("_".ToCharArray())[1];
			_logger.LogDebug("datePart: {0}", datePart);
			path = Path.Combine(path, datePart);
			Directory.CreateDirectory(path);
			FileInfo[] zipFiles = latestBackup.GetFiles("*.zip");
			foreach (FileInfo zipFile in zipFiles)
			{
				string fileName = Path.Combine(path, zipFile.Name);
				_logger.LogDebug("zipfile {0}", fileName);
				zipFile.CopyTo(fileName, true);
			}
		}

		private void CreatePlaceForBackup(string path, UsbDriveElement driveElement)
		{
			SortedList<DateTime, DirectoryInfo> backupDirectories = GetBackupDirectories(path);
			if (backupDirectories.Count == driveElement.NumberOfBackups)
			{
				backupDirectories.First().Value.Delete(true);
			}
		}

		[Obsolete("deze functie komt nu ook voor in de directorymanager")]
		private SortedList<DateTime, DirectoryInfo> GetBackupDirectories(string directory)
		{
			_logger.LogDebug("get backup directories from {0}", directory);
			DirectoryInfo backupDirectories = new DirectoryInfo(directory);
			SortedList<DateTime, DirectoryInfo> backupDirectoriesNames = new SortedList<DateTime, DirectoryInfo>();
			foreach (DirectoryInfo subdir in backupDirectories.GetDirectories())
			{
				string subDirName = subdir.Name;
				DateTime dateStamp = _dateTimeCreator.FromDateStamp(subDirName);
				_logger.LogDebug("datestamp: {0}, directory: {1}", dateStamp.ToString(), subdir.ToString());
				if (!dateStamp.Year.Equals(1900))
				{
					backupDirectoriesNames.Add(dateStamp, subdir);
				}
			}
			return backupDirectoriesNames;
		}
		// Internal Methods (4) 

		internal void DeleteOldBaseBackupDirectories()
		{
			string path = MannusBackupServiceConfiguration.GetConfig().BaseBackupDirectory;
			SortedList<DateTime, DirectoryInfo> backupDirectories = GetBackupDirectoriesFromBase(path);
			int count = backupDirectories.Count;
			if (count > 10)
			{
				IEnumerable<KeyValuePair<DateTime, DirectoryInfo>> oldDirectories = backupDirectories.Take(count - 10);
				foreach (KeyValuePair<DateTime, DirectoryInfo> oldDirectory in oldDirectories)
				{
					oldDirectory.Value.Delete(true);
				}
			}
		}

		internal SortedList<DateTime, DirectoryInfo> GetBackupDirectoriesFromBase(string directory)
		{
			_logger.LogDebug("get backup directories from {0}", directory);
			DirectoryInfo backupDirectories = new DirectoryInfo(directory);
            SortedList<DateTime, DirectoryInfo> backupDirectoriesNames = new SortedList<DateTime, DirectoryInfo>();
            if (!Directory.Exists(directory))
            {
                _logger.LogError("Directory bestaat niet '{0}'", directory);
                return backupDirectoriesNames;
            }
			foreach (DirectoryInfo subdir in backupDirectories.GetDirectories())
			{
				string subDirName = subdir.Name;
				if (subDirName.Contains("backup_") && !subDirName.Equals("backup_results"))
				{
					DateTime dateStamp = _dateTimeCreator.FromDateStamp(subDirName, true);
					if (subdir.GetFiles().Count() == 0)
					{
						subdir.Delete(true);
					}
					else
					{
						backupDirectoriesNames.Add(dateStamp, subdir);
					}
				}
			}
			return backupDirectoriesNames;
		}

		internal DirectoryInfo GetLatestBackup()
		{
			var xmlFileHandler = new BackupResultsXmlFileHandler();
			string directoryName = xmlFileHandler.GetLatestBackup();
            if (string.IsNullOrEmpty(directoryName))
            {
                return new DirectoryInfo(@"c:\");
            }
			return new DirectoryInfo(directoryName);
		}

		// TODO ophalen van meerdere prefixes
		internal bool LatestBackupAtUsbDrive(string path, DirectoryInfo latestBackup)
		{
			_logger.LogDebug("path with backup directories: {0}", path);
			string lastBackUpName = latestBackup.Name.Substring(7);
			_logger.LogDebug(string.Format("name last backup:{0}", lastBackUpName));
			SortedList<DateTime, DirectoryInfo> backupDirectories = GetBackupDirectories(path);
			var results = from directory in backupDirectories.Values
						  where directory.Name.ToLowerInvariant() == lastBackUpName.ToLowerInvariant()
						  select directory;
			return (results.Count() == 1);
		}

		#endregion Methods 
	}
}