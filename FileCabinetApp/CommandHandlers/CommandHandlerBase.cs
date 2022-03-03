namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Base command handler class with next handler field.
    /// </summary>
    public abstract class CommandHandlerBase : ICommandHandler
    {
        /// <summary>
        /// Contains next command handler to handle.
        /// </summary>
        private ICommandHandler? nextHandler;

        /// <summary>
        /// Gets or sets <see cref="nextHandler"/>.
        /// </summary>
        /// <value>
        /// <see cref="nextHandler"/>.
        /// </value>
        protected ICommandHandler NextHandler
        {
            get { return this.nextHandler; }
            set { this.nextHandler = value; }
        }

        /// <inheritdoc/>
        public void SetNext(ICommandHandler commandHandler)
        {
            this.nextHandler = commandHandler;
        }

        /// <inheritdoc/>
        public abstract void Handle(AppCommandRequest appCommandRequest);
    }
}
