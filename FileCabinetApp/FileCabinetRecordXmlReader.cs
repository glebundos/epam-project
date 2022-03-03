using System.Xml.Linq;
using System.Xml.Serialization;
using System.Xml.XPath;

namespace FileCabinetApp
{
    /// <summary>
    /// Class for reading records from xml files.
    /// </summary>
    public class FileCabinetRecordXmlReader
    {
        private StreamReader reader;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetRecordXmlReader"/> class.
        /// </summary>
        /// <param name="reader"> - <see cref="StreamReader"/> to read from.</param>
        public FileCabinetRecordXmlReader(StreamReader reader)
        {
            this.reader = reader;
        }

        /// <summary>
        /// Reads all records from the stream.
        /// </summary>
        /// <returns> <see cref="IEnumerable{FileCabinetRecord}"/> of red records.</returns>
        public IList<FileCabinetRecord> ReadAll()
        {
            List<FileCabinetRecord> fileCabinetRecords = new List<FileCabinetRecord>();
            var xmlSerializer = new XmlSerializer(typeof(FileCabinetRecord[]));
#pragma warning disable CS8600 // Преобразование литерала, допускающего значение NULL или возможного значения NULL в тип, не допускающий значение NULL.
            fileCabinetRecords.AddRange((FileCabinetRecord[])xmlSerializer.Deserialize(this.reader) ?? new FileCabinetRecord[0]);
#pragma warning restore CS8600 // Преобразование литерала, допускающего значение NULL или возможного значения NULL в тип, не допускающий значение NULL.
            this.reader.Dispose();
            return fileCabinetRecords;
        }

        /// <summary>
        /// Dispose method for reader.
        /// </summary>
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