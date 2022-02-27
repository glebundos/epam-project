namespace FileCabinetApp.Comparers
{
    public class WeightComparer : IComparer<FileCabinetRecord>
    {
        public int Compare(FileCabinetRecord? x, FileCabinetRecord? y)
        {
            return GetNumberLength(x.Weight).CompareTo(GetNumberLength(y.Weight));
        }

        private static int GetNumberLength(long number)
        {
            return (int)Math.Log10(number) + 1;
        }

        private static int GetNumberLength(decimal number)
        {
            var count = 0;
            while (number != (long)number)
            {
                number *= 10;
                count++;
            }

            count += GetNumberLength((long)number);
            return count;
        }

    }
}
