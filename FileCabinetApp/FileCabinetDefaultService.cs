namespace FileCabinetApp
{
    /// <summary>
    /// Default configuration of FileCabinetService class.
    /// </summary>
    public class FileCabinetDefaultService : FileCabinetService
    {
        /// <summary>
        /// Validating given parameters.
        /// </summary>
        /// <param name="parameters">Parameters to validate.</param>
        protected override void ValidateParameters(Record parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters), "Parameter is null");
            }

            if (string.IsNullOrWhiteSpace(parameters.FirstName))
            {
                throw new ArgumentNullException(nameof(parameters.FirstName), "Parameter is null");
            }

            if (parameters.FirstName.Length < 2 || parameters.FirstName.Length > 60)
            {
                throw new ArgumentException("Parameter has wrong length", nameof(parameters.FirstName));
            }

            if (string.IsNullOrWhiteSpace(parameters.LastName))
            {
                throw new ArgumentNullException(nameof(parameters.LastName), "Parameter is null");
            }

            if (parameters.LastName.Length < 2 || parameters.LastName.Length > 60)
            {
                throw new ArgumentException("Parameter has wrong length", nameof(parameters.LastName));
            }

            if (DateTime.Compare(parameters.DateOfBirth, new DateTime(1950, 1, 1)) < 0 || DateTime.Compare(parameters.DateOfBirth, DateTime.Now) > 0)
            {
                throw new ArgumentException("Parameter is wrong", nameof(parameters.DateOfBirth));
            }

            if (parameters.Height < 45 || parameters.Height > 252)
            {
                throw new ArgumentException("Parameter is wrong", nameof(parameters.Height));
            }

            if (parameters.Weigth < 2 || parameters.Weigth > 600)
            {
                throw new ArgumentException("Parameter is wrong", nameof(parameters.Weigth));
            }

            char temperament = char.ToUpper(parameters.Temperament, new System.Globalization.CultureInfo("en-US"));
            if (!(temperament == 'P' || temperament == 'S' || temperament == 'C' || temperament == 'M'))
            {
                throw new ArgumentException("Parameter is wrong", nameof(temperament));
            }
        }
    }
}
