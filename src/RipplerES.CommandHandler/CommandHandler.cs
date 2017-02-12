using System;
using System.Linq;
using System.Collections.Generic;

using Microsoft.Extensions.Configuration;
using RipplerES.CommandHandler.Utilities;

namespace RipplerES.CommandHandler
{
    public class CommandHandler<T>
    {
        private readonly IEventRepository _repository;
        private readonly ISerializer _serializer;

        private readonly bool _useSnapshot;
        private readonly int _snapshotInterval;

        private readonly AggregateRoot<T> _aggregateRoot;

        public CommandHandler(IEventRepository repository, 
                              ISerializer serializer, 
                              IServiceProvider serviceProvider,
                              IConfiguration configuration)
        {
            _snapshotInterval = configuration.GetInt("Aggregates", "SnapshotInterval", 1000);
            _useSnapshot = configuration.GetBool("Aggregates", "UseSnapshot", false);

            _repository = repository;
            _serializer = serializer;

            _aggregateRoot = new AggregateRoot<T>(serviceProvider);
        }


        public ICommandResult<T> Handle(Guid id, 
                                        int expectedVersion, 
                                        IAggregateCommand<T> aggregateCommand, 
                                        Dictionary<string,string> metaData = null)
        {
            var aggregateData   = _repository.GetEvents(id, _useSnapshot);
            var events          = aggregateData.Events.Select(  
                                            c => _serializer.Deserialize<T>(c)).ToList();

            IDisposable disposable = null;
            try
            {
                var instance = _aggregateRoot.CreateFromInitialState();
                var snapshotable = instance as ISnapshotable;
                if (snapshotable != null && aggregateData.SnapshotInfo != null)
                {
                    snapshotable.RestoreFromSnapshot(aggregateData.SnapshotInfo.Snapshot);
                }

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
                            ? new CommandSuccessResult<T>(newVersionNumber)
                            : (ICommandResult<T>) new CommandErrorResult<T>(new AggregateConcurrencyError<T>());
            }
            finally
            {
                disposable?.Dispose();
            }
        }
    }
}
