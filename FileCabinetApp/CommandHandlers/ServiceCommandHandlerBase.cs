namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Base command handler class for commands using fileCabinetService.
    /// </summary>
    public abstract class ServiceCommandHandlerBase : CommandHandlerBase
    {
        private IFileCabinetService? service;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceCommandHandlerBase"/> class.
        /// </summary>
        /// <param name="service"> - fileCabinetService to manipulate with.</param>
        protected ServiceCommandHandlerBase(IFileCabinetService service)
        {
            this.service = service;
        }

        /// <summary>
        /// Gets or sets <see cref="service"/> field.
        /// </summary>
        /// <value>
        /// <see cref="service"/>.
        /// </value>
        protected IFileCabinetService Service
        {
            get { return this.service; }
            set { this.service = value; }
        }
    }
}
