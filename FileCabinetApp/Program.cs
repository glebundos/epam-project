namespace FileCabinetApp
{
    public static class Program
    {
        private const string DeveloperName = "Gleb Kontovsky";
        private const string HintMessage = "Enter your command, or enter 'help' to get help.";
        private const int CommandHelpIndex = 0;
        private const int DescriptionHelpIndex = 1;
        private const int ExplanationHelpIndex = 2;
        private static FileCabinetService fileCabinetService = new FileCabinetService();

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
            new string[] { "find <parameter> <value>", "finds a record", "The 'find' finds all records with a specific parameter value." },
        };

        public static void Main(string[] args)
        {
            Console.WriteLine($"File Cabinet Application, developed by {Program.DeveloperName}");
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
                    throw new ArgumentNullException($"{nameof(firstName)}", "Parameter is null");
                }

                if (firstName.Length < 2 || firstName.Length > 60)
                {
                    throw new ArgumentException("Parameter has wrong length", $"{nameof(firstName)}");
                }

                Console.WriteLine("Last name: ");
                string? lastName = string.Empty;
                lastName = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(lastName))
                {
                    throw new ArgumentNullException($"{nameof(lastName)}", "Parameter is null");
                }

                if (lastName.Length < 2 || lastName.Length > 60)
                {
                    throw new ArgumentException("Parameter has wrong length", $"{nameof(lastName)}");
                }

                Console.WriteLine("Date of birth: ");
                string? dateString = string.Empty;
                dateString = Console.ReadLine();
                DateTime dateOfBirth = DateTime.Now;
                bool parsedSuccessfully = DateTime.TryParse(dateString, new System.Globalization.CultureInfo("en-US"), 0, out dateOfBirth);
                DateTime min = new DateTime(1950, 1, 1);
                if (DateTime.Compare(dateOfBirth, min) < 0 || DateTime.Compare(dateOfBirth, DateTime.Now) > 0 || !parsedSuccessfully)
                {
                    throw new ArgumentException("Parameter is wrong", $"{nameof(dateOfBirth)}");
                }

                parsedSuccessfully = true;
                Console.WriteLine("Height: ");
                short height = 0;
                parsedSuccessfully = short.TryParse(Console.ReadLine(), out height);
                if (height < 45 || height > 252 || !parsedSuccessfully)
                {
                    throw new ArgumentException("Parameter is wrong", $"{nameof(height)}");
                }

                parsedSuccessfully = true;
                Console.WriteLine("Weight: ");
                decimal weight = 0;
                parsedSuccessfully = decimal.TryParse(Console.ReadLine(), out weight);
                if (weight < 2 || weight > 600 || !parsedSuccessfully)
                {
                    throw new ArgumentException("Parameter is wrong", $"{nameof(weight)}");
                }

                parsedSuccessfully = true;
                Console.WriteLine("Temperament: ");
                char temperament = ' ';
                parsedSuccessfully = char.TryParse(Console.ReadLine(), out temperament);
                if (!parsedSuccessfully)
                {
                    throw new ArgumentException("Parameter is wrong", $"{nameof(temperament)}");
                }

                temperament = char.ToUpper(temperament, new System.Globalization.CultureInfo("en-US"));
                if (!(temperament == 'P' || temperament == 'S' || temperament == 'C' || temperament == 'M'))
                {
                    throw new ArgumentException("Parameter is wrong", $"{nameof(temperament)}");
                }

                Program.fileCabinetService.CreateRecord(firstName, lastName, dateOfBirth, height, weight, temperament);
            }
            catch (Exception e)
            {
                Console.WriteLine("Некорректный ввод: " + e.Message);
                Create(parameters);
            }
        }

        private static void List(string parameters)
        {
            // TO DO : изменить способ вывода темперамента
            FileCabinetRecord[] records = fileCabinetService.GetRecords();
            for (int i = 0; i < records.Length; i++)
            {
                Console.WriteLine($"#{records[i].Id}, {records[i].FirstName}, {records[i].LastName}, " +
                    $"{records[i].DateOfBirth.ToString("yyyy-MMM-d", new System.Globalization.CultureInfo("en-US"))}, " +
                    $"{records[i].Height} cm, {records[i].Weigth} kg, {records[i].Temperament}");
            }
        }

        private static void Edit(string parameters)
        {
            int id = Convert.ToInt32(parameters, new System.Globalization.CultureInfo("en-US"));
            if (fileCabinetService.IsExistRecord(id) > 0)
            {
                try
                {
                    Console.WriteLine("First name: ");
                    string? firstName = string.Empty;
                    firstName = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(firstName))
                    {
                        throw new ArgumentNullException($"{nameof(firstName)}", "Parameter is null");
                    }

                    if (firstName.Length < 2 || firstName.Length > 60)
                    {
                        throw new ArgumentException("Parameter has wrong length", $"{nameof(firstName)}");
                    }

                    Console.WriteLine("Last name: ");
                    string? lastName = string.Empty;
                    lastName = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(lastName))
                    {
                        throw new ArgumentNullException($"{nameof(lastName)}", "Parameter is null");
                    }

                    if (lastName.Length < 2 || lastName.Length > 60)
                    {
                        throw new ArgumentException("Parameter has wrong length", $"{nameof(lastName)}");
                    }

                    Console.WriteLine("Date of birth: ");
                    string? dateString = string.Empty;
                    dateString = Console.ReadLine();
                    DateTime dateOfBirth = DateTime.Now;
                    bool parsedSuccessfully = DateTime.TryParse(dateString, new System.Globalization.CultureInfo("en-US"), 0, out dateOfBirth);
                    DateTime min = new DateTime(1950, 1, 1);
                    if (DateTime.Compare(dateOfBirth, min) < 0 || DateTime.Compare(dateOfBirth, DateTime.Now) > 0 || !parsedSuccessfully)
                    {
                        throw new ArgumentException("Parameter is wrong", $"{nameof(dateOfBirth)}");
                    }

                    parsedSuccessfully = true;
                    Console.WriteLine("Height: ");
                    short height = 0;
                    parsedSuccessfully = short.TryParse(Console.ReadLine(), out height);
                    if (height < 45 || height > 252 || !parsedSuccessfully)
                    {
                        throw new ArgumentException("Parameter is wrong", $"{nameof(height)}");
                    }

                    parsedSuccessfully = true;
                    Console.WriteLine("Weight: ");
                    decimal weight = 0;
                    parsedSuccessfully = decimal.TryParse(Console.ReadLine(), out weight);
                    if (weight < 2 || weight > 600 || !parsedSuccessfully)
                    {
                        throw new ArgumentException("Parameter is wrong", $"{nameof(weight)}");
                    }

                    parsedSuccessfully = true;
                    Console.WriteLine("Temperament: ");
                    char temperament = ' ';
                    parsedSuccessfully = char.TryParse(Console.ReadLine(), out temperament);
                    if (!parsedSuccessfully)
                    {
                        throw new ArgumentException("Parameter is wrong", $"{nameof(temperament)}");
                    }

                    temperament = char.ToUpper(temperament, new System.Globalization.CultureInfo("en-US"));
                    if (!(temperament == 'P' || temperament == 'S' || temperament == 'C' || temperament == 'M'))
                    {
                        throw new ArgumentException("Parameter is wrong", $"{nameof(temperament)}");
                    }

                    Program.fileCabinetService.EditRecord(id, firstName, lastName, dateOfBirth, height, weight, temperament);
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
            // TODO: кидать исключения при некорректном вводе
            string parameter = parameters.Split()[0].ToLower(new System.Globalization.CultureInfo("en-US"));
            string value = parameters.Split()[1].ToLower(new System.Globalization.CultureInfo("en-US"));

            if (parameter == "firstname")
            {
                var records = fileCabinetService.FindByFirstName(value[1..^1]);
                for (int i = 0; i < records.Length; i++)
                {
                    Console.WriteLine($"#{records[i].Id}, {records[i].FirstName}, {records[i].LastName}, " +
                    $"{records[i].DateOfBirth.ToString("yyyy-MMM-d", new System.Globalization.CultureInfo("en-US"))}, " +
                    $"{records[i].Height} cm, {records[i].Weigth} kg, {records[i].Temperament}");
                }
            }

            if (parameter == "lastname")
            {
                var records = fileCabinetService.FindByLastName(value[1..^1]);
                for (int i = 0; i < records.Length; i++)
                {
                    Console.WriteLine($"#{records[i].Id}, {records[i].FirstName}, {records[i].LastName}, " +
                    $"{records[i].DateOfBirth.ToString("yyyy-MMM-d", new System.Globalization.CultureInfo("en-US"))}, " +
                    $"{records[i].Height} cm, {records[i].Weigth} kg, {records[i].Temperament}");
                }
            }

            if (parameter == "dateofbirth")
            {
                DateTime dateOfBirth;
                bool parsedSuccessfully = DateTime.TryParse(value[1..^1], new System.Globalization.CultureInfo("en-US"), 0, out dateOfBirth);
                var records = fileCabinetService.FindByDateOfBirth(dateOfBirth);
                for (int i = 0; i < records.Length; i++)
                {
                    Console.WriteLine($"#{records[i].Id}, {records[i].FirstName}, {records[i].LastName}, " +
                    $"{records[i].DateOfBirth.ToString("yyyy-MMM-d", new System.Globalization.CultureInfo("en-US"))}, " +
                    $"{records[i].Height} cm, {records[i].Weigth} kg, {records[i].Temperament}");
                }
            }
        }
    }
}