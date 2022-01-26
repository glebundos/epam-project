namespace FileCabinetApp
{
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
        private static FileCabinetService fileCabinetService = new FileCabinetService(new DefaultValidator());

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
        };

        /// <summary>
        /// Еhe main method from which the program execution starts.
        /// </summary>
        /// <param name="args">Initial program arguments.</param>
        public static void Main(string[] args)
        {
            Console.WriteLine($"File Cabinet Application, developed by {Program.DeveloperName}");
            if (args == null || args.Length < 1)
            {
                Console.WriteLine("Using default validation rules.");
            }
            else
            {
                if (args.Length == 2)
                {
                    if (args[1].ToLower(new System.Globalization.CultureInfo("en-US")) == "custom")
                    {
                        fileCabinetService = new FileCabinetService(new CustomValidator());
                        Console.WriteLine("Using custom validation rules.");
                    }
                    else
                    {
                        Console.WriteLine("Using default validation rules.");
                    }
                }
                else
                {
                    args = args[0].Split('=');
                    if (args[1].ToLower(new System.Globalization.CultureInfo("en-US")) == "custom")
                    {
                        fileCabinetService = new FileCabinetService(new CustomValidator());
                        Console.WriteLine("Using custom validation rules.");
                    }
                    else
                    {
                        Console.WriteLine("Using default validation rules.");
                    }
                }
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

                var index = Array.FindIndex(commands, 0, commands.Length, i => i.Item1.Equals(command, StringComparison.InvariantCultureIgnoreCase));
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

        private static void PrintMissedCommandInfo(string command)
        {
            Console.WriteLine($"There is no '{command}' command.");
            Console.WriteLine();
        }

        private static void PrintHelp(string parameters)
        {
            if (!string.IsNullOrEmpty(parameters))
            {
                var index = Array.FindIndex(helpMessages, 0, helpMessages.Length, i => string.Equals(i[Program.CommandHelpIndex], parameters, StringComparison.InvariantCultureIgnoreCase));
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
            try
            {
                Console.WriteLine("First name: ");
                string? firstName = string.Empty;
                firstName = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(firstName))
                {
                    throw new ArgumentNullException(nameof(firstName), "Parameter is null");
                }

                if (firstName.Length < 2 || firstName.Length > 60)
                {
                    throw new ArgumentException($"Firstname length must be between 2 and 60", nameof(firstName));
                }

                Console.WriteLine("Last name: ");
                string? lastName = string.Empty;
                lastName = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(lastName))
                {
                    throw new ArgumentNullException(nameof(lastName), "Parameter is null");
                }

                if (lastName.Length < 2 || lastName.Length > 60)
                {
                    throw new ArgumentException($"Lastname length must be between 2 and 60", nameof(lastName));
                }

                Console.WriteLine("Date of birth: ");
                string? dateString = string.Empty;
                dateString = Console.ReadLine();
                DateTime dateOfBirth = DateTime.Now;
                bool parsedSuccessfully = DateTime.TryParse(dateString, new System.Globalization.CultureInfo("en-US"), 0, out dateOfBirth);
                DateTime min = new DateTime(1950, 1, 1);
                if (DateTime.Compare(dateOfBirth, min) < 0 || DateTime.Compare(dateOfBirth, DateTime.Now) > 0 || !parsedSuccessfully)
                {
                    throw new ArgumentException($"Date of birth must be between 01/01/1950 and today({DateTime.Now})", nameof(dateOfBirth));
                }

                parsedSuccessfully = true;
                Console.WriteLine("Height: ");
                short height = 0;
                parsedSuccessfully = short.TryParse(Console.ReadLine(), out height);
                if (height < 45 || height > 252 || !parsedSuccessfully)
                {
                    throw new ArgumentException("Height must be between 45 and 252", nameof(height));
                }

                parsedSuccessfully = true;
                Console.WriteLine("Weight: ");
                decimal weight = 0;
                parsedSuccessfully = decimal.TryParse(Console.ReadLine(), out weight);
                if (weight < 2 || weight > 600 || !parsedSuccessfully)
                {
                    throw new ArgumentException("Height must be between 2 and 600", nameof(weight));
                }

                parsedSuccessfully = true;
                Console.WriteLine("Temperament: ");
                char temperament = ' ';
                parsedSuccessfully = char.TryParse(Console.ReadLine(), out temperament);
                if (!parsedSuccessfully)
                {
                    throw new ArgumentException("Parameter is wrong", nameof(temperament));
                }

                temperament = char.ToUpper(temperament, new System.Globalization.CultureInfo("en-US"));
                if (!(temperament == 'P' || temperament == 'S' || temperament == 'C' || temperament == 'M'))
                {
                    throw new ArgumentException("Temperament must be P, S, C, or M", nameof(temperament));
                }

                Record newRecord = new Record(firstName, lastName, dateOfBirth, height, weight, temperament);
                Program.fileCabinetService.CreateRecord(newRecord);
            }
            catch (Exception e)
            {
                Console.WriteLine("Некорректный ввод: " + e.Message);
                Create(parameters);
            }
        }

        private static void List(string parameters)
        {
            FileCabinetRecord[] records = fileCabinetService.GetRecords();
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
                try
                {
                    Console.WriteLine("First name: ");
                    string? firstName = string.Empty;
                    firstName = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(firstName))
                    {
                        throw new ArgumentNullException(nameof(firstName), "Parameter is null");
                    }

                    if (firstName.Length < 2 || firstName.Length > 60)
                    {
                        throw new ArgumentException($"Firstname length must be between 2 and 60", nameof(firstName));
                    }

                    Console.WriteLine("Last name: ");
                    string? lastName = string.Empty;
                    lastName = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(lastName))
                    {
                        throw new ArgumentNullException(nameof(lastName), "Parameter is null");
                    }

                    if (lastName.Length < 2 || lastName.Length > 60)
                    {
                        throw new ArgumentException($"Lastname length must be between 2 and 60", nameof(lastName));
                    }

                    Console.WriteLine("Date of birth: ");
                    string? dateString = string.Empty;
                    dateString = Console.ReadLine();
                    DateTime dateOfBirth = DateTime.Now;
                    bool parsedSuccessfully = DateTime.TryParse(dateString, new System.Globalization.CultureInfo("en-US"), 0, out dateOfBirth);
                    DateTime min = new DateTime(1950, 1, 1);
                    if (DateTime.Compare(dateOfBirth, min) < 0 || DateTime.Compare(dateOfBirth, DateTime.Now) > 0 || !parsedSuccessfully)
                    {
                        throw new ArgumentException($"Date of birth must be between 01/01/1950 and today({DateTime.Now})", nameof(dateOfBirth));
                    }

                    parsedSuccessfully = true;
                    Console.WriteLine("Height: ");
                    short height = 0;
                    parsedSuccessfully = short.TryParse(Console.ReadLine(), out height);
                    if (height < 45 || height > 252 || !parsedSuccessfully)
                    {
                        throw new ArgumentException("Height must be between 45 and 252", nameof(height));
                    }

                    parsedSuccessfully = true;
                    Console.WriteLine("Weight: ");
                    decimal weight = 0;
                    parsedSuccessfully = decimal.TryParse(Console.ReadLine(), out weight);
                    if (weight < 2 || weight > 600 || !parsedSuccessfully)
                    {
                        throw new ArgumentException("Height must be between 2 and 600", nameof(weight));
                    }

                    parsedSuccessfully = true;
                    Console.WriteLine("Temperament: ");
                    char temperament = ' ';
                    parsedSuccessfully = char.TryParse(Console.ReadLine(), out temperament);
                    if (!parsedSuccessfully)
                    {
                        throw new ArgumentException("Parameter is wrong", nameof(temperament));
                    }

                    temperament = char.ToUpper(temperament, new System.Globalization.CultureInfo("en-US"));
                    if (!(temperament == 'P' || temperament == 'S' || temperament == 'C' || temperament == 'M'))
                    {
                        throw new ArgumentException("Temperament must be P, S, C, or M", nameof(temperament));
                    }

                    Record newRecord = new Record(firstName, lastName, dateOfBirth, height, weight, temperament);
                    Program.fileCabinetService.EditRecord(id, newRecord);
                    Console.WriteLine($"Record #{id} is updated");
                }
                catch (Exception e)
                {
                    Console.WriteLine("Некорректный ввод: " + e.Message);
                }
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
                var records = Array.Empty<FileCabinetRecord>();

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
                    $"{record.Height} cm, {record.Weigth} kg, {temperament}");
        }
    }
}