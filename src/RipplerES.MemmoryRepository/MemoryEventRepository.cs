using System;
using System.Collections.Generic;
using RipplerES.CommandHandler;

namespace RipplerES.MemoryRepository
{
    public class MemoryEventRepository : IEventRepository
    {
        private static readonly object Lock = new object();

        private readonly Dictionary<Guid, int> _versions;
        private readonly Dictionary<Guid, List<AggregateEventData>> _events;
        private readonly Dictionary<Guid, object> _locks;
        

        public MemoryEventRepository()
        {
            _versions   = new Dictionary<Guid, int>();
            _events     = new Dictionary<Guid, List<AggregateEventData>>();
            _locks      = new Dictionary<Guid, object>();
        }
        public IEnumerable<AggregateEventData> GetEvents(Guid id)
        {
            return _events.ContainsKey(id) ? _events[id] : new List<AggregateEventData>();
        }

        public int Save(Guid id, int expectedVersion, AggregateEventData aggregateEvent)
        {
            EnsureLock(id);
            if (EnsureVersion(id, expectedVersion) == 0)
            {
                if (!_events.ContainsKey(id))
                {
                    _events.Add(id, new List<AggregateEventData>());
                }
                _events[id].Add(aggregateEvent);
            }
            return _versions[id];
        }

        private int EnsureVersion(Guid id, int expectedVersion)
        {
            lock (_locks[id])
            {
                if (!_versions.ContainsKey(id) && expectedVersion != -1) return -2;
                if (_versions.ContainsKey(id)  && expectedVersion == -1) return -3;
                if (_versions.ContainsKey(id) && _versions[id]    != expectedVersion) return -4;

                if (!_versions.ContainsKey(id))
                {
                    _versions.Add(id, 1);
                }
                else
                {
                    _versions[id]++;
                }
            }
            return 0;
        }

        private void EnsureLock(Guid id)
        {
            if (_locks.ContainsKey(id)) return;
            lock (Lock)
            {
                if (_locks.ContainsKey(id)) return;
                _locks.Add(id, new object());
            }
        }
    }
}
