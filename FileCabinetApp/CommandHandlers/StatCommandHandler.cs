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
                var memory = Memoizer.Remember(request);
                if (memory == null)
                {
                    var recordsCount = this.service.GetStat(out int removedCount);
                    Console.WriteLine($"{recordsCount} record(s), {removedCount} of them are removed.");
                    Memoizer.Memoize(request, Tuple.Create(recordsCount, removedCount));
                }
                else
                {
                    Console.WriteLine($"{((Tuple<int, int>)memory).Item1} record(s), {((Tuple<int, int>)memory).Item2} of them are removed.");
                }
            }
            else
            {
                this.nextHandler.Handle(request);
            }
        }
    }
}
