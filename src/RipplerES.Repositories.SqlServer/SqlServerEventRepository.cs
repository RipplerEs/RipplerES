using System;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

using Microsoft.Extensions.Configuration;
using RipplerES.CommandHandler;

using Dapper;
using System.Linq;

namespace RipplerES.Repositories.SqlServer
{
    public class SqlServerEventRepository : IEventRepository
    {
        private readonly IConfigurationRoot _configurationRoot;
        private const string GetEventsByAggregateIdProcedure = "GetEventsByAggregateId";
        private const string SaveEventProcedure = "SaveAggregateEvent";

        private string ConnectionString =>
            _configurationRoot.GetSection("Database")["ConnectionString"];


        public SqlServerEventRepository(IConfigurationRoot configurationRoot)
        {
            _configurationRoot = configurationRoot;
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


                return new AggregateData
                {
                    Events = result.Read<AggregateEventData>(),
                    SnapshotInfo = result.Read<SnapshotInfo>().SingleOrDefault()
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
