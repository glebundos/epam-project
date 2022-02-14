namespace FileCabinetApp
{
    /// <summary>
    /// Builds the FullValidator with given ValidatorsSettings.
    /// </summary>
    public class ValidatorBuilder
    {
        private readonly List<IRecordValidator> validators;

        private ValidatorBuilder() => this.validators = new List<IRecordValidator>();

        /// <summary>
        /// Creating FullValidator instance.
        /// </summary>
        /// <param name="settings">Validators settings.</param>
        /// <returns>FullValidator instance.</returns>
        public static IRecordValidator CreateCompositeValidator(ValidatorsSettings settings)
        {
            if (settings is null)
            {
                throw new ArgumentNullException(nameof(settings), "Settings are null");
            }

            return new ValidatorBuilder()
                .FirstNameValidator(settings.FirstNameLenght_min, settings.FirstNameLenght_max)
                .LastNameValidator(settings.LastNameLenght_min, settings.LastNameLenght_max)
                .DateOfBirthValidator(settings.DateOfBitrth_min, settings.DateOfBitrth_max)
                .HeightValidator(settings.Height_min, settings.Height_max)
                .WeightValidator(settings.Weight_min, settings.Weight_max)
                .TemperamentValidator(settings.AllowedTemperaments)
                .Create();
        }

        private ValidatorBuilder FirstNameValidator(int minFirstNameLength, int maxFirstNameLength)
        {
            this.validators.Add(new FirstNameValidator(minFirstNameLength, maxFirstNameLength));
            return this;
        }

        private ValidatorBuilder LastNameValidator(int minLastNameLength, int maxLastNameLength)
        {
            this.validators.Add(new LastNameValidator(minLastNameLength, maxLastNameLength));
            return this;
        }

        private ValidatorBuilder DateOfBirthValidator(DateTime minDate, DateTime maxDate)
        {
            this.validators.Add(new DateOfBirthValidator(minDate, maxDate));
            return this;
        }

        private ValidatorBuilder HeightValidator(short minHeight, short maxHeight)
        {
            this.validators.Add(new HeightValidator(minHeight, maxHeight));
            return this;
        }

        private ValidatorBuilder WeightValidator(decimal minWeight, decimal maxWeight)
        {
            this.validators.Add(new WeightValidator(minWeight, maxWeight));
            return this;
        }

        private ValidatorBuilder TemperamentValidator(char[] allowedTemperaments)
        {
            this.validators.Add(new TemperamentValidator(allowedTemperaments));
            return this;
        }

        private IRecordValidator Create()
        {
            return new CompositeValidator(this.validators);
        }
    }
}
