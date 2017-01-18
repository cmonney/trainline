using System.IO;
using System.Reflection;
using AddressProcessing.Address;
using AddressProcessing.Address.v1;
using NUnit.Framework;

namespace AddressProcessing.Tests
{
    [TestFixture]
    public class AddressFileProcessorTests
    {
        private FakeMailShotService _fakeMailShotService;
        private const string TestInputFile = @"test_data\contacts.csv";
        private readonly string _testDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        private string _testInputFilePath;

        [SetUp]
        public void SetUp()
        {
            _testInputFilePath = Path.Combine(_testDirectory, TestInputFile);
            _fakeMailShotService = new FakeMailShotService();
       
        }

        [Test]
        public void Should_send_mail_using_mailshot_service()
        {
            var processor = new AddressFileProcessor(_fakeMailShotService);
            processor.Process(_testInputFilePath);

            Assert.That(_fakeMailShotService.Counter, Is.EqualTo(229));
        }

        internal class FakeMailShotService : IMailShot
        {
            internal int Counter { get; private set; }

            public void SendMailShot(string name, string address)
            {
                Counter++;
            }
        }
    }
}