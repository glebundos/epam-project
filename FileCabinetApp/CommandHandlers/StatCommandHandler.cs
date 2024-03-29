﻿namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Command handler class for stat command.
    /// </summary>
    public class StatCommandHandler : ServiceCommandHandlerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StatCommandHandler"/> class.
        /// </summary>
        /// <param name="service"> - fileCabinet service to manipulate with.</param>
        public StatCommandHandler(IFileCabinetService service)
            : base(service)
        {
        }

        /// <inheritdoc/>
        public override void Handle(AppCommandRequest request)
        {
            if (!string.IsNullOrEmpty(request.Command) && request.Command.Equals("stat", StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    this.Stat(request);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Stat failed: " + e.Message);
                }
            }
            else
            {
                this.NextHandler.Handle(request);
            }
        }

        private void Stat(AppCommandRequest request)
        {
            var memory = Memoizer.Remember(request);
            if (memory == null)
            {
                var recordsCount = this.Service.GetStat(out int removedCount);
                Console.WriteLine($"{recordsCount} record(s), {removedCount} of them are removed.");
                Memoizer.Memoize(request, Tuple.Create(recordsCount, removedCount));
            }
            else
            {
                Console.WriteLine($"{((Tuple<int, int>)memory).Item1} record(s), {((Tuple<int, int>)memory).Item2} of them are removed.");
            }
        }
    }
}
