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

        public ICommandResult<T> Handle(Guid id, int version, IAggregateCommand<T> aggregateCommand, Dictionary<string,string> metaData = null)
        {
            var eventData = _repository.GetEvents(id);
            var events = eventData.Select(c => _serializer.Deserialize<T>(c)).ToList();

            var instance = _aggregateRoot.CreateFrom(events);
            var commandResult = _aggregateRoot.Exec(instance, aggregateCommand);
            var error = commandResult as AggregateErrorResult<T>;
            if (error != null)
                return new CommandErrorResult<T>(error);

            var success = commandResult as AggregateSuccessResult<T>;
            if (success == null)
                return new CommandErrorResult<T>(new UnexpectedAggregateEvent<T>(commandResult));

            return _repository.Save(id, version, _serializer.Serialize(success.Event, metaData)) > 0 
                        ? (ICommandResult<T>)new CommandSuccessResult<T>()
                        : (ICommandResult<T>)new CommandErrorResult<T>(new AggregateConcurrencyError<T>());
        }


    }

    public class UnexpectedAggregateEvent<T> : AggregateErrorResult<T>
    {
        public IAggregateCommandResult<T> CommandResult { get; }

        public UnexpectedAggregateEvent(IAggregateCommandResult<T> commandResult)
        {
            CommandResult = commandResult;
        }
    }

    public class AggregateConcurrencyError<T> : AggregateErrorResult<T>
    {

    }
}
