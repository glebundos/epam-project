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
        public void ValidateParameters(Record parameters);
    }
}
