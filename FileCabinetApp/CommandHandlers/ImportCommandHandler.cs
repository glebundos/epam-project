﻿namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Command handler class for import command.
    /// </summary>
    public class ImportCommandHandler : ServiceCommandHandlerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ImportCommandHandler"/> class.
        /// </summary>
        /// <param name="service"> - fileCabinetService to manipulate with.</param>
        public ImportCommandHandler(IFileCabinetService service)
            : base(service)
        {
        }

        /// <inheritdoc/>
        public override void Handle(AppCommandRequest request)
        {
            if (!string.IsNullOrEmpty(request.Command) && request.Command.Equals("import", StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    this.Import(request);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Import failed: " + e.Message);
                }
            }
            else
            {
                this.NextHandler.Handle(request);
            }
        }

        private void Import(AppCommandRequest request)
        {
            if (string.IsNullOrEmpty(request.Parameters))
            {
                throw new ArgumentException("Parameters were empty");
            }

            int readedCount = 0;
            string[] arguments = request.Parameters.Split();
            StreamReader streamReader = new StreamReader(arguments[1][1..^1]);
            FileCabinetServiceSnapshot snapshot = new FileCabinetServiceSnapshot(new List<FileCabinetRecord>());
            if (arguments[0] == "csv")
            {
                readedCount = snapshot.LoadFromCsv(streamReader);
            }
            else if (arguments[0] == "xml")
            {
                readedCount = snapshot.LoadFromXml(streamReader);
            }
            else
            {
                throw new ArgumentException("Wrong parameters.");
            }

            int importedCount = this.Service.Restore(snapshot);
            Console.WriteLine($"{importedCount} records were imported from {arguments[1]}. {readedCount - importedCount} errors");
        }
    }
}
