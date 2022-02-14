namespace FileCabinetApp.CommandHandlers
{
    public class ListCommandHandler : ServiceCommandHandlerBase
    {
        private Action<IEnumerable<FileCabinetRecord>> printer;

        public ListCommandHandler(IFileCabinetService service, Action<IEnumerable<FileCabinetRecord>> printer)
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
                this.printer(records);
            }
            else
            {
                this.nextHandler.Handle(request);
            }
        }
    }
}
