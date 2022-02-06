using System.Collections.ObjectModel;

namespace FileCabinetApp
{
    /// <summary>
    /// Represents interface for services with standart operations with list of records.
    /// </summary>
    public interface IFileCabinetService
    {
        /// <summary>
        /// Creates new record with given parameters.
        /// </summary>
        /// <param name="newRecord">Object with all record parameters.</param>
        /// <returns>Id of the created record or -1 in case of error.</returns>
        public int CreateRecord(FileCabinetRecord newRecord);

        /// <summary>
        /// Edits the record with the given id according to the given parameters.
        /// </summary>
        /// <param name="id">The Id of the record to change.</param>
        /// <param name="newRecord">Object with all record parameters.</param>
        public void EditRecord(int id, FileCabinetRecord newRecord);

        /// <summary>
        /// Searches for a record with given Id.
        /// </summary>
        /// <param name="id">The Id of the record to search.</param>
        /// <returns>Index of the record in list, or -1 if record with that Id was not found.</returns>
        public int RecordIndex(int id);

        /// <summary>
        /// Searches for all records with given first name (Ignoring lower case and upper case differences).
        /// </summary>
        /// <param name="firstName">The first name to search for.</param>
        /// <returns>Array of records with given first name or an empty array if there are no records with that first name.</returns>
        public IReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName);

        /// <summary>
        /// Searches for all records with given last name (Ignoring lower case and upper case differences).
        /// </summary>
        /// <param name="lastName">The last name to search for.</param>
        /// <returns>Array of records with given last name or an empty array if there are no records with that last name.</returns>
        public IReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastName);

        /// <summary>
        /// Searches for all records with given date of birth.
        /// </summary>
        /// <param name="dateOfBirth">The date of birth to search for.</param>
        /// <returns>Array of records with given date of birth or an empty array if there are no records with that date of birth.</returns>
        public IReadOnlyCollection<FileCabinetRecord> FindByDateOfBirth(DateTime dateOfBirth);

        /// <summary>
        /// Gets the all records.
        /// </summary>
        /// <returns>Array of all records.</returns>
        public IReadOnlyCollection<FileCabinetRecord> GetRecords();

        /// <summary>
        /// Gets the number of all records.
        /// </summary>
        /// <returns>Number of all records.</returns>
        public int GetStat();

        /// <summary>
        /// Makes a snapshot of current records list.
        /// </summary>
        /// <returns>Snapshot of current records list.</returns>
        public FileCabinetServiceSnapshot MakeSnapshot();

        public int Restore(FileCabinetServiceSnapshot snapshot);
    }
}
