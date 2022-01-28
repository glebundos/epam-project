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
        public void SaveToCsv(StreamWriter streamWriter)
        {
            FileCabinetRecordCsvWriter writer = new FileCabinetRecordCsvWriter(streamWriter);
            writer.Write(this.records);
        }
    }
}
