using System.Collections.ObjectModel;

namespace FileCabinetApp
{
    /// <summary>
    /// Represents standart operations with list of records in memory.
    /// </summary>
    public class FileCabinetMemoryService : IFileCabinetService
    {
        private readonly List<FileCabinetRecord> list = new List<FileCabinetRecord>();
        private readonly IRecordValidator validator;
        private readonly Dictionary<string, List<FileCabinetRecord>> firstNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private readonly Dictionary<string, List<FileCabinetRecord>> lastNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private readonly Dictionary<DateTime, List<FileCabinetRecord>> dateOfBirthDictionary = new Dictionary<DateTime, List<FileCabinetRecord>>();
        private readonly List<int> idList = new List<int>();
        private readonly System.Globalization.CultureInfo cultureInfo = new System.Globalization.CultureInfo("en-US");

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetService"/> class.
        /// </summary>
        /// <param name="validator">Validator.</param>
        public FileCabinetMemoryService(IRecordValidator validator)
        {
            this.validator = validator;
        }

        /// <summary>
        /// Creates new record with given parameters.
        /// </summary>
        /// <param name="newRecord">Object with all record parameters.</param>
        /// <returns>Id of the created record or -1 in case of error.</returns>
#pragma warning disable CA1062 // newRecord проверяется на null в ValidateParameters.
#pragma warning disable CS8602 // Разыменование вероятной пустой ссылки (проверяется в ValidateParameters).
        public int CreateRecord(FileCabinetRecord newRecord)
        {
            try
            {
                if (!this.validator.ValidateParameters(newRecord))
                {
                    throw new ArgumentException("Wrong parameters", nameof(newRecord));
                }

                int newId = 1;
                if (this.idList.Count > 0)
                {
                    newId = this.idList.Max() + 1;
                }

                var record = new FileCabinetRecord
                {
                    Id = newRecord.Id == 0 ? newId : newRecord.Id,
                    FirstName = newRecord.FirstName,
                    LastName = newRecord.LastName,
                    DateOfBirth = newRecord.DateOfBirth,
                    Height = newRecord.Height,
                    Weight = newRecord.Weight,
                    Temperament = char.ToUpper(newRecord.Temperament, this.cultureInfo),
                };

                this.list.Add(record);

                this.idList.Add(record.Id);

                if (this.firstNameDictionary.ContainsKey(newRecord.FirstName.ToLower(this.cultureInfo)))
                {
                    this.firstNameDictionary[newRecord.FirstName.ToLower(this.cultureInfo)].Add(record);
                }
                else
                {
                    this.firstNameDictionary.Add(newRecord.FirstName.ToLower(this.cultureInfo), new List<FileCabinetRecord>());
                    this.firstNameDictionary[newRecord.FirstName.ToLower(this.cultureInfo)].Add(record);
                }

                if (this.lastNameDictionary.ContainsKey(newRecord.LastName.ToLower(this.cultureInfo)))
                {
                    this.lastNameDictionary[newRecord.LastName.ToLower(this.cultureInfo)].Add(record);
                }
                else
                {
                    this.lastNameDictionary.Add(newRecord.LastName.ToLower(this.cultureInfo), new List<FileCabinetRecord>());
                    this.lastNameDictionary[newRecord.LastName.ToLower(this.cultureInfo)].Add(record);
                }

                if (this.dateOfBirthDictionary.ContainsKey(newRecord.DateOfBirth))
                {
                    this.dateOfBirthDictionary[newRecord.DateOfBirth].Add(record);
                }
                else
                {
                    this.dateOfBirthDictionary.Add(newRecord.DateOfBirth, new List<FileCabinetRecord>());
                    this.dateOfBirthDictionary[newRecord.DateOfBirth].Add(record);
                }

                return record.Id;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return -1;
            }
        }

        /// <summary>
        /// Edits the record with the given id according to the given parameters.
        /// </summary>
        /// <param name="id">The Id of the record to change.</param>
        /// <param name="newRecord">Object with all record parameters.</param>
        public void EditRecord(int id, FileCabinetRecord newRecord)
        {
            try
            {
                int indexOfRecord = this.RecordIndex(id);
                if (indexOfRecord == -1)
                {
                    throw new ArgumentException("Wrong id", $"{nameof(id)}");
                }

                if (!this.validator.ValidateParameters(newRecord))
                {
                    throw new ArgumentException("Wrong parameters", nameof(newRecord));
                }

                this.validator.ValidateParameters(newRecord);
                var oldRecord = this.list[indexOfRecord];
                var record = new FileCabinetRecord
                {
                    Id = id,
                    FirstName = newRecord.FirstName,
                    LastName = newRecord.LastName,
                    DateOfBirth = newRecord.DateOfBirth,
                    Height = newRecord.Height,
                    Weight = newRecord.Weight,
                    Temperament = char.ToUpper(newRecord.Temperament, this.cultureInfo),
                };

                this.list[indexOfRecord] = record;
                string firstNameKey = string.Empty;
                if (oldRecord.FirstName != null)
                {
                    firstNameKey = oldRecord.FirstName.ToLower(this.cultureInfo);
                }

                this.firstNameDictionary[firstNameKey].Remove(oldRecord);
                if (this.firstNameDictionary.ContainsKey(newRecord.FirstName.ToLower(this.cultureInfo)))
                {
                    this.firstNameDictionary[newRecord.FirstName.ToLower(this.cultureInfo)].Add(record);
                }
                else
                {
                    this.firstNameDictionary.Add(newRecord.FirstName.ToLower(this.cultureInfo), new List<FileCabinetRecord>());
                    this.firstNameDictionary[newRecord.FirstName.ToLower(this.cultureInfo)].Add(record);
                }

                string lastNameKey = string.Empty;
                if (oldRecord.LastName != null)
                {
                    lastNameKey = oldRecord.LastName.ToLower(this.cultureInfo);
                }

                this.lastNameDictionary[lastNameKey].Remove(oldRecord);
                if (this.lastNameDictionary.ContainsKey(newRecord.LastName.ToLower(this.cultureInfo)))
                {
                    this.lastNameDictionary[newRecord.LastName.ToLower(this.cultureInfo)].Add(record);
                }
                else
                {
                    this.lastNameDictionary.Add(newRecord.LastName.ToLower(this.cultureInfo), new List<FileCabinetRecord>());
                    this.lastNameDictionary[newRecord.LastName.ToLower(this.cultureInfo)].Add(record);
                }

                this.dateOfBirthDictionary[oldRecord.DateOfBirth].Remove(oldRecord);
                if (this.dateOfBirthDictionary.ContainsKey(newRecord.DateOfBirth))
                {
                    this.dateOfBirthDictionary[newRecord.DateOfBirth].Add(record);
                }
                else
                {
                    this.dateOfBirthDictionary.Add(newRecord.DateOfBirth, new List<FileCabinetRecord>());
                    this.dateOfBirthDictionary[newRecord.DateOfBirth].Add(record);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
#pragma warning restore CA1062 // newRecord проверяется на null в ValidateParameters.
#pragma warning restore CS8602 // Разыменование вероятной пустой ссылки (проверяется в ValidateParameters).

        public bool RemoveRecord(int id)
        {
            if (id < 0)
            {
                throw new ArgumentException("Wrong parameter: ", nameof(id));
            }

            if (!this.idList.Contains(id))
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
                var recordToRemove = this.list[indexToRemove];

                this.list.RemoveAt(indexToRemove);
                this.idList.Remove(id);
                this.firstNameDictionary[recordToRemove.FirstName.ToLower(this.cultureInfo)].Remove(recordToRemove);
                this.lastNameDictionary[recordToRemove.LastName.ToLower(this.cultureInfo)].Remove(recordToRemove);
                this.dateOfBirthDictionary[recordToRemove.DateOfBirth].Remove(recordToRemove);

                return true;
            }
        }

        /// <summary>
        /// Searches for a record with given Id.
        /// </summary>
        /// <param name="id">The Id of the record to search.</param>
        /// <returns>Index of the record in list, or -1 if record with that Id was not found.</returns>
        public int RecordIndex(int id)
        {
            for (int i = 0; i < this.list.Count; i++)
            {
                if (this.list[i].Id == id)
                {
                    int recordIndex = i;
                    return recordIndex;
                }
            }

            return -1;
        }

        /// <summary>
        /// Searches for all records with given first name (Ignoring lower case and upper case differences).
        /// </summary>
        /// <param name="firstName">The first name to search for.</param>
        /// <returns>Array of records with given first name or an empty array if there are no records with that first name.</returns>
        public IEnumerable<FileCabinetRecord> FindByFirstName(string firstName)
        {
            ReadOnlyCollection<FileCabinetRecord> fileCabinetRecordsByFirstName = new ReadOnlyCollection<FileCabinetRecord>(new List<FileCabinetRecord>());
            if (this.firstNameDictionary.ContainsKey(firstName))
            {
                fileCabinetRecordsByFirstName = new (this.firstNameDictionary[firstName].ToArray());
            }

            return fileCabinetRecordsByFirstName;
        }

        /// <summary>
        /// Searches for all records with given last name (Ignoring lower case and upper case differences).
        /// </summary>
        /// <param name="lastName">The last name to search for.</param>
        /// <returns>Array of records with given last name or an empty array if there are no records with that last name.</returns>
        public IEnumerable<FileCabinetRecord> FindByLastName(string lastName)
        {
            ReadOnlyCollection<FileCabinetRecord> fileCabinetRecordsByLastName = new ReadOnlyCollection<FileCabinetRecord>(new List<FileCabinetRecord>());
            if (this.lastNameDictionary.ContainsKey(lastName))
            {
                fileCabinetRecordsByLastName = new (this.lastNameDictionary[lastName].ToArray());
            }

            return fileCabinetRecordsByLastName;
        }

        /// <summary>
        /// Searches for all records with given date of birth.
        /// </summary>
        /// <param name="dateOfBirth">The date of birth to search for.</param>
        /// <returns>Array of records with given date of birth or an empty array if there are no records with that date of birth.</returns>
        public IEnumerable<FileCabinetRecord> FindByDateOfBirth(DateTime dateOfBirth)
        {
            ReadOnlyCollection<FileCabinetRecord> fileCabinetRecordsByDateOfBirth = new ReadOnlyCollection<FileCabinetRecord>(new List<FileCabinetRecord>());
            if (this.dateOfBirthDictionary.ContainsKey(dateOfBirth))
            {
                fileCabinetRecordsByDateOfBirth = new (this.dateOfBirthDictionary[dateOfBirth].ToArray());
            }

            return fileCabinetRecordsByDateOfBirth;
        }

        /// <summary>
        /// Gets the all records.
        /// </summary>
        /// <returns>Array of all records.</returns>
        public List<FileCabinetRecord> GetRecords()
        {
            List<FileCabinetRecord> records = new List<FileCabinetRecord>(this.list);
            return records;
        }

        /// <summary>
        /// Gets the number of all records.
        /// </summary>
        /// <returns>Number of all records.</returns>
        public int GetStat(out int removedCount)
        {
            removedCount = 0;
            return this.list.Count;
        }

        /// <summary>
        /// Makes a snapshot of current records list.
        /// </summary>
        /// <returns>Snapshot of current records list.</returns>
        public FileCabinetServiceSnapshot MakeSnapshot()
        {
            return new FileCabinetServiceSnapshot(this.list);
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

                bool toRewrite = false;
                foreach (var oldRecord in this.list)
                {
                    if (oldRecord.Id == snapshotRecord.Id)
                    {
                        toRewrite = true;
                        break;
                    }
                }

                if (toRewrite)
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
            throw new NotImplementedException("Purge method is unavailable im memory service.");
        }
    }
}
