namespace FileCabinetApp.Comparers
{
    public class LastNameLengthComparer : IComparer<FileCabinetRecord>
    {
        public int Compare(FileCabinetRecord? x, FileCabinetRecord? y)
        {
            return x.LastName.Length.CompareTo(y.LastName.Length);
        }
    }
}
