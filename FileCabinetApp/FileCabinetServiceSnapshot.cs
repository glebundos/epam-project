namespace FileCabinetApp
{
    /// <summary>
    /// Represents the snapshot of records.
    /// </summary>
    public class FileCabinetServiceSnapshot
    {
        private FileCabinetRecord[] records;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetServiceSnapshot"/> class.
        /// </summary>
        /// <param name="list">List of records to make a snapshot.</param>
        public FileCabinetServiceSnapshot(List<FileCabinetRecord> list)
        {
            this.records = list.ToArray();
            this.Records = list;
        }

        /// <summary>
        /// Gets list of <see cref="FileCabinetRecord"/>.
        /// </summary>
        /// <value> Records list.</value>
        public List<FileCabinetRecord> Records { get; }

        /// <summary>
        /// Writes the snapshot into .csv file.
        /// </summary>
        /// <param name="streamWriter">Stream to write to.</param>
        /// <param name="append">Append text if true, rewrite if false.</param>
        public void SaveToCsv(StreamWriter streamWriter, bool append)
        {
            FileCabinetRecordCsvWriter writer = new FileCabinetRecordCsvWriter(streamWriter);
            writer.Write(this.records, append);
        }

        /// <summary>
        /// Reads the records from .csv file into the <see cref="Records"/>.
        /// </summary>
        /// <param name="streamReader">Stream to read from.</param>
        /// <returns>Count of red records.</returns>
        public int LoadFromCsv(StreamReader streamReader)
        {
            FileCabinetRecordCsvReader reader = new FileCabinetRecordCsvReader(streamReader);
            var records = reader.ReadAll();
            this.Records.AddRange(records);
            return records.Count();
        }

        /// <summary>
        /// Reads the records from .xml file into the <see cref="Records"/>.
        /// </summary>
        /// <param name="streamReader">Stream to read from.</param>
        /// <returns>Count of red records.</returns>
        public int LoadFromXml(StreamReader streamReader)
        {
            FileCabinetRecordXmlReader reader = new FileCabinetRecordXmlReader(streamReader);
            var records = reader.ReadAll();
            this.Records.AddRange(records);
            return records.Count;
        }

        /// <summary>
        /// Writes the snapshot into .xml file.
        /// </summary>
        /// <param name="streamWriter">Stream with file path.</param>
        /// <param name="append">Append text if true, rewrite if false.</param>
        public void SaveToXml(StreamWriter streamWriter, bool append)
        {
            FileCabinetRecordXmlWriter writer = new FileCabinetRecordXmlWriter(streamWriter);
            writer.Write(this.records, append);
        }
    }
}
