namespace AddressProcessing.CSV.Interfaces
{
    /// <summary>
    /// WriteCsvService interface 
    /// </summary>
    public interface IWriteCsvService : ICloseService
    {
        void Write(params string[] columns);
    }
}