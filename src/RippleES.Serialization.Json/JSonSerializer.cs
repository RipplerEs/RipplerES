using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using RipplerES.CommandHandler;

namespace RippleES.Serialization.Json
{
    public class JsonSerializer : ISerializer
    {
        public IAggregateEvent<T> Deserialize<T>(AggregateEventData aggregateEventData)
        {
            return (IAggregateEvent<T>)JsonConvert.DeserializeObject(aggregateEventData.Data, 
                                                                     Type.GetType(aggregateEventData.EventType));
        }

        public AggregateEventData Serialize<T>(IAggregateEvent<T> @event, Dictionary<string, string> metaData)
        {
            return new AggregateEventData
            {
                AggregateType = typeof(T).AssemblyQualifiedName,
                AggregateFriendlyName = typeof(T).GetFriendlName(),
                Data = JsonConvert.SerializeObject(@event),
                MetaData = JsonConvert.SerializeObject(metaData),
                EventType = @event.GetType().AssemblyQualifiedName,
                EventFriendlyName = @event.GetType().GetFriendlName(),
            };
        }
    }
}