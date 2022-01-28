namespace FileCabinetApp
{
    /// <summary>
    /// Interface for validators classes.
    /// </summary>
    public interface IRecordValidator
    {
        /// <summary>
        /// Validating given parameters.
        /// </summary>
        /// <param name="parameters">Parameters to validate.</param>
        /// <returns>True if validation successful or false in the other case.</returns>
        public bool ValidateParameters(Record parameters);
    }
}
