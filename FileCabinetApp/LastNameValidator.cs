namespace FileCabinetApp
{
    /// <summary>
    /// Validator for last name.
    /// </summary>
    public class LastNameValidator : IRecordValidator
    {
        private readonly int minNameLength;
        private readonly int maxNameLength;

        /// <summary>
        /// Initializes a new instance of the <see cref="LastNameValidator"/> class.
        /// </summary>
        /// <param name="minNameLength">Minimal possible length of last name.</param>
        /// <param name="maxNameLength">Maximum possible length of last name.</param>
        public LastNameValidator(int minNameLength, int maxNameLength)
        {
            this.minNameLength = minNameLength;
            this.maxNameLength = maxNameLength;
        }

        /// <summary>
        /// Validating given parameters.
        /// </summary>
        /// <param name="parameters">Parameters to validate.</param>
        /// <returns>True if validation successful or false in the other case.</returns>
        public bool ValidateParameters(Record parameters)
        {
            if (parameters is null)
            {
                throw new ArgumentNullException(nameof(parameters), "Record is null");
            }

            if (string.IsNullOrWhiteSpace(parameters.LastName))
            {
                throw new ArgumentNullException(nameof(parameters.LastName), "LastName is null");
            }

            return parameters.LastName.Length >= this.minNameLength && parameters.LastName.Length <= this.maxNameLength;
        }
    }
}