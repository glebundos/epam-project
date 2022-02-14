namespace FileCabinetApp.CommandHandlers
{
    public interface ICommandHandler
    {
        public void SetNext(ICommandHandler commandHandler);

        public void Handle(AppCommandRequest appCommandRequest);
    }
}
