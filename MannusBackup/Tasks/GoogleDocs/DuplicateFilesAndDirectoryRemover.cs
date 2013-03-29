namespace MannusBackup.Tasks.GoogleDocs
{
    /*
    internal class DuplicateFilesAndDirectoryRemover
    {
        private string downloadDirectory;

        internal void RemoveDuplicates(string downloadDirectory)
        {
            this.downloadDirectory = downloadDirectory;
            RemoveDuplicateFiles(downloadDirectory);
            RemoveDuplicateDirectories(downloadDirectory);
        }

        private void RemoveDuplicateDirectories(string path)
        {
            foreach (string directoryName in Directory.GetDirectories(path))
            {
                DirectoryInfo directory = new DirectoryInfo(directoryName);
                int occurences = 0;
                FindMoreOccurencesOfThisDirectory(directory.Name, path, ref occurences);
                if (occurences > 1)
                {
                    RemoveDirectory(directoryName);
                }
            }
        }

        private void RemoveDuplicateFiles(string path)
        {
            foreach (string filename in Directory.GetFiles(path))
            {
                FileInfo file = new FileInfo(filename);
                if (FindMoreOccurencesOfThisFile(file.Name, path))
                {
                    File.Delete(filename);
                }
            }
        }

        private bool FindMoreOccurencesOfThisFile(string filename, string path)
        {
            foreach (string directoryName in Directory.GetDirectories(path))
            {
                foreach (string fileName in Directory.GetFiles(directoryName))
                {
                    FileInfo file = new FileInfo(fileName);
                    if (file.Name.Equals(filename))
                    {
                        return true;
                    }
                }
                foreach (string subDirectoryName in Directory.GetDirectories(directoryName))
                {
                    FindMoreOccurencesOfThisFile(filename, subDirectoryName);
                }
            }
            return false;
        }

        private void FindMoreOccurencesOfThisDirectory(string directoryName, string path, ref int occurences)
        {
            if (Directory.Exists(path))
            {
                foreach (string subdirectory in Directory.GetDirectories(path))
                {
                    DirectoryInfo subDir = new DirectoryInfo(subdirectory);
                    if (subDir.Name.Equals(directoryName))
                    {
                        occurences++;
                    }
                    else
                    {
                        FindMoreOccurencesOfThisDirectory(directoryName, subdirectory, ref occurences);
                    }
                }
            }
        }

        private void RemoveDirectory(string directoryPath)
        {
            int index = directoryPath.LastIndexOf(@"\") + 1;
            string directoryName = string.Format(@"{0}\{1}", downloadDirectory, directoryPath.Substring(index));
            DirectoryInfo directory = new DirectoryInfo(directoryName);
            if (Directory.Exists(directoryName))
            {
                foreach (FileInfo file in directory.GetFiles())
                {
                    file.Delete();
                }
                Directory.Delete(directoryName, true);
            }
        }
    }
     */
}