using System.Diagnostics;

namespace FileCabinetApp
{
    public class ServiceMeter : IFileCabinetService
    {
        private IFileCabinetService service;

        public ServiceMeter(IFileCabinetService service)
        {
            this.service = service;
        }

        public int CreateRecord(FileCabinetRecord newRecord)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            int result = this.service.CreateRecord(newRecord);
            stopWatch.Stop();
            long elapsedTicks = stopWatch.Elapsed.Ticks;
            Console.WriteLine($"Create method execution duration is {elapsedTicks} ticks.");
            return result;
        }

        public void EditRecord(int id, FileCabinetRecord newRecord)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            this.service.EditRecord(id, newRecord);
            stopWatch.Stop();
            long elapsedTicks = stopWatch.Elapsed.Ticks;
            Console.WriteLine($"Edit method execution duration is {elapsedTicks} ticks.");
        }

        public IReadOnlyCollection<FileCabinetRecord> FindByDateOfBirth(DateTime dateOfBirth)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            var result = this.service.FindByDateOfBirth(dateOfBirth);
            stopWatch.Stop();
            long elapsedTicks = stopWatch.Elapsed.Ticks;
            Console.WriteLine($"FindByDateOfBirth method execution duration is {elapsedTicks} ticks.");
            return result;
        }

        public IReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            var result = this.service.FindByFirstName(firstName);
            stopWatch.Stop();
            long elapsedTicks = stopWatch.Elapsed.Ticks;
            Console.WriteLine($"FindByFirstName method execution duration is {elapsedTicks} ticks.");
            return result;
        }

        public IReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastName)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            var result = this.service.FindByLastName(lastName);
            stopWatch.Stop();
            long elapsedTicks = stopWatch.Elapsed.Ticks;
            Console.WriteLine($"FindByLastName method execution duration is {elapsedTicks} ticks.");
            return result;
        }

        public List<FileCabinetRecord> GetRecords()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            var result = this.service.GetRecords();
            stopWatch.Stop();
            long elapsedTicks = stopWatch.Elapsed.Ticks;
            Console.WriteLine($"GetRecords method execution duration is {elapsedTicks} ticks.");
            return result;
        }

        public int GetStat(out int removedCount)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            var result = this.service.GetStat(out removedCount);
            stopWatch.Stop();
            long elapsedTicks = stopWatch.Elapsed.Ticks;
            Console.WriteLine($"GetStat method execution duration is {elapsedTicks} ticks.");
            return result;
        }

        public FileCabinetServiceSnapshot MakeSnapshot()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            var result = this.service.MakeSnapshot();
            stopWatch.Stop();
            long elapsedTicks = stopWatch.Elapsed.Ticks;
            Console.WriteLine($"MakeSnapshot method execution duration is {elapsedTicks} ticks.");
            return result;
        }

        public int Purge()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            var result = this.service.Purge();
            stopWatch.Stop();
            long elapsedTicks = stopWatch.Elapsed.Ticks;
            Console.WriteLine($"Purge method execution duration is {elapsedTicks} ticks.");
            return result;
        }

        public int RecordIndex(int id)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            var result = this.service.RecordIndex(id);
            stopWatch.Stop();
            long elapsedTicks = stopWatch.Elapsed.Ticks;
            Console.WriteLine($"RecordIndex method execution duration is {elapsedTicks} ticks.");
            return result;
        }

        public bool RemoveRecord(int id)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            var result = this.service.RemoveRecord(id);
            stopWatch.Stop();
            long elapsedTicks = stopWatch.Elapsed.Ticks;
            Console.WriteLine($"RemoveRecord method execution duration is {elapsedTicks} ticks.");
            return result;
        }

        public int Restore(FileCabinetServiceSnapshot snapshot)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            var result = this.service.Restore(snapshot);
            stopWatch.Stop();
            long elapsedTicks = stopWatch.Elapsed.Ticks;
            Console.WriteLine($"Restore method execution duration is {elapsedTicks} ticks.");
            return result;
        }
    }
}
