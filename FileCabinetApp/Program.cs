using System.Collections.ObjectModel;

namespace FileCabinetApp
{
#pragma warning disable CS8618 // Поле, не допускающее значения NULL, должно содержать значение, отличное от NULL, при выходе из конструктора. Возможно, стоит объявить поле как допускающее значения NULL.
#pragma warning disable CA2208 // Правильно создавайте экземпляры исключений аргументов
    /// <summary>
    /// Main class of the program.
    /// </summary>
    public static class Program
    {
        private const string DeveloperName = "Gleb Kontovsky";
        private const string HintMessage = "Enter your command, or enter 'help' to get help.";
        private const int CommandHelpIndex = 0;
        private const int DescriptionHelpIndex = 1;
        private const int ExplanationHelpIndex = 2;
        private static ValidatorsSettings validatorsSettings = new ValidatorsSettings();
        private static IFileCabinetService fileCabinetService;

        private static bool isRunning = true;

        private static Tuple<string, Action<string>>[] commands = new Tuple<string, Action<string>>[]
        {
            new Tuple<string, Action<string>>("help", PrintHelp),
            new Tuple<string, Action<string>>("exit", Exit),
            new Tuple<string, Action<string>>("stat", Stat),
            new Tuple<string, Action<string>>("create", Create),
            new Tuple<string, Action<string>>("list", List),
            new Tuple<string, Action<string>>("edit", Edit),
            new Tuple<string, Action<string>>("find", Find),
            new Tuple<string, Action<string>>("export", Export),
            new Tuple<string, Action<string>>("import", Import),
        };

        private static string[][] helpMessages = new string[][]
        {
            new string[] { "help", "prints the help screen", "The 'help' command prints the help screen." },
            new string[] { "exit", "exits the application", "The 'exit' command exits the application." },
            new string[] { "stat", "prints the statistics", "The 'stat' command prints the statistics." },
            new string[] { "create", "creates a new record", "The 'create' command creates a new record." },
            new string[] { "list", "shows a list of all records", "The 'list' command shows a list of all records." },
            new string[] { "edit <id>", "edits a record", "The 'edit' edits a record with a specific id." },
            new string[] { "find <parameter> \"value\"", "finds a record", "The 'find' finds all records with a specific parameter value." },
            new string[] { "export <file extension> \"path\"", "exports records", "The 'export' exports records into the file(path)." },
            new string[] { "import <file extension> path", "imports records", "The 'import' imports records from the file(path)." },
        };

        /// <summary>
        /// Еhe main method from which the program execution starts.
        /// </summary>
        /// <param name="args">Initial program arguments.</param>
        public static void Main(string[] args)
        {
            Console.WriteLine($"File Cabinet Application, developed by {Program.DeveloperName}");
            if (args is not null)
            {
                Dictionary<string, string> configuration = new Dictionary<string, string>();
                for (int i = 0; i < args.Length; i++)
                {
                    string key = args[i].ToLower(new System.Globalization.CultureInfo("en-US"));
                    string value = string.Empty;
                    if (key.Contains('=', StringComparison.InvariantCultureIgnoreCase))
                    {
                        value = key.Split('=')[1];
                        key = key.Split('=')[0];
                    }
                    else
                    {
                        if (++i < args.Length)
                        {
                            value = args[i].ToLower(new System.Globalization.CultureInfo("en-US"));
                        }
                        else
                        {
                            value = "default";
                        }
                    }

                    configuration.Add(key, value);
                }

                Configure(configuration);
            }

            Console.WriteLine(Program.HintMessage);
            Console.WriteLine();

            do
            {
                Console.Write("> ");
                var line = Console.ReadLine();
                var inputs = line != null ? line.Split(' ', 2) : new string[] { string.Empty, string.Empty };
                const int commandIndex = 0;
                var command = inputs[commandIndex];

                if (string.IsNullOrEmpty(command))
                {
                    Console.WriteLine(Program.HintMessage);
                    continue;
                }

                var index = Array.FindIndex(commands, 0, commands.Length, i => i.Item1.Equals(command, StringComparison.OrdinalIgnoreCase));
                if (index >= 0)
                {
                    const int parametersIndex = 1;
                    var parameters = inputs.Length > 1 ? inputs[parametersIndex] : string.Empty;
                    commands[index].Item2(parameters);
                }
                else
                {
                    PrintMissedCommandInfo(command);
                }
            }
            while (isRunning);
        }

        private static void Configure(Dictionary<string, string> configuration)
        {
            string? value = string.Empty;
            if (configuration.TryGetValue("-v", out value) || configuration.TryGetValue("--validation-rules", out value))
            {
                if (value == "custom")
                {
                    validatorsSettings.SetCustomConfig();
                    Console.WriteLine("Using custom validation rules");
                }
                else
                {
                    validatorsSettings.SetDefaultConfig();
                    Console.WriteLine("Using default validation rules");
                }
            }
            else
            {
                validatorsSettings.SetDefaultConfig();
                Console.WriteLine("Using default validation rules");
            }

            IRecordValidator validator = ValidatorBuilder.CreateFullValidator(validatorsSettings);
            if (configuration.TryGetValue("-s", out value) || configuration.TryGetValue("--storage", out value))
            {
                if (value == "file")
                {
                    fileCabinetService = new FileCabinetFilesystemService(validator);
                    Console.WriteLine("Using filesystem service");
                }
                else
                {
                    fileCabinetService = new FileCabinetMemoryService(validator);
                    Console.WriteLine("Using memory service");
                }
            }
            else
            {
                fileCabinetService = new FileCabinetMemoryService(validator);
                Console.WriteLine("Using memory service");
            }
        }

        private static void PrintMissedCommandInfo(string command)
        {
            Console.WriteLine($"There is no '{command}' command.");
            Console.WriteLine();
        }

        private static void PrintHelp(string parameters)
        {
            if (!string.IsNullOrEmpty(parameters))
            {
                var index = Array.FindIndex(helpMessages, 0, helpMessages.Length, i => string.Equals(i[Program.CommandHelpIndex], parameters, StringComparison.OrdinalIgnoreCase));
                if (index >= 0)
                {
                    Console.WriteLine(helpMessages[index][Program.ExplanationHelpIndex]);
                }
                else
                {
                    Console.WriteLine($"There is no explanation for '{parameters}' command.");
                }
            }
            else
            {
                Console.WriteLine("Available commands:");

                foreach (var helpMessage in helpMessages)
                {
                    Console.WriteLine("\t{0}\t- {1}", helpMessage[Program.CommandHelpIndex], helpMessage[Program.DescriptionHelpIndex]);
                }
            }

            Console.WriteLine();
        }

        private static void Exit(string parameters)
        {
            Console.WriteLine("Exiting an application...");
            isRunning = false;
        }

        private static void Stat(string parameters)
        {
            var recordsCount = Program.fileCabinetService.GetStat();
            Console.WriteLine($"{recordsCount} record(s).");
        }

        private static void Create(string parameters)
        {
            Console.Write("First name: ");
            string firstName = (string)ReadInput(StringConverter, FirstNameValidator);

            Console.Write("Last name: ");
            string lastName = (string)ReadInput(StringConverter, LastNameValidator);

            Console.Write("Date of birth: ");
            DateTime dob = (DateTime)ReadInput(DateConverter, DateOfBirthValidator);

            Console.Write("Height: ");
            short height = (short)ReadInput(HeightConverter, HeightValidator);

            Console.Write("Weight: ");
            decimal weight = (decimal)ReadInput(WeightConverter, WeightValidator);

            Console.Write("Temperament: ");
            char temperament = (char)ReadInput(TemperamentConverter, TemperamentValidator);

            FileCabinetRecord newRecord = new FileCabinetRecord()
            {
                Id = 0,
                FirstName = firstName,
                LastName = lastName,
                DateOfBirth = dob,
                Height = height,
                Weight = weight,
                Temperament = temperament,
            };

            Program.fileCabinetService.CreateRecord(newRecord);
        }

        private static Tuple<bool, string> FirstNameValidator(object input)
        {
            FileCabinetRecord onlyFirstNameRecord = new FileCabinetRecord()
            {
                FirstName = input.ToString(),
                LastName = string.Empty,
                DateOfBirth = DateTime.Now,
                Height = 0,
                Weight = 0,
                Temperament = 'G',
            };
            FirstNameValidator firstNameValidator = new FirstNameValidator(validatorsSettings.FirstNameLenght_min, validatorsSettings.FirstNameLenght_max);
            return Tuple.Create(firstNameValidator.ValidateParameters(onlyFirstNameRecord), "Parameter is invalid");
        }

        private static Tuple<bool, string> LastNameValidator(object input)
        {
            FileCabinetRecord onlyLastNameRecord = new FileCabinetRecord()
            {
                FirstName = string.Empty,
                LastName = input.ToString(),
                DateOfBirth = DateTime.Now,
                Height = 0,
                Weight = 0,
                Temperament = 'G',
            };
            LastNameValidator lastNameValidator = new LastNameValidator(validatorsSettings.LastNameLenght_min, validatorsSettings.LastNameLenght_max);
            return Tuple.Create(lastNameValidator.ValidateParameters(onlyLastNameRecord), "Parameter is invalid");
        }

        private static Tuple<bool, string> DateOfBirthValidator(object input)
        {
            FileCabinetRecord onlyDateOfBirthRecord = new FileCabinetRecord()
            {
                FirstName = string.Empty,
                LastName = string.Empty,
                DateOfBirth = (DateTime)input,
                Height = 0,
                Weight = 0,
                Temperament = 'G',
            };
            DateOfBirthValidator dateOfBirthValidator = new DateOfBirthValidator(validatorsSettings.DateOfBitrth_min, validatorsSettings.DateOfBitrth_max);
            return Tuple.Create(dateOfBirthValidator.ValidateParameters(onlyDateOfBirthRecord), "Parameter is invalid");
        }

        private static Tuple<bool, string> HeightValidator(object input)
        {
            FileCabinetRecord onlyHeightRecord = new FileCabinetRecord()
            {
                FirstName = string.Empty,
                LastName = string.Empty,
                DateOfBirth = DateTime.Now,
                Height = (short)input,
                Weight = 0,
                Temperament = 'G',
            };
            HeightValidator heightValidator = new HeightValidator(validatorsSettings.Height_min, validatorsSettings.Height_max);
            return Tuple.Create(heightValidator.ValidateParameters(onlyHeightRecord), "Parameter is invalid");
        }

        private static Tuple<bool, string> WeightValidator(object input)
        {
            FileCabinetRecord onlyWeightRecord = new FileCabinetRecord()
            {
                FirstName = string.Empty,
                LastName = string.Empty,
                DateOfBirth = DateTime.Now,
                Height = 0,
                Weight = (decimal)input,
                Temperament = 'G',
            };
            WeightValidator weightValidator = new WeightValidator(validatorsSettings.Weight_min, validatorsSettings.Weight_max);
            return Tuple.Create(weightValidator.ValidateParameters(onlyWeightRecord), "Parameter is invalid");
        }

        private static Tuple<bool, string> TemperamentValidator(object input)
        {
            FileCabinetRecord onlyTemperamentValidator = new FileCabinetRecord()
            {
                FirstName = string.Empty,
                LastName = string.Empty,
                DateOfBirth = DateTime.Now,
                Height = 0,
                Weight = 0,
                Temperament = (char)input,
            };
            TemperamentValidator temperamentValidator = new TemperamentValidator(validatorsSettings.AllowedTemperaments);
            return Tuple.Create(temperamentValidator.ValidateParameters(onlyTemperamentValidator), "Parameter is invalid");
        }

        private static Tuple<bool, string, object> StringConverter(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return Tuple.Create<bool, string, object>(false, "Input was null", input);
            }

            return Tuple.Create<bool, string, object>(true, "Convertating error", input);
        }

        private static Tuple<bool, string, object> DateConverter(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return Tuple.Create<bool, string, object>(false, "Input was null", input);
            }

            DateTime dateOfBirth;
            DateTime.TryParse(input, new System.Globalization.CultureInfo("en-US"), 0, out dateOfBirth);

            return Tuple.Create<bool, string, object>(true, "Convertating error", dateOfBirth);
        }

        private static Tuple<bool, string, object> HeightConverter(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return Tuple.Create<bool, string, object>(false, "Input was null", input);
            }

            short height;
            bool isConvertered = short.TryParse(input, out height);
            return Tuple.Create<bool, string, object>(isConvertered, "Convertating error", height);
        }

        private static Tuple<bool, string, object> WeightConverter(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return Tuple.Create<bool, string, object>(false, "Input was null", input);
            }

            decimal weight;
            bool isConvertered = decimal.TryParse(input, out weight);
            return Tuple.Create<bool, string, object>(isConvertered, "Convertating error", weight);
        }

        private static Tuple<bool, string, object> TemperamentConverter(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return Tuple.Create<bool, string, object>(false, "Input was null", input);
            }

            char temperament;
            bool isConvertered = char.TryParse(input, out temperament);
            return Tuple.Create<bool, string, object>(isConvertered, "Convertating error", char.ToUpper(temperament, new System.Globalization.CultureInfo("en-US")));
        }

        private static void List(string parameters)
        {
            var records = fileCabinetService.GetRecords();
            foreach (var record in records)
            {
                WriteRecord(record);
            }
        }

        private static void Edit(string parameters)
        {
            if (string.IsNullOrEmpty(parameters))
            {
                Console.WriteLine("Parameter cant be null");
                return;
            }

            int id = Convert.ToInt32(parameters, new System.Globalization.CultureInfo("en-US"));
            if (fileCabinetService.RecordIndex(id) > -1)
            {
                Console.Write("First name: ");
                string firstName = (string)ReadInput(StringConverter, FirstNameValidator);

                Console.Write("Last name: ");
                string lastName = (string)ReadInput(StringConverter, LastNameValidator);

                Console.Write("Date of birth: ");
                DateTime dob = (DateTime)ReadInput(DateConverter, DateOfBirthValidator);

                Console.Write("Height: ");
                short height = (short)ReadInput(HeightConverter, HeightValidator);

                Console.Write("Weight: ");
                decimal weight = (decimal)ReadInput(WeightConverter, WeightValidator);

                Console.Write("Temperament: ");
                char temperament = (char)ReadInput(TemperamentConverter, TemperamentValidator);

                FileCabinetRecord newRecord = new FileCabinetRecord()
                {
                    Id = 0,
                    FirstName = firstName,
                    LastName = lastName,
                    DateOfBirth = dob,
                    Height = height,
                    Weight = weight,
                    Temperament = temperament,
                };
                Program.fileCabinetService.EditRecord(id, newRecord);
            }
            else
            {
                Console.WriteLine($"#{id} record is not found.");
            }
        }

        private static void Find(string parameters)
        {
            try
            {
                string parameter = parameters.Split()[0].ToLower(new System.Globalization.CultureInfo("en-US"));
                if (string.IsNullOrEmpty(parameter))
                {
                    throw new ArgumentException("Wrong parameter", parameter);
                }

                string value = parameters.Split()[1].ToLower(new System.Globalization.CultureInfo("en-US"))[1..^1];
                IReadOnlyCollection<FileCabinetRecord> records = new List<FileCabinetRecord>();

                if (parameter == "firstname")
                {
                    if (string.IsNullOrWhiteSpace(value))
                    {
                        throw new ArgumentNullException(nameof(value), "Value is null");
                    }

                    if (value.Length < 2 || value.Length > 60)
                    {
                        throw new ArgumentException("Value has wrong length", value);
                    }

                    records = fileCabinetService.FindByFirstName(value);
                }
                else if (parameter == "lastname")
                {
                    if (string.IsNullOrWhiteSpace(value))
                    {
                        throw new ArgumentNullException(nameof(value), "Value is null");
                    }

                    if (value.Length < 2 || value.Length > 60)
                    {
                        throw new ArgumentException("Value has wrong length", value);
                    }

                    records = fileCabinetService.FindByLastName(value);
                }
                else if (parameter == "dateofbirth")
                {
                    DateTime dateOfBirth;
                    bool isParsedSuccessfully = DateTime.TryParse(value, new System.Globalization.CultureInfo("en-US"), 0, out dateOfBirth);
                    if (DateTime.Compare(dateOfBirth, new DateTime(1950, 1, 1)) < 0 || DateTime.Compare(dateOfBirth, DateTime.Now) > 0 || !isParsedSuccessfully)
                    {
                        throw new ArgumentException("Parameter is wrong", nameof(dateOfBirth));
                    }

                    records = fileCabinetService.FindByDateOfBirth(dateOfBirth);
                }
                else
                {
                    throw new ArgumentException("Wrong parameter", parameter);
                }

                foreach (var record in records)
                {
                    WriteRecord(record);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Некорректный ввод: " + e.Message);
                Console.WriteLine("Используйте find <parameter name> \"value\"");
            }
        }

        private static void WriteRecord(FileCabinetRecord record)
        {
            string temperament = string.Empty;
            switch (record.Temperament)
            {
                case 'P':
                    temperament = "Phlegmatic";
                    break;
                case 'S':
                    temperament = "Sanguine";
                    break;
                case 'C':
                    temperament = "Choleric";
                    break;
                case 'M':
                    temperament = "Melancholic";
                    break;
                default:
                    temperament = "MISSING";
                    break;
            }

            Console.WriteLine($"#{record.Id}, {record.FirstName}, {record.LastName}, " +
                    $"{record.DateOfBirth.ToString("yyyy-MMM-d", new System.Globalization.CultureInfo("en-US"))}, " +
                    $"{record.Height} cm, {record.Weight} kg, {temperament}");
        }

        private static T ReadInput<T>(Func<string, Tuple<bool, string, T>> converter, Func<T, Tuple<bool, string>> validator)
        {
            do
            {
                T value;

                var input = Console.ReadLine();
                var conversionResult = converter(input);

                if (!conversionResult.Item1)
                {
                    Console.WriteLine($"Conversion failed: {conversionResult.Item2}. Please, correct your input.");
                    continue;
                }

                value = conversionResult.Item3;

                var validationResult = validator(value);
                if (!validationResult.Item1)
                {
                    Console.WriteLine($"Validation failed: {validationResult.Item2}. Please, correct your input.");
                    continue;
                }

                return value;
            }
            while (true);
        }

        private static void Export(string parameters)
        {
            try
            {
                string[] arguments = parameters.Split();
                bool append = false;
                if (arguments[0] == "csv")
                {
                    arguments[1] = arguments[1][1..^1];
                    if (File.Exists(arguments[1]))
                    {
                        Console.WriteLine($"File is exist - rewrite {arguments[1]}? [Y/n]");
                        if (Console.ReadKey().Key == ConsoleKey.N)
                        {
                            append = true;
                        }

                        Console.WriteLine();
                    }

                    StreamWriter streamWriter = new StreamWriter(arguments[1], append);
                    fileCabinetService.MakeSnapshot().SaveToCsv(streamWriter, append);
                }
                else if (arguments[0] == "xml")
                {
                    arguments[1] = arguments[1][1..^1];
                    if (File.Exists(arguments[1]))
                    {
                        Console.WriteLine($"File is exist - rewrite {arguments[1]}? [Y/n]");
                        if (Console.ReadKey().Key == ConsoleKey.N)
                        {
                            append = true;
                        }

                        Console.WriteLine();
                    }

                    StreamWriter streamWriter = new StreamWriter(arguments[1], append);
                    fileCabinetService.MakeSnapshot().SaveToXml(streamWriter, append);
                }
                else
                {
                    throw new ArgumentException("Wrong parameters.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Export failed: " + e.Message);
            }
        }

        private static void Import(string parameters)
        {
            try
            {
                string[] arguments = parameters.Split();
                if (arguments[0] == "csv")
                {
                    StreamReader streamReader = new StreamReader(arguments[1]);
                    FileCabinetServiceSnapshot snapshot = new FileCabinetServiceSnapshot(new List<FileCabinetRecord>());
                    snapshot.LoadFromCsv(streamReader);
                    fileCabinetService.Restore(snapshot);
                }
                /*else if (arguments[0] == "xml")
                {
                    StreamReader streamReader = new StreamReader(arguments[1]);
                    FileCabinetServiceSnapshot snapshot = new FileCabinetServiceSnapshot(new List<FileCabinetRecord>());
                    snapshot.LoadFromCsv(streamReader);
                    fileCabinetService.Restore(snapshot);
                }*/
                else
                {
                    throw new ArgumentException("Wrong parameters.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Import failed: " + e.Message);
            }
        }
    }
}
#pragma warning restore CA2208 // Правильно создавайте экземпляры исключений аргументов