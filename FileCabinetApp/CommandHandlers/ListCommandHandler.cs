namespace FileCabinetApp.CommandHandlers
{
    public class ListCommandHandler : ServiceCommandHandlerBase
    {
        public ListCommandHandler(IFileCabinetService service)
            : base(service)
        {
            this.service = service;
        }

        public override void Handle(AppCommandRequest request)
        {
            if (!string.IsNullOrEmpty(request.Command) && request.Command == "list")
            {
                var records = this.service.GetRecords();
                foreach (var record in records)
                {
                    WriteRecord(record);
                }
            }
            else
            {
                this.nextHandler.Handle(request);
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
