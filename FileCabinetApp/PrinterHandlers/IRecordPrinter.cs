namespace FileCabinetApp.PrinterHandlers
{
    public interface IRecordPrinter
    {
        public void Print(IEnumerable<FileCabinetRecord> records);
    }
}
