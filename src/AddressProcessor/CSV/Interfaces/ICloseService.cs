namespace AddressProcessing.CSV.Interfaces
{
    /// <summary>
    /// This interface contains only Close method which is common to IReadCsvService and IWriteCsvService
    /// </summary>
    public interface ICloseService
    {
        void Close();
    }
}