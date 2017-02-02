namespace RipplerES.CommandHandler
{
    public sealed class AggregateCommandErrorResult<T> : IAggregateCommandResult<T>
    {
        public IAggregateError<T> Error { get; private set; }

        public AggregateCommandErrorResult(IAggregateError<T> error)
        {
            Error = error;
        }
    }
}