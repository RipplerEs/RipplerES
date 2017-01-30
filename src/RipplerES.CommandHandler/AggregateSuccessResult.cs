namespace RipplerES.CommandHandler
{
    public class AggregateSuccessResult<T> : IAggregateCommandResult<T>
    {
        public IAggregateEvent<T> Event { get; private set; }

        public AggregateSuccessResult(IAggregateEvent<T> @event)
        {
            Event = @event;
        }
    }
}