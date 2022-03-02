namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Command handler class for purge command.
    /// </summary>
    public class PurgeCommandHandler : ServiceCommandHandlerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PurgeCommandHandler"/> class.
        /// </summary>
        /// <param name="service"> - fileCabinetService to manipulate with.</param>
        public PurgeCommandHandler(IFileCabinetService service)
            : base(service)
        {
        }

        /// <inheritdoc/>
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
            int startCount = this.Service.GetStat(out _);
            int purgedCount = this.Service.Purge();
            Console.WriteLine($"Data file processing is completed: {purgedCount} of {startCount} records were purged.");
        }
    }
}
