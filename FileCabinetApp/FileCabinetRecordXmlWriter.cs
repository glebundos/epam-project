using System.Xml.Linq;
using System.Xml.Serialization;

namespace FileCabinetApp
{
    /// <summary>
    /// Writing <see cref="FileCabinetRecord"/> to the .xml file.
    /// </summary>
    public class FileCabinetRecordXmlWriter
    {
        private TextWriter writer;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetRecordXmlWriter"/> class.
        /// </summary>
        /// <param name="writer">Contains <see cref="TextWriter"/> to use.</param>
        public FileCabinetRecordXmlWriter(TextWriter writer)
        {
            this.writer = writer;
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="FileCabinetRecordXmlWriter"/> class.
        /// </summary>
        ~FileCabinetRecordXmlWriter() => this.Dispose(false);

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Write a collection of <see cref="FileCabinetRecord"/> sequences to XML file and save it.
        /// </summary>
        /// <param name="records">Collection of actual <see cref="FileCabinetRecord"/> data.</param>
        /// <param name="append">Append text if true, rewrite if false.</param>
        public void Write(IReadOnlyCollection<FileCabinetRecord> records, bool append)
        {
            var xmlSerializer = new XmlSerializer(typeof(FileCabinetRecord[]));
            xmlSerializer.Serialize(this.writer, records);
            this.writer.Dispose();
        }

        private void Dispose(bool disposing)
        {
            this.writer?.Dispose();
        }
    }
}
