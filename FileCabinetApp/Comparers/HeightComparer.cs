namespace FileCabinetApp.Comparers
{
    public class HeightComparer : IComparer<FileCabinetRecord>
    {
        public int Compare(FileCabinetRecord? x, FileCabinetRecord? y)
        {
            return x.Height.CompareTo(y.Height);
        }
    }
}
