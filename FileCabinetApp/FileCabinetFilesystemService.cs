using System.Collections.ObjectModel;
using System.Text;

#pragma warning disable CS8602 // Разыменование вероятной пустой ссылки.
#pragma warning disable SA1305 // Field names should not use Hungarian notation
namespace FileCabinetApp
{
    /// <summary>
    /// Represents standart operations with list of records in file system.
    /// </summary>
    public class FileCabinetFilesystemService : IFileCabinetService
    {
        private const int MaxRecordLength = 258 + sizeof(short) + sizeof(decimal) + sizeof(char);

        private readonly string pathToDb = @"D:\Прога\epam-project\cabinet-records.db";
        private readonly Dictionary<int, int> idOffsetDictionary = new Dictionary<int, int>();
        private readonly Dictionary<string, List<int>> firstNameOffsetDictionary = new Dictionary<string, List<int>>(StringComparer.CurrentCultureIgnoreCase);
        private readonly Dictionary<string, List<int>> lastNameOffsetDictionary = new Dictionary<string, List<int>>(StringComparer.CurrentCultureIgnoreCase);
        private readonly Dictionary<DateTime, List<int>> dateOfBirthOffsetDictionary = new Dictionary<DateTime, List<int>>();

        private FileStream fileStream;
        private IRecordValidator validator;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetFilesystemService"/> class.
        /// </summary>
        /// <param name="validator">Validator.</param>
        public FileCabinetFilesystemService(IRecordValidator validator)
        {
            this.validator = validator;
            this.fileStream = File.Open(this.pathToDb, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            if (this.fileStream.Length >= MaxRecordLength)
            {
                this.fileStream.Position = 0;
                while (this.fileStream.Position != this.fileStream.Length)
                {
                    var record = this.ReadRecord((int)this.fileStream.Position);
                    if (!record.Item2)
                    {
                        this.AddToDictionaries(record.Item1, (int)this.fileStream.Position - MaxRecordLength);
                    }
                }
            }
        }

        /// <inheritdoc/>
        public int CreateRecord(FileCabinetRecord newRecord)
        {
            if (!this.validator.ValidateParameters(newRecord))
            {
                Console.WriteLine("Invalid parameters. Id: " + newRecord.Id);
                return -1;
            }

            int newId = newRecord.Id == 0 ? this.LastId() + 1 : newRecord.Id;
            FileCabinetRecord record = new FileCabinetRecord()
            {
                Id = newId,
                FirstName = newRecord.FirstName,
                LastName = newRecord.LastName,
                DateOfBirth = newRecord.DateOfBirth,
                Height = newRecord.Height,
                Weight = newRecord.Weight,
                Temperament = newRecord.Temperament,
            };

            byte[] recordByteArray = RecordToByte(record, false);
            this.fileStream.Position = this.fileStream.Length;
            this.AddToDictionaries(record, (int)this.fileStream.Position);
            this.fileStream.Write(recordByteArray);
            this.fileStream.Flush();

            return newId;
        }

        /// <inheritdoc/>
        public void EditRecord(int id, FileCabinetRecord newRecord)
        {
            int index = this.RecordIndex(id);
            if (index < 0)
            {
                throw new ArgumentException("Wrong id", nameof(id));
            }

            this.fileStream.Position = index * MaxRecordLength;
            var oldRecord = this.ReadRecord((int)this.fileStream.Position);
            if (oldRecord.Item2)
            {
                throw new ArgumentException("Wrong id", nameof(id));
            }

            this.fileStream.Position -= MaxRecordLength;
            FileCabinetRecord record = new FileCabinetRecord()
            {
                Id = id,
                FirstName = newRecord.FirstName,
                LastName = newRecord.LastName,
                DateOfBirth = newRecord.DateOfBirth,
                Height = newRecord.Height,
                Weight = newRecord.Weight,
                Temperament = newRecord.Temperament,
            };

            byte[] recordByteArray = RecordToByte(record, false);
            this.UpdateDictionaries(record, oldRecord.Item1, (int)this.fileStream.Position);
            this.fileStream.Write(recordByteArray);
            this.fileStream.Flush();
        }

        /// <inheritdoc/>
        public bool RemoveRecord(int id)
        {
            if (id < 0)
            {
                throw new ArgumentException("Wrong parameter: ", nameof(id));
            }

            int indexToRemove = this.RecordIndex(id);
            if (indexToRemove == -1)
            {
                return false;
            }
            else
            {
                this.fileStream.Position = indexToRemove * MaxRecordLength;
                var recordToRemove = this.ReadRecord((int)this.fileStream.Position);
                this.RemoveFromDictionaries(recordToRemove.Item1);
                this.fileStream.Position -= MaxRecordLength;
                byte[] byteStatus = new byte[2];
                byteStatus = BitConverter.GetBytes((short)4);
                this.fileStream.Write(byteStatus);
                return true;
            }
        }

        /// <inheritdoc/>
        public int RecordIndex(int id)
        {
            byte[] byteStatus = new byte[2];
            byte[] byteId = new byte[4];
            int readedId;
            int index = 0;
            this.fileStream.Position = 0;
            while (this.fileStream.Position < this.fileStream.Length)
            {
                this.fileStream.Read(byteStatus, 0, 2);
                if (BitConverter.ToInt16(byteStatus) == 4)
                {
                    index++;
                    this.fileStream.Position += MaxRecordLength - 2;
                    continue;
                }

                this.fileStream.Read(byteId, 0, 4);
                readedId = BitConverter.ToInt32(byteId);
                if (readedId == id)
                {
                    return index;
                }
                else
                {
                    index++;
                    this.fileStream.Position += MaxRecordLength - 6;
                }
            }

            return -1;
        }

        /// <inheritdoc/>
        public IEnumerable<FileCabinetRecord> FindByFirstName(string firstName)
        {
            if (!this.firstNameOffsetDictionary.ContainsKey(firstName) || string.IsNullOrEmpty(firstName))
            {
                return new List<FileCabinetRecord>();
            }

            return this.FindByFirstNameEnumerable(firstName);
        }

        private IEnumerable<FileCabinetRecord> FindByFirstNameEnumerable(string firstName)
        {
            foreach (var offset in this.firstNameOffsetDictionary[firstName])
            {
                var record = this.ReadRecord(offset);
                yield return record.Item1;
            }
        }

        /// <inheritdoc/>
        public IEnumerable<FileCabinetRecord> FindByLastName(string lastName)
        {
            if (!this.lastNameOffsetDictionary.ContainsKey(lastName) || string.IsNullOrEmpty(lastName))
            {
                return new List<FileCabinetRecord>();
            }

            return this.FindByLastNameEnumerable(lastName);
        }

        private IEnumerable<FileCabinetRecord> FindByLastNameEnumerable(string lastName)
        {
            foreach (var offset in this.lastNameOffsetDictionary[lastName])
            {
                var record = this.ReadRecord(offset);
                yield return record.Item1;
            }
        }

        /// <inheritdoc/>
        public IEnumerable<FileCabinetRecord> FindByDateOfBirth(DateTime dateOfBirth)
        {
            if (!this.dateOfBirthOffsetDictionary.ContainsKey(dateOfBirth))
            {
                return new List<FileCabinetRecord>();
            }

            return this.FindByDateOfBirthEnumerable(dateOfBirth);
        }

        private IEnumerable<FileCabinetRecord> FindByDateOfBirthEnumerable(DateTime dateOfBirth)
        {
            foreach (var offset in this.dateOfBirthOffsetDictionary[dateOfBirth])
            {
                var record = this.ReadRecord(offset);
                yield return record.Item1;
            }
        }

        /// <inheritdoc/>
        public List<FileCabinetRecord> GetRecords()
        {
            var allRecords = this.ReadRecords();
            List<FileCabinetRecord> aliveRecords = new List<FileCabinetRecord>();
            foreach (var record in allRecords)
            {
                if (!record.Item2)
                {
                    aliveRecords.Add(record.Item1);
                }
            }

            return aliveRecords;
        }

        /// <inheritdoc/>
        public FileCabinetServiceSnapshot MakeSnapshot()
        {
            List<FileCabinetRecord> records = this.GetRecords();
            return new FileCabinetServiceSnapshot(records);
        }

        /// <inheritdoc/>
        public int GetStat(out int removedCount)
        {
            var allRecords = this.ReadRecords();
            removedCount = 0;
            foreach (var record in allRecords)
            {
                if (record.Item2)
                {
                    removedCount++;
                }
            }

            return allRecords.Count;
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

        private static byte[] RecordToByte(FileCabinetRecord record, bool isDeleted)
        {
            byte[] recordByteArray = new byte[MaxRecordLength];
            using var memoryStream = new MemoryStream(recordByteArray);
            using var binaryWriter = new BinaryWriter(memoryStream);
            var byteFirstName = StringToByte(record.FirstName);
            var byteLastName = StringToByte(record.LastName);
            short status = isDeleted ? (short)4 : (short)0;
            binaryWriter.Write(status);
            binaryWriter.Write(record.Id);
            binaryWriter.Write(byteFirstName);
            binaryWriter.Write(byteLastName);
            binaryWriter.Write(record.DateOfBirth.Year);
            binaryWriter.Write(record.DateOfBirth.Month);
            binaryWriter.Write(record.DateOfBirth.Day);
            binaryWriter.Write(record.Height);
            binaryWriter.Write(record.Weight);
            binaryWriter.Write(record.Temperament);

            return recordByteArray;
        }

        private static (FileCabinetRecord, bool) ByteToRecord(byte[] byteRecord)
        {
            using MemoryStream memoryStream = new MemoryStream(byteRecord);
            using BinaryReader binaryReader = new BinaryReader(memoryStream);

            short status = binaryReader.ReadInt16();
            var record = new FileCabinetRecord()
            {
                Id = binaryReader.ReadInt32(),
                FirstName = Encoding.ASCII.GetString(binaryReader.ReadBytes(120)).TrimEnd('\0'),
                LastName = Encoding.ASCII.GetString(binaryReader.ReadBytes(120)).TrimEnd('\0'),
                DateOfBirth = new DateTime(year: binaryReader.ReadInt32(), month: binaryReader.ReadInt32(), day: binaryReader.ReadInt32()),
                Height = binaryReader.ReadInt16(),
                Weight = binaryReader.ReadDecimal(),
                Temperament = binaryReader.ReadChar(),
            };

            bool isDeleted = false;
            if (status == 4)
            {
                isDeleted = true;
            }

            return (record, isDeleted);
        }

        private int LastId()
        {
            if (this.idOffsetDictionary.Count > 0)
            {
                return this.idOffsetDictionary.Keys.Max();
            }

            return 0;
        }

        private void AddToDictionaries(FileCabinetRecord newRecord, int position)
        {
            if (this.idOffsetDictionary.ContainsKey(newRecord.Id))
            {
                throw new ArgumentException("WRONG ID");
            }
            else
            {
                this.idOffsetDictionary.Add(newRecord.Id, position);
            }

            if (this.firstNameOffsetDictionary.ContainsKey(newRecord.FirstName))
            {
                this.firstNameOffsetDictionary[newRecord.FirstName].Add(position);
            }
            else
            {
                this.firstNameOffsetDictionary.Add(newRecord.FirstName, new List<int>());
                this.firstNameOffsetDictionary[newRecord.FirstName].Add(position);
            }

            if (this.lastNameOffsetDictionary.ContainsKey(newRecord.LastName))
            {
                this.lastNameOffsetDictionary[newRecord.LastName].Add(position);
            }
            else
            {
                this.lastNameOffsetDictionary.Add(newRecord.LastName, new List<int>());
                this.lastNameOffsetDictionary[newRecord.LastName].Add(position);
            }

            if (this.dateOfBirthOffsetDictionary.ContainsKey(newRecord.DateOfBirth))
            {
                this.dateOfBirthOffsetDictionary[newRecord.DateOfBirth].Add(position);
            }
            else
            {
                this.dateOfBirthOffsetDictionary.Add(newRecord.DateOfBirth, new List<int>());
                this.dateOfBirthOffsetDictionary[newRecord.DateOfBirth].Add(position);
            }
        }

        private void UpdateDictionaries(FileCabinetRecord newRecord, FileCabinetRecord oldRecord, int position)
        {
            this.firstNameOffsetDictionary[oldRecord.FirstName].Remove(this.idOffsetDictionary[oldRecord.Id]);
            if (this.firstNameOffsetDictionary.ContainsKey(newRecord.FirstName))
            {
                this.firstNameOffsetDictionary[newRecord.FirstName].Add(position);
            }
            else
            {
                this.firstNameOffsetDictionary.Add(newRecord.FirstName, new List<int>());
                this.firstNameOffsetDictionary[newRecord.FirstName].Add(position);
            }

            this.lastNameOffsetDictionary[oldRecord.LastName].Remove(this.idOffsetDictionary[oldRecord.Id]);
            if (this.lastNameOffsetDictionary.ContainsKey(newRecord.LastName))
            {
                this.lastNameOffsetDictionary[newRecord.LastName].Add(position);
            }
            else
            {
                this.lastNameOffsetDictionary.Add(newRecord.LastName, new List<int>());
                this.lastNameOffsetDictionary[newRecord.LastName].Add(position);
            }

            this.dateOfBirthOffsetDictionary[oldRecord.DateOfBirth].Remove(this.idOffsetDictionary[oldRecord.Id]);
            if (this.dateOfBirthOffsetDictionary.ContainsKey(newRecord.DateOfBirth))
            {
                this.dateOfBirthOffsetDictionary[newRecord.DateOfBirth].Add(position);
            }
            else
            {
                this.dateOfBirthOffsetDictionary.Add(newRecord.DateOfBirth, new List<int>());
                this.dateOfBirthOffsetDictionary[newRecord.DateOfBirth].Add(position);
            }
        }

        private void RemoveFromDictionaries(FileCabinetRecord recordToRemove)
        {
            this.firstNameOffsetDictionary[recordToRemove.FirstName].Remove(this.idOffsetDictionary[recordToRemove.Id]);
            this.lastNameOffsetDictionary[recordToRemove.LastName].Remove(this.idOffsetDictionary[recordToRemove.Id]);
            this.dateOfBirthOffsetDictionary[recordToRemove.DateOfBirth].Remove(this.idOffsetDictionary[recordToRemove.Id]);
            this.idOffsetDictionary.Remove(recordToRemove.Id);
        }

        private (FileCabinetRecord, bool) ReadRecord(int startIndex)
        {
            byte[] byteRecord = new byte[MaxRecordLength];
            this.fileStream.Position = startIndex;
            this.fileStream.Read(byteRecord);

            return ByteToRecord(byteRecord);
        }

        private List<(FileCabinetRecord, bool)> ReadRecords()
        {
            List<(FileCabinetRecord, bool)> result = new List<(FileCabinetRecord, bool)>();
            this.fileStream.Position = 0;
            while (this.fileStream.Position != this.fileStream.Length)
            {
                var record = this.ReadRecord((int)this.fileStream.Position);
                result.Add(record);
            }

            return result;
        }

        public int Restore(FileCabinetServiceSnapshot snapshot)
        {
            int counter = 0;
            foreach (var snapshotRecord in snapshot.Records)
            {
                if (!this.validator.ValidateParameters(snapshotRecord))
                {
                    Console.WriteLine("Invalid parameters. Id: " + snapshotRecord.Id);
                    continue;
                }

                if (this.idOffsetDictionary.ContainsKey(snapshotRecord.Id))
                {
                    this.EditRecord(snapshotRecord.Id, snapshotRecord);
                    counter++;
                }
                else
                {
                    this.CreateRecord(snapshotRecord);
                    counter++;
                }
            }

            return counter;
        }

        public int Purge()
        {
            var allRecords = this.ReadRecords();
            List<FileCabinetRecord> aliveRecords = new List<FileCabinetRecord>();
            foreach (var record in allRecords)
            {
                if (!record.Item2)
                {
                    aliveRecords.Add(record.Item1);
                }
            }

            long startLength = this.fileStream.Length;
            this.fileStream.Close();
            this.fileStream = File.Open(pathToDb, FileMode.Create, FileAccess.ReadWrite);
            this.fileStream.Position = 0;
            foreach (var record in aliveRecords)
            {
                byte[] recordByteArray = RecordToByte(record, false);
                this.fileStream.Position = this.fileStream.Length;
                this.fileStream.Write(recordByteArray);
            }

            this.fileStream.Flush();
            long purgedLength = this.fileStream.Length;

            return (int)(startLength - purgedLength) / MaxRecordLength;
        }
    }
}
#pragma warning restore CS8602 // Разыменование вероятной пустой ссылки.