using System.IO;
using AddressProcessing.CSV.Interfaces;

namespace AddressProcessing.CSV.Implementations
{
    /// <summary>
    /// ReadCsvService has one responsibility - Read and simplified logic
    /// </summary>
    public class ReadCsvService : IReadCsvService
    {
        private StreamReader _readerStream;

        public ReadCsvService(string fileName)
        {
            if (!File.Exists(fileName))
            {
                throw new FileNotFoundException($"{fileName} does not exist");
            }

            Open(fileName);
        }

        private void Open(string fileName)
        {
            _readerStream = File.OpenText(fileName);
        }

        public void Close()
        {
            _readerStream?.Close();
        }

        public bool Read(out string column1, out string column2)
        {
            const int FIRST_COLUMN = 0;
            const int SECOND_COLUMN = 1;

            char[] separator = { '\t' };

            var line = _readerStream.ReadLine();

            if (line == null)
            {
                column1 = null;
                column2 = null;

                return false;
            }

            var columns = line.Split(separator);

            if (columns.Length == 0)
            {
                column1 = null;
                column2 = null;

                return false;
            }

            column1 = columns[FIRST_COLUMN];
            column2 = columns[SECOND_COLUMN];

            return true;
        }
    }
}