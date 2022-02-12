namespace FileCabinetApp.CommandHandlers
{
    public class ExitCommandHandler : CommandHandlerBase
    {
        private Action<bool> action;

        public ExitCommandHandler(Action<bool> action)
        {
            this.action = action;
        }

        public override void Handle(AppCommandRequest request)
        {
            if (!string.IsNullOrEmpty(request.Command) && request.Command == "exit")
            {
                Console.WriteLine("Exiting an application...");
                this.action(false);
            }
            else
            {
                this.nextHandler.Handle(request);
            }
        }
    }
}
