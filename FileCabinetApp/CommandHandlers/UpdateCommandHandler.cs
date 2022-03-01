namespace FileCabinetApp.CommandHandlers
{
    public class UpdateCommandHandler : ServiceCommandHandlerBase
    {
        private ValidatorsSettings settings;

        public UpdateCommandHandler(IFileCabinetService service, ValidatorsSettings settings)
            : base(service)
        {
            this.service = service;
            this.settings = settings;
        }

        public override void Handle(AppCommandRequest request)
        {
            if (!string.IsNullOrEmpty(request.Command) && request.Command == "update")
            {
                try
                {
                    this.Update(request);
                    Memoizer.Clear();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Update failed: " + e.Message);
                }

            }
            else
            {
                this.nextHandler.Handle(request);
            }
        }

        private void Update(AppCommandRequest request)
        {
            if (string.IsNullOrEmpty(request.Parameters))
            {
                throw new ArgumentException("Parameters were empty");
            }

            string[] oldParams;
            string[] oldValues;
            string[] newParams;
            string[] newValues;
            SplitParameters(request.Parameters, out oldParams, out oldValues, out newParams, out newValues);

            for (int i = 0; i < oldParams.Length; i++)
            {
                if (oldParams[i] == "id")
                {
                    string temp = oldParams[0];
                    oldParams[0] = oldParams[i];
                    oldParams[i] = temp;

                    temp = oldValues[0];
                    oldValues[0] = oldValues[i];
                    oldValues[i] = temp;
                    break;
                }
            }

            if (oldParams[0] == "id")
            {
                var record = this.service.GetById(Convert.ToInt32(oldValues[0]));
                for (int i = 1; i < oldParams.Length; i++)
                {
                    switch (oldParams[i])
                    {
                        case "firstname":
                            if (record.FirstName.ToLower() != oldValues[i].ToLower())
                            {
                                throw new ArgumentException("No such record");
                            }

                            break;
                        case "lastname":
                            if (record.LastName.ToLower() != oldValues[i].ToLower())
                            {
                                throw new ArgumentException("No such record");
                            }

                            break;
                        case "dateofbirth":
                            if (record.DateOfBirth != DateTime.Parse(oldValues[i], new System.Globalization.CultureInfo("en-US")))
                            {
                                throw new ArgumentException("No such record");
                            }

                            break;
                    }
                }

                string firstName = record.FirstName;
                string lastName = record.LastName;
                DateTime dateOfBirth = record.DateOfBirth;
                short height = record.Height;
                decimal weight = record.Weight;
                char temperament = record.Temperament;

                for (int i = 0; i < newParams.Length; i++)
                {
                    switch (newParams[i])
                    {
                        case "firstname":
                            firstName = newValues[i];
                            break;
                        case "lastname":
                            lastName = newValues[i];
                            break;
                        case "dateofbirth":
                            dateOfBirth = DateTime.Parse(newValues[i], new System.Globalization.CultureInfo("en-US"));
                            break;
                        case "height":
                            height = Convert.ToInt16(newValues[i]);
                            break;
                        case "weight":
                            weight = Convert.ToDecimal(newValues[i], System.Globalization.CultureInfo.InvariantCulture);
                            break;
                        case "temperament":
                            temperament = char.ToUpper(Convert.ToChar(newValues[i]));
                            break;
                    }
                }

                var newRecord = new FileCabinetRecord()
                {
                    Id = record.Id,
                    FirstName = firstName,
                    LastName = lastName,
                    DateOfBirth = dateOfBirth,
                    Height = height,
                    Weight = weight,
                    Temperament = temperament,
                };

                this.service.EditRecord(record.Id, newRecord);
            }
            else
            {
                List<FileCabinetRecord> oldRecords = new List<FileCabinetRecord>();
                switch (oldParams[0])
                {
                    case "firstname":
                        oldRecords = this.service.FindByFirstName(oldValues[0].ToLower()).ToList();
                        break;
                    case "lastname":
                        oldRecords = this.service.FindByLastName(oldValues[0].ToLower()).ToList();
                        break;
                    case "dateofbirth":
                        oldRecords = this.service.FindByDateOfBirth(DateTime.Parse(oldValues[0], new System.Globalization.CultureInfo("en-US"))).ToList();
                        break;
                }

                List<FileCabinetRecord> oldValidRecords = new List<FileCabinetRecord>(oldRecords);
                foreach (var oldRecord in oldRecords)
                {
                    for (int i = 0; i < oldParams.Length; i++)
                    {
                        switch (oldParams[i])
                        {
                            case "firstname":
                                if (oldRecord.FirstName.ToLower() != oldValues[i].ToLower())
                                {
                                    oldValidRecords.Remove(oldRecord);
                                }

                                break;
                            case "lastname":
                                if (oldRecord.LastName.ToLower() != oldValues[i].ToLower())
                                {
                                    oldValidRecords.Remove(oldRecord);
                                }

                                break;
                            case "dateofbirth":
                                if (oldRecord.DateOfBirth != DateTime.Parse(oldValues[i], new System.Globalization.CultureInfo("en-US")))
                                {
                                    oldValidRecords.Remove(oldRecord);
                                }

                                break;
                        }
                    }
                }

                if (!oldValidRecords.Any())
                {
                    throw new ArgumentException("No records with such parameters");
                }

                for (int i = 0; i < oldValidRecords.Count; i++)
                {
                    string firstName = oldValidRecords[i].FirstName;
                    string lastName = oldValidRecords[i].LastName;
                    DateTime dateOfBirth = oldValidRecords[i].DateOfBirth;
                    short height = oldValidRecords[i].Height;
                    decimal weight = oldValidRecords[i].Weight;
                    char temperament = oldValidRecords[i].Temperament;

                    for (int j = 0; j < newParams.Length; j++)
                    {
                        switch (newParams[j])
                        {
                            case "firstname":
                                firstName = newValues[j];
                                break;
                            case "lastname":
                                lastName = newValues[j];
                                break;
                            case "dateofbirth":
                                dateOfBirth = DateTime.Parse(newValues[j], new System.Globalization.CultureInfo("en-US"));
                                break;
                            case "height":
                                height = Convert.ToInt16(newValues[j]);
                                break;
                            case "weight":
                                weight = Convert.ToDecimal(newValues[j], System.Globalization.CultureInfo.InvariantCulture);
                                break;
                            case "temperament":
                                temperament = char.ToUpper(Convert.ToChar(newValues[j], new System.Globalization.CultureInfo("en-US")), new System.Globalization.CultureInfo("en-US"));
                                break;
                        }
                    }

                    var newRecord = new FileCabinetRecord()
                    {
                        Id = oldValidRecords[i].Id,
                        FirstName = firstName,
                        LastName = lastName,
                        DateOfBirth = dateOfBirth,
                        Height = height,
                        Weight = weight,
                        Temperament = temperament,
                    };

                    this.service.EditRecord(oldValidRecords[i].Id, newRecord);
                }
            }
        }

        private static string RemoveWhitespace(string input)
        {
            return new string(input.ToCharArray()
                .Where(c => !char.IsWhiteSpace(c))
                .ToArray());
        }

        private static void SplitParameters(string parameters, out string[] oldParams, out string[] oldValues, out string[] newParams, out string[] newValues)
        {
            if (!parameters.Contains("set", StringComparison.InvariantCultureIgnoreCase)
                || !parameters.Contains("where", StringComparison.InvariantCultureIgnoreCase))
            {
                throw new ArgumentException("Wrong syntax");
            }

            parameters = RemoveWhitespace(parameters);
            string newPart = parameters.Split("where")[0];

            if (!newPart.Contains("set", StringComparison.InvariantCultureIgnoreCase))
            {
                throw new ArgumentException("Wrong syntax");
            }

            newPart = newPart.Remove(0, 3);
            string[] newParamsValues = new string[1];
            if (newPart.Contains(',', StringComparison.InvariantCultureIgnoreCase))
            {
                newParamsValues = newPart.Split(',');
            }
            else
            {
                newParamsValues[0] = newPart;
            }

            newParams = new string[newParamsValues.Length];
            newValues = new string[newParamsValues.Length];
            for (int i = 0; i < newParamsValues.Length; i++)
            {
                newParams[i] = newParamsValues[i].Split('=')[0];
                newValues[i] = newParamsValues[i].Split('=')[1];
            }

            string oldPart = parameters.Split("where")[1];
            string[] oldParamsValues = new string[1];
            if (oldPart.Contains("and", StringComparison.InvariantCultureIgnoreCase))
            {
                oldParamsValues = oldPart.Split("and");
            }
            else
            {
                oldParamsValues[0] = oldPart;
            }

            oldParams = new string[oldParamsValues.Length];
            oldValues = new string[oldParamsValues.Length];
            for (int i = 0; i < oldParamsValues.Length; i++)
            {
                oldParams[i] = oldParamsValues[i].Split('=')[0];
                oldValues[i] = oldParamsValues[i].Split('=')[1];
            }

            if (oldParams.Length != oldValues.Length || newParams.Length != newValues.Length)
            {
                throw new ArgumentException("Wrong syntax (wrong parameters count)");
            }

            for (int i = 0; i < oldParams.Length; i++)
            {
                oldParams[i] = oldParams[i].ToLower();
                if (oldParams[i] != "id" && oldParams[i] != "firstname" && oldParams[i] != "lastname" && oldParams[i] != "dateofbirth")
                {
                    throw new ArgumentException("Wrong parameter: " + oldParams[i]);
                }
            }

            for (int i = 0; i < newParams.Length; i++)
            {
                newParams[i] = newParams[i].ToLower();
                if (newParams[i] != "firstname" && newParams[i] != "lastname" && newParams[i] != "dateofbirth"
                    && newParams[i] != "height" && newParams[i] != "weight" && newParams[i] != "temperament")
                {
                    throw new ArgumentException("Wrong parameter: " + newParams[i]);
                }
            }
        }
    }
}
