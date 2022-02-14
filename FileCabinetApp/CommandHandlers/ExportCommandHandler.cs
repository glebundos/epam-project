namespace FileCabinetApp.CommandHandlers
{
    public class ExportCommandHandler : ServiceCommandHandlerBase
    {
        public ExportCommandHandler(IFileCabinetService service)
            : base(service)
        {
            this.service = service;
        }

        public override void Handle(AppCommandRequest request)
        {
            if (!string.IsNullOrEmpty(request.Command) && request.Command == "export")
            {
                try
                {
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
                        this.service.MakeSnapshot().SaveToCsv(streamWriter, append);
                    }
                    else if (arguments[0] == "xml")
                    {
                        // TODO: append всегда false в случае с xml т.к. если присоединять записи к уже существующим десериализация ломается, пофиксить если возможно.
                        arguments[1] = arguments[1][1..^1];
                        StreamWriter streamWriter = new StreamWriter(arguments[1], append);
                        this.service.MakeSnapshot().SaveToXml(streamWriter, append);
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
            else
            {
                this.nextHandler.Handle(request);
            }
        }
    }
}
