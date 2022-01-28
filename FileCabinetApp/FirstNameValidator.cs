namespace FileCabinetApp
{
    /// <summary>
    /// Validator for first name.
    /// </summary>
    public class FirstNameValidator : IRecordValidator
    {
        private readonly int minNameLength;
        private readonly int maxNameLength;

        /// <summary>
        /// Initializes a new instance of the <see cref="FirstNameValidator"/> class.
        /// </summary>
        /// <param name="minNameLength">Minimal possible length of first name.</param>
        /// <param name="maxNameLength">Maximum possible length of first name.</param>
        public FirstNameValidator(int minNameLength, int maxNameLength)
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

            if (string.IsNullOrWhiteSpace(parameters.FirstName))
            {
                throw new ArgumentNullException(nameof(parameters.FirstName), "FirstName is null");
            }

            return parameters.FirstName.Length >= this.minNameLength && parameters.FirstName.Length <= this.maxNameLength;
        }
    }
}
