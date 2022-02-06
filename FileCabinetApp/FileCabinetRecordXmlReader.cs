using System.Xml.Linq;
using System.Xml.Serialization;
using System.Xml.XPath;

namespace FileCabinetApp
{
    public class FileCabinetRecordXmlReader
    {
        private StreamReader reader;

        public FileCabinetRecordXmlReader(StreamReader reader)
        {
            this.reader = reader;
        }

        public IList<FileCabinetRecord> ReadAll()
        {
            List<FileCabinetRecord> fileCabinetRecords = new List<FileCabinetRecord>();
            var xmlSerializer = new XmlSerializer(typeof(FileCabinetRecord[]));
            fileCabinetRecords.AddRange((FileCabinetRecord[])xmlSerializer.Deserialize(this.reader));
            this.reader.Dispose();
            return fileCabinetRecords;
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            this.reader?.Dispose();
        }
    }
}