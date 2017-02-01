namespace RipplerES.CommandHandler
{
    public sealed class AggregateCommandSuccessResult<T> : IAggregateCommandResult<T>
    {
        public IAggregateEvent<T> Event { get; private set; }

        public AggregateCommandSuccessResult(IAggregateEvent<T> @event)
        {
            Event = @event;
        }
    }

    public sealed class AggregateCommandErrorResult<T> : IAggregateCommandResult<T>
    {
        public IAggregateError<T> Error { get; private set; }

        public AggregateCommandErrorResult(IAggregateError<T> error)
        {
            Error = error;
        }
    }
}