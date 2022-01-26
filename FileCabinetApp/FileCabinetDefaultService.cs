namespace FileCabinetApp
{
    /// <summary>
    /// Default configuration of FileCabinetService class.
    /// </summary>
    public class FileCabinetDefaultService : FileCabinetService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetDefaultService"/> class.
        /// </summary>
        public FileCabinetDefaultService()
            : base(new DefaultValidator())
        {
        }
    }
}
