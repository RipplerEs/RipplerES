using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using RipplerES.CommandHandler;

namespace RipplerES.NewtonSoft.Json.Serializer
{
    public class JSonSerializer : ISerializer
    {
        public IAggregateEvent<T> Deserialize<T>(AggregateEventData aggregateEventData)
        {
            return (IAggregateEvent<T>)JsonConvert.DeserializeObject(aggregateEventData.Data, Type.GetType(aggregateEventData.Type));
        }

        public AggregateEventData Serialize<T>(IAggregateEvent<T> @event, Dictionary<string, string> metaData)
        {
            return new AggregateEventData
            {
                Data = JsonConvert.SerializeObject(@event),
                MetaData = JsonConvert.SerializeObject(metaData),
                Type = @event.GetType().AssemblyQualifiedName
            };
        }
    }
}