using System.IO;
using System.Reflection;
using AddressProcessing.CSV.Implementations;
using AddressProcessing.CSV.Interfaces;
using NUnit.Framework;

namespace AddressProcessing.Tests.CSV
{
    [TestFixture]
    public class ReadCsvServiceTests
    {
        [SetUp]
        public void SetUp()
        {
            _testInputFilePath = Path.Combine(_testDirectory, TestInputFile);
            _nonExistentTestFilePath = Path.Combine(_testDirectory, NonExistentTestInputFile);
        }

        [TearDown]
        public void TearDown()
        {
            _service?.Close();
        }

        private IReadCsvService _service;
        private const string TestInputFile = @"test_data\contacts.csv";
        private const string NonExistentTestInputFile = @"test_data\notpresent.csv";
        private readonly string _testDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        private string _testInputFilePath;
        private string _nonExistentTestFilePath;

        [Test]
        public void ServiceInstantiationShouldFailIfFileDoesNotExist()
        {
            Assert.Throws<FileNotFoundException>(() => _service = new ReadCsvService(_nonExistentTestFilePath));
        }

        [Test]
        public void ServiceInstantiationShouldSucceedIfFileExist()
        {
            Assert.That(() => _service = new ReadCsvService(_testInputFilePath), Throws.Nothing);
        }


        [Test]
        public void ServiceShouldSucceedInReadingTheWholeFile()
        {
            _service = new ReadCsvService(_testInputFilePath);

            string column1, column2;

            var counter = 0;

            while (_service.Read(out column1, out column2))
            {
                counter++;
            }

            Assert.AreEqual(229, counter);
        }
    }
}