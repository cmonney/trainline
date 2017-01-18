namespace AddressProcessing.CSV.Interfaces
{
    public interface ICsvReaderWriterServiceBuilder
    {
        IReadCsvService BuildReadCsvService(string fileName);
        IWriteCsvService BuildWriteCsvService(string fileName);
    }
}