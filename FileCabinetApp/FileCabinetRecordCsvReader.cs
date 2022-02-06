namespace FileCabinetApp
{
    public class FileCabinetRecordCsvReader
    {
        private StreamReader reader;

        public FileCabinetRecordCsvReader(StreamReader reader)
        {
            this.reader = reader;
        }

        public IList<FileCabinetRecord> ReadAll()
        {
            List<FileCabinetRecord> fileCabinetRecords = new List<FileCabinetRecord>();
            while (!this.reader.EndOfStream)
            {
                fileCabinetRecords.Add(this.Read());
            }

            this.Dispose();
            return fileCabinetRecords;
        }

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
                Weight = Convert.ToDecimal(recordString[5]),
                Temperament = recordString[6][0],
            };
        }

        private void Dispose(bool disposing)
        {
            this.reader?.Dispose();
        }
    }
}