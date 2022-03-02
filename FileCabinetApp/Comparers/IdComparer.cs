namespace FileCabinetApp.Comparers
{
    /// <summary>
    /// Comparer class for comparing FileCabinetRecords by id.
    /// </summary>
    public class IdComparer : IComparer<FileCabinetRecord>
    {
        /// <inheritdoc/>
        public int Compare(FileCabinetRecord? x, FileCabinetRecord? y)
        {
            return x.Id.CompareTo(y.Id);
        }
    }
}
