using System.Collections.Generic;

namespace RipplerES.CommandHandler
{
    public interface ISerializer
    {
        IAggregateEvent<T> Deserialize<T>(AggregateEventData aggregateEventData);
        AggregateEventData Serialize<T>(IAggregateEvent<T> @event, Dictionary<string, string> metaData);
    }
}