using System.Xml.Serialization;

namespace FileCabinetApp
{
    /// <summary>
    /// Contains all record parameters and its Id in the FileCabinet.
    /// </summary>
    [Serializable]
    public class FileCabinetRecord : IEquatable<FileCabinetRecord>
    {
        /// <summary>
        /// Gets the value that matches the Id of the record.
        /// </summary>
        /// <value>
        /// The value that matches the Id of the record.
        /// </value>
        [XmlAttribute]
        public int Id { get; init; }

        /// <summary>
        /// Gets the first name of the record.
        /// </summary>
        /// <value>
        /// The first name of the record.
        /// </value>
        public string? FirstName { get; init; }

        /// <summary>
        /// Gets the last name of the record.
        /// </summary>
        /// <value>
        /// The last name of the record.
        /// </value>
        public string? LastName { get; init; }

        /// <summary>
        /// Gets the date of birth of the record.
        /// </summary>
        /// <value>
        /// Date of birth of the record.
        /// </value>
        public DateTime DateOfBirth { get; init; }

        /// <summary>
        /// Gets the height of the record.
        /// </summary>
        /// <value>
        /// Height of the record in cm.
        /// </value>
        public short Height { get; init; }

        /// <summary>
        /// Gets the weight of the record.
        /// </summary>
        /// <value>
        /// Weight of the record in kg.
        /// </value>
        public decimal Weight { get; init; }

        /// <summary>
        /// Gets the temperament of the record.
        /// </summary>
        /// <value>
        /// First letter of temperament.
        /// </value>
        public char Temperament { get; init; }

        public bool Equals(FileCabinetRecord? other)
        {
            if (ReferenceEquals(other, null))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return this.Id.Equals(other.Id);
        }

        public override int GetHashCode()
        {
            return this.Id == 0 ? 0 : this.Id.GetHashCode();
        }
    }
}
