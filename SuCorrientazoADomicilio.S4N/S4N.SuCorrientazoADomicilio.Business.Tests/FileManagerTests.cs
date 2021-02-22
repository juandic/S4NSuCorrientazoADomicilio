using System.IO;
using System.Text;
using NUnit.Framework;
using S4N.SuCorrientazoADomicilio.Business.Services;

namespace S4N.SuCorrientazoADomicilio.Business.Tests
{
    public class FileManagerTests
    {
        private FileManager _fileManager;
        [SetUp]
        public void Setup()
        {
            _fileManager = new FileManager();
        }

        [Test]
        public void ReadFile_ValidFileName_StringList()
        {
            //Arrange
            var fileName = "TestFile.txt";

            //Act
            var result = _fileManager.ReadFile(fileName, Directory.GetCurrentDirectory());

            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.GreaterThan(0));
            var firstValue = result[0];
            Assert.That(firstValue, Is.EqualTo("AAAAIAA"));
        }

        [Test]
        public void ReadFile_NotExistingFileName_EmptyStringList()
        {
            //Arrange
            var fileName = "OtherFile.txt";

            //Act
            var result = _fileManager.ReadFile(fileName, Directory.GetCurrentDirectory());

            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(0));
        }

        [Test]
        public void WriteFile_ValidFileName_StringList()
        {
            //Arrange
            var fileName = "OtherFile.txt";
            var fileText = new StringBuilder("TestFile.txt");

            //Act
            _fileManager.WriteFile(fileName, fileText);

        }
    }
}