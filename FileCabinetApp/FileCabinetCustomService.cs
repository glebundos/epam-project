namespace FileCabinetApp
{
    /// <summary>
    /// Custom configuration of FileCabinetService class (Adult configuration).
    /// </summary>
    public class FileCabinetCustomService : FileCabinetService
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

            if (parameters.FirstName.Length < 4 || parameters.FirstName.Length > 30)
            {
                throw new ArgumentException("Parameter has wrong length", nameof(parameters.FirstName));
            }

            if (string.IsNullOrWhiteSpace(parameters.LastName))
            {
                throw new ArgumentNullException(nameof(parameters.LastName), "Parameter is null");
            }

            if (parameters.LastName.Length < 4 || parameters.LastName.Length > 30)
            {
                throw new ArgumentException("Parameter has wrong length", nameof(parameters.LastName));
            }

            DateTime dateTimeNow = DateTime.Now;
            if (DateTime.Compare(parameters.DateOfBirth, new DateTime(dateTimeNow.Year - 18, dateTimeNow.Month, dateTimeNow.Day)) > 0)
            {
                throw new ArgumentException("Parameter is wrong", nameof(parameters.DateOfBirth));
            }

            if (parameters.Height < 60 || parameters.Height > 252)
            {
                throw new ArgumentException("Parameter is wrong", nameof(parameters.Height));
            }

            if (parameters.Weigth < 25 || parameters.Weigth > 600)
            {
                throw new ArgumentException("Parameter is wrong", nameof(parameters.Weigth));
            }

            char temperament = char.ToUpper(parameters.Temperament, new System.Globalization.CultureInfo("en-US"));
            if (!(temperament == 'P' || temperament == 'S' || temperament == 'C'))
            {
                throw new ArgumentException("Parameter is wrong", nameof(temperament));
            }
        }
    }
}
