using System.IO;
using AddressProcessing.CSV.Interfaces;

namespace AddressProcessing.CSV.Implementations
{
    public class WriteCsvService : IWriteCsvService
    {
        private StreamWriter _writerStream = null;

        public WriteCsvService(string fileName)
        {
            Open(fileName);
        }

        private void Open(string fileName)
        {
            var fileInfo = new FileInfo(fileName);
            _writerStream = fileInfo.CreateText();
        }

        public void Close()
        {
            _writerStream?.Close();
        }

        public void Write(params string[] columns)
        {
            var outPut = "";

            for (int i = 0; i < columns.Length; i++)
            {
                outPut += columns[i];
                if ((columns.Length - 1) != i)
                {
                    outPut += "\t";
                }
            }

            _writerStream.WriteLine(outPut);
        }
    }
}