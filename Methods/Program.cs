using System;
using System.Text.RegularExpressions;

namespace Methods
{
    class Program
    {
        private static string path = "D:\\Business English";
        
        private static readonly Regex searchTerm = new Regex(@"x$");
        
        private static readonly Func<string, bool> filter = fileName => searchTerm.IsMatch(fileName);

        private static readonly bool onlyFilteredFiles = false;

        private static readonly bool finishAfterFirstMatch = true;


        static void Main(string[] args)
        {
            var fileVisitor = new FileSystemVisitor(filter, onlyFilteredFiles, finishAfterFirstMatch);
            SubscribeToEvents(fileVisitor);

            var values = fileVisitor.GetContent(path);
            foreach (var value in values) { }
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
