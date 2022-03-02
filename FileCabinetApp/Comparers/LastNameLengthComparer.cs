namespace FileCabinetApp.Comparers
{
    /// <summary>
    /// Comparer class for comparing FileCabinetRecords by lastNameLength.
    /// </summary>
    public class LastNameLengthComparer : IComparer<FileCabinetRecord>
    {
        /// <inheritdoc/>
        public int Compare(FileCabinetRecord? x, FileCabinetRecord? y)
        {
            return x.LastName.Length.CompareTo(y.LastName.Length);
        }
    }
}
