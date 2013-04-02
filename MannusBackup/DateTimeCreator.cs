using System;
using System.Globalization;
using ESRINederland.Framework.Logging;
using MannusBackup.Entities;
using MannusBackup.Storage;

namespace MannusBackup
{
    internal class DateTimeCreator
    {
        private ILogger _logger;
        private StorageLocation _storageLocation;

        public DateTimeCreator(StorageLocation location = StorageLocation.Usb)
        {
            _logger = Logger.GetLogger();
            _storageLocation = location;
        }

        internal DateTime FromDateStamp(string subDirName, bool directoryFromBase = false)
        {
            _logger.LogDebug("getDateStamp: {0}", subDirName);
            DateDefinition parsedDate = ParseDirectoryNameToDateDefinition(subDirName, directoryFromBase);
            string dateString = string.Format("{0} {1} {2}", parsedDate.Day, parsedDate.Month, parsedDate.Year);
            string format = "dd MMMM yyyy";
            try
            {
                DateTime dateTime = DateTime.ParseExact(dateString, format, CultureInfo.InvariantCulture);
                return dateTime;
            }
            catch (FormatException e)
            {
                _logger.LogException(e);
                return new DateTime(1900, 1, 1);
            }
        }

        private DateDefinition ParseDirectoryNameToDateDefinition(string directoryName, bool fromBaseDirectory)
        {
            DateDefinition result = new DateDefinition();
            if (fromBaseDirectory)
            {
                result = ParseDirectoryNameToDateDefinitionWithBaseDirectory(directoryName);
            }
            else
            {
                result = ParseDirectoryNameToDateDefinitionWithoutBaseDirectory(directoryName);
            }
            return result;
        }

        private DateDefinition ParseDirectoryNameToDateDefinitionWithBaseDirectory(string directoryName)
        {
            DateDefinition result = new DateDefinition();
            if (directoryName.Length < 16)
            {
                return result;
            }
            result.Day = directoryName.Substring(7, 2);
            result.Month = directoryName.Substring(9, directoryName.Length - 13);
            result.Year = directoryName.Substring(directoryName.Length - 4);
            return result;
        }

        private DateDefinition ParseDirectoryNameToDateDefinitionWithoutBaseDirectory(string directoryName)
        {
            switch (_storageLocation)
            {
                case StorageLocation.Usb:
                    return ParseDirectoryNameToDateDefinitionWithoutBaseDirectoryForUsb(directoryName);
            case StorageLocation.Local:
                    return ParseDirectoryNameToDateDefinitionWithoutBaseDirectoryForLocal(directoryName);
            }
            return new DateDefinition() { Day = "1", Month = "1", Year = "1900" };   
        }

        private DateDefinition ParseDirectoryNameToDateDefinitionWithoutBaseDirectoryForUsb(string directoryName)
        {
                    DateDefinition result = new DateDefinition();
                    if (directoryName.Length < 9)
                    {
                        return result;
                    }
                    result.Day = directoryName.Substring(0, 2);
                    result.Month = directoryName.Substring(2, directoryName.Length - 6);
                    result.Year = directoryName.Substring(directoryName.Length - 4);
                    return result;
        }

        private DateDefinition ParseDirectoryNameToDateDefinitionWithoutBaseDirectoryForLocal(string directoryName)
        {
            // TODO 02042013 fixen
            DateDefinition result = new DateDefinition();
            if (directoryName.Length < 9)
            {
                return result;
            }
            result.Day = directoryName.Substring(0, 2);
            result.Month = directoryName.Substring(2, directoryName.Length - 6);
            result.Year = directoryName.Substring(directoryName.Length - 4);
            return result;
        }

    }
}
