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
        public FileCabinetServiceSnapshot(IReadOnlyCollection<FileCabinetRecord> list)
        {
            this.records = list.ToArray();
        }

        /// <summary>
        /// Writes the snapshot into .csv file.
        /// </summary>
        /// <param name="streamWriter">Stream with file path.</param>
        /// <param name="append">Append text if true, rewrite if false.</param>
        public void SaveToCsv(StreamWriter streamWriter, bool append)
        {
            FileCabinetRecordCsvWriter writer = new FileCabinetRecordCsvWriter(streamWriter);
            writer.Write(this.records, append);
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
