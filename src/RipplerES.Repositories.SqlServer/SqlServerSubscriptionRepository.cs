using System;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using System.Linq;
using Microsoft.Extensions.Configuration;
using RipplerES.CommandHandler;
using RipplerES.CommandHandler.Utilities;
using RipplerES.SubscriptionHandler;

namespace RipplerES.Repositories.SqlServer
{
    public class SqlServerSubscriptionRepository : ISubscriptionRepository
    {
        private readonly IConfiguration _configuration;
        private const string SubscribeProcedure = "Subscribe";
        private const string FetchProcedure = "Fetch";

        private string ConnectionString
            => _configuration.GetString("Database", "ConnectionString", string.Empty);

        public SqlServerSubscriptionRepository(IConfiguration configuration)
        {
            _configuration = configuration;

        }

        public void Subscribe(Guid channelId, string name)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.QueryMultiple(SubscribeProcedure,
                    new
                    {
                        ChannelId = channelId,
                        Name = name
                    },
                    commandType: CommandType.StoredProcedure);
            }
        }

        public EventData[] Fetch(Guid channelId)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var result = connection.QueryMultiple(FetchProcedure,
                    new
                    {
                        ChannelId = channelId,
                    },
                    commandType: CommandType.StoredProcedure);

                return result.Read<EventData>().ToArray();
            }
        }
    }
}
