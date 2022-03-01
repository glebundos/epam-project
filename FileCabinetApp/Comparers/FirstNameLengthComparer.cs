namespace FileCabinetApp.Comparers
{
    public class FirstNameLengthComparer : IComparer<FileCabinetRecord>
    {
        public int Compare(FileCabinetRecord? x, FileCabinetRecord? y)
        {
            return x.FirstName.Length.CompareTo(y.FirstName.Length);
        }
    }
}
