using System;
using System.Collections.Generic;
using System.IO;

namespace Methods
{
    public class FileSystemVisitor
    {
        private CustomEventArgs args;

        private bool isStopped;

        public FileSystemVisitor(Func<string, bool> searchPattern, bool onlyFiltered, bool finishAfterFirstMatch)
        {
            this.searchPattern = searchPattern;
            args = new CustomEventArgs(onlyFiltered, finishAfterFirstMatch);
        }

        private readonly Func<string, bool> searchPattern;

        private readonly Queue<FileSystemInfo> pendingDirectories = new Queue<FileSystemInfo>();

        public delegate void OnProgress();

        public event OnProgress Start;

        public event OnProgress Finish;

        public event EventHandler<CustomEventArgs> FileFound;

        public event EventHandler<CustomEventArgs> DirectoryFound;

        public event EventHandler<CustomEventArgs> FilteredFileFound;

        public event EventHandler<CustomEventArgs> FilteredDirectoryFound;

        public IEnumerable<FileSystemInfo> GetDirectoryContent(string rootFolderPath)
        {
            Start();
            FileSystemInfo rootdir = new DirectoryInfo(rootFolderPath);
            pendingDirectories.Enqueue(rootdir);

            while (pendingDirectories.Count > 0)
            {
                if (isStopped) break;
                FileSystemInfo[] directoryContent;
                rootdir = pendingDirectories.Dequeue();
                var dir = new DirectoryInfo(rootdir.FullName);

                try
                {
                    directoryContent = dir.GetFiles();
                }
                catch (UnauthorizedAccessException) { continue; }
                catch (DirectoryNotFoundException) { continue; }

                foreach (var file in GetFiles(directoryContent))
                {
                    yield return file;
                }

                directoryContent = dir.GetDirectories();

                foreach (var directory in GetDirectories(directoryContent))
                {
                    yield return directory;
                }
            }
            Finish();
        }

        public IEnumerable<FileSystemInfo> GetFiles(FileSystemInfo[] directoryContent)
        {
            foreach (var file in directoryContent)
            {
                if (searchPattern(file.Name))
                {
                    FilteredFileFound(file.FullName, args);
                    if (IsIteratorBeStopped()) yield break;
                    yield return file;
                }
                else
                {
                    FileFound(file.FullName, args);
                }
            }
        }

        public IEnumerable<FileSystemInfo> GetDirectories(FileSystemInfo[] directoryContent)
        {
            foreach (var directory in directoryContent)
            {
                if (searchPattern(directory.Name))
                {
                    FilteredDirectoryFound(directory.FullName, args);
                    if (IsIteratorBeStopped()) yield break;
                    yield return directory;
                }
                else
                {
                    DirectoryFound(directory.FullName, args);
                }
                pendingDirectories.Enqueue(directory);
            }
        }

        private bool IsIteratorBeStopped()
        {
            if (args.FinishAfterFirstMatch) isStopped = true;
            return args.FinishAfterFirstMatch;
        }
    }
}
