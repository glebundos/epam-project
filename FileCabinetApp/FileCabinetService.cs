namespace FileCabinetApp
{
    /// <summary>
    /// Represents standart operations with list of records.
    /// </summary>
    public class FileCabinetService
    {
        private readonly List<FileCabinetRecord> list = new List<FileCabinetRecord>();
        private readonly Dictionary<string, List<FileCabinetRecord>> firstNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private readonly Dictionary<string, List<FileCabinetRecord>> lastNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private readonly Dictionary<string, List<FileCabinetRecord>> dateOfBirthDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private readonly System.Globalization.CultureInfo cultureInfo = new System.Globalization.CultureInfo("en-US");

        /// <summary>
        /// Creates new record with given parameters.
        /// </summary>
        /// <param name="firstName">The first name.</param>
        /// <param name="lastName">The last name.</param>
        /// <param name="dateOfBirth">Date of birth.</param>
        /// <param name="height">Height in cm.</param>
        /// <param name="weight">Weight in kg.</param>
        /// <param name="temperament">The first letter of temperament.</param>
        /// <returns>Id of the created record.</returns>
        public int CreateRecord(string firstName, string lastName, DateTime dateOfBirth, short height, decimal weight, char temperament)
        {
            if (string.IsNullOrWhiteSpace(firstName))
            {
                throw new ArgumentNullException($"{nameof(firstName)}", "Parameter is null");
            }

            if (firstName.Length < 2 || firstName.Length > 60)
            {
                throw new ArgumentException("Parameter has wrong length", $"{nameof(firstName)}");
            }

            if (string.IsNullOrWhiteSpace(lastName))
            {
                throw new ArgumentNullException($"{nameof(lastName)}", "Parameter is null");
            }

            if (lastName.Length < 2 || lastName.Length > 60)
            {
                throw new ArgumentException("Parameter has wrong length", $"{nameof(lastName)}");
            }

            if (DateTime.Compare(dateOfBirth, new DateTime(1950, 1, 1)) < 0 || DateTime.Compare(dateOfBirth, DateTime.Now) > 0)
            {
                throw new ArgumentException("Parameter is wrong", $"{nameof(dateOfBirth)}");
            }

            if (height < 45 || height > 252)
            {
                throw new ArgumentException("Parameter is wrong", $"{nameof(height)}");
            }

            if (weight < 2 || weight > 600)
            {
                throw new ArgumentException("Parameter is wrong", $"{nameof(weight)}");
            }

            temperament = char.ToUpper(temperament, new System.Globalization.CultureInfo("en-US"));
            if (!(temperament == 'P' || temperament == 'S' || temperament == 'C' || temperament == 'M'))
            {
                throw new ArgumentException("Parameter is wrong", $"{nameof(temperament)}");
            }

            var record = new FileCabinetRecord
            {
                Id = this.list.Count + 1,
                FirstName = firstName,
                LastName = lastName,
                DateOfBirth = dateOfBirth,
                Height = height,
                Weigth = weight,
                Temperament = temperament,
            };

            this.list.Add(record);
            if (this.firstNameDictionary.ContainsKey(firstName.ToLower(this.cultureInfo)))
            {
                this.firstNameDictionary[firstName.ToLower(this.cultureInfo)].Add(record);
            }
            else
            {
                this.firstNameDictionary.Add(firstName.ToLower(this.cultureInfo), new List<FileCabinetRecord>());
                this.firstNameDictionary[firstName.ToLower(this.cultureInfo)].Add(record);
            }

            if (this.lastNameDictionary.ContainsKey(lastName.ToLower(this.cultureInfo)))
            {
                this.lastNameDictionary[lastName.ToLower(this.cultureInfo)].Add(record);
            }
            else
            {
                this.lastNameDictionary.Add(lastName.ToLower(this.cultureInfo), new List<FileCabinetRecord>());
                this.lastNameDictionary[lastName.ToLower(this.cultureInfo)].Add(record);
            }

            if (this.dateOfBirthDictionary.ContainsKey(dateOfBirth.ToString(this.cultureInfo)))
            {
                this.dateOfBirthDictionary[dateOfBirth.ToString(this.cultureInfo)].Add(record);
            }
            else
            {
                this.dateOfBirthDictionary.Add(dateOfBirth.ToString(this.cultureInfo), new List<FileCabinetRecord>());
                this.dateOfBirthDictionary[dateOfBirth.ToString(this.cultureInfo)].Add(record);
            }

            return record.Id;
        }

        /// <summary>
        /// Edits the record with the given id according to the given parameters.
        /// </summary>
        /// <param name="id">The Id of the record to change.</param>
        /// <param name="firstName">The first name.</param>
        /// <param name="lastName">The last name.</param>
        /// <param name="dateOfBirth">Date of birth.</param>
        /// <param name="height">Height in cm.</param>
        /// <param name="weight">Weight in kg.</param>
        /// <param name="temperament">The first letter of temperament.</param>
        public void EditRecord(int id, string firstName, string lastName, DateTime dateOfBirth, short height, decimal weight, char temperament)
        {
            int indexOfRecord = this.RecordIndex(id);

            if (indexOfRecord == -1)
            {
                throw new ArgumentException("Wrong id", $"{nameof(id)}");
            }

            if (string.IsNullOrWhiteSpace(firstName))
            {
                throw new ArgumentNullException($"{nameof(firstName)}", "Parameter is null");
            }

            if (firstName.Length < 2 || firstName.Length > 60)
            {
                throw new ArgumentException("Parameter has wrong length", $"{nameof(firstName)}");
            }

            if (string.IsNullOrWhiteSpace(lastName))
            {
                throw new ArgumentNullException($"{nameof(lastName)}", "Parameter is null");
            }

            if (lastName.Length < 2 || lastName.Length > 60)
            {
                throw new ArgumentException("Parameter has wrong length", $"{nameof(lastName)}");
            }

            if (DateTime.Compare(dateOfBirth, new DateTime(1950, 1, 1)) < 0 || DateTime.Compare(dateOfBirth, DateTime.Now) > 0)
            {
                throw new ArgumentException("Parameter is wrong", $"{nameof(dateOfBirth)}");
            }

            if (height < 45 || height > 252)
            {
                throw new ArgumentException("Parameter is wrong", $"{nameof(height)}");
            }

            if (weight < 2 || weight > 600)
            {
                throw new ArgumentException("Parameter is wrong", $"{nameof(weight)}");
            }

            temperament = char.ToUpper(temperament, new System.Globalization.CultureInfo("en-US"));
            if (!(temperament == 'P' || temperament == 'S' || temperament == 'C' || temperament == 'M'))
            {
                throw new ArgumentException("Parameter is wrong", $"{nameof(temperament)}");
            }

            var oldRecord = this.list[indexOfRecord];
            var record = new FileCabinetRecord
            {
                Id = id,
                FirstName = firstName,
                LastName = lastName,
                DateOfBirth = dateOfBirth,
                Height = height,
                Weigth = weight,
                Temperament = temperament,
            };

            this.list[indexOfRecord] = record;
            string firstNameKey = string.Empty;
            if (oldRecord.FirstName != null)
            {
                firstNameKey = oldRecord.FirstName.ToLower(this.cultureInfo);
            }

            this.firstNameDictionary[firstNameKey].Remove(oldRecord);
            if (this.firstNameDictionary.ContainsKey(firstName.ToLower(this.cultureInfo)))
            {
                this.firstNameDictionary[firstName.ToLower(this.cultureInfo)].Add(record);
            }
            else
            {
                this.firstNameDictionary.Add(firstName.ToLower(this.cultureInfo), new List<FileCabinetRecord>());
                this.firstNameDictionary[firstName.ToLower(this.cultureInfo)].Add(record);
            }

            string lastNameKey = string.Empty;
            if (oldRecord.LastName != null)
            {
                lastNameKey = oldRecord.LastName.ToLower(this.cultureInfo);
            }

            this.lastNameDictionary[lastNameKey].Remove(oldRecord);
            if (this.lastNameDictionary.ContainsKey(lastName.ToLower(this.cultureInfo)))
            {
                this.lastNameDictionary[lastName.ToLower(this.cultureInfo)].Add(record);
            }
            else
            {
                this.lastNameDictionary.Add(lastName.ToLower(this.cultureInfo), new List<FileCabinetRecord>());
                this.lastNameDictionary[lastName.ToLower(this.cultureInfo)].Add(record);
            }

            this.dateOfBirthDictionary[oldRecord.DateOfBirth.ToString(this.cultureInfo)].Remove(oldRecord);
            if (this.dateOfBirthDictionary.ContainsKey(dateOfBirth.ToString(this.cultureInfo)))
            {
                this.dateOfBirthDictionary[dateOfBirth.ToString(this.cultureInfo)].Add(record);
            }
            else
            {
                this.dateOfBirthDictionary.Add(dateOfBirth.ToString(this.cultureInfo), new List<FileCabinetRecord>());
                this.dateOfBirthDictionary[dateOfBirth.ToString(this.cultureInfo)].Add(record);
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
    }
}
