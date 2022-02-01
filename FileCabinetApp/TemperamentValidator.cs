namespace FileCabinetApp
{
    /// <summary>
    /// Validator for temperament.
    /// </summary>
    public class TemperamentValidator : IRecordValidator
    {
        private readonly char[] allowedTemperaments;

        /// <summary>
        /// Initializes a new instance of the <see cref="TemperamentValidator"/> class.
        /// </summary>
        /// <param name="allowedTemperaments">Array of allowed temperaments.</param>
        public TemperamentValidator(char[] allowedTemperaments)
        {
            this.allowedTemperaments = new char[allowedTemperaments.Length];
            for (int i = 0; i < allowedTemperaments.Length; i++)
            {
                allowedTemperaments[i] = char.ToUpper(allowedTemperaments[i], new System.Globalization.CultureInfo("en-US"));
            }

            Array.Copy(allowedTemperaments, this.allowedTemperaments, allowedTemperaments.Length);
        }

        /// <summary>
        /// Validating given parameters.
        /// </summary>
        /// <param name="parameters">Parameters to validate.</param>
        /// <returns>True if validation successful or false in the other case.</returns>
        public bool ValidateParameters(FileCabinetRecord parameters)
        {
            if (parameters is null)
            {
                throw new ArgumentNullException(nameof(parameters), "Record is null");
            }

            foreach (var temperament in this.allowedTemperaments)
            {
                if (parameters.Temperament == temperament)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
