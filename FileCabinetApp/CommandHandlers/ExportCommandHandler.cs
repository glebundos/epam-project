namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Command handler class for export command.
    /// </summary>
    public class ExportCommandHandler : ServiceCommandHandlerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExportCommandHandler"/> class.
        /// </summary>
        /// <param name="service"> - fileCabinetService to manipulate with.</param>
        public ExportCommandHandler(IFileCabinetService service)
            : base(service)
        {
        }

        /// <inheritdoc/>
        public override void Handle(AppCommandRequest request)
        {
            if (!string.IsNullOrEmpty(request.Command) && request.Command.Equals("export", StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    this.Export(request);
                    Memoizer.Clear();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Export failed: " + e.Message);
                }
            }
            else
            {
                this.NextHandler.Handle(request);
            }
        }

        private void Export(AppCommandRequest request)
        {
            if (string.IsNullOrEmpty(request.Parameters))
            {
                throw new ArgumentException("Parameters were empty");
            }

            string[] arguments = request.Parameters.Split();
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
                this.Service.MakeSnapshot().SaveToCsv(streamWriter, append);
            }
            else if (arguments[0] == "xml")
            {
                arguments[1] = arguments[1][1..^1];
                StreamWriter streamWriter = new StreamWriter(arguments[1], append);
                this.Service.MakeSnapshot().SaveToXml(streamWriter, append);
            }
            else
            {
                throw new ArgumentException("Wrong parameters.");
            }
        }
    }
}
