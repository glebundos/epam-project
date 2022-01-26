namespace FileCabinetApp
{
    /// <summary>
    /// Custom configuration of FileCabinetService class.
    /// </summary>
    public class FileCabinetCustomService : FileCabinetService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetCustomService"/> class.
        /// </summary>
        public FileCabinetCustomService()
            : base(new DefaultValidator())
        {
        }
    }
}
