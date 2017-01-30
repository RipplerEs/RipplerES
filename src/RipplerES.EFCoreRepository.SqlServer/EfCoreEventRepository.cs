using System;
using System.Linq;
using System.Collections.Generic;
using RipplerES.CommandHandler;

namespace RipplerES.EFCoreRepository
{
    public class EfCoreEventRepository : IEventRepository, IDisposable
    {
        private readonly EventContext _db;

        public EfCoreEventRepository(EventContext db)
        {
            _db = db;
        }

        public IEnumerable<AggregateEventData> GetEvents(Guid id)
        {
            return _db.Events.Where(c => c.AggregateId == id).Select(e => new AggregateEventData
            {
                Type = e.Type,
                Data = e.Data
            });
            
        }

        public int Save(Guid id, int expectedVersion, AggregateEventData aggregateEvent)
        {
            var last = _db.Events.Where(c => c.AggregateId == id)
                .OrderByDescending(c => c.Version)
                .FirstOrDefault();

            if (last != null && expectedVersion != last.Version) throw new Exception("Wrong version");
            if (last == null && expectedVersion != -1) throw new Exception("Not found");

            _db.Events.Add(
                new Event
                {
                    AggregateId = id,
                    Version = NextVersion(expectedVersion),
                    Type = aggregateEvent.Type,
                    Data = aggregateEvent.Data,
                    MetaData = aggregateEvent.MetaData

                });

            _db.SaveChanges();
            return NextVersion(expectedVersion);
        }

        private static int NextVersion(int expectedVersion)
        {
            return expectedVersion == -1 ? 1 : expectedVersion + 1;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
