using System;

namespace RipplerES.SubscriptionHandler
{
    public class SubscriptionHandler
    {
        private readonly Guid _subscriptionId;
        private readonly string _name;
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly ITypedEventHandler[] _eventHandlers;
        private readonly IEventHandler _unhandledEventHandler;

        public SubscriptionHandler(
            Guid subscriptionId,
            string name,
            ISubscriptionRepository subscriptionRepository,
            ITypedEventHandler[] eventHandlers,
            IEventHandler unhandledEventHandler)
        {
            _subscriptionId = subscriptionId;
            _name = name;
            _subscriptionRepository = subscriptionRepository;
            _unhandledEventHandler = unhandledEventHandler;
            _eventHandlers = eventHandlers;
        }

        public void Initialize()
        {
            _subscriptionRepository.Subscribe(_subscriptionId, _name);
        }

        public void Execute()
        {
            foreach (var @event in _subscriptionRepository.Fetch(_subscriptionId))
            {
                foreach (var handler in _eventHandlers.CanHandle(_unhandledEventHandler, @event))
                {
                    handler.Handle(@event.AggregateId, @event.Version, @event.Data, @event.MetaData);
                }
            }
        }
    }
}