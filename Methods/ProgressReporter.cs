using System;

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

        public static void FileFoundMessage(object name, CustomEventArgs e)
        {
            if (!e.OnlyFiltered) Console.WriteLine($"File was found {name}" );
        }

        public static void DirectoryFoundMessage(object name, CustomEventArgs e)
        {
            if (!e.OnlyFiltered) Console.WriteLine($"Directory was found {name}");
        }
        public static void FilteredFileFoundMessage(object name, CustomEventArgs e)
        {
            Console.WriteLine($"Filtered directory was found {name}");
            if (e.FinishAfterFirstMatch) QuitConsole();
        }

        public static void FilteredDirectoryFoundMessage(object name, CustomEventArgs e)
        {
            Console.WriteLine($"Filtered directory was found {name}");
            if (e.FinishAfterFirstMatch) QuitConsole();
        }

        private static void QuitConsole()
        {
            Console.WriteLine("Press any key to exit ...");
            Console.ReadKey();
            Environment.Exit(0);
        }
    }
}
