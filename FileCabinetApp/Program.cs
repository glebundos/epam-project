using System.Collections.ObjectModel;
using FileCabinetApp.CommandHandlers;

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
                IRecordValidator validatorDefault = ValidatorBuilder.CreateFullValidator(validatorsSettings);
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

            value = string.Empty;
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

        private static ICommandHandler CreateCommandHandlers()
        {
            var helpHandler = new HelpCommandHandler();
            var statHandler = new StatCommandHandler(fileCabinetService);
            var createHandler = new CreateCommandHandler(fileCabinetService, validatorsSettings);
            var listHandler = new ListCommandHandler(fileCabinetService);
            var editHandler = new EditCommandHandler(fileCabinetService, validatorsSettings);
            var findHandler = new FindCommandHandler(fileCabinetService);
            var exportHandler = new ExportCommandHandler(fileCabinetService);
            var importHandler = new ImportCommandHandler(fileCabinetService);
            var removeHandler = new RemoveCommandHandler(fileCabinetService);
            var purgeHandler = new PurgeCommandHandler(fileCabinetService);
            var exitHandler = new ExitCommandHandler(Exit);
            var missedHelpHandler = new MissedHelpCommandHandler();

            helpHandler.SetNext(statHandler);
            statHandler.SetNext(createHandler);
            createHandler.SetNext(listHandler);
            listHandler.SetNext(editHandler);
            editHandler.SetNext(findHandler);
            findHandler.SetNext(exportHandler);
            exportHandler.SetNext(importHandler);
            importHandler.SetNext(removeHandler);
            removeHandler.SetNext(purgeHandler);
            purgeHandler.SetNext(exitHandler);

            exitHandler.SetNext(missedHelpHandler);

            return helpHandler;
        }

        private static void Exit(bool status)
        {
            isRunning = status;
        }
    }
}
#pragma warning restore CA2208 // Правильно создавайте экземпляры исключений аргументов