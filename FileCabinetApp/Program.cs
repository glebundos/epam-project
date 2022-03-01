using System.Text;
using FileCabinetApp.CommandHandlers;
using FileCabinetApp.Comparers;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

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
        private static ValidatorsSettings validatorsSettings = new ValidatorsSettings();
        private static IFileCabinetService fileCabinetService;

        private static bool isRunning = true;

        /// <summary>
        /// Еhe main method from which the program execution starts.
        /// </summary>
        /// <param name="args">Initial program arguments.</param>
        public static void Main(string[] args)
        {
            Console.WriteLine($"File Cabinet Application, developed by {Program.DeveloperName}");
            Configure(args);
            Console.WriteLine(Program.HintMessage);
            Console.WriteLine();
            var commandHandler = CreateCommandHandlers();

            do
            {
                Console.Write("> ");
                var line = Console.ReadLine();
                var inputs = line != null ? line.Split(' ', 2) : new string[] { string.Empty, string.Empty };
                string command = inputs[0];

                if (string.IsNullOrEmpty(command))
                {
                    Console.WriteLine(Program.HintMessage);
                    continue;
                }

                string parameters = inputs.Length > 1 ? inputs[1] : string.Empty;
                commandHandler.Handle(new AppCommandRequest() { Command = command, Parameters = parameters });
            }
            while (isRunning);
        }

        private static void Configure(string[] args)
        {
            if (args is null)
            {
                validatorsSettings.SetDefaultConfig();
                Console.WriteLine("Using default validation rules");
                IRecordValidator validatorDefault = ValidatorBuilder.CreateCompositeValidator(validatorsSettings);
                fileCabinetService = new FileCabinetMemoryService(validatorDefault);
                Console.WriteLine("Using memory service");
                return;
            }

            Dictionary<string, string> configuration = new Dictionary<string, string>();
            string? value = string.Empty;
            for (int i = 0; i < args.Length; i++)
            {
                string key = args[i].ToLower(new System.Globalization.CultureInfo("en-US"));
                value = string.Empty;
                if (key.Contains('=', StringComparison.InvariantCultureIgnoreCase))
                {
                    value = key.Split('=')[1];
                    key = key.Split('=')[0];
                }
                else if (key.Contains('-', StringComparison.InvariantCultureIgnoreCase))
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
                else
                {
                    value = "default";
                }

                configuration.Add(key, value);
            }

            ValidatorsSettings defaultSettings = null;
            ValidatorsSettings customSettings = null;
            if (File.Exists("validation-rules.json"))
            {
                var settings = JsonConvert.DeserializeObject<Dictionary<string, ValidatorsSettings>>(File.ReadAllText("validation-rules.json"));
                defaultSettings = settings["Default"];
                customSettings = settings["Custom"];
            }

            value = string.Empty;
            if (configuration.TryGetValue("-v", out value) || configuration.TryGetValue("--validation-rules", out value))
            {
                if (value == "custom")
                {
                    validatorsSettings = customSettings;
                    Console.WriteLine("Using custom validation rules");
                }
                else
                {
                    validatorsSettings = defaultSettings;
                    Console.WriteLine("Using default validation rules");
                }
            }
            else
            {
                validatorsSettings = defaultSettings;
                Console.WriteLine("Using default validation rules");
            }

            IRecordValidator validator = ValidatorBuilder.CreateCompositeValidator(validatorsSettings);
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

            if (configuration.ContainsKey("us") || configuration.ContainsKey("--use-stopwatch"))
            {
                fileCabinetService = new ServiceMeter(fileCabinetService);
                Console.WriteLine("Using stopwatch");
            }

            if (configuration.ContainsKey("ul") || configuration.ContainsKey("--use-logger"))
            {
                fileCabinetService = new ServiceLogger(fileCabinetService);
                Console.WriteLine("Using logger");
            }
        }

        private static ICommandHandler CreateCommandHandlers()
        {
            var helpHandler = new HelpCommandHandler();
            var statHandler = new StatCommandHandler(fileCabinetService);
            var createHandler = new CreateCommandHandler(fileCabinetService, validatorsSettings);
            var exportHandler = new ExportCommandHandler(fileCabinetService);
            var importHandler = new ImportCommandHandler(fileCabinetService);
            var purgeHandler = new PurgeCommandHandler(fileCabinetService);
            var exitHandler = new ExitCommandHandler(Exit);
            var missedHelpHandler = new MissedHelpCommandHandler();
            var insertHandler = new InsertCommandHandler(fileCabinetService, validatorsSettings);
            var deleteHandler = new DeleteCommandHandler(fileCabinetService);
            var updateHandler = new UpdateCommandHandler(fileCabinetService, validatorsSettings);
            var selectHandler = new SelectCommandHandler(fileCabinetService, Print);

            helpHandler.SetNext(statHandler);
            statHandler.SetNext(createHandler);
            createHandler.SetNext(exportHandler);
            exportHandler.SetNext(importHandler);
            importHandler.SetNext(purgeHandler);
            purgeHandler.SetNext(insertHandler);
            insertHandler.SetNext(deleteHandler);
            deleteHandler.SetNext(updateHandler);
            updateHandler.SetNext(selectHandler);
            selectHandler.SetNext(exitHandler);

            exitHandler.SetNext(missedHelpHandler);

            return helpHandler;
        }

        private static void Print(IEnumerable<FileCabinetRecord> records, string[] outParams)
        {
            List<FileCabinetRecord> recordsList = records.ToList();
            int[] allignments;
            if (outParams.Contains("*"))
            {
                allignments = new int[7];
                Console.Write("|");

                recordsList.Sort(new IdComparer());
                allignments[0] = GetNumberLength(recordsList[^1].Id) > 2 ? GetNumberLength(recordsList[^1].Id) : 2;
                Console.Write("{0," + allignments[0] + "}|", "Id");

                recordsList.Sort(new FirstNameLengthComparer());
                allignments[1] = -(recordsList[^1].FirstName.Length > 9 ? recordsList[^1].FirstName.Length : 9);
                Console.Write("{0," + allignments[1] + "}|", "FirstName");

                recordsList.Sort(new LastNameLengthComparer());
                allignments[2] = -(recordsList[^1].LastName.Length > 8 ? recordsList[^1].LastName.Length : 8);
                Console.Write("{0," + allignments[2] + "}|", "LastName");

                recordsList.Sort(new DateOfBirthComparer());
                allignments[3] = -(recordsList[^1].DateOfBirth.ToString("yyyy-MMM-d", new System.Globalization.CultureInfo("en-US")).Length > 11 ?
                                   recordsList[^1].DateOfBirth.ToString("yyyy-MMM-d", new System.Globalization.CultureInfo("en-US")).Length : 11);
                Console.Write("{0," + allignments[3] + "}|", "DateOfBirth");

                recordsList.Sort(new HeightComparer());
                allignments[4] = GetNumberLength(recordsList[^1].Height) > 6 ? GetNumberLength(recordsList[^1].Height) : 6;
                Console.Write("{0," + allignments[4] + "}|", "Height");

                recordsList.Sort(new WeightComparer());
                allignments[5] = GetNumberLength(recordsList[^1].Weight) > 6 ? GetNumberLength(recordsList[^1].Weight) : 6;
                Console.Write("{0," + allignments[5] + "}|", "Weight");

                recordsList.Sort(new TemperamentComparer());
                allignments[6] = -11;
                Console.Write("{0," + allignments[6] + "}|", "Temperament");

                Console.WriteLine();
            }
            else
            {
                allignments = new int[outParams.Length];
                Console.Write("|");
                for (int i = 0; i < outParams.Length; i++)
                {
                    switch (outParams[i])
                    {
                        case "id":
                            recordsList.Sort(new IdComparer());
                            allignments[i] = GetNumberLength(recordsList[^1].Id) > 2 ? GetNumberLength(recordsList[^1].Id) : 2;
                            Console.Write("{0," + allignments[i] + "}|", "Id");
                            break;
                        case "firstname":
                            recordsList.Sort(new FirstNameLengthComparer());
                            allignments[i] = -(recordsList[^1].FirstName.Length > 9 ? recordsList[^1].FirstName.Length : 9);
                            Console.Write("{0," + allignments[i] + "}|", "FirstName");
                            break;
                        case "lastname":
                            recordsList.Sort(new LastNameLengthComparer());
                            allignments[i] = -(recordsList[^1].LastName.Length > 8 ? recordsList[^1].LastName.Length : 8);
                            Console.Write("{0," + allignments[i] + "}|", "LastName");
                            break;
                        case "dateofbirth":
                            recordsList.Sort(new DateOfBirthComparer());
                            allignments[i] = -(recordsList[^1].DateOfBirth.ToString("yyyy-MMM-d", new System.Globalization.CultureInfo("en-US")).Length > 11 ?
                                               recordsList[^1].DateOfBirth.ToString("yyyy-MMM-d", new System.Globalization.CultureInfo("en-US")).Length : 11);
                            Console.Write("{0," + allignments[i] + "}|", "DateOfBirth");
                            break;
                        case "height":
                            recordsList.Sort(new HeightComparer());
                            allignments[i] = GetNumberLength(recordsList[^1].Height) > 6 ? GetNumberLength(recordsList[^1].Height) : 6;
                            Console.Write("{0," + allignments[i] + "}|", "Height");
                            break;
                        case "weight":
                            recordsList.Sort(new WeightComparer());
                            allignments[i] = GetNumberLength(recordsList[^1].Weight) > 6 ? GetNumberLength(recordsList[^1].Weight) : 6;
                            Console.Write("{0," + allignments[i] + "}|", "Weight");
                            break;
                        case "temperament":
                            recordsList.Sort(new TemperamentComparer());
                            allignments[i] = -11;
                            Console.Write("|{0," + allignments[i] + "}|", "Temperament");
                            break;
                    }
                }

                Console.WriteLine();
            }

            Console.Write('+');
            foreach (var allignment in allignments)
            {
                for (int i = 0; i < Math.Abs(allignment); i++)
                {
                    Console.Write('-');
                }

                Console.Write('+');
            }

            Console.WriteLine();
            foreach (var record in records)
            {
                string temperament;
                Console.Write("|");
                for (int i = 0; i < outParams.Length; i++)
                {
                    switch (outParams[i])
                    {
                        case "id":
                            Console.Write("{0," + allignments[i] + "}|", record.Id);
                            break;
                        case "firstname":
                            Console.Write("{0," + allignments[i] + "}|", record.FirstName);
                            break;
                        case "lastname":
                            Console.Write("{0," + allignments[i] + "}|", record.LastName);
                            break;
                        case "dateofbirth":
                            Console.Write("{0," + allignments[i] + "}|", record.DateOfBirth.ToString("yyyy-MMM-d", new System.Globalization.CultureInfo("en-US")));
                            break;
                        case "height":
                            Console.Write("{0," + allignments[i] + "}|", record.Height);
                            break;
                        case "weight":
                            Console.Write("{0," + allignments[i] + "}|", record.Weight);
                            break;
                        case "temperament":
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

                            Console.Write("|{0," + allignments[i] + "}|", temperament);
                            break;
                        case "*":
                            Console.Write("{0," + allignments[0] + "}|", record.Id);
                            Console.Write("{0," + allignments[1] + "}|", record.FirstName);
                            Console.Write("{0," + allignments[2] + "}|", record.LastName);
                            Console.Write("{0," + allignments[3] + "}|", record.DateOfBirth.ToString("yyyy-MMM-d", new System.Globalization.CultureInfo("en-US")));
                            Console.Write("{0," + allignments[4] + "}|", record.Height);
                            Console.Write("{0," + allignments[5] + "}|", record.Weight);
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

                            Console.Write("{0," + allignments[6] + "}|", temperament);
                            break;
                    }
                }

                Console.WriteLine();
            }

            Console.Write('+');
            foreach (var allignment in allignments)
            {
                for (int i = 0; i < Math.Abs(allignment); i++)
                {
                    Console.Write('-');
                }

                Console.Write('+');
            }

            Console.WriteLine();
        }

        private static void Exit(bool status)
        {
            isRunning = status;
        }

        private static int GetNumberLength(long number)
        {
            return (int)Math.Log10(number) + 1;
        }

        private static int GetNumberLength(decimal number)
        {
            var count = 0;
            if (number - (long)number != 0)
            {
                count++;
            }

            while (number - (long)number != 0)
            {
                number *= 10;
            }

            count += GetNumberLength((long)number);
            return count;
        }
    }
}
#pragma warning restore CA2208 // Правильно создавайте экземпляры исключений аргументов