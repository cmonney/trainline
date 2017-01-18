using System.IO;
using System.Reflection;
using AddressProcessing.CSV.Implementations;
using AddressProcessing.CSV.Interfaces;
using NUnit.Framework;

namespace AddressProcessing.Tests.CSV
{
    [TestFixture]
    public class WriteCsvServiceTests
    {
        [SetUp]
        public void SetUp()
        {
            _testOutputFilePath = Path.Combine(_testDirectory, TestOutputFile);
            _service = new WriteCsvService(_testOutputFilePath);
        }

        [TearDown]
        public void TearDown()
        {
            _service.Close();

            if (File.Exists(_testOutputFilePath))
            {
                File.Delete(_testOutputFilePath);
            }
        }

        private IWriteCsvService _service;
        private const string TestOutputFile = @"test_data\output.csv";
        private readonly string _testDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        private string _testOutputFilePath;

        [Test]
        public void ServiceShouldSucceedWithAnEmptyColumnsInput()
        {
            var columns = new string[0];
            Assert.That(() => _service.Write(columns), Throws.Nothing);
        }

        [Test]
        public void ServiceShouldSucceedWithNonEmptyColumnsInput()
        {
            var columns = new[] {"column1", "column2", "column3"};
            Assert.That(() => _service.Write(columns), Throws.Nothing);
        }
    }
}