namespace FileCabinetApp.PrinterHandlers
{
    public class DefaultRecordPrinter : IRecordPrinter
    {
        public void Print(IEnumerable<FileCabinetRecord> records)
        {
            foreach (var record in records)
            {
                WriteRecord(record);
            }
        }

        private static void WriteRecord(FileCabinetRecord record)
        {
            string temperament = string.Empty;
            switch (record.Temperament)
            {
                case 'P':
                    temperament = "Phlegmatic";
                    break;
                case 'S':
                    temperament = "Sanguine";
                    break;
                case 'C':
                    temperament = "Choleric";
                    break;
                case 'M':
                    temperament = "Melancholic";
                    break;
                default:
                    temperament = "MISSING";
                    break;
            }

            Console.WriteLine($"#{record.Id}, {record.FirstName}, {record.LastName}, " +
                    $"{record.DateOfBirth.ToString("yyyy-MMM-d", new System.Globalization.CultureInfo("en-US"))}, " +
                    $"{record.Height} cm, {record.Weight} kg, {temperament}");
        }
    }
}
