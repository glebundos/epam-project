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
        /// Removes records with given id.
        /// </summary>
        /// <param name="id"> - id of the record to remove.</param>
        /// <returns>true - if record was deleted, otherwise - false.</returns>
        public bool RemoveRecord(int id);

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
        public IEnumerable<FileCabinetRecord> FindByFirstName(string firstName);

        /// <summary>
        /// Searches for all records with given last name (Ignoring lower case and upper case differences).
        /// </summary>
        /// <param name="lastName">The last name to search for.</param>
        /// <returns>Array of records with given last name or an empty array if there are no records with that last name.</returns>
        public IEnumerable<FileCabinetRecord> FindByLastName(string lastName);

        /// <summary>
        /// Searches for all records with given date of birth.
        /// </summary>
        /// <param name="dateOfBirth">The date of birth to search for.</param>
        /// <returns>Array of records with given date of birth or an empty array if there are no records with that date of birth.</returns>
        public IEnumerable<FileCabinetRecord> FindByDateOfBirth(DateTime dateOfBirth);

        /// <summary>
        /// Gets record with a specific id.
        /// </summary>
        /// <param name="id"> - id of record.</param>
        /// <returns>Record with a specific id or throws an exception.</returns>
        public FileCabinetRecord GetById(int id);

        /// <summary>
        /// Gets the all records.
        /// </summary>
        /// <returns>Array of all records.</returns>
        public List<FileCabinetRecord> GetRecords();

        /// <summary>
        /// Gets the number of all records.
        /// </summary>
        /// <returns>Number of all records.</returns>
        /// <param name="removedCount"> - count of removed records.</param>
        public int GetStat(out int removedCount);

        /// <summary>
        /// Makes a snapshot of current records list.
        /// </summary>
        /// <returns>Snapshot of current records list.</returns>
        public FileCabinetServiceSnapshot MakeSnapshot();

        /// <summary>
        /// Restores records list from given snapshot.
        /// </summary>
        /// <param name="snapshot"> - snapshot of records list.</param>
        /// <returns>Count of restored records.</returns>
        public int Restore(FileCabinetServiceSnapshot snapshot);

        /// <summary>
        /// Purges all removed records.
        /// </summary>
        /// <returns>Count of purged records.</returns>
        public int Purge();
    }
}
