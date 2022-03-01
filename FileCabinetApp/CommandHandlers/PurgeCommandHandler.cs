namespace FileCabinetApp.CommandHandlers
{
    public class PurgeCommandHandler : ServiceCommandHandlerBase
    {
        public PurgeCommandHandler(IFileCabinetService service)
            : base(service)
        {
            this.service = service;
        }

        public override void Handle(AppCommandRequest request)
        {
            if (!string.IsNullOrEmpty(request.Command) && request.Command == "purge")
            {
                int startCount = this.service.GetStat(out _);
                int purgedCount = this.service.Purge();
                Console.WriteLine($"Data file processing is completed: {purgedCount} of {startCount} records were purged.");
                Memoizer.Clear();
            }
            else
            {
                this.nextHandler.Handle(request);
            }
        }
    }
}
