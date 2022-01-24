namespace FileCabinetApp
{
    public class FileCabinetService
    {
        private readonly List<FileCabinetRecord> list = new List<FileCabinetRecord>();
        private readonly Dictionary<string, List<FileCabinetRecord>> firstNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private readonly System.Globalization.CultureInfo cultureInfo = new System.Globalization.CultureInfo("en-US");

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

            DateTime min = new DateTime(1950, 1, 1);
            if (DateTime.Compare(dateOfBirth, min) < 0 || DateTime.Compare(dateOfBirth, DateTime.Now) > 0)
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

            return record.Id;
        }

        public void EditRecord(int id, string firstName, string lastName, DateTime dateOfBirth, short height, decimal weight, char temperament)
        {
            int index = this.IsExistRecord(id);

            if (index == -1)
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

            DateTime min = new DateTime(1950, 1, 1);
            if (DateTime.Compare(dateOfBirth, min) < 0 || DateTime.Compare(dateOfBirth, DateTime.Now) > 0)
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

            var oldRecord = this.list[index];
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

            this.list[index] = record;
            this.firstNameDictionary[oldRecord.FirstName.ToLower(this.cultureInfo)].Remove(oldRecord);
            if (this.firstNameDictionary.ContainsKey(firstName.ToLower(this.cultureInfo)))
            {
                this.firstNameDictionary[firstName.ToLower(this.cultureInfo)].Add(record);
            }
            else
            {
                this.firstNameDictionary.Add(firstName.ToLower(this.cultureInfo), new List<FileCabinetRecord>());
                this.firstNameDictionary[firstName.ToLower(this.cultureInfo)].Add(record);
            }
        }

        public int IsExistRecord(int id)
        {
            for (int i = 0; i < this.list.Count; i++)
            {
                if (this.list[i].Id == id)
                {
                    return i;
                }
            }

            return -1;
        }

        public FileCabinetRecord[] FindByFirstName(string firstName)
        {
            if (this.firstNameDictionary.ContainsKey(firstName))
            {
                return this.firstNameDictionary[firstName].ToArray();
            }

            return Array.Empty<FileCabinetRecord>();
        }

        public FileCabinetRecord[] FindByLastName(string lastName)
        {
            var result = new List<FileCabinetRecord>();
            for (int i = 0; i < this.list.Count; i++)
            {
                if (this.list[i].LastName.ToLower(new System.Globalization.CultureInfo("en-US")) == lastName)
                {
                    result.Add(this.list[i]);
                }
            }

            return result.ToArray();
        }

        public FileCabinetRecord[] FindByDateOfBirth(DateTime dateOfBirth)
        {
            var result = new List<FileCabinetRecord>();
            for (int i = 0; i < this.list.Count; i++)
            {
                if (this.list[i].DateOfBirth.CompareTo(dateOfBirth) == 0)
                {
                    result.Add(this.list[i]);
                }
            }

            return result.ToArray();
        }

        public FileCabinetRecord[] GetRecords()
        {
            return this.list.ToArray();
        }

        public int GetStat()
        {
            return this.list.Count;
        }
    }
}
