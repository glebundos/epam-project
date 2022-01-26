namespace FileCabinetApp
{
    /// <summary>
    /// Custom configuration of FileCabinetService class.
    /// </summary>
    public class FileCabinetCustomService : FileCabinetService
    {
        /// <summary>
        /// Creating a CustomValidator instance.
        /// </summary>
        /// <returns>CustomValidator instance.</returns>
        protected override IRecordValidator CreateValidator()
        {
            return new CustomValidator();
        }
    }
}
