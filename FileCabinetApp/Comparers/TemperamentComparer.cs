namespace FileCabinetApp.Comparers
{
    /// <summary>
    /// Comparer class for comparing FileCabinetRecords by temperamentLength.
    /// </summary>
    public class TemperamentComparer : IComparer<FileCabinetRecord>
    {
        /// <inheritdoc/>
        public int Compare(FileCabinetRecord? x, FileCabinetRecord? y)
        {
            if (x.Temperament == y.Temperament)
            {
                return 0;
            }

            int xpos = x.Temperament switch {
                'C' => 1,
                'S' => 2,
                'P' => 3,
                'M' => 4,
                _ => -1,
            };

            int ypos = x.Temperament switch
            {
                'C' => 1,
                'S' => 2,
                'P' => 3,
                'M' => 4,
                _ => -1,
            };

            return xpos.CompareTo(ypos);
        }
    }
}
