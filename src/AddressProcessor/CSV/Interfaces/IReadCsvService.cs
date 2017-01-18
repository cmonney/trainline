namespace AddressProcessing.CSV.Interfaces
{
    /// <summary>
    /// ReadCsvService interface
    /// </summary>
    public interface IReadCsvService : ICloseService
    {
        bool Read(out string column1, out string column2);
    }
}