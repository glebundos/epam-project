namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Contains command and its parameters for handlers.
    /// </summary>
    public class AppCommandRequest : IEquatable<AppCommandRequest>
    {
        private string command;

        private string parameters;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppCommandRequest"/> class.
        /// </summary>
        public AppCommandRequest()
        {
            this.command = string.Empty;
            this.parameters = string.Empty;
        }

        /// <summary>
        /// Gets command from request.
        /// </summary>
        /// <value>
        /// <see cref="Command"/>.
        /// </value>
        public string Command
        {
            get
            {
                return this.command;
            }

            init
            {
                this.command = value ?? string.Empty;
            }
        }

        /// <summary>
        /// Gets parameters from request.
        /// </summary>
        /// <value>
        /// <see cref="Parameters"/>.
        /// </value>
        public string Parameters
        {
            get
            {
                return this.parameters;
            }

            init
            {
                this.parameters = value ?? string.Empty;
            }
        }

        /// <inheritdoc/>
        public bool Equals(AppCommandRequest? other)
        {
            if (ReferenceEquals(other, null))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return this.command.Equals(other.Command, StringComparison.Ordinal) && this.parameters.Equals(other.Parameters, StringComparison.Ordinal);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return this.parameters.GetHashCode(StringComparison.InvariantCultureIgnoreCase) ^ this.Parameters.GetHashCode(StringComparison.InvariantCulture);
        }

        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            return this.Equals(obj as AppCommandRequest);
        }
    }
}
