using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace RipplerES.CommandHandler
{
    public class AggregateRoot<T>
    {
        public  readonly IServiceProvider _services;
        private readonly IDictionary<Type, MethodInfo> _aggregateEvents;
        private readonly IDictionary<Type, MethodInfo> _aggregateCommands;
        private readonly Func<T> _factory;

        public AggregateRoot(IServiceProvider services)
        {
            _services = services;
            _factory = () => _services.GetRequiredService<T>();

            _aggregateEvents = typeof (T).GetAggregateEvents();
            _aggregateCommands = typeof (T).GetAggregateCommands();
        }
        
        private void Apply(T obj, IAggregateEvent<T> aggregateEvent)
        {
            if (!_aggregateEvents.ContainsKey(aggregateEvent.GetType())) return;
            _aggregateEvents[aggregateEvent.GetType()].Invoke(obj, new [] { aggregateEvent });
        }

        private T CreateFromInitialState()
        {
            return (T) _factory.Invoke();
        }

        public T CreateFrom(List<IAggregateEvent<T>> events)
        {
            var instance = CreateFromInitialState();
            if (events == null) return instance;

            foreach (var @event in events)
            {
                Apply(instance, @event);
            }

            return instance;
        }

        public IAggregateCommandResult<T> Exec(T instance, IAggregateCommand<T> aggregateCommand)
        {
            if (!_aggregateCommands.ContainsKey(aggregateCommand.GetType())) throw new CommandNotFoundException(typeof(T), aggregateCommand.GetType());
            return (IAggregateCommandResult<T>)_aggregateCommands[aggregateCommand.GetType()]
                .Invoke(instance, new[] { aggregateCommand });
        }
    }
}
