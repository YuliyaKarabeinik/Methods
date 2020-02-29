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

                foreach (var file in GetItems(directoryContent, true))
                {
                    yield return file;
                }

                directoryContent = Directory.GetDirectories(rootFolderPath);

                foreach (var directory in GetItems(directoryContent, false))
                {
                    yield return directory;
                }
            }
            Finish();
        }

        private IEnumerable<string> GetItems(string[] directoryContent, bool isFile)
        {
            foreach (var item in directoryContent)
            {
                if (searchPattern(item))
                {
                    FilteredItemFound(isFile);
                    yield return item;
                }
                else
                {
                    ItemFound(isFile);
                }

                EnqueueDirectory(item, isFile);
            }
        }

        private void EnqueueDirectory(string item, bool isFile)
        {
            if (!isFile)
            {
                pendingDirectories.Enqueue(item);
            }
        }
    }
}
