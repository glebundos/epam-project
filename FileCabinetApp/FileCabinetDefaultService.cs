namespace FileCabinetApp
{
    /// <summary>
    /// Default configuration of FileCabinetService class.
    /// </summary>
    public class FileCabinetDefaultService : FileCabinetService
    {
        /// <summary>
        /// Creating a DefaultValidator instance.
        /// </summary>
        /// <returns>DefaultValidator instance.</returns>
        protected override IRecordValidator CreateValidator()
        {
            return new DefaultValidator();
        }
    }
}
