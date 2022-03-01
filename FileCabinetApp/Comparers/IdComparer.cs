namespace FileCabinetApp.Comparers
{
    public class IdComparer : IComparer<FileCabinetRecord>
    {
        public int Compare(FileCabinetRecord? x, FileCabinetRecord? y)
        {
            return x.Id.CompareTo(y.Id);
        }
    }
}
