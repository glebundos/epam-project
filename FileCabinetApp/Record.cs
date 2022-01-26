namespace FileCabinetApp
{
    /// <summary>
    /// Contains all record parameters.
    /// </summary>
    public class Record
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Record"/> class.
        /// </summary>
        /// <param name="firstName">The first name.</param>
        /// <param name="lastName">The last name.</param>
        /// <param name="dateOfBirth">Date of birth.</param>
        /// <param name="height">Height in cm.</param>
        /// <param name="weight">Weight in kg.</param>
        /// <param name="temperament">The first letter of temperament.</param>
        public Record(string firstName, string lastName, DateTime dateOfBirth, short height, decimal weight, char temperament)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.DateOfBirth = dateOfBirth;
            this.Height = height;
            this.Weigth = weight;
            this.Temperament = temperament;
        }

        /// <summary>
        /// Gets the first name of the record.
        /// </summary>
        /// <value>
        /// The first name of the record.
        /// </value>
        public string? FirstName { get; }

        /// <summary>
        /// Gets the last name of the record.
        /// </summary>
        /// <value>
        /// The last name of the record.
        /// </value>
        public string? LastName { get; }

        /// <summary>
        /// Gets the date of birth of the record.
        /// </summary>
        /// <value>
        /// Date of birth of the record.
        /// </value>
        public DateTime DateOfBirth { get; }

        /// <summary>
        /// Gets the height of the record.
        /// </summary>
        /// <value>
        /// Height of the record in cm.
        /// </value>
        public short Height { get; }

        /// <summary>
        /// Gets the weight of the record.
        /// </summary>
        /// <value>
        /// Weight of the record in kg.
        /// </value>
        public decimal Weigth { get; }

        /// <summary>
        /// Gets the temperament of the record.
        /// </summary>
        /// <value>
        /// First letter of temperament.
        /// </value>
        public char Temperament { get; }
    }
}
