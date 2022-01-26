namespace FileCabinetApp
{
    /// <summary>
    /// Contains all record parameters and its Id in the FileCabinet.
    /// </summary>
    public class FileCabinetRecord
    {
        /// <summary>
        /// Gets or sets the value that matches the Id of the record.
        /// </summary>
        /// <value>
        /// The value that matches the Id of the record.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the first name of the record.
        /// </summary>
        /// <value>
        /// The first name of the record.
        /// </value>
        public string? FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name of the record.
        /// </summary>
        /// <value>
        /// The last name of the record.
        /// </value>
        public string? LastName { get; set; }

        /// <summary>
        /// Gets or sets the date of birth of the record.
        /// </summary>
        /// <value>
        /// Date of birth of the record.
        /// </value>
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// Gets or sets the height of the record.
        /// </summary>
        /// <value>
        /// Height of the record in cm.
        /// </value>
        public short Height { get; set; }

        /// <summary>
        /// Gets or sets the weight of the record.
        /// </summary>
        /// <value>
        /// Weight of the record in kg.
        /// </value>
        public decimal Weigth { get; set; }

        /// <summary>
        /// Gets or sets the temperament of the record.
        /// </summary>
        /// <value>
        /// First letter of temperament.
        /// </value>
        public char Temperament { get; set; }
    }
}
