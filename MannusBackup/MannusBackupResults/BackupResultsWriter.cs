using System.Linq;
using ESRINederland.Framework.Logging;
using MannusBackup.BackupResults;

namespace MannusBackup.BackupResults
{
    // TODO: refactoren dat het een repository gaat worden
    internal class BackupResultsWriter
    {
        private ILogger _logger;
        private BackupResultsDatabaseHandler _database;
        private BackupResultsXmlFileHandler _xmlFile;

        public BackupResultsWriter()
        {
            _logger = Logger.GetLogger();
            _database = new BackupResultsDatabaseHandler();
            _xmlFile = new BackupResultsXmlFileHandler();
        }

        internal void WriteLogToDatabase()
        {
            var archivedRecords = _database.GetResults();
            var allRecords = _xmlFile.GetResults();
            if (archivedRecords.Count() == 0)
            {
                _database.SaveNewResults(allRecords);
                return;
            }
            BackupResultEqualityComparer comparer = new BackupResultEqualityComparer();
            var result = allRecords.Except(archivedRecords, comparer);
            _database.SaveNewResults(result);
        }
    }
}