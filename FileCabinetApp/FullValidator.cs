namespace FileCabinetApp
{
    /// <summary>
    /// Contains all validators.
    /// </summary>
    public class FullValidator : IRecordValidator
    {
        private readonly IReadOnlyCollection<IRecordValidator> validators;

        /// <summary>
        /// Initializes a new instance of the <see cref="FullValidator"/> class.
        /// </summary>
        /// <param name="validators">List of validators.</param>
        public FullValidator(IReadOnlyCollection<IRecordValidator> validators)
        {
            this.validators = validators;
        }

        /// <summary>
        /// Validating given parameters.
        /// </summary>
        /// <param name="parameters">Parameters to validate.</param>
        /// <returns>True if validation successful or false if at least one parameter is not validated.</returns>
        public bool ValidateParameters(FileCabinetRecord parameters)
        {
            return this.validators.All(validator => validator.ValidateParameters(parameters));
        }
    }
}
