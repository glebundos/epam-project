namespace FileCabinetApp.CommandHandlers
{
    public class MissedHelpCommandHandler : CommandHandlerBase
    {
        public override void Handle(AppCommandRequest request)
        {
            Console.WriteLine($"There is no '{request.Command}' command.");
            Console.WriteLine();
        }
    }
}
