namespace FileCabinetApp.CommandHandlers
{
    public class RemoveCommandHandler : ServiceCommandHandlerBase
    {
        public RemoveCommandHandler(IFileCabinetService service)
            : base(service)
        {
            this.service = service;
        }

        public override void Handle(AppCommandRequest request)
        {
            if (!string.IsNullOrEmpty(request.Command) && request.Command == "remove")
            {
                try
                {
                    int id = Convert.ToInt32(request.Parameters);
                    if (this.service.RemoveRecord(id))
                    {
                        Console.WriteLine("Record #" + id + " is removed.");
                    }
                    else
                    {
                        Console.WriteLine("Record #" + id + " doesn't exists.");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Remove error: " + e.Message);
                }
            }
            else
            {
                this.nextHandler.Handle(request);
            }
        }
    }
}
