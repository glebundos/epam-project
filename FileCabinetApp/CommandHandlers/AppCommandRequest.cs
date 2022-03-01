namespace FileCabinetApp.CommandHandlers
{
    public class AppCommandRequest : IEquatable<AppCommandRequest>
    {
        public string Command { get; init; }

        public string Parameters { get; init; }

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

            return this.Command.Equals(other.Command, StringComparison.Ordinal) && this.Parameters.Equals(other.Parameters, StringComparison.Ordinal);
        }

        public override int GetHashCode()
        {
            return this.Command.GetHashCode() ^ this.Parameters.GetHashCode();
        }
    }
}
