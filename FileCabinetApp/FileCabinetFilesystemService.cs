using System.Collections.ObjectModel;

namespace FileCabinetApp
{
    public class FileCabinetFilesystemService : IFileCabinetService
    {
        private FileStream fileStream;
        private IRecordValidator validator;

        public FileCabinetFilesystemService(IRecordValidator validator)
        {
            this.validator = validator;
            this.fileStream = File.Open("cabinet-records.db", FileMode.OpenOrCreate, FileAccess.ReadWrite);
        }

        public int CreateRecord(Record newRecord)
        {
            throw new NotImplementedException();
        }

        public void EditRecord(int id, Record newRecord)
        {
            throw new NotImplementedException();
        }

        public int RecordIndex(int id)
        {
            throw new NotImplementedException();
        }

        public ReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName)
        {
            throw new NotImplementedException();
        }

        public ReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastName)
        {
            throw new NotImplementedException();
        }

        public ReadOnlyCollection<FileCabinetRecord> FindByDateOfBirth(DateTime dateOfBirth)
        {
            throw new NotImplementedException();
        }

        public ReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            throw new NotImplementedException();
        }

        public int GetStat()
        {
            throw new NotImplementedException();
        }

        public FileCabinetServiceSnapshot MakeSnapshot()
        {
            throw new NotImplementedException();
        }
    }
}
