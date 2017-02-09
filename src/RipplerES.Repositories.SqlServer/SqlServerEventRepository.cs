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
        private const string GetEventsByAggregateIdProcedure = "GetEventsByAggregateId";
        private const string SaveEventProcedure = "SaveAggregateEvent";

        private string _connectionString;

        public SqlServerEventRepository()
        {
            Initialize();
        }

        private void Initialize()
        {
            var configuration = ReadConfigurationFile();
            _connectionString = ExtractConnectionString(configuration);
        }

        private static string ExtractConnectionString(IConfiguration configuration)
        {
            return configuration.GetSection("Database")["ConnectionString"];
        }

        private static IConfigurationRoot ReadConfigurationFile()
        {
            return new ConfigurationBuilder()
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile("config.json").Build();
        }

        public IEnumerable<AggregateEventData> GetEvents(Guid id)
        {
            using(var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<AggregateEventData>(GetEventsByAggregateIdProcedure, 
                                                            new { AggregateId = id }, 
                                                            commandType: CommandType.StoredProcedure);
            }
        }

        public int Save(Guid id, int expectedVersion, AggregateEventData aggregateEvent, string snapshot)
        {
            using (var connection = new SqlConnection(_connectionString))
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
