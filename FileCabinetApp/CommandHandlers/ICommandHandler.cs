namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Basic interface for command handler classes.
    /// </summary>
    public interface ICommandHandler
    {
        /// <summary>
        /// Sets next <see cref="ICommandHandler"/> to handle.
        /// </summary>
        /// <param name="commandHandler"> - next handler.</param>
        public void SetNext(ICommandHandler commandHandler);

        /// <summary>
        /// Handle given <see cref="AppCommandRequest"/>.
        /// </summary>
        /// <param name="appCommandRequest"> - request with command and parameters.</param>
        public void Handle(AppCommandRequest appCommandRequest);
    }
}
