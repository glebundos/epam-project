namespace FileCabinetApp
{
    public class ServiceLogger : IFileCabinetService
    {
        private IFileCabinetService service;

        private TextWriter writer;

        public ServiceLogger(IFileCabinetService service)
        {
            this.service = service;
            this.writer = new StreamWriter("service-logger.txt", true);
        }

        public int CreateRecord(FileCabinetRecord newRecord)
        {
            string date = DateTime.Now.ToString("MM-dd-yyyy HH:mm");
            this.writer.WriteLine($"{date} - Calling CreateRecord with FirstName = '{newRecord.FirstName}', LastName = '{newRecord.LastName}', DateOfBirth = '{newRecord.DateOfBirth}', " +
                              $"Height = '{newRecord.Height}', Weight = '{newRecord.Weight}', Temperament = '{newRecord.Temperament}'");
            var result = this.service.CreateRecord(newRecord);
            this.writer.WriteLine($"{date} - CreateRecord returned {result}");
            this.writer.Flush();
            return result;
        }

        public void EditRecord(int id, FileCabinetRecord newRecord)
        {
            string date = DateTime.Now.ToString("MM-dd-yyyy HH:mm");
            this.writer.WriteLine($"{date} -Calling EditRecord with Id = '{id}', FirstName = '{newRecord.FirstName}', LastName = '{newRecord.LastName}', DateOfBirth = '{newRecord.DateOfBirth}', " +
                              $"Height = '{newRecord.Height}', Weight = '{newRecord.Weight}', Temperament = '{newRecord.Temperament}'");
            this.service.EditRecord(id, newRecord);
            this.writer.Flush();
        }

        public IEnumerable<FileCabinetRecord> FindByDateOfBirth(DateTime dateOfBirth)
        {
            string date = DateTime.Now.ToString("MM-dd-yyyy HH:mm");
            this.writer.WriteLine($"{date} - Calling FindByDateOfBirth with DateOfBirth = '{dateOfBirth}'");
            var result = this.service.FindByDateOfBirth(dateOfBirth);
            this.writer.WriteLine($"{date} - FindByDateOfBirth returned {result}");
            this.writer.Flush();
            return result;
        }

        public IEnumerable<FileCabinetRecord> FindByFirstName(string firstName)
        {
            string date = DateTime.Now.ToString("MM-dd-yyyy HH:mm");
            this.writer.WriteLine($"{date} - Calling FindByFirstName with FirstName = '{firstName}'");
            var result = this.service.FindByFirstName(firstName);
            this.writer.WriteLine($"{date} - FindByFirstName returned {result}");
            this.writer.Flush();
            return result;
        }

        public IEnumerable<FileCabinetRecord> FindByLastName(string lastName)
        {
            string date = DateTime.Now.ToString("MM-dd-yyyy HH:mm");
            this.writer.WriteLine($"{date} - Calling FindByLastName with LastName = '{lastName}'");
            var result = this.service.FindByLastName(lastName);
            this.writer.WriteLine($"{date} - FindByLastName returned {result}");
            this.writer.Flush();
            return result;
        }

        public FileCabinetRecord GetById(int id)
        {
            string date = DateTime.Now.ToString("MM-dd-yyyy HH:mm");
            this.writer.WriteLine($"{date} - Calling GetById with Id = '{id}'");
            var result = this.service.GetById(id);
            this.writer.WriteLine($"{date} - GetById returned {result}");
            this.writer.Flush();
            return result;
        }

        public List<FileCabinetRecord> GetRecords()
        {
            string date = DateTime.Now.ToString("MM-dd-yyyy HH:mm");
            this.writer.WriteLine($"{date} - Calling GetRecords");
            var result = this.service.GetRecords();
            this.writer.Flush();
            return result;
        }

        public int GetStat(out int removedCount)
        {
            string date = DateTime.Now.ToString("MM-dd-yyyy HH:mm");
            this.writer.WriteLine($"{date} - Calling GetStat");
            var result = this.service.GetStat(out removedCount);
            this.writer.WriteLine($"{date} - GetStat returned {result} (RemovedCount = {removedCount})");
            this.writer.Flush();
            return result;
        }

        public FileCabinetServiceSnapshot MakeSnapshot()
        {
            string date = DateTime.Now.ToString("MM-dd-yyyy HH:mm");
            this.writer.WriteLine($"{date} - Calling MakeSnapshot");
            var result = this.service.MakeSnapshot();
            this.writer.Flush();
            return result;
        }

        public int Purge()
        {
            string date = DateTime.Now.ToString("MM-dd-yyyy HH:mm");
            this.writer.WriteLine($"{date} - Calling Purge");
            var result = this.service.Purge();
            this.writer.WriteLine($"{date} - Purge returned {result}");
            this.writer.Flush();
            return result;
        }

        public int RecordIndex(int id)
        {
            string date = DateTime.Now.ToString("MM-dd-yyyy HH:mm");
            this.writer.WriteLine($"{date} - Calling RecordIndex with Id = '{id}'");
            var result = this.service.RecordIndex(id);
            this.writer.WriteLine($"{date} - RecordIndex returned {result}");
            this.writer.Flush();
            return result;
        }

        public bool RemoveRecord(int id)
        {
            string date = DateTime.Now.ToString("MM-dd-yyyy HH:mm");
            this.writer.WriteLine($"{date} - Calling RemoveRecord with Id = '{id}'");
            var result = this.service.RemoveRecord(id);
            this.writer.WriteLine($"{date} - RemoveRecord returned {result}");
            this.writer.Flush();
            return result;
        }

        public int Restore(FileCabinetServiceSnapshot snapshot)
        {
            string date = DateTime.Now.ToString("MM-dd-yyyy HH:mm");
            this.writer.WriteLine($"{date} - Calling Restore");
            var result = this.service.Restore(snapshot);
            this.writer.WriteLine($"{date} - Restore returned {result}");
            this.writer.Flush();
            return result;
        }
    }
}
