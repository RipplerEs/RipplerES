namespace RipplerES.CommandHandler
{
    public class AggregateEventData
    {
        public int Version { get; set; }
        public string AggregateType { get; set; }
        public string AggregateFriendlyName { get; set; }
        public string Data { get; set; }
        public string EventType { get; set; }
        public string EventFriendlyName { get; set; }
        public string MetaData { get; set; }

    }
}