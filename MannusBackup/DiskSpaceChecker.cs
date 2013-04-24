using System.IO;

namespace MannusBackup
{
    /// <summary>
    /// Controleer de harde schijf
    /// </summary>
    public class DiskSpaceChecker
    {
        private long _minimumSpace;
        private string _rootDirectory;

        public DiskSpaceChecker(string rootDirectory, long minimumSpaceRequiredInGigabytes)
        {
            _rootDirectory = rootDirectory;
            _minimumSpace = minimumSpaceRequiredInGigabytes;
        }

        /// <summary>
        /// Controleer grootte van harde schijf voor een bepaalde grootte
        /// </summary>
        public bool CheckAvailableDiskspace()
        {
            if (!Directory.Exists(_rootDirectory))
            {
                return false;
            }
            string root = Directory.GetDirectoryRoot(_rootDirectory);
            DriveInfo drive = new DriveInfo(root);
            long availableSpace = 0;
            if (drive.IsReady)
            {
                availableSpace = drive.AvailableFreeSpace;
            }
            _minimumSpace = _minimumSpace * 1000000000;
            return _minimumSpace < availableSpace;
        }
    }
}