using System.Collections.ObjectModel;
using System.Text;

namespace FileCabinetApp
{
    public class FileCabinetFilesystemService : IFileCabinetService
    {
        private const int MaxRecordLength = 258 + sizeof(short) + sizeof(decimal) + sizeof(char);
        private FileStream fileStream;
        private IRecordValidator validator;
        private int count;

        public FileCabinetFilesystemService(IRecordValidator validator)
        {
            this.validator = validator;
            this.fileStream = File.Open(@"D:\Прога\epam-project\cabinet-records.db", FileMode.OpenOrCreate, FileAccess.ReadWrite);
            this.count = (int)(this.fileStream.Length / MaxRecordLength);
        }

        public int CreateRecord(Record newRecord)
        {
            if (!this.validator.ValidateParameters(newRecord))
            {
                Console.WriteLine("Invalid parameters.");
                return -1;
            }

            this.count++;
            byte[] recordByteArray = RecordToByte(newRecord, this.count);
            this.fileStream.Position = this.fileStream.Length;
            this.fileStream.Write(recordByteArray);
            this.fileStream.Flush();
            return this.count;
        }

        private byte[] RecordToByte(Record record, int id)
        {
            byte[] recordByteArray = new byte[MaxRecordLength];
            using var memoryStream = new MemoryStream(recordByteArray);
            using var binaryWriter = new BinaryWriter(memoryStream);
            var byteFirstName = StringToByte(record.FirstName);
            var byteLastName = StringToByte(record.LastName);
            short status = 0;
            binaryWriter.Write(status);
            binaryWriter.Write(id);
            binaryWriter.Write(byteFirstName);
            binaryWriter.Write(byteLastName);
            binaryWriter.Write(record.DateOfBirth.Day);
            binaryWriter.Write(record.DateOfBirth.Month);
            binaryWriter.Write(record.DateOfBirth.Year);
            binaryWriter.Write(record.Height);
            binaryWriter.Write(record.Weight);
            binaryWriter.Write(record.Temperament);

            return recordByteArray;
        }

        private static byte[] StringToByte(string text)
        {
            var result = new byte[120];
            var asciiText = Encoding.ASCII.GetBytes(text);
            int textLength = text.Length;
            if (textLength > 120)
            {
                textLength = 120;
            }

            Array.Copy(asciiText, result, textLength);
            return result;
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
