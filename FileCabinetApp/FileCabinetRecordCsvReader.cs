namespace FileCabinetApp
{
    /// <summary>
    /// Class for reading records from csv files.
    /// </summary>
    public class FileCabinetRecordCsvReader
    {
        private StreamReader reader;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetRecordCsvReader"/> class.
        /// </summary>
        /// <param name="reader"> - <see cref="StreamReader"/> to read from.</param>
        public FileCabinetRecordCsvReader(StreamReader reader)
        {
            this.reader = reader;
        }

        /// <summary>
        /// Reads all records from the stream.
        /// </summary>
        /// <returns> <see cref="IEnumerable{FileCabinetRecord}"/> of red records.</returns>
        public IEnumerable<FileCabinetRecord> ReadAll()
        {
            while (!this.reader.EndOfStream)
            {
                yield return this.Read();
            }

            this.Dispose();
        }

        /// <summary>
        /// Dispose method for reader.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        private FileCabinetRecord Read()
        {
            string[] recordString = this.reader.ReadLine().Split(',');

            string[] dob = recordString[3].Split('/');
            return new FileCabinetRecord()
            {
                Id = Convert.ToInt32(recordString[0]),
                FirstName = recordString[1],
                LastName = recordString[2],
                DateOfBirth = new DateTime(month: Convert.ToInt32(dob[0]), day: Convert.ToInt32(dob[1]), year: Convert.ToInt32(dob[2])),
                Height = Convert.ToInt16(recordString[4]),
                Weight = Convert.ToDecimal(recordString[5], System.Globalization.CultureInfo.InvariantCulture),
                Temperament = recordString[6][0],
            };
        }

        private void Dispose(bool disposing)
        {
            this.reader?.Dispose();
        }
    }
}