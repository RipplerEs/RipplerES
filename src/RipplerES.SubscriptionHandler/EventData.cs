using System;

namespace RipplerES.SubscriptionHandler
{
    public class EventData
    {
        public Guid AggregateId { get; set; }
        public int Version { get; set; }
        public string EventType { get; set; }
        public string Data { get; set; }
        public string MetaData { get; set; }
    }
}