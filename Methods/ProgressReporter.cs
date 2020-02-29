using System;
using System.Collections.Generic;
using System.Text;

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
        public static void ItemFoundMessage(bool isFile)
        {
            var message = isFile ? "File found." : "Directory found.";
            Console.WriteLine(message);
        }

        public static void FilteredItemFoundMessage(bool isFile)
        {
            var message = isFile ? "Filtered file found." : "Filtered directory found.";
            Console.WriteLine(message);
        }
    }
}
