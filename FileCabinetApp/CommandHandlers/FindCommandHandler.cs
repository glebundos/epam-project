using FileCabinetApp.PrinterHandlers;

namespace FileCabinetApp.CommandHandlers
{
    public class FindCommandHandler : ServiceCommandHandlerBase
    {
        private IRecordPrinter printer;

        public FindCommandHandler(IFileCabinetService service, IRecordPrinter printer)
            : base(service)
        {
            this.service = service;
            this.printer = printer;
        }

        public override void Handle(AppCommandRequest request)
        {
            if (!string.IsNullOrEmpty(request.Command) && request.Command == "find")
            {
                try
                {
                    string parameter = request.Parameters.Split()[0].ToLower(new System.Globalization.CultureInfo("en-US"));
                    if (string.IsNullOrEmpty(parameter))
                    {
                        throw new ArgumentException("Wrong parameter", parameter);
                    }

                    string value = request.Parameters.Split()[1].ToLower(new System.Globalization.CultureInfo("en-US"))[1..^1];
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

                        records = this.service.FindByFirstName(value);
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

                        records = this.service.FindByLastName(value);
                    }
                    else if (parameter == "dateofbirth")
                    {
                        DateTime dateOfBirth;
                        bool isParsedSuccessfully = DateTime.TryParse(value, new System.Globalization.CultureInfo("en-US"), 0, out dateOfBirth);
                        if (DateTime.Compare(dateOfBirth, new DateTime(1950, 1, 1)) < 0 || DateTime.Compare(dateOfBirth, DateTime.Now) > 0 || !isParsedSuccessfully)
                        {
                            throw new ArgumentException("Parameter is wrong", nameof(dateOfBirth));
                        }

                        records = this.service.FindByDateOfBirth(dateOfBirth);
                    }
                    else
                    {
                        throw new ArgumentException("Wrong parameter", parameter);
                    }

                    this.printer.Print(records);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Некорректный ввод: " + e.Message);
                    Console.WriteLine("Используйте find <parameter name> \"value\"");
                }
            }
            else
            {
                this.nextHandler.Handle(request);
            }
        }
    }
}
