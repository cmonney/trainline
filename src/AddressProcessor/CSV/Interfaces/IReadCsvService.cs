namespace AddressProcessing.CSV.Interfaces
{
    public interface IReadCsvService : ICloseService
    {
        bool Read(out string column1, out string column2);
    }
}