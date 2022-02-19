namespace FileCabinetApp.CommandHandlers
{
    public class EditCommandHandler : ServiceCommandHandlerBase
    {
        private static ValidatorsSettings settings;

        public EditCommandHandler(IFileCabinetService service, ValidatorsSettings validatorSettings)
            : base(service)
        {
            this.service = service;
            settings = validatorSettings;
        }

        public override void Handle(AppCommandRequest request)
        {
            if (!string.IsNullOrEmpty(request.Command) && request.Command == "edit")
            {
                try
                {
                    string parameters = request.Parameters;
                    if (string.IsNullOrEmpty(parameters))
                    {
                        Console.WriteLine("Parameter cant be null");
                        return;
                    }

                    int id = Convert.ToInt32(parameters, new System.Globalization.CultureInfo("en-US"));
                    if (this.service.RecordIndex(id) > -1)
                    {
                        Console.Write("First name: ");
                        string firstName = (string)ReadInput(StringConverter, FirstNameValidator);

                        Console.Write("Last name: ");
                        string lastName = (string)ReadInput(StringConverter, LastNameValidator);

                        Console.Write("Date of birth: ");
                        DateTime dob = (DateTime)ReadInput(DateConverter, DateOfBirthValidator);

                        Console.Write("Height: ");
                        short height = (short)ReadInput(HeightConverter, HeightValidator);

                        Console.Write("Weight: ");
                        decimal weight = (decimal)ReadInput(WeightConverter, WeightValidator);

                        Console.Write("Temperament: ");
                        char temperament = (char)ReadInput(TemperamentConverter, TemperamentValidator);

                        FileCabinetRecord newRecord = new FileCabinetRecord()
                        {
                            Id = 0,
                            FirstName = firstName,
                            LastName = lastName,
                            DateOfBirth = dob,
                            Height = height,
                            Weight = weight,
                            Temperament = temperament,
                        };
                        this.service.EditRecord(id, newRecord);
                    }
                    else
                    {
                        Console.WriteLine($"#{id} record is not found.");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Edit error:" + e.Message);
                }
            }
            else
            {
                this.nextHandler.Handle(request);
            }
        }

        private static Tuple<bool, string> FirstNameValidator(object input)
        {
            FileCabinetRecord onlyFirstNameRecord = new FileCabinetRecord()
            {
                FirstName = input.ToString(),
                LastName = string.Empty,
                DateOfBirth = DateTime.Now,
                Height = 0,
                Weight = 0,
                Temperament = 'G',
            };
            FirstNameValidator firstNameValidator = new FirstNameValidator(settings.FirstNameLength_min, settings.FirstNameLength_max);
            return Tuple.Create(firstNameValidator.ValidateParameters(onlyFirstNameRecord), "Parameter is invalid");
        }

        private static Tuple<bool, string> LastNameValidator(object input)
        {
            FileCabinetRecord onlyLastNameRecord = new FileCabinetRecord()
            {
                FirstName = string.Empty,
                LastName = input.ToString(),
                DateOfBirth = DateTime.Now,
                Height = 0,
                Weight = 0,
                Temperament = 'G',
            };
            LastNameValidator lastNameValidator = new LastNameValidator(settings.LastNameLength_min, settings.LastNameLength_max);
            return Tuple.Create(lastNameValidator.ValidateParameters(onlyLastNameRecord), "Parameter is invalid");
        }

        private static Tuple<bool, string> DateOfBirthValidator(object input)
        {
            FileCabinetRecord onlyDateOfBirthRecord = new FileCabinetRecord()
            {
                FirstName = string.Empty,
                LastName = string.Empty,
                DateOfBirth = (DateTime)input,
                Height = 0,
                Weight = 0,
                Temperament = 'G',
            };
            DateOfBirthValidator dateOfBirthValidator = new DateOfBirthValidator(settings.DateOfBirth_min, settings.DateOfBirth_max);
            return Tuple.Create(dateOfBirthValidator.ValidateParameters(onlyDateOfBirthRecord), "Parameter is invalid");
        }

        private static Tuple<bool, string> HeightValidator(object input)
        {
            FileCabinetRecord onlyHeightRecord = new FileCabinetRecord()
            {
                FirstName = string.Empty,
                LastName = string.Empty,
                DateOfBirth = DateTime.Now,
                Height = (short)input,
                Weight = 0,
                Temperament = 'G',
            };
            HeightValidator heightValidator = new HeightValidator(settings.Height_min, settings.Height_max);
            return Tuple.Create(heightValidator.ValidateParameters(onlyHeightRecord), "Parameter is invalid");
        }

        private static Tuple<bool, string> WeightValidator(object input)
        {
            FileCabinetRecord onlyWeightRecord = new FileCabinetRecord()
            {
                FirstName = string.Empty,
                LastName = string.Empty,
                DateOfBirth = DateTime.Now,
                Height = 0,
                Weight = (decimal)input,
                Temperament = 'G',
            };
            WeightValidator weightValidator = new WeightValidator(settings.Weight_min, settings.Weight_max);
            return Tuple.Create(weightValidator.ValidateParameters(onlyWeightRecord), "Parameter is invalid");
        }

        private static Tuple<bool, string> TemperamentValidator(object input)
        {
            FileCabinetRecord onlyTemperamentValidator = new FileCabinetRecord()
            {
                FirstName = string.Empty,
                LastName = string.Empty,
                DateOfBirth = DateTime.Now,
                Height = 0,
                Weight = 0,
                Temperament = (char)input,
            };
            TemperamentValidator temperamentValidator = new TemperamentValidator(settings.AllowedTemperaments);
            return Tuple.Create(temperamentValidator.ValidateParameters(onlyTemperamentValidator), "Parameter is invalid");
        }

        private static Tuple<bool, string, object> StringConverter(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return Tuple.Create<bool, string, object>(false, "Input was null", input);
            }

            return Tuple.Create<bool, string, object>(true, "Convertating error", input);
        }

        private static Tuple<bool, string, object> DateConverter(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return Tuple.Create<bool, string, object>(false, "Input was null", input);
            }

            DateTime dateOfBirth;
            DateTime.TryParse(input, new System.Globalization.CultureInfo("en-US"), 0, out dateOfBirth);

            return Tuple.Create<bool, string, object>(true, "Convertating error", dateOfBirth);
        }

        private static Tuple<bool, string, object> HeightConverter(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return Tuple.Create<bool, string, object>(false, "Input was null", input);
            }

            short height;
            bool isConvertered = short.TryParse(input, out height);
            return Tuple.Create<bool, string, object>(isConvertered, "Convertating error", height);
        }

        private static Tuple<bool, string, object> WeightConverter(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return Tuple.Create<bool, string, object>(false, "Input was null", input);
            }

            decimal weight;
            bool isConvertered = decimal.TryParse(input, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out weight);
            return Tuple.Create<bool, string, object>(isConvertered, "Convertating error", weight);
        }

        private static Tuple<bool, string, object> TemperamentConverter(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return Tuple.Create<bool, string, object>(false, "Input was null", input);
            }

            char temperament;
            bool isConvertered = char.TryParse(input, out temperament);
            return Tuple.Create<bool, string, object>(isConvertered, "Convertating error", char.ToUpper(temperament, new System.Globalization.CultureInfo("en-US")));
        }

        private static T ReadInput<T>(Func<string, Tuple<bool, string, T>> converter, Func<T, Tuple<bool, string>> validator)
        {
            do
            {
                T value;

                var input = Console.ReadLine();
                var conversionResult = converter(input);

                if (!conversionResult.Item1)
                {
                    Console.WriteLine($"Conversion failed: {conversionResult.Item2}. Please, correct your input.");
                    continue;
                }

                value = conversionResult.Item3;

                var validationResult = validator(value);
                if (!validationResult.Item1)
                {
                    Console.WriteLine($"Validation failed: {validationResult.Item2}. Please, correct your input.");
                    continue;
                }

                return value;
            }
            while (true);
        }
    }
}
