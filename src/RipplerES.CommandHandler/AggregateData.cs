using System.Collections.Generic;

namespace RipplerES.CommandHandler
{
    public class AggregateData
    {
        public SnapshotInfo SnapshotInfo { get; set; }
        public IEnumerable<AggregateEventData> Events { get; set; } 
    }
}