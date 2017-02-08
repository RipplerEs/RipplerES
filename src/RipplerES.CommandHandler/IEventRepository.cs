using System;
using System.Collections.Generic;

namespace RipplerES.CommandHandler
{
    public interface IEventRepository
    {
        IEnumerable<AggregateEventData> GetEvents(Guid id);
        int Save(Guid id, int expectedVersion, AggregateEventData aggregateEvent, string snapshot);
    }
}