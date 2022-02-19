namespace FileCabinetApp.CommandHandlers
{
    public class HelpCommandHandler : CommandHandlerBase
    {
        private const int CommandHelpIndex = 0;
        private const int DescriptionHelpIndex = 1;
        private const int ExplanationHelpIndex = 2;

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
            new string[] { "import <file extension> <path>", "imports records", "The 'import' imports records from the file(path)." },
            new string[] { "remove <id>", "removes record", "The 'remove' removes record with given id." },
            new string[] { "purge", "purges records", "The 'purge' purges removed records." },
            new string[] { "insert (p1, p2,...,p7) values (v1,v2,...,v7) ", "inserts record", "The 'insert' inserts record." },
            new string[] { "delete where <parameter> = <value>", "deletes record", "The 'delete' deletes records with given parameter." },
        };

        public override void Handle(AppCommandRequest request)
        {
            if (!string.IsNullOrEmpty(request.Command) && request.Command == "help")
            {
                string parameters = request.Parameters;
                if (!string.IsNullOrEmpty(parameters))
                {
                    var index = Array.FindIndex(helpMessages, 0, helpMessages.Length, i => string.Equals(i[CommandHelpIndex], parameters, StringComparison.OrdinalIgnoreCase));
                    if (index >= 0)
                    {
                        Console.WriteLine(helpMessages[index][ExplanationHelpIndex]);
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
                        Console.WriteLine("\t{0}\t- {1}", helpMessage[CommandHelpIndex], helpMessage[DescriptionHelpIndex]);
                    }
                }

                Console.WriteLine();
            }
            else
            {
                this.nextHandler.Handle(request);
            }

        }
    }
}
