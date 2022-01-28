namespace FileCabinetApp
{
    /// <summary>
    /// Contents all limits for validators.
    /// </summary>
    public class ValidatorsSettings
    {
        /// <summary>
        /// Gets or sets minimal possible length of first name.
        /// </summary>
        /// <value>
        /// Minimal possible length of first name.
        /// </value>
        public int FirstNameLenght_min { get; set; }

        /// <summary>
        /// Gets or sets maximum possible length of first name.
        /// </summary>
        /// <value>
        /// Maximum possible length of first name.
        /// </value>
        public int FirstNameLenght_max { get; set; }

        /// <summary>
        /// Gets or sets minimal possible length of last name.
        /// </summary>
        /// <value>
        /// Minimal possible length of last name.
        /// </value>
        public int LastNameLenght_min { get; set; }

        /// <summary>
        /// Gets or sets maximum possible length of last name.
        /// </summary>
        /// <value>
        /// Maximum possible length of last name.
        /// </value>
        public int LastNameLenght_max { get; set; }

        /// <summary>
        /// Gets or sets minimal possible date of birth.
        /// </summary>
        /// <value>
        /// Minimal possible date of birth.
        /// </value>
        public DateTime DateOfBitrth_min { get; set; }

        /// <summary>
        /// Gets or sets maximum possible date of birth.
        /// </summary>
        /// <value>
        /// Maximum possible date of birth.
        /// </value>
        public DateTime DateOfBitrth_max { get; set; }

        /// <summary>
        /// Gets or sets minimum possible height.
        /// </summary>
        /// <value>
        /// Minimum possible height.
        /// </value>
        public short Height_min { get; set; }

        /// <summary>
        /// Gets or sets maximum possible height.
        /// </summary>
        /// <value>
        /// Maximum possible height.
        /// </value>
        public short Height_max { get; set; }

        /// <summary>
        /// Gets or sets minimum possible weight.
        /// </summary>
        /// <value>
        /// Minimum possible weight.
        /// </value>
        public decimal Weight_min { get; set; }

        /// <summary>
        /// Gets or sets maximum possible weight.
        /// </summary>
        /// <value>
        /// Maximum possible weight.
        /// </value>
        public decimal Weight_max { get; set; }

        /// <summary>
        /// Gets or sets array of allowed temperaments.
        /// </summary>
        /// <value>
        /// Array of allowed temperaments.
        /// </value>
        public char[] AllowedTemperaments { get; set; }

        /// <summary>
        /// Sets all settings to default preset values.
        /// </summary>
        public void SetDefaultConfig()
        {
            this.FirstNameLenght_min = 2;
            this.FirstNameLenght_max = 60;
            this.LastNameLenght_min = 2;
            this.LastNameLenght_max = 60;
            this.DateOfBitrth_min = new DateTime(1950, 1, 1);
            this.DateOfBitrth_max = DateTime.Now;
            this.Height_min = 45;
            this.Height_max = 252;
            this.Weight_min = 2;
            this.Weight_max = 600;
            this.AllowedTemperaments = new char[] { 'P', 'C', 'S', 'M', };
        }

        /// <summary>
        /// Sets all settings to custom preset values.
        /// </summary>
        public void SetCustomConfig()
        {
            this.FirstNameLenght_min = 4;
            this.FirstNameLenght_max = 30;
            this.LastNameLenght_min = 4;
            this.LastNameLenght_max = 30;
            this.DateOfBitrth_min = new DateTime(DateTime.Now.Year - 150, DateTime.Now.Month, DateTime.Now.Day);
            this.DateOfBitrth_max = new DateTime(DateTime.Now.Year - 18, DateTime.Now.Month, DateTime.Now.Day);
            this.Height_min = 90;
            this.Height_max = 252;
            this.Weight_min = 25;
            this.Weight_max = 600;
            this.AllowedTemperaments = new char[] { 'P', 'C', 'S', };
        }
    }
}
