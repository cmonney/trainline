using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using AddressProcessing.CSV;
using NUnit.Framework;

namespace Csv.Tests
{
    [TestFixture]
    public class CSVReaderWriterTests
    {
        private CSVReaderWriter _csvReaderWriter;

        private const string TestOutputFile = @"test_data\output.csv";
        private const string TestInputFile = @"test_data\contacts.csv";
        private const string NonExistentTestInputFile = @"test_data\notpresent.csv";
        private readonly string _testDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        private string _testInputFilePath;
        private string _testOutputFilePath;
        private string _nonExistentTestFilePath;

        [SetUp]
        public void SetUp()
        {
            _testInputFilePath = Path.Combine(_testDirectory, TestInputFile);
            _testOutputFilePath = Path.Combine(_testDirectory, TestOutputFile);
            _nonExistentTestFilePath = Path.Combine(_testDirectory, NonExistentTestInputFile);

            _csvReaderWriter = new CSVReaderWriter();
        }

        [TearDown]
        public void TearDown()
        {
            _csvReaderWriter.Close();

            if (File.Exists(_testOutputFilePath))
            {
                File.Delete(_testOutputFilePath);
            }
        }

        [Test]
        public void OpenInReadModeShouldSucceedIfFileExist()
        {
            Assert.That(() => _csvReaderWriter.Open(_testInputFilePath, CSVReaderWriter.Mode.Read), Throws.Nothing);
        }

        [Test]
        public void OpenInReadModeShouldFailIfFileDoesNotExist()
        {
            Assert.Throws<FileNotFoundException>(() => _csvReaderWriter.Open(_nonExistentTestFilePath, CSVReaderWriter.Mode.Read));
        }

        [Test]
        public void OpenInWriteModeShouldSucceed()
        {
            Assert.That(() => _csvReaderWriter.Open(_testOutputFilePath, CSVReaderWriter.Mode.Write), Throws.Nothing);
        }

        [Test]
        public void OpenInNullModeShouldThrowAnException()
        {
            Assert.Throws<Exception>(() => _csvReaderWriter.Open(_testInputFilePath, (CSVReaderWriter.Mode) 3));
        }

        [Test]
        public void WriteShouldSucceedWithAnEmptyColumnsInput()
        {
            var columns = new string[0];
            _csvReaderWriter.Open(_testOutputFilePath, CSVReaderWriter.Mode.Write);
            Assert.That(() => _csvReaderWriter.Write(columns), Throws.Nothing);
        }

        [Test]
        public void WriteShouldSucceedWithNonEmptyColumnsInput()
        {
            var columns = new []{"column1", "column2", "column3"};
            _csvReaderWriter.Open(_testOutputFilePath, CSVReaderWriter.Mode.Write);
            Assert.That(() => _csvReaderWriter.Write(columns), Throws.Nothing);
        }

        [Test]
        public void ReadShouldSucceedInReadingTheWholeFile()
        {
            _csvReaderWriter.Open(_testInputFilePath, CSVReaderWriter.Mode.Read);

            string column1, column2;

            var counter = 0;

            while (_csvReaderWriter.Read(out column1, out column2))
            {
                counter++;
            }

            Assert.AreEqual(229, counter);
        }
    }
}
