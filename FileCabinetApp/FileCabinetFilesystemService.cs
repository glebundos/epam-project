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

        private readonly Dictionary<int, int> idOffsetDictionary = new Dictionary<int, int>();
        private readonly Dictionary<string, List<int>> firstNameOffsetDictionary = new Dictionary<string, List<int>>();
        private readonly Dictionary<string, List<int>> lastNameOffsetDictionary = new Dictionary<string, List<int>>();
        private readonly Dictionary<DateTime, List<int>> dateOfBirthOffsetDictionary = new Dictionary<DateTime, List<int>>();

        private readonly System.Globalization.CultureInfo cultureInfo = new System.Globalization.CultureInfo("en-US");
        private FileStream fileStream;
        private IRecordValidator validator;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetFilesystemService"/> class.
        /// </summary>
        /// <param name="validator">Validator.</param>
        public FileCabinetFilesystemService(IRecordValidator validator)
        {
            this.validator = validator;
            this.fileStream = File.Open(@"D:\Прога\epam-project\cabinet-records.db", FileMode.OpenOrCreate, FileAccess.ReadWrite);
            if (this.fileStream.Length >= MaxRecordLength)
            {
                byte[] byteRecord = new byte[MaxRecordLength];
                this.fileStream.Position = 0;
                while (this.fileStream.Position != this.fileStream.Length)
                {
                    this.fileStream.Read(byteRecord);
                    var record = ByteToRecord(byteRecord);
                    this.AddToDictionaries(record, (int)this.fileStream.Position - MaxRecordLength);
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

            byte[] recordByteArray = RecordToByte(record);
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
                throw new ArgumentException("Wrong id", $"{nameof(id)}");
            }

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
            this.fileStream.Position = index * MaxRecordLength;
            var oldRecord = this.ReadRecord((int)this.fileStream.Position);
            this.fileStream.Position -= MaxRecordLength;
            byte[] recordByteArray = RecordToByte(record);
            this.UpdateDictionaries(record, oldRecord, (int)this.fileStream.Position);
            this.fileStream.Write(recordByteArray);
            this.fileStream.Flush();
        }

        /// <inheritdoc/>
        public int RecordIndex(int id)
        {
            byte[] byteId = new byte[4];
            int readedId = 0;
            int index = 0;
            this.fileStream.Position = 2;
            while (this.fileStream.Position < this.fileStream.Length)
            {
                this.fileStream.Read(byteId, 0, 4);
                readedId = BitConverter.ToInt32(byteId);
                if (readedId == id)
                {
                    return index;
                }
                else
                {
                    index++;
                    this.fileStream.Position += MaxRecordLength - 4;
                }
            }

            return -1;
        }

        /// <inheritdoc/>
        public IReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName)
        {
            List<FileCabinetRecord> fileCabinetRecordsByFirstName = new List<FileCabinetRecord>();
            if (this.firstNameOffsetDictionary.ContainsKey(firstName))
            {
                foreach (var item in this.firstNameOffsetDictionary[firstName])
                {
                    var record = this.ReadRecord(item);
                    fileCabinetRecordsByFirstName.Add(record);
                }
            }

            return fileCabinetRecordsByFirstName;
        }

        /// <inheritdoc/>
        public IReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastName)
        {
            List<FileCabinetRecord> fileCabinetRecordsByLastName = new List<FileCabinetRecord>();
            if (this.lastNameOffsetDictionary.ContainsKey(lastName))
            {
                foreach (var item in this.lastNameOffsetDictionary[lastName])
                {
                    var record = this.ReadRecord(item);
                    fileCabinetRecordsByLastName.Add(record);
                }
            }

            return fileCabinetRecordsByLastName;
        }

        /// <inheritdoc/>
        public IReadOnlyCollection<FileCabinetRecord> FindByDateOfBirth(DateTime dateOfBirth)
        {
            List<FileCabinetRecord> fileCabinetRecordsByDateOfBirth = new List<FileCabinetRecord>();
            if (this.dateOfBirthOffsetDictionary.ContainsKey(dateOfBirth))
            {
                foreach (var item in this.dateOfBirthOffsetDictionary[dateOfBirth])
                {
                    var record = this.ReadRecord(item);
                    fileCabinetRecordsByDateOfBirth.Add(record);
                }
            }

            return fileCabinetRecordsByDateOfBirth;
        }

        /// <inheritdoc/>
        public IReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            var result = this.ReadRecords();
            return result;
        }

        /// <inheritdoc/>
        public FileCabinetServiceSnapshot MakeSnapshot()
        {
            return new FileCabinetServiceSnapshot(this.ReadRecords());
        }

        /// <inheritdoc/>
        public int GetStat()
        {
            return this.ReadRecords().Count;
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

        private static byte[] RecordToByte(FileCabinetRecord record)
        {
            byte[] recordByteArray = new byte[MaxRecordLength];
            using var memoryStream = new MemoryStream(recordByteArray);
            using var binaryWriter = new BinaryWriter(memoryStream);
            var byteFirstName = StringToByte(record.FirstName);
            var byteLastName = StringToByte(record.LastName);
            short status = 0;
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

        private static FileCabinetRecord ByteToRecord(byte[] byteRecord)
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

            return record;
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

            if (this.firstNameOffsetDictionary.ContainsKey(newRecord.FirstName.ToLower(this.cultureInfo)))
            {
                this.firstNameOffsetDictionary[newRecord.FirstName.ToLower(this.cultureInfo)].Add(position);
            }
            else
            {
                this.firstNameOffsetDictionary.Add(newRecord.FirstName.ToLower(this.cultureInfo), new List<int>());
                this.firstNameOffsetDictionary[newRecord.FirstName.ToLower(this.cultureInfo)].Add(position);
            }

            if (this.lastNameOffsetDictionary.ContainsKey(newRecord.LastName.ToLower(this.cultureInfo)))
            {
                this.lastNameOffsetDictionary[newRecord.LastName.ToLower(this.cultureInfo)].Add(position);
            }
            else
            {
                this.lastNameOffsetDictionary.Add(newRecord.LastName.ToLower(this.cultureInfo), new List<int>());
                this.lastNameOffsetDictionary[newRecord.LastName.ToLower(this.cultureInfo)].Add(position);
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
            this.firstNameOffsetDictionary[oldRecord.FirstName.ToLower(this.cultureInfo)].Remove(this.idOffsetDictionary[oldRecord.Id]);
            if (this.firstNameOffsetDictionary.ContainsKey(newRecord.FirstName.ToLower(this.cultureInfo)))
            {
                this.firstNameOffsetDictionary[newRecord.FirstName.ToLower(this.cultureInfo)].Add(position);
            }
            else
            {
                this.firstNameOffsetDictionary.Add(newRecord.FirstName.ToLower(this.cultureInfo), new List<int>());
                this.firstNameOffsetDictionary[newRecord.FirstName.ToLower(this.cultureInfo)].Add(position);
            }

            this.lastNameOffsetDictionary[oldRecord.LastName.ToLower(this.cultureInfo)].Remove(this.idOffsetDictionary[oldRecord.Id]);
            if (this.lastNameOffsetDictionary.ContainsKey(newRecord.LastName.ToLower(this.cultureInfo)))
            {
                this.lastNameOffsetDictionary[newRecord.LastName.ToLower(this.cultureInfo)].Add(position);
            }
            else
            {
                this.lastNameOffsetDictionary.Add(newRecord.LastName.ToLower(this.cultureInfo), new List<int>());
                this.lastNameOffsetDictionary[newRecord.LastName.ToLower(this.cultureInfo)].Add(position);
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

        private FileCabinetRecord ReadRecord(int startIndex)
        {
            byte[] byteRecord = new byte[MaxRecordLength];
            this.fileStream.Position = startIndex;
            this.fileStream.Read(byteRecord);
            return ByteToRecord(byteRecord);
        }

        private List<FileCabinetRecord> ReadRecords()
        {
            byte[] byteRecord = new byte[MaxRecordLength];
            List<FileCabinetRecord> result = new List<FileCabinetRecord>();
            this.fileStream.Position = 0;
            while (this.fileStream.Position != this.fileStream.Length)
            {
                this.fileStream.Read(byteRecord);
                var record = ByteToRecord(byteRecord);
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

                if (idOffsetDictionary.ContainsKey(snapshotRecord.Id))
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
    }
}
#pragma warning restore CS8602 // Разыменование вероятной пустой ссылки.