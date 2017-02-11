﻿using System;
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
        private readonly IConfigurationRoot _configurationRoot;

        private int _snapshotInterval = 1000;
        private bool _useSnapshot = false;

        private readonly AggregateRoot<T> _aggregateRoot;

        private string SnapshotInterval =>
            _configurationRoot.GetSection("Aggregates")["SnapshotInterval"];

        private string UseSnapshot =>
            _configurationRoot.GetSection("Aggregates")["UseSnapshot"];

        public CommandHandler(IEventRepository repository, 
                              ISerializer serializer, 
                              IServiceProvider serviceProvider,
                              IConfigurationRoot configurationRoot)
        {
            _repository = repository;
            _serializer = serializer;
            _configurationRoot = configurationRoot;

            Initialize();

            _aggregateRoot = new AggregateRoot<T>(serviceProvider);
        }
        
        private void Initialize()
        {
            _snapshotInterval   = ExtractSnapshotInterval();
            _useSnapshot        = ExtractUseSnapshot();
        }

        private int ExtractSnapshotInterval()
        {
            if (string.IsNullOrWhiteSpace(SnapshotInterval)) return _snapshotInterval;

            int value;
            return int.TryParse(SnapshotInterval, out value) 
                        ? value 
                        : _snapshotInterval;
        }

        private bool ExtractUseSnapshot()
        {
            if (string.IsNullOrWhiteSpace(UseSnapshot)) return _useSnapshot;

            bool value;
            return bool.TryParse(UseSnapshot, out value)
                        ? value
                        : _useSnapshot;
        }
        
        public ICommandResult<T> Handle(Guid id, 
                                        int expectedVersion, 
                                        IAggregateCommand<T> aggregateCommand, 
                                        Dictionary<string,string> metaData = null)
        {
            var aggregateData = _repository.GetEvents(id, _useSnapshot);
            var events = aggregateData.Events.Select(c => _serializer.Deserialize<T>(c)).ToList();

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
