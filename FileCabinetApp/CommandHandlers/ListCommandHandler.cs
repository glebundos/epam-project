using FileCabinetApp.PrinterHandlers;

namespace FileCabinetApp.CommandHandlers
{
    public class ListCommandHandler : ServiceCommandHandlerBase
    {
        private IRecordPrinter printer;

        public ListCommandHandler(IFileCabinetService service, IRecordPrinter printer)
            : base(service)
        {
            this.service = service;
            this.printer = printer;
        }

        public override void Handle(AppCommandRequest request)
        {
            if (!string.IsNullOrEmpty(request.Command) && request.Command == "list")
            {
                var records = this.service.GetRecords();
                this.printer.Print(records);
            }
            else
            {
                this.nextHandler.Handle(request);
            }
        }
    }
}
