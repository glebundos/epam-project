namespace FileCabinetApp
{
    /// <summary>
    /// Contents all limits for validators.
    /// </summary>
    [Serializable]
    public class ValidatorsSettings
    {
        /// <summary>
        /// Gets or sets minimal possible length of first name.
        /// </summary>
        /// <value>
        /// Minimal possible length of first name.
        /// </value>
        public int FirstNameLength_min { get; set; }

        /// <summary>
        /// Gets or sets maximum possible length of first name.
        /// </summary>
        /// <value>
        /// Maximum possible length of first name.
        /// </value>
        public int FirstNameLength_max { get; set; }

        /// <summary>
        /// Gets or sets minimal possible length of last name.
        /// </summary>
        /// <value>
        /// Minimal possible length of last name.
        /// </value>
        public int LastNameLength_min { get; set; }

        /// <summary>
        /// Gets or sets maximum possible length of last name.
        /// </summary>
        /// <value>
        /// Maximum possible length of last name.
        /// </value>
        public int LastNameLength_max { get; set; }

        /// <summary>
        /// Gets or sets minimal possible date of birth.
        /// </summary>
        /// <value>
        /// Minimal possible date of birth.
        /// </value>
        public DateTime DateOfBirth_min { get; set; }

        /// <summary>
        /// Gets or sets maximum possible date of birth.
        /// </summary>
        /// <value>
        /// Maximum possible date of birth.
        /// </value>
        public DateTime DateOfBirth_max { get; set; }

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
#pragma warning disable CS8618 // Поле, не допускающее значения NULL, должно содержать значение, отличное от NULL, при выходе из конструктора. Возможно, стоит объявить поле как допускающее значения NULL.
        public char[] AllowedTemperaments { get; set; }
#pragma warning restore CS8618 // Поле, не допускающее значения NULL, должно содержать значение, отличное от NULL, при выходе из конструктора. Возможно, стоит объявить поле как допускающее значения NULL.

        /// <summary>
        /// Sets all settings to default preset values.
        /// </summary>
        public void SetDefaultConfig()
        {
            this.FirstNameLength_min = 2;
            this.FirstNameLength_max = 60;
            this.LastNameLength_min = 2;
            this.LastNameLength_max = 60;
            this.DateOfBirth_min = new DateTime(1950, 1, 1);
            this.DateOfBirth_max = DateTime.Now;
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
            this.FirstNameLength_min = 4;
            this.FirstNameLength_max = 30;
            this.LastNameLength_min = 4;
            this.LastNameLength_max = 30;
            this.DateOfBirth_min = new DateTime(1870, 1, 1);
            this.DateOfBirth_max = new DateTime(2004, 2, 15);
            this.Height_min = 90;
            this.Height_max = 252;
            this.Weight_min = 25;
            this.Weight_max = 600;
            this.AllowedTemperaments = new char[] { 'P', 'C', 'S', };
        }
    }
}
