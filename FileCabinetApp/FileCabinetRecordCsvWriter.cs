namespace FileCabinetApp
{
    /// <summary>
    /// Writing <see cref="FileCabinetRecord"/> to the .csv file.
    /// </summary>
    public class FileCabinetRecordCsvWriter
    {
        private TextWriter writer;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetRecordCsvWriter"/> class.
        /// </summary>
        /// <param name="writer">Contains <see cref="TextWriter"/> to use.</param>
        public FileCabinetRecordCsvWriter(TextWriter writer)
        {
            this.writer = writer;
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="FileCabinetRecordCsvWriter"/> class.
        /// </summary>
        ~FileCabinetRecordCsvWriter() => this.Dispose(false);

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Write a collection of <see cref="FileCabinetRecord"/> sequences to CSV file and save it.
        /// </summary>
        /// <param name="records">Collection of actual <see cref="FileCabinetRecord"/> data.</param>
        /// <param name="append">Append text if true, rewrite if false.</param>
        public void Write(IReadOnlyCollection<FileCabinetRecord> records, bool append)
        {
            foreach (var record in records ?? throw new ArgumentNullException(nameof(records), "Records can't be null"))
            {
                if (record is null)
                {
                    throw new ArgumentNullException(nameof(record), "Record can't be null");
                }

                this.Write(record);
            }

            this.writer.Flush();
            this.writer.Dispose();
        }

        /// <summary>
        /// Write <see cref="FileCabinetRecord"/> sequence to CSV file and save it.
        /// </summary>
        /// <param name="record">Contains actual <see cref="FileCabinetRecord"/> data.</param>
        private void Write(FileCabinetRecord record)
        {
            this.writer.WriteLine($"{record.Id},{record.FirstName},{record.LastName},{record.DateOfBirth.ToString("MM/dd/yyyy", new System.Globalization.CultureInfo("en-US"))},{record.Height},{record.Weight},{record.Temperament}.");
        }

        private void Dispose(bool disposing)
        {
            this.writer?.Dispose();
        }
    }
}
