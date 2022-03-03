namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Command handler class for exit command.
    /// </summary>
    public class ExitCommandHandler : CommandHandlerBase
    {
        private Action<bool> action;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExitCommandHandler"/> class.
        /// </summary>
        /// <param name="action"> - action of exit from the app.</param>
        public ExitCommandHandler(Action<bool> action)
        {
            this.action = action;
        }

        /// <inheritdoc/>
        public override void Handle(AppCommandRequest request)
        {
            if (!string.IsNullOrEmpty(request.Command) && request.Command.Equals("exit", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Exiting an application...");
                this.action(false);
            }
            else
            {
                this.NextHandler.Handle(request);
            }
        }
    }
}
