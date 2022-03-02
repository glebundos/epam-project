namespace FileCabinetApp.CommandHandlers
{
    public class PurgeCommandHandler : ServiceCommandHandlerBase
    {
        public PurgeCommandHandler(IFileCabinetService service)
            : base(service)
        {
        }

        public override void Handle(AppCommandRequest request)
        {
            if (!string.IsNullOrEmpty(request.Command) && request.Command.Equals("purge", StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    this.Purge();
                    Memoizer.Clear();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Purge failed: " + e.Message);
                }
            }
            else
            {
                this.NextHandler.Handle(request);
            }
        }

        private void Purge()
        {
            int startCount = this.service.GetStat(out _);
            int purgedCount = this.service.Purge();
            Console.WriteLine($"Data file processing is completed: {purgedCount} of {startCount} records were purged.");
        }
    }
}
