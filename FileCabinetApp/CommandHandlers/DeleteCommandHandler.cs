using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Command handler class for delete command.
    /// </summary>
    public class DeleteCommandHandler : ServiceCommandHandlerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteCommandHandler"/> class.
        /// </summary>
        /// <param name="service"> - fileCabinetService to manipulate with.</param>
        public DeleteCommandHandler(IFileCabinetService service)
            : base(service)
        {
        }

        /// <inheritdoc/>
        public override void Handle(AppCommandRequest request)
        {
            if (!string.IsNullOrEmpty(request.Command) && request.Command.Equals("delete", StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    this.Delete(request);
                    Memoizer.Clear();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Delete error: " + e.Message);
                }
            }
            else
            {
                this.NextHandler.Handle(request);
            }
        }

        private static string RemoveWhitespace(string input)
        {
            return new string(input.ToCharArray()
                .Where(c => !char.IsWhiteSpace(c))
                .ToArray());
        }

        private void Delete(AppCommandRequest request)
        {
            if (string.IsNullOrEmpty(request.Parameters))
            {
                throw new ArgumentException("Parameters were empty");
            }

            if (request.Parameters.Split(' ')[0] != "where")
            {
                throw new ArgumentException("Wrong parameters, use \"delete where parameter = value\"");
            }

            string parameter = RemoveWhitespace(request.Parameters.Remove(0, 6)).Split('=')[0].ToLower();

            if (parameter == "id")
            {
                int idtodelete = Convert.ToInt32(RemoveWhitespace(request.Parameters.Remove(0, 6)).Split('=')[1]);
                if (this.Service.RemoveRecord(idtodelete))
                {
                    Console.WriteLine("Record #" + idtodelete + " is deleted.");
                }
                else
                {
                    Console.WriteLine("Record #" + idtodelete + " doesn't exists.");
                }
            }
            else if (parameter == "firstname")
            {
                var foundedRecords = this.Service.FindByFirstName(RemoveWhitespace(request.Parameters.Remove(0, 6)).Split('=')[1].ToLower());
                if (!foundedRecords.Any())
                {
                    throw new ArgumentException("No records with such parameter");
                }

                StringBuilder outputStringBuilder = new StringBuilder();
                outputStringBuilder.Append("Record(s) ");
                foreach (var record in foundedRecords.ToList())
                {
                    if (this.Service.RemoveRecord(record.Id))
                    {
                        outputStringBuilder.Append("#" + record.Id + ", ");
                    }
                }

                outputStringBuilder.Remove(outputStringBuilder.Length - 2, 2);
                outputStringBuilder.Append(" are deleted");
                Console.WriteLine(outputStringBuilder);
            }
            else if (parameter == "lastname")
            {
                var foundedRecords = this.Service.FindByLastName(RemoveWhitespace(request.Parameters.Remove(0, 6)).Split('=')[1].ToLower());
                if (!foundedRecords.Any())
                {
                    throw new ArgumentException("No records with such parameter");
                }

                StringBuilder outputStringBuilder = new StringBuilder();
                outputStringBuilder.Append("Record(s) ");
                foreach (var record in foundedRecords.ToList())
                {
                    if (this.Service.RemoveRecord(record.Id))
                    {
                        outputStringBuilder.Append("#" + record.Id + ", ");
                    }
                }

                outputStringBuilder.Remove(outputStringBuilder.Length - 2, 2);
                outputStringBuilder.Append(" are deleted");
                Console.WriteLine(outputStringBuilder);
            }
            else if (parameter == "dateofbirth")
            {
                var foundedRecords = this.Service.FindByDateOfBirth(DateTime.Parse(RemoveWhitespace(request.Parameters.Remove(0, 6)).Split('=')[1], new System.Globalization.CultureInfo("en-US")));
                if (!foundedRecords.Any())
                {
                    throw new ArgumentException("No records with such parameter");
                }

                StringBuilder outputStringBuilder = new StringBuilder();
                outputStringBuilder.Append("Record(s) ");
                foreach (var record in foundedRecords.ToList())
                {
                    if (this.Service.RemoveRecord(record.Id))
                    {
                        outputStringBuilder.Append("#" + record.Id + ", ");
                    }
                }

                outputStringBuilder.Remove(outputStringBuilder.Length - 2, 2);
                outputStringBuilder.Append(" are deleted");
                Console.WriteLine(outputStringBuilder);
            }
            else
            {
                throw new ArgumentException("Wrong parameter: " + parameter);
            }
        }
    }
}
