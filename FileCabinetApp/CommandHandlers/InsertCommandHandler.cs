namespace FileCabinetApp.CommandHandlers
{
    public class InsertCommandHandler : ServiceCommandHandlerBase
    {
        private ValidatorsSettings settings;

        public InsertCommandHandler (IFileCabinetService service, ValidatorsSettings settings)
            : base(service)
        {
            this.settings = settings;
        }

        public override void Handle(AppCommandRequest request)
        {
            if (!string.IsNullOrEmpty(request.Command) && request.Command.Equals("insert", StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    this.Insert(request);
                    Memoizer.Clear();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Insert failed: " + e.Message);
                }
            }
            else
            {
                this.nextHandler.Handle(request);
            }
        }

        private void Insert(AppCommandRequest request)
        {
            if (string.IsNullOrEmpty(request.Parameters))
            {
                throw new ArgumentException("Parameters were empty");
            }

            string[] parameters = RemoveWhitespace(request.Parameters.Split("values")[0])[1..^1].ToLower().Split(',');
            if (parameters.Length != 7)
            {
                throw new ArgumentException("Invalid parameters count");
            }

            string[] values = RemoveWhitespace(request.Parameters.Split("values")[1])[1..^1].Split(',');
            if (values.Length != parameters.Length)
            {
                throw new ArgumentException("Invalid values count");
            }

            int id = -1;
            string firstname = string.Empty;
            string lastname = string.Empty;
            DateTime dateOfBirth = DateTime.Now;
            short height = -1;
            decimal weight = -1;
            char temperament = '\0';

            for (int i = 0; i < parameters.Length; i++)
            {
                switch (parameters[i])
                {
                    case "id":
                        id = Convert.ToInt32(values[i]);
                        break;
                    case "firstname":
                        firstname = values[i];
                        break;
                    case "lastname":
                        lastname = values[i];
                        break;
                    case "dateofbirth":
                        dateOfBirth = DateTime.Parse(values[i], new System.Globalization.CultureInfo("en-US"));
                        break;
                    case "height":
                        height = Convert.ToInt16(values[i]);
                        break;
                    case "weight":
                        weight = Convert.ToDecimal(values[i], System.Globalization.CultureInfo.InvariantCulture);
                        break;
                    case "temperament":
                        temperament = char.ToUpper(Convert.ToChar(values[i]));
                        break;
                    default:
                        throw new ArgumentException("Wrong parameter name: ", parameters[i]);
                }
            }

            if (this.service.RecordIndex(id) != -1)
            {
                throw new ArgumentException("Record with given id is already exists.");
            }

            var record = new FileCabinetRecord()
            {
                Id = id,
                FirstName = firstname,
                LastName = lastname,
                DateOfBirth = dateOfBirth,
                Height = height,
                Weight = weight,
                Temperament = temperament,
            };

            if (ValidatorBuilder.CreateCompositeValidator(this.settings).ValidateParameters(record))
            {
                this.service.CreateRecord(record);
            }
            else
            {
                throw new ArgumentException("Invalid values");
            }
        }

        private static string RemoveWhitespace(string input)
        {
            return new string(input.ToCharArray()
                .Where(c => !char.IsWhiteSpace(c))
                .ToArray());
        }
    }
}
