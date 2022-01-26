namespace FileCabinetApp
{
    /// <summary>
    /// Represents standart operations with list of records.
    /// </summary>
    public abstract class FileCabinetService
    {
        private readonly List<FileCabinetRecord> list = new List<FileCabinetRecord>();
        private readonly Dictionary<string, List<FileCabinetRecord>> firstNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private readonly Dictionary<string, List<FileCabinetRecord>> lastNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private readonly Dictionary<string, List<FileCabinetRecord>> dateOfBirthDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private readonly System.Globalization.CultureInfo cultureInfo = new System.Globalization.CultureInfo("en-US");

        /// <summary>
        /// Creates new record with given parameters.
        /// </summary>
        /// <param name="newRecord">Object with all record parameters.</param>
        /// <returns>Id of the created record or -1 in case of error.</returns>
#pragma warning disable CA1062 // newRecord проверяется на null в ValidateParameters.
#pragma warning disable CS8602 // Разыменование вероятной пустой ссылки (проверяется в ValidateParameters).
        public int CreateRecord(Record newRecord)
        {
            try
            {
                this.ValidateParameters(newRecord);
                var record = new FileCabinetRecord
                {
                    Id = this.list.Count + 1,
                    FirstName = newRecord.FirstName,
                    LastName = newRecord.LastName,
                    DateOfBirth = newRecord.DateOfBirth,
                    Height = newRecord.Height,
                    Weigth = newRecord.Weigth,
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

                if (this.dateOfBirthDictionary.ContainsKey(newRecord.DateOfBirth.ToString(this.cultureInfo)))
                {
                    this.dateOfBirthDictionary[newRecord.DateOfBirth.ToString(this.cultureInfo)].Add(record);
                }
                else
                {
                    this.dateOfBirthDictionary.Add(newRecord.DateOfBirth.ToString(this.cultureInfo), new List<FileCabinetRecord>());
                    this.dateOfBirthDictionary[newRecord.DateOfBirth.ToString(this.cultureInfo)].Add(record);
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
        public void EditRecord(int id, Record newRecord)
        {
            try
            {
                int indexOfRecord = this.RecordIndex(id);
                if (indexOfRecord == -1)
                {
                    throw new ArgumentException("Wrong id", $"{nameof(id)}");
                }

                this.ValidateParameters(newRecord);
                var oldRecord = this.list[indexOfRecord];
                var record = new FileCabinetRecord
                {
                    Id = id,
                    FirstName = newRecord.FirstName,
                    LastName = newRecord.LastName,
                    DateOfBirth = newRecord.DateOfBirth,
                    Height = newRecord.Height,
                    Weigth = newRecord.Weigth,
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

                this.dateOfBirthDictionary[oldRecord.DateOfBirth.ToString(this.cultureInfo)].Remove(oldRecord);
                if (this.dateOfBirthDictionary.ContainsKey(newRecord.DateOfBirth.ToString(this.cultureInfo)))
                {
                    this.dateOfBirthDictionary[newRecord.DateOfBirth.ToString(this.cultureInfo)].Add(record);
                }
                else
                {
                    this.dateOfBirthDictionary.Add(newRecord.DateOfBirth.ToString(this.cultureInfo), new List<FileCabinetRecord>());
                    this.dateOfBirthDictionary[newRecord.DateOfBirth.ToString(this.cultureInfo)].Add(record);
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
        public FileCabinetRecord[] FindByFirstName(string firstName)
        {
            if (this.firstNameDictionary.ContainsKey(firstName))
            {
                FileCabinetRecord[] fileCabinetRecordsByFirstName = this.firstNameDictionary[firstName].ToArray();
                return fileCabinetRecordsByFirstName;
            }

            return Array.Empty<FileCabinetRecord>();
        }

        /// <summary>
        /// Searches for all records with given last name (Ignoring lower case and upper case differences).
        /// </summary>
        /// <param name="lastName">The last name to search for.</param>
        /// <returns>Array of records with given last name or an empty array if there are no records with that last name.</returns>
        public FileCabinetRecord[] FindByLastName(string lastName)
        {
            if (this.lastNameDictionary.ContainsKey(lastName))
            {
                FileCabinetRecord[] fileCabinetRecordsByLastName = this.lastNameDictionary[lastName].ToArray();
                return fileCabinetRecordsByLastName;
            }

            return Array.Empty<FileCabinetRecord>();
        }

        /// <summary>
        /// Searches for all records with given date of birth.
        /// </summary>
        /// <param name="dateOfBirth">The date of birth to search for.</param>
        /// <returns>Array of records with given date of birth or an empty array if there are no records with that date of birth.</returns>
        public FileCabinetRecord[] FindByDateOfBirth(DateTime dateOfBirth)
        {
            if (this.dateOfBirthDictionary.ContainsKey(dateOfBirth.ToString(this.cultureInfo)))
            {
                FileCabinetRecord[] fileCabinetRecordsByDateofBirth = this.dateOfBirthDictionary[dateOfBirth.ToString(this.cultureInfo)].ToArray();
                return fileCabinetRecordsByDateofBirth;
            }

            return Array.Empty<FileCabinetRecord>();
        }

        /// <summary>
        /// Gets the all records.
        /// </summary>
        /// <returns>Array of all records.</returns>
        public FileCabinetRecord[] GetRecords()
        {
            return this.list.ToArray();
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
        /// Validating parameters.
        /// </summary>
        /// <param name="parameters">Parameters to validate.</param>
        protected abstract void ValidateParameters(Record parameters);
    }
}
