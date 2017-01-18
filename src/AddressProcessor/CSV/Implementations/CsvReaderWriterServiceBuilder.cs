using AddressProcessing.CSV.Interfaces;

namespace AddressProcessing.CSV.Implementations
{
    /// <summary>
    /// Implementation of ICsvReaderWriterServiceBuilder to build services
    /// </summary>
    public class CsvReaderWriterServiceBuilder : ICsvReaderWriterServiceBuilder
    {
        public  IReadCsvService BuildReadCsvService(string fileName)
        {
            return new ReadCsvService(fileName);
        }

        public  IWriteCsvService BuildWriteCsvService(string fileName)
        {
            return new WriteCsvService(fileName);
        }
    }
}