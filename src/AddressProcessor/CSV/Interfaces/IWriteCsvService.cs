namespace AddressProcessing.CSV.Interfaces
{
    public interface IWriteCsvService : ICloseService
    {
        void Write(params string[] columns);
    }
}