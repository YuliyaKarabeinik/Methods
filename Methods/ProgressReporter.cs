using System;
using System.IO;

namespace Methods
{
    static class ProgressReporter
    {
        public static void StartMessage()
        {
            Console.WriteLine("Search started.");
        }

        public static void FinishMessage()
        {
            Console.WriteLine("Search finished.");
        }

        public static void ItemFoundMessage(object item, CustomEventArgs e)
        {
            if (!e.OnlyFiltered) Console.WriteLine($"{GetItemType(item)} was found {item}");
        }

        public static void FilteredItemFoundMessage(object item, CustomEventArgs e)
        {
            Console.WriteLine($"Filtered {GetItemType(item)} was found {item}");
        }

        private static string GetItemType(object item)
        {
            return ((FileSystemInfo)item).Attributes.HasFlag(FileAttributes.Directory) ? "Directory" : "File";
        }
    }
}

