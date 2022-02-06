using FileCabinetApp;

namespace FileCabinetGenerator
{
    public static class Program
    {
        private const string DeveloperName = "Gleb Kontovsky";
        private const string HintMessage = "Enter your command, or enter 'help' to get help.";
        private const int CommandHelpIndex = 0;
        private const int DescriptionHelpIndex = 1;
        private const int ExplanationHelpIndex = 2;
        private static Config config;
        private static bool isRunning = true;
        private static IReadOnlyCollection<FileCabinetRecord> fileCabinetRecords = new List<FileCabinetRecord>();

        private static Tuple<string, Action<string>>[] commands = new Tuple<string, Action<string>>[]
        {
            new Tuple<string, Action<string>>("help", PrintHelp),
            new Tuple<string, Action<string>>("exit", Exit),
            new Tuple<string, Action<string>>("generate", Generate),
            new Tuple<string, Action<string>>("stat", Stat),
            new Tuple<string, Action<string>>("list", List),
            new Tuple<string, Action<string>>("export", Export),
        };

        private static string[][] helpMessages = new string[][]
        {
            new string[] { "help", "prints the help screen", "The 'help' command prints the help screen." },
            new string[] { "exit", "exits the application", "The 'exit' command exits the application." },
            new string[] { "generate", "generates list of records", "The 'generate' command generates a list of records with given configuration." },
            new string[] { "stat", "writes the amount of records", "The 'stat' command writes the amount of records." },
            new string[] { "list", "shows a list of all records", "The 'list' command shows a list of all records." },
            new string[] { "export <file extension> \"path\"", "exports records", "The 'export' exports records into the file(path)." },
        };

        private static void Generate(string parameters)
        {
            fileCabinetRecords = RecordGenerator.GenerateRecords(config);
        }

        private static void Stat(string parameters)
        {
            Console.WriteLine(fileCabinetRecords.Count);
        }

        private static void List(string parameters)
        {
            foreach (var el in fileCabinetRecords)
            {
                WriteRecord(el);
            }
        }

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
                            value = args[i].ToLower();
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

        private static void Configure(Dictionary<string, string> configuration)
        {
            string? type = string.Empty;
            if (configuration.TryGetValue("-t", out type) || configuration.TryGetValue("--output-type", out type))
            {
                if (type.ToLower() == "xml")
                {
                    type = "xml";
                }
                else
                {
                    type = "csv";
                }
            }
            else
            {
                type = "csv";
            }

            string? filePath = string.Empty;
            if (!(configuration.TryGetValue("-o", out filePath) || configuration.TryGetValue("--output", out filePath)))
            {
                if (type == "xml")
                {
                    filePath = "records.xml";
                }
                else
                {
                    filePath = "records.csv";
                }
            }

            string? amountValue = "0";
            int amount = 100;
            if (configuration.TryGetValue("-a", out amountValue) || configuration.TryGetValue("--records-amount", out amountValue))
            {
                amount = Convert.ToInt32(amountValue);
            }

            string? idValue = "1";
            int id = 0;
            if (configuration.TryGetValue("-i", out idValue) || configuration.TryGetValue("--start-id", out idValue))
            {
                id = Convert.ToInt32(idValue);
            }

            config = new Config()
            {
                Type = type,
                FilePath = filePath,
                Amount = amount,
                StartId = id,
            };
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

        private static void Export(string parameters)
        {
            try
            {
                bool append = false;
                if (config.Type == "csv")
                {
                    if (File.Exists(config.FilePath))
                    {
                        Console.WriteLine($"File is exist - rewrite {config.FilePath}? [Y/n]");
                        if (Console.ReadKey().Key == ConsoleKey.N)
                        {
                            append = true;
                        }

                        Console.WriteLine();
                    }

                    StreamWriter streamWriter = new StreamWriter(config.FilePath, append);
                    FileCabinetRecordCsvWriter csvWriter = new FileCabinetRecordCsvWriter(streamWriter);
                    csvWriter.Write(fileCabinetRecords, append);
                }
                else if (config.Type == "xml")
                {
                    // TODO: append всегда false в случае с xml т.к. если присоединять записи к уже существующим десериализация ломается, пофиксить если возможно.
                    StreamWriter streamWriter = new StreamWriter(config.FilePath, append);
                    FileCabinetRecordXmlWriter xmlWriter = new FileCabinetRecordXmlWriter(streamWriter);
                    xmlWriter.Write(fileCabinetRecords, append);
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
    }
}
