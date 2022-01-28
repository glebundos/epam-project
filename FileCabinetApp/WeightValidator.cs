namespace FileCabinetApp
{
    /// <summary>
    /// Validator for weight.
    /// </summary>
    public class WeightValidator : IRecordValidator
    {
        private readonly decimal minWeight;
        private readonly decimal maxWeight;

        /// <summary>
        /// Initializes a new instance of the <see cref="WeightValidator"/> class.
        /// </summary>
        /// <param name="minWeight">Minimum possible weight.</param>
        /// <param name="maxWeight">Maximum possible weight.</param>
        public WeightValidator(decimal minWeight, decimal maxWeight)
        {
            this.minWeight = minWeight;
            this.maxWeight = maxWeight;
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

            return parameters.Weight > this.minWeight && parameters.Weight < this.maxWeight;
        }
    }
}