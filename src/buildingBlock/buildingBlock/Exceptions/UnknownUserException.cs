namespace buildingBlock.Exceptions
{
    public class UnknownUserException : Exception
    {
        public UnknownUserException(Guid userId)
        : base($"Unknown user '{userId}'")
        {
            this.UserId = userId;
        }

        public Guid UserId { get; }
    }
}
