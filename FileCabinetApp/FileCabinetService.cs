using System.Collections.ObjectModel;

namespace FileCabinetApp
{
    /// <summary>
    /// Represents standart operations with list of records in memory.
    /// </summary>
    public class FileCabinetService : IFileCabinetService
    {
        private readonly List<FileCabinetRecord> list = new List<FileCabinetRecord>();
        private readonly IRecordValidator validator;
        private readonly Dictionary<string, List<FileCabinetRecord>> firstNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private readonly Dictionary<string, List<FileCabinetRecord>> lastNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private readonly Dictionary<DateTime, List<FileCabinetRecord>> dateOfBirthDictionary = new Dictionary<DateTime, List<FileCabinetRecord>>();
        private readonly System.Globalization.CultureInfo cultureInfo = new System.Globalization.CultureInfo("en-US");

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetService"/> class.
        /// </summary>
        /// <param name="validator">Validator.</param>
        public FileCabinetService(IRecordValidator validator)
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

                var record = new FileCabinetRecord
                {
                    Id = this.list.Count + 1,
                    FirstName = newRecord.FirstName,
                    LastName = newRecord.LastName,
                    DateOfBirth = newRecord.DateOfBirth,
                    Height = newRecord.Height,
                    Weight = newRecord.Weight,
                    Temperament = char.ToUpper(newRecord.Temperament, this.cultureInfo),
                };

                this.list.Add(record);

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
        public ReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName)
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
        public ReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastName)
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
        public ReadOnlyCollection<FileCabinetRecord> FindByDateOfBirth(DateTime dateOfBirth)
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
        public ReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            ReadOnlyCollection<FileCabinetRecord> records = new ReadOnlyCollection<FileCabinetRecord>(this.list);
            return records;
        }

        /// <summary>
        /// Gets the number of all records.
        /// </summary>
        /// <returns>Number of all records.</returns>
        public int GetStat()
        {
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
    }
}
