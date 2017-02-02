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
}