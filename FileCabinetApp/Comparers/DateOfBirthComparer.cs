namespace FileCabinetApp.Comparers
{
    /// <summary>
    /// Comparer class for comparing FileCabinetRecords by dateOfBirth.
    /// </summary>
    public class DateOfBirthComparer : IComparer<FileCabinetRecord>
    {
        /// <inheritdoc/>
        public int Compare(FileCabinetRecord? x, FileCabinetRecord? y)
        {
            return DateTime.Compare(x.DateOfBirth, y.DateOfBirth);
        }
    }
}
