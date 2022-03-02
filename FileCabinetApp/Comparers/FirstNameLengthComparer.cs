namespace FileCabinetApp.Comparers
{
    /// <summary>
    /// Comparer class for comparing FileCabinetRecords by firstNameLengh.
    /// </summary>
    public class FirstNameLengthComparer : IComparer<FileCabinetRecord>
    {
        /// <inheritdoc/>
        public int Compare(FileCabinetRecord? x, FileCabinetRecord? y)
        {
            return x.FirstName.Length.CompareTo(y.FirstName.Length);
        }
    }
}
