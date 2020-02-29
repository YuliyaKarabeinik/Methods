using System;
using System.Text.RegularExpressions;

namespace Methods
{
    class Program
    {
        private static string path = "D:\\Business English";
        
        private static Regex searchTerm = new Regex(@"x$");
        
        private static Func<string, bool> filter = fileName =>
        {
            var matches = searchTerm.Matches(fileName);
            return matches.Count > 0;
        };

        static void Main(string[] args)
        {
            var fileVisitor = new FileSystemVisitor(filter);
            SubscribeToEvents(fileVisitor);
            var values = fileVisitor.GetDirectoryContent(path);

            foreach (var value in values)
            {
               Console.WriteLine(value);
            }

            Console.ReadKey();
        }

        private static void SubscribeToEvents(FileSystemVisitor fileVisitor)
        {
            fileVisitor.Start += ProgressReporter.StartMessage;
            fileVisitor.Finish += ProgressReporter.FinishMessage;
            fileVisitor.ItemFound += ProgressReporter.ItemFoundMessage;
            fileVisitor.FilteredItemFound += ProgressReporter.FilteredItemFoundMessage;
        }
    }
}
