using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using FluentAssertions;
using Methods;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        private FileSystemVisitor fileVisitor;

        private static readonly Regex searchTerm = new Regex(@"^test$");

        private static readonly Func<string, bool> filter = fileName => searchTerm.IsMatch(fileName);

        [TestInitialize]
        public void SetUp()
        {
            fileVisitor = new FileSystemVisitor(filter, true, false);
        }

        [TestMethod]
        public void TestMethod1()
        { 
            var mockFileSystemInfo = new Mock<FileSystemInfo>();
            mockFileSystemInfo.SetupSequence(x => x.Name)
                .Returns("test")
                .Returns("test")
                .Returns("nottest");

            fileVisitor.FilteredItemFound += (name, e) => Console.WriteLine();
            fileVisitor.ItemFound += (name, e) => Console.WriteLine();

            var results = fileVisitor.GetItems(new FileSystemInfo[] { mockFileSystemInfo.Object, mockFileSystemInfo.Object, mockFileSystemInfo.Object });

            results.Count().Should().Be(2);
        }
    }
}
