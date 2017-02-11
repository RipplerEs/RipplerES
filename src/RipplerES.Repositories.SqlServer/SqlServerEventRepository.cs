using System;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

using Microsoft.Extensions.Configuration;
using RipplerES.CommandHandler;

using Dapper;
using System.Linq;
using RipplerES.CommandHandler.Utilities;

namespace RipplerES.Repositories.SqlServer
{
    public class SqlServerEventRepository : IEventRepository
    {
        private readonly IConfiguration _configuration;
        private const string GetEventsByAggregateIdProcedure    = "GetEventsByAggregateId";
        private const string SaveEventProcedure                 = "SaveAggregateEvent";

        private string ConnectionString 
            => _configuration.GetString("Database", "ConnectionString", string.Empty);

        public SqlServerEventRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public AggregateData GetEvents(Guid id, bool useSnapshot)
        {
            using(var connection = new SqlConnection(ConnectionString))
            {
                var result = connection.QueryMultiple(GetEventsByAggregateIdProcedure, 
                                         new {
                                             AggregateId = id,
                                             useSnapshot
                                         }, 
                                         commandType: CommandType.StoredProcedure);

                if (useSnapshot)
                {
                    return new AggregateData
                    {
                        Events = result.Read<AggregateEventData>(),
                        SnapshotInfo = result.Read<SnapshotInfo>().SingleOrDefault()
                    };
                }

                return new AggregateData
                {
                    Events = result.Read<AggregateEventData>()
                };

            }
        }

        public int Save(Guid id, int expectedVersion, AggregateEventData aggregateEvent, string snapshot)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                return connection.Query<int>(SaveEventProcedure,
                                             new {  AggregateId = id,
                                                    expectedVersion,
                                                    aggregateEvent.AggregateType,
                                                    aggregateEvent.EventType,
                                                    aggregateEvent.Data,
                                                    aggregateEvent.MetaData,
                                                    snapshot
                                             },
                                             commandType: CommandType.StoredProcedure)
                                             .FirstOrDefault();
            }
        }
    }
}
