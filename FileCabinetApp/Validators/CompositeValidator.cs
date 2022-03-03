namespace FileCabinetApp
{
    /// <summary>
    /// Contains all validators.
    /// </summary>
    public class CompositeValidator : IRecordValidator
    {
        private readonly IReadOnlyCollection<IRecordValidator> validators;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeValidator"/> class.
        /// </summary>
        /// <param name="validators">List of validators.</param>
        public CompositeValidator(IReadOnlyCollection<IRecordValidator> validators)
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
