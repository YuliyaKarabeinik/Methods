using System;
using System.Collections.Generic;
using System.IO;

namespace Methods
{
    class FileSystemVisitor
    {
        public FileSystemVisitor(Func<string, bool> searchPattern)
        {
            this.searchPattern = searchPattern;
        }

        private readonly Func<string, bool> searchPattern;

        private readonly Queue<string> pendingDirectories = new Queue<string>();

        public delegate void OnProgress();

        public delegate void OnFind(bool isFile);

        public event OnProgress Start;

        public event OnProgress Finish;

        public event OnFind ItemFound;

        public event OnFind FilteredItemFound;

        public IEnumerable<string> GetDirectoryContent(string rootFolderPath)
        {
            Start();

            pendingDirectories.Enqueue(rootFolderPath);

            while (pendingDirectories.Count > 0)
            {
                string[] directoryContent;
                rootFolderPath = pendingDirectories.Dequeue();

                try
                {
                    directoryContent = Directory.GetFiles(rootFolderPath);
                }
                catch (UnauthorizedAccessException) { continue; }
                catch (DirectoryNotFoundException) { continue; }

                foreach (var file in GetFiles(directoryContent))
                {
                    yield return file;
                }

                directoryContent = Directory.GetDirectories(rootFolderPath);

                foreach (var directory in GetDirectories(directoryContent))
                {
                    yield return directory;
                }
            }
            Finish();
        }
        
        private IEnumerable<string> GetFiles(string[] directoryContent)
        {
            foreach (var file in directoryContent)
            {
                if (searchPattern(file))
                {
                    FilteredItemFound(true);
                    yield return file;
                }
                else
                {
                    ItemFound(true);
                }
            }
        }

        private IEnumerable<string> GetDirectories(string[] directoryContent)
        {
            foreach (var directory in directoryContent)
            {
                if (searchPattern(directory))
                {
                    FilteredItemFound(false);
                    yield return directory;
                }
                else
                {
                    ItemFound(false);
                }
                pendingDirectories.Enqueue(directory);
            }
        }
    }
}
