namespace AddressProcessing.CSV.Interfaces
{
    /// <summary>
    /// Service Builder interface to build IReadCsvService and IWriteCsvService
    /// Both included for simplicity but could have been separated into two builders to adhere to Single Responsibility Principle
    /// I see no benefit in doing that
    /// </summary>
    public interface ICsvReaderWriterServiceBuilder
    {
        IReadCsvService BuildReadCsvService(string fileName);
        IWriteCsvService BuildWriteCsvService(string fileName);
    }
}