using System;
using System.IO;
using System.Reflection;
using AddressProcessing.CSV;
using AddressProcessing.CSV.Interfaces;
using Moq;
using NUnit.Framework;

namespace AddressProcessing.Tests.CSV
{
    [TestFixture]
    public class CSVReaderWriterTestsWithMockServices
    {
        [SetUp]
        public void SetUp()
        {
            _testInputFilePath = Path.Combine(_testDirectory, TestInputFile);
            _testOutputFilePath = Path.Combine(_testDirectory, TestOutputFile);
            _nonExistentTestFilePath = Path.Combine(_testDirectory, NonExistentTestInputFile);

            _readCsvServiceMock = new Mock<IReadCsvService>();
            _writeCsvServiceMock = new Mock<IWriteCsvService>();

            _csvReaderWriter = new CSVReaderWriter(new MockServiceBuilder(_readCsvServiceMock, _writeCsvServiceMock));
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

        private Mock<IReadCsvService> _readCsvServiceMock;
        private Mock<IWriteCsvService> _writeCsvServiceMock;

        private CSVReaderWriter _csvReaderWriter;

        private const string TestOutputFile = @"test_data\output.csv";
        private const string TestInputFile = @"test_data\contacts.csv";
        private const string NonExistentTestInputFile = @"test_data\notpresent.csv";
        private readonly string _testDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        private string _testInputFilePath;
        private string _testOutputFilePath;
        private string _nonExistentTestFilePath;

        internal class MockServiceBuilder : ICsvReaderWriterServiceBuilder
        {
            private readonly Mock<IReadCsvService> _readCsvServiceMock;
            private readonly Mock<IWriteCsvService> _writeCsvServiceMock;

            public MockServiceBuilder(Mock<IReadCsvService> readCsvServiceMock,
                Mock<IWriteCsvService> writeCsvServiceMock)
            {
                _readCsvServiceMock = readCsvServiceMock;
                _writeCsvServiceMock = writeCsvServiceMock;
            }

            public IReadCsvService BuildReadCsvService(string fileName)
            {
                if (fileName.EndsWith("notpresent.csv"))
                {
                    throw new FileNotFoundException("file not found");
                }

                return _readCsvServiceMock.Object;
            }

            public IWriteCsvService BuildWriteCsvService(string fileName)
            {
                return _writeCsvServiceMock.Object;
            }
        }

        [Test]
        public void OpenInNullModeShouldThrowAnException()
        {
            Assert.Throws<Exception>(() => _csvReaderWriter.Open(_testInputFilePath, (CSVReaderWriter.Mode) 3));
        }

        [Test]
        public void OpenInReadModeShouldFailIfFileDoesNotExist()
        {
            Assert.Throws<FileNotFoundException>(
                () => _csvReaderWriter.Open(_nonExistentTestFilePath, CSVReaderWriter.Mode.Read));
        }

        [Test]
        public void OpenInReadModeShouldSucceedIfFileExist()
        {
            Assert.That(() => _csvReaderWriter.Open(_testInputFilePath, CSVReaderWriter.Mode.Read), Throws.Nothing);
        }

        [Test]
        public void OpenInWriteModeShouldSucceed()
        {
            Assert.That(() => _csvReaderWriter.Open(_testOutputFilePath, CSVReaderWriter.Mode.Write), Throws.Nothing);
        }

        [Test]
        public void ReadShouldSucceedInReadingTheWholeFile()
        {
            _csvReaderWriter.Open(_testInputFilePath, CSVReaderWriter.Mode.Read);

            string column1, column2;

            var counter = 0;
            var callCounter = 0;

            _readCsvServiceMock.Setup(x => x.Read(out column1, out column2))
                .Returns(() => callCounter++ < 20)
                .Verifiable();

            while (_csvReaderWriter.Read(out column1, out column2))
            {
                counter++;
            }

            Assert.AreEqual(20, counter);
            _readCsvServiceMock.VerifyAll();
        }

        [Test]
        public void WriteShouldSucceedWithAnEmptyColumnsInput()
        {
            var columns = new string[0];

            _writeCsvServiceMock.Setup(x => x.Write(columns)).Verifiable();

            _csvReaderWriter.Open(_testOutputFilePath, CSVReaderWriter.Mode.Write);
            Assert.That(() => _csvReaderWriter.Write(columns), Throws.Nothing);
            _writeCsvServiceMock.Verify();
        }

        [Test]
        public void WriteShouldSucceedWithNonEmptyColumnsInput()
        {
            var columns = new[] {"column1", "column2", "column3"};
            _writeCsvServiceMock.Setup(x => x.Write(columns)).Verifiable();
            _csvReaderWriter.Open(_testOutputFilePath, CSVReaderWriter.Mode.Write);
            Assert.That(() => _csvReaderWriter.Write(columns), Throws.Nothing);
            _writeCsvServiceMock.Verify();
        }
    }
}