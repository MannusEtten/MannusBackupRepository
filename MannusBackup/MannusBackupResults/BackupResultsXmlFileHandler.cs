using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using ESRINederland.Framework.Logging;
using MannusBackup.Configuration;
using System.Globalization;
using System;

namespace MannusBackup.BackupResults
{
    internal class BackupResultsXmlFileHandler
    {
        private ILogger _logger;
        private string _filename;

        public BackupResultsXmlFileHandler()
        {
            _logger = Logger.GetLogger();
            string baseDirectory = MannusBackupConfiguration.GetConfig().BaseBackupDirectory;
            _filename = GetBackupResultsXmlFileName(baseDirectory);
        }

        public IEnumerable<BackupResult> GetResults()
        {
            if (!File.Exists(_filename))
            {
                return new List<BackupResult>();
            }
            XElement xmlDocument = XElement.Load(_filename);
            var items = from resultaat in xmlDocument.Elements("resultaat")
                        select BackupResult.FromXml(resultaat);
            return items;
        }

        internal string GetLatestBackup()
        {
            string baseDirectory = MannusBackupServiceConfiguration.GetConfig().BaseBackupDirectory;
            var filename = GetBackupResultsXmlFileName(baseDirectory);
            if (!File.Exists(filename))
            {
                _logger.LogError("backup results file niet gevonden in '{0}'", filename);
                return null;
            }
            XElement xmlDocument = XElement.Load(filename);
            var directory = xmlDocument.Elements("resultaat").Where(e => e.Element("status").Value == "afgerond").Last();
            return directory.Element("naam").Value;
        }

        private string GetBackupResultsXmlFileName(string baseDirectory)
        {
            _logger.LogDebug(baseDirectory);
            string path = string.Format(@"{0}\backup_results", baseDirectory);
            var filename = string.Format(@"{0}\backupresults.xml", path);
            _logger.LogDebug("naam xml-file", filename);
            return filename;
        }

        internal void SaveResult(BackupResult result)
        {
            if (!File.Exists(_filename))
            {
                XElement newDocument = new XElement("resultaten");
                try
                {
                    newDocument.Save(_filename);
                }
                catch (IOException e)
                {
                    _logger.LogException(e);
                    _logger.LogError("xml file met statistieken niet opgeslagen");
                    return;
                }
            }
            XElement xmlDocument = XElement.Load(_filename);
            var element = BackupResult.ToXml(result);
            xmlDocument.Add(element);
            xmlDocument.Save(_filename);
            _logger.LogDebug("xml file opgeslagen");
        }
    }
}