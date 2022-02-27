namespace FileCabinetApp.CommandHandlers
{
    public class ListCommandHandler : ServiceCommandHandlerBase
    {
        private Action<IEnumerable<FileCabinetRecord>, string[]> printer;

        public ListCommandHandler(IFileCabinetService service, Action<IEnumerable<FileCabinetRecord>, string[]> printer)
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
                this.printer(records, new string[1] { "*" });
            }
            else
            {
                this.nextHandler.Handle(request);
            }
        }
    }
}
