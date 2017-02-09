using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;


using Microsoft.Extensions.Configuration;

namespace RipplerES.CommandHandler
{
    public class CommandHandler<T>
    {
        public const int NewAggregateVersion = -1;

        private readonly IEventRepository _repository;
        private readonly ISerializer _serializer;
        private int _snapshotInterval = 1000;

        private readonly AggregateRoot<T> _aggregateRoot;

        public CommandHandler(IEventRepository repository, ISerializer serializer, IServiceProvider serviceProvider)
        {
            Initialize();
            _repository = repository;
            _serializer = serializer;

            _aggregateRoot = new AggregateRoot<T>(serviceProvider);
        }
        
        private void Initialize()
        {
            var configuration = ReadConfigurationFile();
            _snapshotInterval = ExtractSnapshotInterval(configuration);
        }

        private int ExtractSnapshotInterval(IConfiguration configuration)
        {
            var literal = configuration.GetSection("Aggregate")["SnapshotInterval"];
            if (string.IsNullOrWhiteSpace(literal)) return _snapshotInterval;

            int value;
            return int.TryParse(literal, out value) 
                        ? value 
                        : _snapshotInterval;
        }

        private static IConfigurationRoot ReadConfigurationFile()
        {
            return new ConfigurationBuilder()
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile("config.json").Build();
        }
        
        public ICommandResult<T> Handle(Guid id, int expectedVersion, IAggregateCommand<T> aggregateCommand, Dictionary<string,string> metaData = null)
        {
            var eventData = _repository.GetEvents(id);
            var events = eventData.Select(c => _serializer.Deserialize<T>(c)).ToList();

            IDisposable disposable = null;
            try
            {
                var instance = _aggregateRoot.CreateFromInitialState();
                var snapshotable = instance as ISnapshotable;

                disposable = instance as IDisposable;

                _aggregateRoot.Apply(instance, events);

                var commandResult = _aggregateRoot.Exec(instance, aggregateCommand);
                var error = commandResult as AggregateCommandErrorResult<T>;

                if (error != null)
                    return new CommandErrorResult<T>(error.Error);

                var success = commandResult as AggregateCommandSuccessResult<T>;
                if (success == null)
                    return new CommandErrorResult<T>(new UnexpectedAggregateEvent<T>(commandResult));
                
                string snapshot = null;
                if (snapshotable != null && (expectedVersion + 1) % _snapshotInterval == 0)
                {
                    snapshot = snapshotable.TakeSnapshot();
                }

                var newVersionNumber = _repository.Save(id, 
                                                        expectedVersion, 
                                                        _serializer.Serialize(success.Event, metaData),
                                                        snapshot);
                return newVersionNumber > 0
                            ? (ICommandResult<T>) new CommandSuccessResult<T>(newVersionNumber)
                            : (ICommandResult<T>) new CommandErrorResult<T>(new AggregateConcurrencyError<T>());
            }
            finally
            {
                disposable?.Dispose();
            }
        }


    }

    public class UnexpectedAggregateEvent<T> : IAggregateError<T>
    {
        public IAggregateCommandResult<T> CommandResult { get; }

        public UnexpectedAggregateEvent(IAggregateCommandResult<T> commandResult)
        {
            CommandResult = commandResult;
        }
    }

    public class AggregateConcurrencyError<T> : IAggregateError<T>
    {

    }
}
