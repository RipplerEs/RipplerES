using System;
using System.Collections.Generic;

namespace RipplerES.CommandHandler
{
    public interface IEventRepository
    {
        AggregateData GetEvents(Guid id);
        int Save(Guid id, int expectedVersion, AggregateEventData aggregateEvent, string snapshot);
    }

    public class SnapshotInfo
    {
        public int SnapshotVersion { get; set; }
        public string Snapshot { get; set; }
    }

    public class AggregateData
    {
        public SnapshotInfo SnapshotInfo { get; set; }
        public IEnumerable<AggregateEventData> Events { get; set; } 
    }
}