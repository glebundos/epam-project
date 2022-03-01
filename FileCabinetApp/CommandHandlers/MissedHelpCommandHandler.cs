using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    public class MissedHelpCommandHandler : CommandHandlerBase
    {
        private static string[] commands = new string[]
        {
            "help", "exit", "stat", "create", "export", "import", "purge", "insert", "delete", "update", "select",
        };

        public override void Handle(AppCommandRequest request)
        {
            Console.WriteLine($"There is no '{request.Command}' command.");
            string input = request.Command.ToLower();
            List<string> possibleCommands = new List<string>();
            foreach (var command in commands)
            {
                StringBuilder commandSB = new StringBuilder(command);
                int counter = 0;
                for (int i = 0; i < input.Length; i++)
                {
                    if (counter >= command.Length / 2)
                    {
                        break;
                    }

                    for (int j = 0; j < commandSB.Length; j++)
                    {
                        if (input[i] == commandSB[j])
                        {
                            if (Math.Abs(j - i) < 2 + counter)
                            {
                                counter++;
                            }
                            else
                            {
                                counter += 1 / Math.Abs(j - i);
                            }

                            commandSB.Remove(j, 1);
                            break;
                        }
                    }
                }

                if (counter >= command.Length / 2)
                {
                    possibleCommands.Add(command);
                }
            }

            if (possibleCommands.Any())
            {
                Console.WriteLine("The most similar commands are:");
                foreach (var command in possibleCommands)
                {
                    Console.WriteLine("\t" + command);
                }
            }

            Console.WriteLine();
        }
    }
}