namespace FileCabinetApp.Comparers
{
    public class TemperamentComparer : IComparer<FileCabinetRecord>
    {
        public int Compare(FileCabinetRecord? x, FileCabinetRecord? y)
        {
            if (x.Temperament == y.Temperament)
            {
                return 0;
            }

            int xPos;
            switch (x.Temperament)
            {
                case 'C':
                    xPos = 1;
                    break;
                case 'S':
                    xPos = 2;
                    break;
                case 'P':
                    xPos = 3;
                    break;
                case 'M':
                    xPos = 4;
                    break;
                default:
                    xPos = -1;
                    break;
            }

            int yPos;
            switch (y.Temperament)
            {
                case 'C':
                    yPos = 1;
                    break;
                case 'S':
                    yPos = 2;
                    break;
                case 'P':
                    yPos = 3;
                    break;
                case 'M':
                    yPos = 4;
                    break;
                default:
                    yPos = -1;
                    break;
            }

            return xPos.CompareTo(yPos);
        }
    }
}
