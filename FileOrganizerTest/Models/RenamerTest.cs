using System.Collections.Generic;
using System.IO;
using FileOrganizer.Models;
using NUnit.Framework;

namespace FileOrganizerTest.Models
{
    public class RenamerTest
    {
        private readonly string testDirectoryName = "testDirectory";

        [SetUp]
        public void Setup()
        {
            Directory.CreateDirectory(testDirectoryName);
            File.Create($"{testDirectoryName}\\a.temp").Close();
            File.Create($"{testDirectoryName}\\b.temp").Close();
            File.Create($"{testDirectoryName}\\c.temp").Close();
            File.Create($"{testDirectoryName}\\d.temp").Close();
        }

        [TearDown]
        public void TearDown()
        {
            foreach (string p in Directory.GetFiles(testDirectoryName))
            {
                File.Delete(p);
            }

            Directory.Delete(testDirectoryName);
        }

        [Test]
        public void AppendNumberTest()
        {
            var renamer = new Renamer();
            var files = new List<ExtendFileInfo>()
            {
                new ExtendFileInfo($"{testDirectoryName}\\a.temp"),
                new ExtendFileInfo($"{testDirectoryName}\\b.temp"),
                new ExtendFileInfo($"{testDirectoryName}\\c.temp"),
                new ExtendFileInfo($"{testDirectoryName}\\d.temp"),
            };

            renamer.AppendNumber(files);

            Assert.That(files[0].FileInfo.Name, Is.EqualTo("0001_a.temp"));
            Assert.That(files[1].FileInfo.Name, Is.EqualTo("0002_b.temp"));
            Assert.That(files[2].FileInfo.Name, Is.EqualTo("0003_c.temp"));
            Assert.That(files[3].FileInfo.Name, Is.EqualTo("0004_d.temp"));
        }
    }
}