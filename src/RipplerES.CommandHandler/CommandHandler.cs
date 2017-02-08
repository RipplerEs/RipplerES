using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace RipplerES.CommandHandler
{
    public class CommandHandler<T>
    {
        public const int NewAggregateVersion = -1;

        private readonly IEventRepository _repository;
        private readonly ISerializer _serializer;

        private readonly AggregateRoot<T> _aggregateRoot;

        public CommandHandler(IEventRepository repository, ISerializer serializer, IServiceProvider serviceProvider)
        {
            _repository = repository;
            _serializer = serializer;

            _aggregateRoot = new AggregateRoot<T>(serviceProvider);
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

                //TODO TO Config
                var snapshotInterval = 1000;
                string snapshot = null;
                if (snapshotable != null && (expectedVersion + 1) % snapshotInterval == 0)
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
