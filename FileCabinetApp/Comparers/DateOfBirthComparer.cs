namespace FileCabinetApp.Comparers
{
    public class DateOfBirthComparer : IComparer<FileCabinetRecord>
    {
        public int Compare(FileCabinetRecord? x, FileCabinetRecord? y)
        {
            return DateTime.Compare(x.DateOfBirth, y.DateOfBirth);
        }
    }
}
