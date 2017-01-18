using System;
using AddressProcessing.CSV.Implementations;
using AddressProcessing.CSV.Interfaces;

namespace AddressProcessing.CSV
{
    /*
        2) Refactor this class into clean, elegant, rock-solid & well performing code, without over-engineering.
           Assume this code is in production and backwards compatibility must be maintained.
        
        Changes made:
        - I have kept the contracts as they are and simplified the implementation by introducing two services; One to read CSV and the other to write CSV
        - I am using a  service builder to build IReadCsvService and IWriteCsvService when the Open method is called
        - So that I can mock out these services during test, I introduced a service builder in separate constructor
        - To prevent the Write and Read being called before the service is built, I throw an exception if the service in question is null
        - Gang of Four - Program to interfaces, not implementations
        - TODO: One Read method is redundant and should be deleted - commented out for now
    */


    public class CSVReaderWriter
    {
        [Flags]
        public enum Mode
        {
            Read = 1,
            Write = 2
        }

        private readonly ICsvReaderWriterServiceBuilder _serviceBuilder;
        private IReadCsvService _readCsvService;
        private IWriteCsvService _writeCsvService;

        public CSVReaderWriter()
        {
            _serviceBuilder = new CsvReaderWriterServiceBuilder();
        }

        public CSVReaderWriter(ICsvReaderWriterServiceBuilder builder)
        {
            _serviceBuilder = builder;
        }

        public void Open(string fileName, Mode mode)
        {
            switch (mode)
            {
                case Mode.Read:
                    _readCsvService = _serviceBuilder.BuildReadCsvService(fileName);
                    break;
                case Mode.Write:
                    _writeCsvService = _serviceBuilder.BuildWriteCsvService(fileName);
                    break;
                default:
                    throw new Exception("Unknown file mode for " + fileName);
            }
        }

        public void Write(params string[] columns)
        {
            if (_writeCsvService == null)
            {
                throw new Exception("Call Open first before calling Write");
            }

            _writeCsvService.Write(columns);
        }

        // TODO: Removed method because it was not used (Commented out for now).
        //public bool Read( string column1, string column2)
        //{
        //    return Read(out column1, out column2);
        //}

        public bool Read(out string column1, out string column2)
        {
            if (_readCsvService == null)
            {
                throw new Exception("Call Open first before calling Read");
            }

            return _readCsvService.Read(out column1, out column2);
        }

        public void Close()
        {
            _readCsvService?.Close();
            _writeCsvService?.Close();
        }
    }
}