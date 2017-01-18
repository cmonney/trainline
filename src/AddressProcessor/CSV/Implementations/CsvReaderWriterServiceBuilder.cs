using AddressProcessing.CSV.Interfaces;

namespace AddressProcessing.CSV.Implementations
{
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