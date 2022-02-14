namespace FileCabinetApp.CommandHandlers
{
    public class StatCommandHandler : ServiceCommandHandlerBase
    {
        public StatCommandHandler(IFileCabinetService service)
            : base(service)
        {
            this.service = service;
        }

        public override void Handle(AppCommandRequest request)
        {
            if (!string.IsNullOrEmpty(request.Command) && request.Command == "stat")
            {
                var recordsCount = this.service.GetStat(out int removedCount);
                Console.WriteLine($"{recordsCount} record(s), {removedCount} of them are removed.");
            }
            else
            {
                this.nextHandler.Handle(request);
            }
        }
    }
}
