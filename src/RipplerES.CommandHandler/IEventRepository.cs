using System;

namespace RipplerES.CommandHandler
{
    public interface IEventRepository
    {
        AggregateData GetEvents(Guid id, bool useSnapshot = false);
        int Save(Guid id, int expectedVersion, AggregateEventData aggregateEvent, string snapshot);
    }
}