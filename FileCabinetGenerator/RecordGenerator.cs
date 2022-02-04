using System.Text;
using FileCabinetApp;

namespace FileCabinetGenerator
{
    public static class RecordGenerator
    {
        private static Random gen = new Random();
        public static IReadOnlyCollection<FileCabinetRecord> GenerateRecords(Config config)
        {
            List<FileCabinetRecord> fileCabinetRecords = new List<FileCabinetRecord>();
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            for (int i = config.StartId; i < config.StartId + config.Amount; i++)
            {
                string firstName = GenerateString(chars, 4, 60);
                string lastName = GenerateString(chars, 4, 60);
                DateTime dob = GenerateDateTime();
                short height = (short)gen.Next(45, 253);
                decimal weight = gen.Next(2, 601);
                char temperament = GenerateTemperament(new char[] { 'P', 'C', 'S', 'C', });
                FileCabinetRecord record = new FileCabinetRecord()
                {
                    Id = i,
                    FirstName = GenerateString(chars, 4, 60),
                    LastName = GenerateString(chars, 4, 60),
                    DateOfBirth = GenerateDateTime(),
                    Height = (short)gen.Next(45, 253),
                    Weight = gen.Next(2, 601),
                    Temperament = GenerateTemperament(new char[] { 'P', 'C', 'S', 'C', }),
                };

                fileCabinetRecords.Add(record);
            }

            return fileCabinetRecords;
        }

        private static char GenerateTemperament(char[] temperaments)
        {
            return temperaments[gen.Next(0, temperaments.Length)];
        }

        private static DateTime GenerateDateTime()
        {
            DateTime start = new DateTime(1950, 1, 1);
            int range = (DateTime.Today - start).Days;
            return start.AddDays(gen.Next(range));
        }

        private static string GenerateString(string chars, int minLength, int maxLength)
        {
            StringBuilder builder = new StringBuilder();
            gen = new Random();
            int lenght = gen.Next(minLength, maxLength + 1);
            for (int i = 0; i < lenght; i++)
            {
                builder.Append(chars[gen.Next(0, chars.Length)]);
            }

            return builder.ToString();
        }
    }
}
