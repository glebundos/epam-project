namespace FileCabinetApp.CommandHandlers
{
    public class SelectCommandHandler : ServiceCommandHandlerBase
    {
        private Action<IEnumerable<FileCabinetRecord>, string[]> printer;

        public SelectCommandHandler(IFileCabinetService service, Action<IEnumerable<FileCabinetRecord>, string[]> printer)
            : base(service)
        {
            this.service = service;
            this.printer = printer;
        }

        public override void Handle(AppCommandRequest request)
        {
            if (!string.IsNullOrEmpty(request.Command) && request.Command == "select")
            {
                try
                {
                    this.Select(request);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Select failed: " + e.Message);
                }
            }
            else
            {
                this.nextHandler.Handle(request);
            }
        }

        private void Select(AppCommandRequest request)
        {
            if (string.IsNullOrEmpty(request.Parameters))
            {
                throw new ArgumentException("Parameters were empty");
            }

            var memory = Memoizer.Remember(request);
            if (memory == null)
            {
                string[] outParams;
                string[] searchParams;
                string[] searchValues;
                bool andOperator;
                SplitParameters(request.Parameters, out outParams, out searchParams, out searchValues, out andOperator);

                if (searchParams[0] == "*")
                {
                    var records = this.service.GetRecords();
                    this.printer(records, outParams);
                    Memoizer.Memoize(request, Tuple.Create(records, outParams));
                    return;
                }

                if (andOperator)
                {
                    for (int i = 0; i < searchParams.Length; i++)
                    {
                        if (searchParams[i] == "id")
                        {
                            string temp = searchParams[0];
                            searchParams[0] = searchParams[i];
                            searchParams[i] = temp;

                            temp = searchValues[0];
                            searchValues[0] = searchValues[i];
                            searchValues[i] = temp;
                            break;
                        }
                    }

                    if (searchParams[0] == "id")
                    {
                        var record = this.service.GetById(Convert.ToInt32(searchValues[0]));
                        for (int i = 1; i < searchParams.Length; i++)
                        {
                            switch (searchParams[i])
                            {
                                case "firstname":
                                    if (record.FirstName.ToLower() != searchValues[i].ToLower())
                                    {
                                        throw new ArgumentException("No such records");
                                    }

                                    break;
                                case "lastname":
                                    if (record.LastName.ToLower() != searchValues[i].ToLower())
                                    {
                                        throw new ArgumentException("No such records");
                                    }

                                    break;
                                case "dateofbirth":
                                    if (record.DateOfBirth != DateTime.Parse(searchValues[i], new System.Globalization.CultureInfo("en-US")))
                                    {
                                        throw new ArgumentException("No such records");
                                    }

                                    break;
                            }
                        }

                        List<FileCabinetRecord> recordList = new List<FileCabinetRecord>();
                        recordList.Add(record);
                        this.printer(recordList, outParams);
                        Memoizer.Memoize(request, Tuple.Create(recordList, outParams));
                    }
                    else
                    {
                        List<FileCabinetRecord> foundRecords = new List<FileCabinetRecord>();
                        switch (searchParams[0])
                        {
                            case "firstname":
                                foundRecords = this.service.FindByFirstName(searchValues[0].ToLower()).ToList();
                                break;
                            case "lastname":
                                foundRecords = this.service.FindByLastName(searchValues[0].ToLower()).ToList();
                                break;
                            case "dateofbirth":
                                foundRecords = this.service.FindByDateOfBirth(DateTime.Parse(searchValues[0], new System.Globalization.CultureInfo("en-US"))).ToList();
                                break;
                        }

                        List<FileCabinetRecord> foundValidRecords = new List<FileCabinetRecord>(foundRecords);
                        foreach (var foundRecord in foundRecords)
                        {
                            for (int i = 0; i < searchParams.Length; i++)
                            {
                                switch (searchParams[i])
                                {
                                    case "firstname":
                                        if (foundRecord.FirstName.ToLower() != searchValues[i].ToLower())
                                        {
                                            foundValidRecords.Remove(foundRecord);
                                        }

                                        break;
                                    case "lastname":
                                        if (foundRecord.LastName.ToLower() != searchValues[i].ToLower())
                                        {
                                            foundValidRecords.Remove(foundRecord);
                                        }

                                        break;
                                    case "dateofbirth":
                                        if (foundRecord.DateOfBirth != DateTime.Parse(searchValues[i], new System.Globalization.CultureInfo("en-US")))
                                        {
                                            foundValidRecords.Remove(foundRecord);
                                        }

                                        break;
                                }
                            }
                        }

                        if (!foundValidRecords.Any())
                        {
                            throw new ArgumentException("No records with such parameters");
                        }

                        this.printer(foundValidRecords, outParams);
                        Memoizer.Memoize(request, Tuple.Create(foundValidRecords, outParams));
                    }
                }
                else
                {
                    List<FileCabinetRecord> foundRecords = new List<FileCabinetRecord>();
                    for (int i = 0; i < searchParams.Length; i++)
                    {
                        switch (searchParams[i])
                        {
                            case "firstname":
                                foundRecords.AddRange(this.service.FindByFirstName(searchValues[i].ToLower()));
                                break;
                            case "lastname":
                                foundRecords.AddRange(this.service.FindByLastName(searchValues[i].ToLower()));
                                break;
                            case "dateofbirth":
                                foundRecords.AddRange(this.service.FindByDateOfBirth(DateTime.Parse(searchValues[i],
                                                      new System.Globalization.CultureInfo("en-US"))));
                                break;
                        }
                    }

                    foundRecords = foundRecords.Distinct().ToList();

                    if (!foundRecords.Any())
                    {
                        throw new ArgumentException("No records with such parameters");
                    }

                    this.printer(foundRecords, outParams);
                    Memoizer.Memoize(request, Tuple.Create(foundRecords, outParams));
                }
            }
            else
            {
                this.printer(((Tuple<List<FileCabinetRecord>, string[]>)memory).Item1, ((Tuple<List<FileCabinetRecord>, string[]>)memory).Item2);
            }
        }

        private static string RemoveWhitespace(string input)
        {
            return new string(input.ToCharArray()
                .Where(c => !char.IsWhiteSpace(c))
                .ToArray());
        }

        private static void SplitParameters(string parameters, out string[] outParams, out string[] searchParams, out string[] searchValues, out bool andOperator)
        {
            if (!parameters.Contains("where", StringComparison.InvariantCultureIgnoreCase))
            {
                throw new ArgumentException("Wrong syntax");
            }

            string outPart = parameters.Split("where")[0];
            if (string.IsNullOrWhiteSpace(outPart))
            {
                outParams = new string[1] { "*" };
            }
            else
            {
                outPart = RemoveWhitespace(outPart);
                if (outPart.Contains(',', StringComparison.InvariantCultureIgnoreCase))
                {
                    outParams = outPart.Split(',');
                }
                else
                {
                    outParams = new string[1] { outPart };
                }
            }

            string searchPart;
            string[] searchParamsValues;
            searchPart = RemoveWhitespace(parameters.Split("where")[1]);
            searchParamsValues = new string[1];
            if (searchPart.Contains("and", StringComparison.InvariantCultureIgnoreCase))
            {
                searchParamsValues = searchPart.Split("and");
                andOperator = true;
            }
            else if (searchPart.Contains("or", StringComparison.InvariantCultureIgnoreCase))
            {
                searchParamsValues = searchPart.Split("or");
                andOperator = false;
            }
            else if (string.IsNullOrWhiteSpace(searchPart))
            {
                searchParamsValues = new string[1];
                searchParamsValues[0] = "*=*";
                andOperator = false;
            }
            else
            {
                searchParamsValues[0] = searchPart;
                andOperator = false;
            }

            searchParams = new string[searchParamsValues.Length];
            searchValues = new string[searchParamsValues.Length];
            for (int i = 0; i < searchParamsValues.Length; i++)
            {
                searchParams[i] = searchParamsValues[i].Split('=')[0];
                searchValues[i] = searchParamsValues[i].Split('=')[1];
            }

            if (searchParams.Length != searchValues.Length)
            {
                throw new ArgumentException("Wrong syntax (wrong parameters count)");
            }

            for (int i = 0; i < searchParams.Length; i++)
            {
                searchParams[i] = searchParams[i].ToLower();
                if (searchParams[i] != "id" && searchParams[i] != "firstname" && searchParams[i] != "lastname"
                    && searchParams[i] != "dateofbirth" && searchParams[i] != "*")
                {
                    throw new ArgumentException("Wrong parameter: " + searchParams[i]);
                }
            }

            for (int i = 0; i < outParams.Length; i++)
            {
                outParams[i] = outParams[i].ToLower();
                if (outParams[i] != "firstname" && outParams[i] != "lastname" && outParams[i] != "dateofbirth"
                    && outParams[i] != "height" && outParams[i] != "weight" && outParams[i] != "temperament"
                    && outParams[i] != "*" && outParams[i] != "id")
                {
                    throw new ArgumentException("Wrong parameter: " + outParams[i]);
                }
            }
        }
    }
}
