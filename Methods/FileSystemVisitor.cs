using System;
using System.Collections.Generic;
using System.IO;

namespace Methods
{
    class FileSystemVisitor
    {
        private CustomEventArgs args;

        public FileSystemVisitor(Func<string, bool> searchPattern, bool onlyFiltered, bool finishAfterFirstMatch)
        {
            this.searchPattern = searchPattern;
            args = new CustomEventArgs(onlyFiltered, finishAfterFirstMatch);
        }

        private readonly Func<string, bool> searchPattern;

        private readonly Queue<string> pendingDirectories = new Queue<string>();

        public delegate void OnProgress();

        public event OnProgress Start;

        public event OnProgress Finish;

        public event EventHandler<CustomEventArgs> FileFound;

        public event EventHandler<CustomEventArgs> DirectoryFound;

        public event EventHandler<CustomEventArgs> FilteredFileFound;

        public event EventHandler<CustomEventArgs> FilteredDirectoryFound;

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
                    FilteredFileFound(file, args);
                    yield return file;
                }
                else
                { 
                    FileFound(file, args);
                }
            }
        }

        private IEnumerable<string> GetDirectories(string[] directoryContent)
        {
            foreach (var directory in directoryContent)
            {
                if (searchPattern(directory))
                {
                    FilteredDirectoryFound(directory, args);
                    yield return directory;
                }
                else
                {
                    DirectoryFound(directory, args);

                }
                pendingDirectories.Enqueue(directory);
            }
        }
    }
}
