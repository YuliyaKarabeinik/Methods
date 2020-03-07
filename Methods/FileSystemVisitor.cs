using System;
using System.Collections.Generic;
using System.IO;

namespace Methods
{
    public class FileSystemVisitor
    {
        private readonly CustomEventArgs args;
        
        public FileSystemVisitor() : this(filename => true, false, false) { }

        public FileSystemVisitor(Func<string, bool> searchPattern, bool onlyFiltered, bool finishAfterFirstMatch)
        {
            this.searchPattern = searchPattern;
            args = new CustomEventArgs(onlyFiltered, finishAfterFirstMatch);
        }

        private readonly Func<string, bool> searchPattern;
        
        public delegate void OnProgress();

        public event OnProgress Start;

        public event OnProgress Finish;

        public event EventHandler<CustomEventArgs> ItemFound;

        public event EventHandler<CustomEventArgs> FilteredItemFound;

        public IEnumerable<FileSystemInfo> GetContent(string rootFolderPath)
        {
            Start();
            var directory = new DirectoryInfo(rootFolderPath);
            var content = directory.GetFileSystemInfos("*", SearchOption.AllDirectories);
            foreach (var item in content)
            {
                if (searchPattern(item.Name))
                {
                    FilteredItemFound(item, args);
                    if (IsIteratorBeStopped()) yield break;
                    yield return item;
                }
                else ItemFound(item, args);
            }
            Finish();
        }
        private bool IsIteratorBeStopped()
        {
            return args.FinishAfterFirstMatch;
        }
    }
}
