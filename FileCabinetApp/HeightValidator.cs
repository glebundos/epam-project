namespace FileCabinetApp
{
    /// <summary>
    /// Validator for height.
    /// </summary>
    public class HeightValidator : IRecordValidator
    {
        private readonly short minHeight;
        private readonly short maxHeight;

        /// <summary>
        /// Initializes a new instance of the <see cref="HeightValidator"/> class.
        /// </summary>
        /// <param name="minHeight">Minimal possible height.</param>
        /// <param name="maxHeight">Maximum possible height.</param>
        public HeightValidator(short minHeight, short maxHeight)
        {
            this.minHeight = minHeight;
            this.maxHeight = maxHeight;
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

            return parameters.Height > this.minHeight && parameters.Height < this.maxHeight;
        }
    }
}