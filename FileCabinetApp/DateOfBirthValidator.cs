namespace FileCabinetApp
{
    /// <summary>
    /// Validator for date of birth.
    /// </summary>
    public class DateOfBirthValidator : IRecordValidator
    {
        private readonly DateTime minDate;
        private readonly DateTime maxDate;

        /// <summary>
        /// Initializes a new instance of the <see cref="DateOfBirthValidator"/> class.
        /// </summary>
        /// <param name="minDate">Minimal possible date of birth.</param>
        /// <param name="maxDate">Maximum possible date of birth.</param>
        public DateOfBirthValidator(DateTime minDate, DateTime maxDate)
        {
            this.minDate = minDate;
            this.maxDate = maxDate;
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

            return DateTime.Compare(parameters.DateOfBirth, this.minDate) >= 0 && DateTime.Compare(parameters.DateOfBirth, this.maxDate) <= 0;
        }
    }
}